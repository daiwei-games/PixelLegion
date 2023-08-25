using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1.���a�_�l���
/// 2.Ū����Ʈw���s����
/// 3.�ҿ�ܪ��^��
/// </summary>
[CreateAssetMenu(fileName = "New Player Data", menuName = "Data Object/Player Data")]
public class playerDataObject : ScriptableObject
{
    /// <summary>
    /// ���s��ƪ����_
    /// </summary>
    [Header("���a���_")]
    public string PlayerKey;
    /// <summary>
    /// ���a�W��
    /// </summary>
    [Header("���a�W��")]
    public string PlayerName;
    /// <summary>
    /// �w�g��ܪ��^��
    /// </summary>
    [Header("�w�g��ܪ��^��")]
    public List<Transform> SelectedHeroList;
    ///<summary>
    ///���a����
    ///</summary>
    [Header("���a����")]
    public int PlayerCoin;
    ///<summary>
    ///���a�p��
    ///</summary>
    [Header("���a�p��")]
    public int PlayerDiamond;
    ///<summary>
    ///���a��O
    ///</summary>
    [Header("���a��O")]
    public int PlayerEnergy;
}
