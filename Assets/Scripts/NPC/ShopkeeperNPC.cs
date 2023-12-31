using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopkeeperNPC : MonoBehaviour
{
    #region Variables

    [Header("Clothes Datas")]
    [SerializeField] private ClotheData[] clotheDatas;

    [Header("Screens and Objects")]
    [SerializeField] private GameObject interactTextObject;
    [SerializeField] private GameObject backgroundOptionsObject;
    [SerializeField] private GameObject chatOptionsObject;
    [SerializeField] private GameObject sellOptionObject;
    [SerializeField] private GameObject buyOptionObject;
    [SerializeField] private GameObject currentMoneyObject;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private GameObject pressEscObject;
    
    [Header("Containers")]
    [SerializeField] private GameObject buyClotheContainer;
    [SerializeField] private GameObject sellClotheContainer;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject buyClothePrefab;
    [SerializeField] private GameObject sellClothePrefab;
    
    [Header("Result Screens")]
    [SerializeField] private GameObject interactionBuySuccessObject;
    [SerializeField] private GameObject interactionBuyFailedObject;
    [SerializeField] private GameObject interactionSellSuccessObject;

    private PlayerController _playerController;
    private Player _player;
    private int _currentMoney;
    
    private List<ClotheData> _availableClotheDatas = new List<ClotheData>();
    private List<GameObject> _buyButtons = new List<GameObject>();
    private List<GameObject> _sellButtons = new List<GameObject>();

    #endregion

    #region Messages
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player")) return;
        
        GetPlayerRefs(col);
        if(_player == null || _playerController == null) return;

        interactTextObject.SetActive(true);
        _playerController.OnEPressed += StartInteraction;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        
        interactTextObject.SetActive(false);
        _playerController.OnEPressed -= StartInteraction;
    }


    #endregion

    #region Methods
    
    private void GetPlayerRefs(Collider2D col)
    {
        _playerController = col.GetComponent<PlayerController>();
        _player = col.GetComponent<Player>();

        UpdateAvailableClothes();
        UpdateCurrentMoney();
    }

    private void UpdateAvailableClothes()
    {
        _availableClotheDatas.Clear();
        
        foreach (var clothe in clotheDatas)
        {
            if (!_player.ClothesInInventory.Contains(clothe))
                _availableClotheDatas.Add(clothe);
        }
    }

    private void StartInteraction()
    {
        interactTextObject.SetActive(false);
        backgroundOptionsObject.SetActive(true);
        OptionSelected();
        
        _playerController.ToggleMoveInputs(false);
        _playerController.OnEPressed -= StartInteraction;
    }
    
    public void StopInteraction()
    {
        CloseAllChatObjects();
        DestroyBuyButtons();
        DestroySellButtons();
        interactTextObject.SetActive(true);
        
        _playerController.ToggleMoveInputs(true);
        _playerController.OnEscPressed -= StopInteraction;
        _playerController.OnEPressed += StartInteraction;
    }

    private void ToggleShowCurrentMoney(bool show)
    {
        currentMoneyObject.SetActive(show);
    }

    public void BuyOptionSelected()
    {
        chatOptionsObject.SetActive(false);
        buyOptionObject.SetActive(true);
        pressEscObject.SetActive(true);
        PopulateBuyOptions();
        
        ToggleShowCurrentMoney(true);
        
        _playerController.OnEscPressed -= StopInteraction;
        _playerController.OnEscPressed += OptionSelected;
    }
    
    public void SellOptionSelected()
    {
        chatOptionsObject.SetActive(false);
        sellOptionObject.SetActive(true);
        pressEscObject.SetActive(true);
        PopulateSellOptions();
        
        ToggleShowCurrentMoney(true);
        
        _playerController.OnEscPressed -= StopInteraction;
        _playerController.OnEscPressed += OptionSelected;
    }
    
    public void OptionSelected()
    {
        buyOptionObject.SetActive(false);
        sellOptionObject.SetActive(false);
        DestroyBuyButtons();
        DestroySellButtons();
        chatOptionsObject.SetActive(true);
        pressEscObject.SetActive(true);
        
        ToggleShowCurrentMoney(false);
        
        _playerController.OnEscPressed -= OptionSelected;
        _playerController.OnEscPressed += StopInteraction;
    }

    private void CloseAllChatObjects()
    {
        buyOptionObject.SetActive(false);
        sellOptionObject.SetActive(false);
        chatOptionsObject.SetActive(false);
        backgroundOptionsObject.SetActive(false);
        pressEscObject.SetActive(false);
    }

    private void UpdateCurrentMoney()
    {
        _currentMoney = _player.CurrentMoney;
        currentMoneyText.text = _player.CurrentMoney.ToString();
    }

    private void PopulateBuyOptions()
    {
        foreach (var clothe in _availableClotheDatas)
        {
            var buyButtonInstance = Instantiate(buyClothePrefab, buyClotheContainer.transform);
            var buyClotheButton = buyButtonInstance.GetComponent<BuyClotheButton>();
            
            if(buyClotheButton == null) continue;

            buyClotheButton.clotheNameText.text = clothe.ClotheName;
            buyClotheButton.clotheDescriptionText.text = clothe.Description;
            buyClotheButton.clotheBuyPriceText.text = clothe.BuyPrice.ToString();
            buyClotheButton.clotheImage.sprite = clothe.ClotheSprite;
            buyClotheButton.buyButton.onClick.AddListener(() => TryBuyClothe(clothe));
            
            _buyButtons.Add(buyButtonInstance);
        }
    }
    
    private void PopulateSellOptions()
    {
        foreach (var clothe in _player.ClothesInInventory)
        {
            var sellButtonInstance = Instantiate(sellClothePrefab, sellClotheContainer.transform);
            var sellClotheButton = sellButtonInstance.GetComponent<SellClotheButton>();

            if(sellClotheButton == null) continue;
            
            sellClotheButton.clotheNameText.text = clothe.ClotheName;
            sellClotheButton.clotheDescriptionText.text = clothe.Description;
            sellClotheButton.clotheSellPriceText.text = clothe.SellPrice.ToString();
            sellClotheButton.clotheImage.sprite = clothe.ClotheSprite;
            sellClotheButton.sellButton.onClick.AddListener(() => SellClothe(clothe));
            
            _sellButtons.Add(sellButtonInstance);
        }
    }

    private void DestroyBuyButtons()
    {
        foreach (var button in _buyButtons)
        {
            Destroy(button);
        }
    }
    
    private void DestroySellButtons()
    {
        foreach (var button in _sellButtons)
        {
            Destroy(button);
        }
    }

    private void TryBuyClothe(ClotheData clothe)
    {
        pressEscObject.SetActive(false);
        
        if(_currentMoney >= clothe.BuyPrice)
            BuyClothe(clothe);
        else
            BuyClotheFailed();
        
        ToggleShowCurrentMoney(false);
    }

    private void BuyClothe(ClotheData clothe)
    {
        _playerController.OnEscPressed -= OptionSelected;
        _player.AddClotheToInventory(clothe);
        _player.SpendMoney(clothe.BuyPrice);
        
        UpdateAvailableClothes();
        
        buyOptionObject.SetActive(false);
        interactionBuySuccessObject.SetActive(true);
        
        UpdateCurrentMoney();
        DestroyBuyButtons();
        DestroySellButtons();
    }

    private void BuyClotheFailed()
    {
        _playerController.OnEscPressed -= OptionSelected;
        interactionBuyFailedObject.SetActive(true);
        
        UpdateCurrentMoney();
        DestroyBuyButtons();
        DestroySellButtons();
    }
    
    private void SellClothe(ClotheData clothe)
    {
        _playerController.OnEscPressed -= OptionSelected;
        _player.RemoveClotheFromInventory(clothe);
        _player.ReceiveMoney(clothe.SellPrice);
        
        ToggleShowCurrentMoney(false);
        
        UpdateAvailableClothes();
        
        sellOptionObject.SetActive(false);
        pressEscObject.SetActive(false);
        interactionSellSuccessObject.SetActive(true);
        
        UpdateCurrentMoney();
        DestroyBuyButtons();
        DestroySellButtons();
    }

    #endregion
}
