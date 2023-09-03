using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopkeeperNPC : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject interactTextObject;
    [SerializeField] private GameObject backgroundOptionsObject;
    [SerializeField] private GameObject chatOptionsObject;
    [SerializeField] private GameObject sellOptionObject;
    [SerializeField] private GameObject buyOptionObject;

    private PlayerController _playerController;

    #endregion

    #region Messages

    private void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    #endregion

    #region Methods

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player")) return;
        
        interactTextObject.SetActive(true);
        _playerController.OnEPressed += StartInteraction;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        
        interactTextObject.SetActive(false);
        _playerController.OnEPressed -= StartInteraction;
    }

    private void StartInteraction()
    {
        interactTextObject.SetActive(false);
        backgroundOptionsObject.SetActive(true);
        OptionSelected();
        
        _playerController.ToggleMoveInputs(false);
        _playerController.OnEPressed -= StartInteraction;
    }
    
    private void StopInteraction()
    {
        CloseAllChatObjects();
        interactTextObject.SetActive(true);
        
        _playerController.ToggleMoveInputs(true);
        _playerController.OnEscPressed -= StopInteraction;
        _playerController.OnEPressed += StartInteraction;
    }

    public void BuyOptionSelected()
    {
        chatOptionsObject.SetActive(false);
        buyOptionObject.SetActive(true);
        
        _playerController.OnEscPressed -= StopInteraction;
        _playerController.OnEscPressed += OptionSelected;
    }
    
    public void SellOptionSelected()
    {
        chatOptionsObject.SetActive(false);
        sellOptionObject.SetActive(true);
        
        _playerController.OnEscPressed -= StopInteraction;
        _playerController.OnEscPressed += OptionSelected;
    }
    
    private void OptionSelected()
    {
        buyOptionObject.SetActive(false);
        sellOptionObject.SetActive(false);
        chatOptionsObject.SetActive(true);
        
        _playerController.OnEscPressed -= OptionSelected;
        _playerController.OnEscPressed += StopInteraction;
    }

    private void CloseAllChatObjects()
    {
        buyOptionObject.SetActive(false);
        sellOptionObject.SetActive(false);
        chatOptionsObject.SetActive(false);
        backgroundOptionsObject.SetActive(false);
    }

    #endregion
}
