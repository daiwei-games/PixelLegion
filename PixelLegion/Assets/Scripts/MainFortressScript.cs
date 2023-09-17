using UnityEngine;
using TMPro;
using Assets.Scripts;
using Assets.Scripts.BaseClass;

public class MainFortressScript : MainFortressBaseScript
{
    /// <summary>
    /// 主堡資料物件
    /// </summary>
    [Header("主堡資料庫")]
    public mainFortressDataObject _mainFortressObj;

    private void Awake()
    {
        MainFortressDataInitializ();

    }
    public override void MainFortressDataInitializ()
    {
        _Tf = transform; // 取得物件transform
        _Go = gameObject; // 取得物件gameobject
        enemyMainFortressTag = staticPublicObjectsStaticName.DarkMainFortressTag; // 取得敵人主堡tag
        soldierTag = staticPublicObjectsStaticName.PlayerSoldierTag; // 取得士兵tag
        _gameManager = GameObject.Find("GameManager"); // 取得遊戲管理器
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>(); // 取得遊戲管理器腳本

            _gameManagerScript._mainFortressScript.Add(this);
            _gameManagerScript._mainFortressScriptList.Add(this);
        }
        _Go.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.MainFortressLayer); // 設定主堡圖層
        _mfEnemyLayerMask = LayerMask.GetMask(staticPublicObjectsStaticName.DarkSoldierLayer,
                        staticPublicObjectsStaticName.DarkHeroLayer,
                        staticPublicObjectsStaticName.DarkMainFortressLayer); // 取得主堡敵人圖層
        _hp = _mainFortressObj.maxhp; // 取得主堡血量
        _soldierCount = _mainFortressObj.soldierCount; // 取得主堡兵數

        TextMeshPro[] TextMeshProArray = _Tf.GetComponentsInChildren<TextMeshPro>(); // 取得所有子物件的TextMeshPro
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

    public override void ProduceSoldier()
    {
        if (_soldierCount <= 0) return;
        if (_gameManagerScript._soldierList.Count >= 200) return; // 士兵數量超過300不產生
        if (soldierProduceTimeNow >= soldierProduceTime)
        {
            //產生士兵
            Vector3 ParentPosition = _Tf.position; // 取得主堡位置
            ParentPosition.y += 1;
            Vector2 ParentScale;
            Transform InstantiateTransform;
            SoldierScript _soldierScript;
            Transform _enemymf;
            if (enemyMainFortressList.Count > 0)
            {
                for (int i = 0; i < enemyMainFortressList.Count; i++)
                {
                    _enemymf = enemyMainFortressList[i];
                    if (_enemymf == null)
                    {
                        enemyMainFortressList.RemoveAt(i);
                        continue;
                    }
                    InstantiateTransform = Instantiate(selectedSoldierList[Random.Range(0, selectedSoldierList.Count)], ParentPosition, Quaternion.identity);
                    InstantiateTransform.tag = soldierTag;
                    InstantiateTransform.gameObject.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.PlayerSoldierLayer);
                    ParentScale = InstantiateTransform.localScale; // 取得士兵的Scale
                    if (_enemymf.position.x < _Tf.position.x) ParentScale.x *= -1; // 判斷敵人主堡的位置，決定士兵的Scale
                    InstantiateTransform.localScale = ParentScale; // 更新士兵的Scale

                    _soldierScript = InstantiateTransform.GetComponent<SoldierScript>();
                    _soldierScript._enemyLayerMask = _mfEnemyLayerMask;
                    _soldierScript._enemyNowMainFortress = _enemymf;
                    if (WhoHitMeTransform.Count > 0) // 如果有被攻擊，就將敵人變成目標
                    {
                        Transform _whotf;
                        for (int whoHitMeIndex = 0; whoHitMeIndex < WhoHitMeTransform.Count; whoHitMeIndex++)
                        {
                            _whotf = WhoHitMeTransform[whoHitMeIndex];
                            if (_whotf == null)
                            {
                                WhoHitMeTransform.RemoveAt(whoHitMeIndex);
                                continue;
                            }
                            if(_whotf != null)
                            {
                                _soldierScript._enemyNowMainFortress = _whotf;
                                break;
                            }
                        }
                    }
                    _soldierScript._gameManagerScript = _gameManagerScript;
                    _soldierScript._soldierScript = _soldierScript;
                    _gameManagerScript._soldierList.Add(_soldierScript);//將資料存入玩家士兵清單

                    _soldierCount -= 1; // 士兵數量減少
                }
            }
            soldierProduceTimeNow = 0; // 重置士兵產生時間
            MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字
        }
    }

    public override void MainFortressHit(int hit)
    {
        if (_hp <= 0) return;
        _hp -= hit;
        MainFortressHpTextMeshPro();

        if (_hp <= 0)
        {
            _gameManagerScript.MainFortressOver(this);
            Destroy(_Go, 1);
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
            for (int i = 0; i < enemyMainFortressArray.Length; i++)
            {
                enemyMainFortressList.Add(enemyMainFortressArray[i].transform);
            }
        }
    }
}
