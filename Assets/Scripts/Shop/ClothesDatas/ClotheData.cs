using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClotheData", menuName = "Scriptable Objects/ClotheData")]
public class ClotheData : ScriptableObject
{
    #region Variables

    [Header("Money")]
    [SerializeField] private int price;
    [SerializeField] private Sprite clotheSprite;
    [SerializeField] private string clotheName;
    [SerializeField] private string description;

    #endregion

    #region Proprieties

    public int Price => price;
    public Sprite ClotheSprite => clotheSprite;
    public string ClotheName => clotheName;
    public string Description => description;

    #endregion

}