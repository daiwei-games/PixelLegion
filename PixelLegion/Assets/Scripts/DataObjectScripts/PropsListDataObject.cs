using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Props Data", menuName = "Data Object/Props Data")]
public class PropsListDataObject : ScriptableObject
{
    #region �D��M��
    /// <summary>
    /// �Ҧ��D��M��
    /// </summary>
    [Header("�Ҧ��D��M��")]
    public List<PropsDataScript> AllProps;
    /// <summary>
    /// �w�g�֦����D��M�� (�I�])
    /// </summary>
    [Header("�w�g�֦����D��M�� (�I�])")]
    public List<PropsBackpackDataScript> AlreadyHaveProps;

    /// <summary>
    /// �w�����D���Ų
    /// </summary>
    [Header("�w�����D���Ų")]
    public List<PropsDataScript> AlreadyKnowProps;

    /// <summary>
    /// ����
    /// </summary>
    [Header("����")]
    public int Money;
    /// <summary>
    /// �ޯ�g���
    /// </summary>
    [Header("�w�g��o���ޯ�g���")]
    public int AlreadyHaveSkillExp;
    #endregion
}
