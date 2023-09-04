using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    #region Variables

    [Header("Money")]
    [SerializeField] private int startingMoney;
    [SerializeField] private ClotheData startingClothe;

    #endregion

    #region Proprieties

    public int StartingMoney => startingMoney;
    public ClotheData StartingClothe => startingClothe;

    #endregion

}
