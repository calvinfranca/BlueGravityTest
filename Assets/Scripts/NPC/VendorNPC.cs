using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorNPC : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject interactTextObject;
    [SerializeField] private GameObject backgroundOptionsObject;
    [SerializeField] private GameObject chatOptionsObject;
    [SerializeField] private GameObject sellOptionObject;
    [SerializeField] private GameObject buyOptionObject;

    private bool _waitingForInput = false;
    private PlayerController _playerController;

    #endregion

    #region Messages

    private void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(!_waitingForInput) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }
    }

    #endregion

    #region Methods

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        interactTextObject.SetActive(true);
        _waitingForInput = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        interactTextObject.SetActive(false);
        _waitingForInput = false;
    }

    private void StartInteraction()
    {
        interactTextObject.SetActive(false);
        backgroundOptionsObject.SetActive(true);
        chatOptionsObject.SetActive(true);
        _waitingForInput = false;
    }
    
    private void StopInteraction()
    {
        interactTextObject.SetActive(false);
        backgroundOptionsObject.SetActive(true);
        chatOptionsObject.SetActive(true);
    }

    public void BuyOptionSelected()
    {
        chatOptionsObject.SetActive(false);
        buyOptionObject.SetActive(true);
        _playerController.OnEscPressed += OptionSelected;
    }
    
    public void SellOptionSelected()
    {
        chatOptionsObject.SetActive(false);
        sellOptionObject.SetActive(true);
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

    #endregion
}
