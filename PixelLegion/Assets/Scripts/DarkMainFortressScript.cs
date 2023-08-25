using Assets.Scripts;
using Assets.Scripts.BaseClass;
using Assets.Scripts.IFace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �ĤH�D��
/// �t�O�b�S���I�s���a���
/// �S���I�s�D����ơA�ӬO�I�s���O darkMainFortressDataObject
/// </summary>
public class DarkMainFortressScript : MainFortressBaseScript
{
    private void Awake()
    {
        MainFortressDataInitializ();

    }
    public override void MainFortressDataInitializ()
    {
        _transform = transform; // ���o����transform
        _gameObject = gameObject; // ���o����gameobject
        _gameManager = GameObject.Find("GameManager"); // ���o�C���޲z��
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>(); // ���o�C���޲z���}��
            _gameManagerScript._darkMainFortressScript = GetComponent<DarkMainFortressScript>();
        }


        _hp = _darkMainFortressDataObject.maxhp; // ���o�D����q
        _soldierCount = _darkMainFortressDataObject.soldierCount; // ���o�D���L��

        TextMeshPro[] TextMeshProArray = _transform.GetComponentsInChildren<TextMeshPro>(); // ���o�Ҧ��l����TextMeshPro
        foreach (var item in TextMeshProArray)
        {
            switch (item.name)
            {
                case staticPublicObjectsStaticName.MainFortressHpObjectName:
                    _hpMeshPro = item; // ���o�D����q��r����
                    break;
                case staticPublicObjectsStaticName.MainFortressSoldierObjectName:
                    _soldierCountMeshPro = item; // ���o�D���L�Ƥ�r����
                    break;
            }
        }

        MainFortressHpTextMeshPro(); // ��s�D����q��r
        MainForTressSoldierCountTextMeshPro(); // ��s�D���L�Ƥ�r
    }
    public override void MainFortressHpTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_hpMeshPro == null) return;
        _hpMeshPro.text = $"HP: {_hp} /{_darkMainFortressDataObject.maxhp}";
    }

    public override void MainForTressSoldierCountTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_soldierCountMeshPro == null) return;
        _soldierCountMeshPro.text = $"{_soldierCount}";
    }
}
