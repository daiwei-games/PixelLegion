using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// �����D���}��
    /// </summary>
    public MainFortressScript _mainFortressScript;
    /// <summary>
    /// �·t�D���}��
    /// </summary>
    public DarkMainFortressScript _darkMainFortressScript;
    /// <summary>
    /// �{�b���b�ާ@������
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
