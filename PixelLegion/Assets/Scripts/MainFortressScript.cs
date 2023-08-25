using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.IFace;
using Assets.Scripts;
using Assets.Scripts.BaseClass;
using Unity.VisualScripting;

public class MainFortressScript : MainFortressBaseScript
{

    private void Awake()
    {
        MainFortressDataInitializ();

    }
    public override void MainFortressDataInitializ()
    {
        _transform = transform; // 取得物件transform
        _gameObject = gameObject; // 取得物件gameobject
        _gameManager = GameObject.Find("GameManager"); // 取得遊戲管理器
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>(); // 取得遊戲管理器腳本
            _gameManagerScript._mainFortressScript = GetComponent<MainFortressScript>();
        }


        _hp = _mainFortressDataObject.maxhp; // 取得主堡血量
        _soldierCount = _mainFortressDataObject.soldierCount; // 取得主堡兵數

        TextMeshPro[] TextMeshProArray = _transform.GetComponentsInChildren<TextMeshPro>(); // 取得所有子物件的TextMeshPro
        foreach (var item in TextMeshProArray)
        {
            switch (item.name)
            {
                case staticPublicObjectsStaticName.MainFortressHpObjectName:
                    _hpMeshPro = item; // 取得主堡血量文字物件
                    break;
                case staticPublicObjectsStaticName.MainFortressSoldierObjectName:
                    _soldierCountMeshPro = item; // 取得主堡兵數文字物件
                    break;
            }
        }

        MainFortressHpTextMeshPro(); // 更新主堡血量文字
        MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字
    }

    public override void MainFortressHpTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_hpMeshPro == null) return;
        _hpMeshPro.text = $"HP: {_hp} /{_mainFortressDataObject.maxhp}";
    }

    public override void MainForTressSoldierCountTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_soldierCountMeshPro == null) return;
        _soldierCountMeshPro.text = $"{_soldierCount}";
    }

}
