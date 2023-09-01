using Assets.Scripts;
using Assets.Scripts.IFace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IPlayerFunc
{
    [Header("玩家資料")]
    public playerDataObject _playerDataObject;

    private void Awake()
    {
        
    }
    public void PlayerDataInitializ()
    {

    }


}
