using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClotheData", menuName = "Scriptable Objects/ClotheData")]
public class ClotheData : ScriptableObject
{
    #region Variables

    [Header("Money")]
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;
    [SerializeField] private Sprite clotheSprite;
    [SerializeField] private string clotheName;
    [SerializeField] private string description;

    #endregion

    #region Proprieties

    public int BuyPrice => buyPrice;
    public int SellPrice => sellPrice;
    public Sprite ClotheSprite => clotheSprite;
    public string ClotheName => clotheName;
    public string Description => description;

    #endregion

}