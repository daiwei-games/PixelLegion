using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1.�^���ӧO�_�l���
/// 2.Ū����Ʈw�N�^���ӧO����
/// </summary>
[CreateAssetMenu(fileName = "New Hero Data", menuName = "Data Object/Hero Data")]
public class heroDataObject : ScriptableObject
{
    /// <summary>
    /// �Ͳ��ɶ�
    /// </summary>
    [Header("�Ͳ��ɶ�")]
    public float produceTime;
    /// <summary>
    /// �^���w�
    /// </summary>
    [Header("�^���w�")]
    public Transform heroPrefab;
    /// <summary>
    /// �^���W��
    /// </summary>
    [Header("�^���W��")]
    public string heroName;
}
