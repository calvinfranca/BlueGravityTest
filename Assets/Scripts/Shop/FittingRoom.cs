using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FittingRoom : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private GameObject interactTextObject;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private GameObject changeClothesObject;
    [SerializeField] private GameObject optionsObject;
    [SerializeField] private GameObject clotheChangeOptionsContainer;
    [SerializeField] private GameObject clotheChangeOptionPrefab;
    
    private PlayerController _playerController;
    private Player _player;
    private List<ClotheData> _playerClothes = new List<ClotheData>();
    
    private bool _changingClothes;
    private List<GameObject> _clotheOptions = new List<GameObject>();

    #endregion
    
    #region Messages
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player") || _changingClothes) return;
        
        GetPlayerRefs(col);
        if(_player == null || _playerController == null) return;

        interactTextObject.SetActive(true);
        _playerController.OnEPressed += StartInteraction;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player") || _changingClothes) return;
        
        interactTextObject.SetActive(false);
        _changingClothes = false;
        _playerController.OnEPressed -= StartInteraction;
    }

    #endregion

    #region Methods

    private void GetPlayerRefs(Collider2D col)
    {
        _playerController = col.GetComponent<PlayerController>();
        _player = col.GetComponent<Player>();
        _playerClothes = _player.ClothesInInventory;
    }
    
    private void StartInteraction()
    {
        _changingClothes = true;
        interactTextObject.SetActive(false);
        backgroundObject.SetActive(true);
        OptionsSelected();
        
        _playerController.ToggleMoveInputs(false);
    }
    
    public void StopInteraction()
    {
        _changingClothes = false;
        
        interactTextObject.SetActive(false);
        optionsObject.SetActive(false);
        backgroundObject.SetActive(false);
        changeClothesObject.SetActive(false);

        _playerController.ToggleMoveInputs(true);
    }

    public void ChangeClothesSelected()
    {
        PopulateClothesOptions();
        optionsObject.SetActive(false);
        changeClothesObject.SetActive(true);
    }
    
    public void OptionsSelected()
    {
        DestroyClotheOptions();
        optionsObject.SetActive(true);
        changeClothesObject.SetActive(false);
    }

    private void PopulateClothesOptions()
    {
        foreach (var clothe in _player.ClothesInInventory)
        {
            if(_player.CurrentClothe == clothe) continue;
            
            var changeOptionInstance = Instantiate(clotheChangeOptionPrefab, clotheChangeOptionsContainer.transform);
            var clotheChangeOption = changeOptionInstance.GetComponent<ClotheChangeOption>();
            
            if(clotheChangeOption == null) continue;

            clotheChangeOption.clotheNameText.text = clothe.ClotheName;
            clotheChangeOption.clotheImage.sprite = clothe.ClotheSprite;
            clotheChangeOption.selectButton.onClick.AddListener(() => ChangeClothe(clothe));
            
            _clotheOptions.Add(changeOptionInstance);
        }
    }

    private void DestroyClotheOptions()
    {
        foreach (var option in _clotheOptions)
        {
            Destroy(option);
        }
        _clotheOptions.Clear();
    }

    private void ChangeClothe(ClotheData clothe)
    {
        SpawnNewPlayer(clothe.ClotheID);
        StopInteraction();
    }

    private void SpawnNewPlayer(int clotheId)
    {
        var lastPosition = _player.transform.position;
        Destroy(_player.gameObject);
        var newPrefabInstance = Instantiate(playerPrefabs[clotheId], lastPosition, Quaternion.identity);

        _player = newPrefabInstance.GetComponent<Player>();
        _playerController = newPrefabInstance.GetComponent<PlayerController>();

        foreach (var clothe in _playerClothes)
        {
            _player.AddClotheToInventory(clothe);
        }
    }

    #endregion
}
