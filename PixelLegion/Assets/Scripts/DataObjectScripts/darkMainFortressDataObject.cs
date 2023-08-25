using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �t�¥D�����
/// </summary>
[CreateAssetMenu(fileName = "New Dark Main Fortress Data", menuName = "Data Object/Dark Main Fortress Data")]
public class darkMainFortressDataObject : ScriptableObject
{
    /// <summary>
    /// �̤j��q
    /// </summary>
    [Header("�̤j��q")]
    public int maxhp;
    /// <summary>
    /// �̤j�h�L�H��
    /// </summary>
    [Header("�̤j�h�L�H��")]
    public int soldierCount;
    /// <summary>
    /// �w��ܪ��h�L�M��
    /// </summary>
    [Header("�w��ܪ��h�L�M��")]
    public List<Transform> soldierSelectedList;
}
