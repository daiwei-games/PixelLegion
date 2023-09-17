using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DuelScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 取得GM script
    /// </summary>
    public GameManager GameManagerScript;
    /// <summary>
    /// 清單數量已經OK
    /// </summary>
    private bool MoveListCountIsOK;
    /// <summary>
    /// 玩家的動作清單
    /// </summary>
    [Header("動作清單")]
    private List<HeroState> PlayerMoveList;
    /// <summary>
    /// 敵人的動作清單
    /// </summary>
    private List<HeroState> EnemyMoveList;
    /// <summary>
    /// 動作數量
    /// </summary>
    [Header("動作數量")]
    [SerializeField]
    private int MoveListMax;
    /// <summary>
    /// 玩家的動作是否完成
    /// </summary>
    private bool PlayerMoveIsOK;
    /// <summary>
    /// 敵人的動作是否完成
    /// </summary>
    private bool EnemyMoveIsOK;
    /// <summary>
    /// 是否正式決鬥
    /// </summary>
    private bool IsItPossibleToformalDuel;
    /// <summary>
    /// 是否決鬥已結束
    /// 執行一次協程
    /// </summary>
    private bool IsEndDuelFunc;
    /// <summary>
    /// 動作索引
    /// </summary>
    private int MoveIndex;
    /// <summary>
    /// 玩家英雄
    /// </summary>
    [Header("英雄")]
    [SerializeField]
    private HeroScript PlayerHero;
    private Transform PlayerHeroTf;
    private Rigidbody2D PlayerHeroRb;
    /// <summary>
    /// 敵人英雄
    /// </summary>
    [SerializeField]
    private HeroScript EnemyHero;
    private Transform EnemyHeroTf;
    private Rigidbody2D EnemyHeroRb;
    /// <summary>
    /// 玩家動畫控制器
    /// </summary>
    private Animator PlayerAnimator;
    /// <summary>
    /// 敵人動畫控制器
    /// </summary>
    private Animator EnemyAnimator;
    /// <summary>
    /// 倒數秒數
    /// </summary>
    private int Seconds;
    /// <summary>
    /// 最大秒數
    /// </summary>
    [Header("最大秒數"), SerializeField]
    private int SecondsMax;
    /// <summary>
    /// 倒數計時文字物件
    /// </summary>
    [Header("倒數計時文字物件"), SerializeField]
    private TextMeshPro CountdownText;
    private GameObject _CountdownTextObj;
    /// <summary>
    /// 停止倒數計時協程 CountdownAction;
    /// </summary>
    private Coroutine StopCountdownAction;
    /// <summary>
    /// 玩家總傷害
    /// </summary>
    private int PlayerHitTotal;
    /// <summary>
    /// 敵人總傷害
    /// </summary>
    private int EnemyHitTotal;
    /// <summary>
    /// 當動畫出現Bug的時候多少秒會直接跳過
    /// </summary>
    [Header("當動畫出現Bug的時候多少秒會直接跳過")]
    public float bugTimeMax;
    private float bugTime;

    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;

        _CountdownTextObj = CountdownText.gameObject;
        _CountdownTextObj.SetActive(false);

        Seconds = SecondsMax;
        MoveListCountIsOK = false;
        GameManagerScript = FindObjectOfType<GameManager>();
        if (GameManagerScript != null)
            GameManagerScript.duelScript = this;

        IsItPossibleToformalDuel = false;
        IsEndDuelFunc = true;

        PlayerHitTotal = 0;
        EnemyHitTotal = 0;
    }

    public void HeroData(HeroScript _playerHeroScript, HeroScript _enemyHeroScript)
    {
        if (_playerHeroScript == null || _enemyHeroScript == null) return; // 如果其中一方是null就不執行下面的程式碼
        PlayerHero = _playerHeroScript;
        PlayerHero.IsItPossibleToDuel = true;
        PlayerAnimator = PlayerHero._animator;
        PlayerAnimator.speed = 1f; // 玩家動畫速度
        PlayerHeroTf = PlayerHero._Tf;
        PlayerHeroRb = PlayerHero._rg;
        PlayerMoveList = new List<HeroState>();
        PlayerMoveIsOK = true;

        Vector2 _playerPos = PlayerHero._tfposition;
        _playerPos.y = 0;
        CountdownText.transform.position = _playerPos;

        EnemyHero = _enemyHeroScript;
        PlayerHero.IsItPossibleToDuel = true;
        EnemyAnimator = EnemyHero._animator;
        EnemyAnimator.speed = 1f; // 敵人動畫速度
        EnemyHeroTf = EnemyHero._Tf;
        EnemyHeroRb = EnemyHero._rg;
        EnemyMoveIsOK = true;

        // 自動新增敵人的動作
        HeroState[] _hState = new HeroState[3];
        _hState[0] = HeroState.Attack;
        _hState[1] = HeroState.Def;
        _hState[2] = HeroState.Attack;
        EnemyMoveList = new List<HeroState>();
        for (int i = 0; i < MoveListMax; i++)
        {
            EnemyMoveList.Add(_hState[Random.Range(0, _hState.Length)]);
        }
        _CountdownTextObj.SetActive(true); //開啟倒數計時文字物件
        GameManagerScript.OpenDuelUI(); //開啟對決UI
        StopCountdownAction = StartCoroutine(CountdownAction(CountdownText)); //開啟倒數計時協程
    }
    /// <summary>
    /// 點擊動作按鈕
    /// </summary>
    /// <param name="_hState">HeroState 狀態</param>
    public void PlayerMoveClick(HeroState _hState)
    {
        // 動作未完成時可以取得動作
        if (!MoveListCountIsOK)
        {
            if (PlayerMoveList.Count < MoveListMax)
                PlayerMoveList.Add(_hState);
        }

        // 當動作清單數量等於13時，表示動作已經選擇完成
        if (PlayerMoveList.Count == MoveListMax)
        {
            MoveListCountIsOK = true; //動作清單數量已經OK
            StopCoroutine(StopCountdownAction); //取消倒數計時協程
            _CountdownTextObj.SetActive(false); //關閉倒數計時文字物件
            GameManagerScript.CloseDuelUI(); //關閉對決UI
            MoveIndex = 0; //動作索引歸零
        }
    }

    /// <summary>
    /// 玩家動作按鈕 AUTO 自動完成
    /// </summary>
    public void PlayerMoveClick()
    {
        // 自動新增玩家的動作
        HeroState[] _hState = new HeroState[3];
        _hState[0] = HeroState.Attack;
        _hState[1] = HeroState.Def;
        _hState[2] = HeroState.Attack;
        while (PlayerMoveList.Count < MoveListMax)
        {
            PlayerMoveClick(_hState[Random.Range(0, _hState.Length)]);
        }
    }
    /// <summary>
    /// 倒計時協程
    /// </summary>
    /// <param name="_text">到計時文字物件</param>
    IEnumerator CountdownAction(TextMeshPro _text)
    {
        _text.text = Seconds.ToString();
        while (Seconds > 0)
        {
            yield return new WaitForSeconds(0.1f);
            if (MoveListCountIsOK)
            {
                GameManagerScript.CloseDuelUI();
                yield break;
            }
            Countdown(_text);
        }
    }
    /// <summary>
    /// 倒數計時
    /// </summary>
    /// <param name="_text">到計時文字物件</param>

    public void Countdown(TextMeshPro _text)
    {
        Seconds -= 1;
        _text.text = ((float)Seconds / 10).ToString();
        if (Seconds == 0)
        {
            _CountdownTextObj.SetActive(false);
            while (PlayerMoveList.Count < MoveListMax)
            {
                PlayerMoveList.Add(HeroState.Idle);
            }
            MoveListCountIsOK = true; //動作清單數量已經OK
            GameManagerScript.CloseDuelUI(); //關閉對決UI
            MoveIndex = 0; //動作索引歸零
            return;
        }
    }

    /// <summary>
    /// 進行動作，移動判斷方向、位置並且到定點之後，開始攻防
    /// </summary>
    public void MoveAction()
    {
        if (!MoveListCountIsOK) return;
        if (IsEndDuelFunc)
            HerosDistance();

        FormalDuel();

    }
    /// <summary>
    /// 英雄距離、移動、方校矯正
    /// </summary>
    public void HerosDistance()
    {
        // 雙方英雄距離多遠
        float _distance = Vector2.Distance(PlayerHeroTf.position, EnemyHeroTf.position);
        if (_distance <= 6)
        { //開始動作
            GameManagerScript.CorrectionDirection(PlayerHeroTf, EnemyHeroTf); // 矯正方向
            GameManagerScript.CorrectionDirection(EnemyHeroTf, PlayerHeroTf); // 矯正方向
            IsItPossibleToformalDuel = true;
            if (_distance < 1.5f)
            {
                // 當距離過近，舊執行走路或衝刺，讓AI矯正位置
                IsItPossibleToformalDuel = false;
                HeroRun_DashOrMiss(PlayerHero, PlayerHeroTf, EnemyHeroTf, false, true);
                HeroRun_DashOrMiss(EnemyHero, EnemyHeroTf, PlayerHeroTf, false, true);
                return;
            }
        }
        else if (_distance > 30) // 超大距離
        {
            HeroRun_DashOrMiss(PlayerHero, PlayerHeroTf, EnemyHeroTf); // 走路或衝刺
            HeroRun_DashOrMiss(EnemyHero, EnemyHeroTf, PlayerHeroTf); // 走路或衝刺
        }
        else if (_distance > 6) // 距離不算遠
        {
            // 執行走路
            HeroRun_DashOrMiss(PlayerHero, PlayerHeroTf, EnemyHeroTf, false);
            HeroRun_DashOrMiss(EnemyHero, EnemyHeroTf, PlayerHeroTf, false);
        }
    }
    /// <summary>
    /// 亂數出現 false 讓英雄可以活動並互換位置
    /// </summary>
    private bool RandomHeroState()
    {
        bool[] g = new bool[10];
        for (int i = 0; i < g.Length; i++)
        {
            g[i] = true;
            if (i % 3 == 1) g[i] = false;
        }
        return g[Random.Range(0, g.Length)];
    }

    /// <summary>
    /// 判斷閃現、衝刺、跑步
    /// </summary>
    /// <param name="_heroScript">英雄腳本</param>
    /// <param name="_tf">玩家Transform</param>
    /// <param name="_EnemyTf">敵人Transform</param>
    /// <param name="isRun">是否為只要跑步</param>
    private void HeroRun_DashOrMiss(HeroScript _heroScript, Transform _tf, Transform _EnemyTf, bool isRun = true, bool reverse = false)
    {
        Vector2 _Direction = GameManagerScript.CorrectionDirection(_tf, _EnemyTf); ;
        
        if (reverse)
            _Direction = -_Direction;

        if (!isRun)
        {
            
            return;
        }

        bool IsMiss = _heroScript.isMiss0 && _heroScript.isMiss1;
        bool IsDash = _heroScript.isDash;
        if (IsMiss) // 有閃現
        {
            
        }
        else if (IsDash) // 有衝刺
        {
            
        }
        else // 玩家沒有閃現跟衝刺
        {

        }
    }
    /// <summary>
    /// 正式開始決鬥
    /// </summary>
    public void FormalDuel()
    {
        if (!IsItPossibleToformalDuel) return; //如果不可以進入決鬥，直接跳出
        if (PlayerMoveIsOK && EnemyMoveIsOK) //雙方都可以進入動作時間
        {
            bugTime = 0; //計算bug時間歸零
            if (EnemyMoveList.Count == MoveIndex) //動作清單索引已經跟數量一樣
            {
                PlayerMoveList.Clear(); //清空動作清單
                EnemyMoveList.Clear(); //清空動作清單
                MoveIndex = 0; //動作索引歸零
                PlayerAnimator.speed = 1; //玩家動畫速度歸零
                EnemyAnimator.speed = 1; //敵人動畫速度歸零

                DuelEnd(PlayerHero, EnemyHero); //決鬥結束
                return;
            }
            IsItPossibleToformalDuel = RandomHeroState(); //亂數布林值出現
            if (!IsItPossibleToformalDuel) //如果為 false 不進入清單的動作，開始互相交換位置
            {
                HeroRun_DashOrMiss(PlayerHero, PlayerHeroTf, EnemyHeroTf); //玩家移動
                HeroRun_DashOrMiss(EnemyHero, EnemyHeroTf, PlayerHeroTf); //敵人移動
                return;
            }
            PlayerAnimator.speed = 2; //玩家動畫速度變兩倍
            HeroState pState = PlayerMoveList[MoveIndex];
            PlayerHero.HeroDuelStateFunc(pState);
            if (pState == HeroState.Attack) {
                PlayerHitTotal += PlayerHero.Attack;
                PlayerHero.HeroAtkTarget(EnemyHero, PlayerHero.Attack);
            } 
            PlayerMoveIsOK = false; //玩家動作未完成

            EnemyAnimator.speed = 2; //敵人動畫速度變兩倍
            HeroState eState = EnemyMoveList[MoveIndex];
            EnemyHero.HeroDuelStateFunc(eState);
            if (eState == HeroState.Attack) EnemyHero.HeroAtkTarget(PlayerHero, EnemyHero.Attack);
            EnemyMoveIsOK = false; //敵人動作未完成

            MoveIndex++;
        }
        float playerNormalizedTime = PlayerHero.AnimatorStateInfoNormalizedTime(); //玩家動畫正規化時間
        if (playerNormalizedTime > 0.85f) PlayerMoveIsOK = true; //玩家動作完成

        float enemyNormalizedTime = EnemyHero.AnimatorStateInfoNormalizedTime(); //敵人動畫正規化時間
        if (enemyNormalizedTime > 0.85f) EnemyMoveIsOK = true; //敵人動作完成
        if (!PlayerMoveIsOK || !EnemyMoveIsOK)
        {
            bugTime += Time.deltaTime; //如果玩家或敵人動作未完成，bugTime 累加
            if (bugTime >= bugTimeMax)
            {
                bugTime = 0; //bugTime 歸零
                PlayerMoveIsOK = true; //玩家動作完成
                EnemyMoveIsOK = true; //敵人動作完成
                PlayerHero.animatorPlay("", -1, 0);
                EnemyHero.animatorPlay("", -1, 0);
            }
        }
    }
    /// <summary>
    /// 決鬥結束
    /// </summary>
    /// <param name="Player">玩家英雄腳本</param>
    /// <param name="Enemy">敵人英雄腳本</param>
    private void DuelEnd(HeroScript Player, HeroScript Enemy)
    {
        if (!IsEndDuelFunc) return; //顯示不再可以進入決鬥結束的函數
        StartCoroutine(RetreatOrDie(Player, Enemy, Player._Tf, Enemy._Tf)); //開始退場或死亡
        IsEndDuelFunc = false; //false 不再可以進入決鬥結束的函數
    }
    /// <summary>
    /// 退場或死亡
    /// </summary>
    /// <param name="hero">玩家英雄腳本</param>
    /// <param name="enemy">敵人英雄腳本</param>
    /// <param name="_Tf">玩家Transform</param>
    /// <param name="_enemyTf">敵人Transform</param>
    IEnumerator RetreatOrDie(HeroScript hero, HeroScript enemy, Transform _Tf, Transform _enemyTf)
    {
        if (!IsEndDuelFunc) yield break; //顯示不再可以進入決鬥結束的函數
        hero._animator.speed = 0.5f; //玩家動畫速度變成0.5倍
        HeroState hState = HeroState.Attack; //玩家動作狀態為攻擊
        if (hero.Hp <= 0) hState = HeroState.Die; //如果玩家血量小於等於0，玩家動作狀態為死亡
        hero.HeroDuelStateFunc(hState); //玩家動作狀態

        hState = HeroState.Attack; //敵人動作狀態為攻擊
        if (enemy.Hp <= 0) hState = HeroState.Die; //如果敵人血量小於等於0，敵人動作狀態為死亡
        enemy.HeroDuelStateFunc(hState); //敵人動作狀態

        Vector3 _DirectionPlayer = _Tf.position; //玩家位置
        if (_Tf.localScale.x < 0) _DirectionPlayer.x += 10; //玩家面向左邊，玩家後退位置加10
        if (_Tf.localScale.x > 0) _DirectionPlayer.x -= 10; //玩家面向右邊，玩家後退位置減10

        Vector3 _DirectionEnemy = _enemyTf.position; //敵人位置
        if (_enemyTf.localScale.x < 0) _DirectionEnemy.x += 10; //敵人面向左邊，敵人後退位置加10
        if (_enemyTf.localScale.x > 0) _DirectionEnemy.x -= 10; //敵人面向右邊，敵人後退位置減10

        for (int i = 0; i < 20; i++) 
        {
            IsEndDuelFunc = false; 
            if (hero.Hp <= 0) 
                //玩家血量小於等於0，玩家位置往後飛越
                _Tf.position = Vector3.Slerp(_Tf.position, _DirectionPlayer, 0.5f);
            else
                //玩家血量大於0，玩家位置往後移動
                _Tf.position = Vector2.MoveTowards(_Tf.position, _DirectionPlayer, 0.5f);

            if (enemy.Hp <= 0)
                //敵人血量小於等於0，敵人位置往後飛越
                _enemyTf.position = Vector3.Slerp(_enemyTf.position, _DirectionEnemy, 0.5f);
            else
                //敵人血量大於0，敵人位置往後移動
                _enemyTf.position = Vector2.MoveTowards(_enemyTf.position, _DirectionEnemy, 0.5f);
            yield return new WaitForSeconds(0.05f);
        }
        hero._animator.speed = 1; //玩家動畫速度變成1倍
        DuelFormat(); //格式化資料
    }
    /// <summary>
    /// 格式化資料
    /// </summary>
    public void DuelFormat()
    {
        PlayerHero.IsItPossibleToDuel = false;
        PlayerAnimator.speed = 1;
        PlayerHero = null;
        PlayerAnimator = null;
        PlayerHeroTf = null;
        PlayerMoveList.Clear();
        PlayerHitTotal = 0;

        EnemyHero.IsItPossibleToDuel = false;
        EnemyAnimator.speed = 1;
        EnemyHero = null;
        EnemyAnimator = null;
        EnemyHeroTf = null;
        EnemyMoveList.Clear();
        EnemyHitTotal = 0;

        MoveListCountIsOK = false;
        PlayerMoveIsOK = false;
        EnemyMoveIsOK = false;

        Seconds = SecondsMax;
        IsEndDuelFunc = true;
        MoveIndex = 0;
        StopCoroutine(StopCountdownAction);
        _CountdownTextObj.SetActive(false);
        GameManagerScript.Duel();
    }
}
