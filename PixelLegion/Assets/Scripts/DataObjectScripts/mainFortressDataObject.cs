using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1.�D�����_�l���
/// 2.Ū����ƪ���
/// </summary>
[CreateAssetMenu(fileName = "New Main Fortress Data", menuName = "Data Object/Main Fortress Data")]
public class mainFortressDataObject : ScriptableObject
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
    /// <summary>
    /// �h�L�Ͳ��ɶ�
    /// </summary>
    [Header("�h�L�Ͳ��ɶ�")]
    public float soldierProduceTimeMax;
    /// <summary>
    /// �ثe��ܪ��^��
    /// </summary>
    [Header("�ثe��ܪ��^��")]
    public List<Transform> selectedHeroList;
    /// <summary>
    /// �^���Ͳ��ɶ�
    /// �C�@��^���һݭn���ͪ��ɶ��[�`
    /// </summary>
    [Header("�^���Ͳ��ɶ�")]
    public float heroProduceTimeMax;
}
