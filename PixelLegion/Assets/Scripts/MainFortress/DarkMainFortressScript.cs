using Assets.Scripts;
using UnityEngine;
using TMPro;

/// <summary>
/// 敵人主堡
/// 差別在沒有呼叫玩家資料
/// 沒有呼叫主堡資料，而是呼叫的是 darkMainFortressDataObject
/// </summary>
public class DarkMainFortressScript : MainFortressScript
{
    private void Start()
    {
        MainFortressDataInitializ();
    }
    public override void MainFortressDataInitializ()
    {
        _Tf = transform; // 取得物件transform
        _Go = gameObject; // 取得物件gameobject
        enemyMainFortressTag = staticPublicObjectsStaticName.MainFortressTag; // 取得敵人主堡tag
        soldierTag = staticPublicObjectsStaticName.DARKSoldierTag; // 取得士兵tag

        _gameManagerScript = FindFirstObjectByType<GameManager>(); // 取得遊戲管理器腳本
        
        _Go.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.DarkMainFortressLayer); // 設定主堡圖層

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

        soldierProduceTime = soldierProduceTimeMax;
        ProduceHeroTime = ProduceHeroTimeMax;
        _gameManagerScript.MainFortressDataFormat(this);
        GetEnemyMainFortress();
    }



    /// <summary>
    /// 產生士兵
    /// </summary>
    public override void ProduceSoldier()
    {
        if (_soldierCount == 0) return;
        if (selectedSoldierList.Count == 0) return;
        if (_gameManagerScript._soldierList.Count >= 200) return; // 士兵數量超過300不產生
        if (soldierProduceTime >= soldierProduceTimeMax)
        {
            //產生士兵
            Vector3 ParentPosition = _Tf.position;
            ParentPosition.y += 1;
            Vector2 ParentScale;
            Transform InstantiateTransform;
            GameObject _go;
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
                    if (_enemymf.CompareTag(staticPublicObjectsStaticName.MainFortressTag)) {
                        _soldierScript = Instantiate(selectedSoldierList[Random.Range(0, selectedSoldierList.Count)], ParentPosition, Quaternion.identity);
                        _go = _soldierScript.gameObject;
                        _go.tag = soldierTag;
                        _go.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.DarkSoldierLayer);
                        InstantiateTransform = _go.transform;
                        ParentScale = InstantiateTransform.localScale; // 取得士兵的Scale
                        if (_enemymf.position.x < _Tf.position.x) ParentScale.x *= -1; // 判斷敵人主堡的位置，決定士兵的Scale
                        InstantiateTransform.localScale = ParentScale; // 更新士兵的Scale
                        _soldierScript._enemyNowMainFortress = _enemymf;
                        _gameManagerScript.SoldierDataFormat(_soldierScript,false);

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
                                if (_whotf != null)
                                {
                                    _soldierScript._enemyNowMainFortress = _whotf;
                                    break;
                                }
                            }
                        }
                        _soldierCount -= 1; // 士兵數量減少
                    }

                }

            }
            soldierProduceTime = 0; // 重置士兵產生時間
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





    
    /// <summary>
    /// 創建英雄
    /// </summary>
    /// <param name="_time">計算產生英雄的時間</param>
    public override void ProduceHero(float _time)
    {
        if (GetHeroList.Count == 0) return; //如果沒有英雄就不執行
        ProduceHeroTime += _time; //計算時間
        if (ProduceHeroTime < ProduceHeroTimeMax) return; //如果時間沒有到就不執行
        ProduceHeroTime = 0; //重置時間

        int HeroListIndex = Random.Range(0, GetHeroList.Count); //隨機選擇英雄
        HeroScript _hero = Instantiate(GetHeroList[HeroListIndex], _Tf.position, Quaternion.identity, null); //產生英雄
        GetHeroList.RemoveAt(HeroListIndex); //將英雄從清單中移除

        _hero.tag = staticPublicObjectsStaticName.DarkHeroTag; //設定英雄tag
        _hero.gameObject.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.DarkHeroLayer); //設定英雄圖層

        _hero.GetEmenyTarget(_gameManagerScript._MainFortressScriptList);
        _gameManagerScript.HeroDataFormat(_hero, false); //設定英雄資料
    }
}
