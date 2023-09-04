using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [SerializeField] private PlayerData data;

    private int _currentMoney;
    private ClotheData _currentClothe;
    private List<ClotheData> _clothesInInventory = new List<ClotheData>();

    #endregion

    #region Proprieties

    public List<ClotheData> ClothesInInventory => _clothesInInventory;
    public int CurrentMoney => _currentMoney;
    public ClotheData CurrentClothe => _currentClothe;

    #endregion

    #region Messages

    private void Awake()
    {
        _currentMoney = data.StartingMoney;
        _clothesInInventory.Add(data.StartingClothe);
        _currentClothe = data.StartingClothe;
    }

    #endregion

    #region Methods

    public void SpendMoney(int value)
    {
        _currentMoney -= value;
        if (_currentMoney < 0)
            _currentMoney = 0;
    }
    
    public void ReceiveMoney(int value)
    {
        _currentMoney += value;
    }

    public void ChangeClothes(ClotheData clothe)
    {
        _currentClothe = clothe;
    }

    public void AddClotheToInventory(ClotheData clothe)
    {
        if(!_clothesInInventory.Contains(clothe))
            _clothesInInventory.Add(clothe);
    }
    
    public void RemoveClotheFromInventory(ClotheData clothe)
    {
        if(_clothesInInventory.Contains(clothe))
            _clothesInInventory.Remove(clothe);
    }

    #endregion
}
