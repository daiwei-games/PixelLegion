using Assets.Scripts;
using Assets.Scripts.BaseClass;
using Assets.Scripts.IFace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 敵人主堡
/// 差別在沒有呼叫玩家資料
/// 沒有呼叫主堡資料，而是呼叫的是 darkMainFortressDataObject
/// </summary>
public class DarkMainFortressScript : MainFortressBaseScript
{
    /// <summary>
    /// 主堡資料物件
    /// </summary>
    public darkMainFortressDataObject _mainFortressObj;
    /// <summary>
    /// 敵人主堡tag
    /// </summary>
    public string enemyMainFortressTag;
    /// <summary>
    /// 士兵 tag
    /// </summary>
    public string soldierTag;
    private void Awake()
    {
        MainFortressDataInitializ();

    }
    public override void MainFortressDataInitializ()
    {
        _transform = transform; // 取得物件transform
        _gameObject = gameObject; // 取得物件gameobject
        _mainFortressObj = _darkMainFortressDataObject;
        enemyMainFortressTag = staticPublicObjectsStaticName.MainFortressTag; // 取得敵人主堡tag
        soldierTag = staticPublicObjectsStaticName.DARKSoldierTag; // 取得士兵tag
        _gameManager = GameObject.Find("GameManager"); // 取得遊戲管理器
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>(); // 取得遊戲管理器腳本
            _gameManagerScript._darkMainFortressScript.Add(GetComponent<DarkMainFortressScript>());
            _gameManagerScript._mainFortressScriptList.Add(GetComponent<DarkMainFortressScript>());
        }
        _gameObject.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.DarkMainFortressLayer); // 設定主堡圖層
        _mfEnemyLayerMask = LayerMask.GetMask(staticPublicObjectsStaticName.PlayerSoldierLayer,
                    staticPublicObjectsStaticName.HeroLayer,
                    staticPublicObjectsStaticName.MainFortressLayer); // 設定主堡敵人圖層
        _hp = _mainFortressObj.maxhp; // 取得主堡血量
        _soldierCount = _mainFortressObj.soldierCount; // 取得主堡兵數

        TextMeshPro[] TextMeshProArray = _transform.GetComponentsInChildren<TextMeshPro>(); // 取得所有子物件的TextMeshPro
        for (int i = 0; i < TextMeshProArray.Length; i++)
        {
            switch (TextMeshProArray[i].name)
            {
                case staticPublicObjectsStaticName.MainFortressHpObjectName:
                    _hpMeshPro = TextMeshProArray[i]; // 取得主堡血量文字物件
                    break;
                case staticPublicObjectsStaticName.MainFortressSoldierObjectName:
                    _soldierCountMeshPro = TextMeshProArray[i]; // 取得主堡兵數文字物件
                    break;
            }
        }
        MainFortressHpTextMeshPro(); // 更新主堡血量文字
        MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字
        selectedSoldierList = _mainFortressObj.soldierSelectedList; // 取得已選擇士兵清單
        soldierProduceTime = _mainFortressObj.soldierProduceTimeMax; // 取得士兵產生時間
        soldierProduceTimeNow = soldierProduceTime;


        GetEnemyMainFortress();
    }
    public override void MainFortressHpTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_hpMeshPro == null) return;
        _hpMeshPro.text = $"HP: {_hp} /{_mainFortressObj.maxhp}";
    }

    public override void MainForTressSoldierCountTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_soldierCountMeshPro == null) return;
        _soldierCountMeshPro.text = $"{_soldierCount}";
    }
    /// <summary>
    /// 產生士兵
    /// </summary>
    public override void ProduceSoldier()
    {
        if (_soldierCount <= 0) return;
        if (_gameManagerScript._soldierList.Count >= 300) return; // 士兵數量超過300不產生
        if (soldierProduceTimeNow >= soldierProduceTime)
        {
            //產生士兵
            Vector3 ParentPosition = _transform.position;
            ParentPosition.y += 1;
            Vector2 ParentScale;
            Transform InstantiateTransform;
            SoldierScript _soldierScript;
            if (enemyMainFortressList.Count > 0)
            {
                for (int i = 0; i < enemyMainFortressList.Count; i++)
                {
                    if (enemyMainFortressList[i] == null)
                    {
                        enemyMainFortressList.RemoveAt(i);
                        return;
                    }
                    InstantiateTransform = Instantiate(selectedSoldierList[Random.Range(0, selectedSoldierList.Count)], ParentPosition, Quaternion.identity);
                    InstantiateTransform.tag = soldierTag;
                    InstantiateTransform.gameObject.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.DarkSoldierLayer);
                    ParentScale = InstantiateTransform.localScale; // 取得士兵的Scale
                    if (enemyMainFortressList[i].position.x < _transform.position.x) ParentScale.x *= -1; // 判斷敵人主堡的位置，決定士兵的Scale
                    InstantiateTransform.localScale = ParentScale; // 更新士兵的Scale

                    _soldierScript = InstantiateTransform.GetComponent<SoldierScript>();
                    _soldierScript._enemyLayerMask = _mfEnemyLayerMask; // 取得敵人圖層
                    _soldierScript._enemyNowMainFortress = enemyMainFortressList[i];
                    _soldierScript._gameManagerScript = _gameManagerScript;
                    _soldierScript._soldierScript = _soldierScript;
                    _gameManagerScript._soldierList.Add(_soldierScript);//將資料存入玩家士兵清單

                    _soldierCount -= 1; // 士兵數量減少
                }

            }
            soldierProduceTimeNow = 0; // 重置士兵產生時間
            MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字
        }
        if (soldierProduceTimeNow < soldierProduceTime)
        {
            soldierProduceTimeNow += Time.deltaTime;
        }
    }
    public override void MainFortressHit(int hit)
    {
        if (_hp <= 0) return;
        _hp -= hit;
        MainFortressHpTextMeshPro();
        if (_hp <= 0)
        {

        }

    }
    /// <summary>
    /// 取得敵人主堡
    /// </summary>
    public override void GetEnemyMainFortress()
    {
        GameObject[] enemyMainFortressArray = GameObject.FindGameObjectsWithTag(enemyMainFortressTag); // 取得敵人主堡清單
        if (enemyMainFortressArray.Length > 0)
        {
            foreach (var item in enemyMainFortressArray)
            {
                enemyMainFortressList.Add(item.transform);
            }
        }
    }
}
