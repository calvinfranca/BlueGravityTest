using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [SerializeField] private PlayerData data;

    private int _currentMoney;
    private List<ClotheData> _clothesInInventory = new List<ClotheData>();
    private SpriteRenderer _spriteRenderer;

    #endregion

    #region Proprieties

    public List<ClotheData> ClothesInInventory => _clothesInInventory;
    public int CurrentMoney => _currentMoney;

    #endregion

    #region Messages

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentMoney = data.StartingMoney;
        _clothesInInventory.Add(data.StartingClothe);
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

    public void ChangeClothes(Sprite newSprite)
    {
        _spriteRenderer.sprite = newSprite;
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
