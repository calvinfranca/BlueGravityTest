using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [SerializeField] private PlayerData data;

    private int _currentMoney;
    
    private SpriteRenderer _spriteRenderer;

    #endregion

    #region Messages

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.StartSprite;
        _currentMoney = data.StartingMoney;
    }

    #endregion

    #region Methods

    public void SpendMoney(int value)
    {
        _currentMoney -= value;
        if (_currentMoney < 0)
            _currentMoney = 0;
    }

    public void ChangeClothes(Sprite newSprite)
    {
        _spriteRenderer.sprite = newSprite;
    }

    #endregion
}
