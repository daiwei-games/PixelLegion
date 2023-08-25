using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 光明主堡腳本
    /// </summary>
    public MainFortressScript _mainFortressScript;
    /// <summary>
    /// 黑暗主堡腳本
    /// </summary>
    public DarkMainFortressScript _darkMainFortressScript;
    /// <summary>
    /// 現在正在操作的角色
    /// </summary>
    public GameObject _nowSteerTheCharacter;
    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (staticPublicGameStopSwitch.gameStop) return;
    }
    private void FixedUpdate()
    {
        if (staticPublicGameStopSwitch.gameStop) return;
    }

}
