using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 管理NPC所有可以集中管理的腳本
/// </summary>
public class NPCManagerScript : MonoBehaviour
{
    /// <summary>
    /// 所有有功能事件的NPC腳本
    /// </summary>
    [Header("這個NPC是否可以對話")]
    public List<NPCFunctionEventScript> NPCList;
    bool isForActionNPCList;
    /// <summary>
    /// 射線偵測圖層
    /// </summary>
    private LayerMask _Lm;
    private void Awake()
    {
        NPCFunctionEventScript[] NPCFunctionEventScriptArray = FindObjectsOfType<NPCFunctionEventScript>();

        if(NPCFunctionEventScriptArray.Length > 0)
            NPCList = NPCFunctionEventScriptArray.ToList();


        _Lm = LayerMask.GetMask(staticPublicObjectsStaticName.HeroLayer);

        isForActionNPCList = true;
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        if (isForActionNPCList)
        {
            NPCListAction();
        }
    }
    /// <summary>
    /// 執行NPC的腳本
    /// </summary>
    private void NPCListAction()
    {
        foreach (var item in NPCList)
        {
            item.PhyOverlapBoxAll(_Lm);
            item.DialogueSystem();
            
            isForActionNPCList = false;
        }
        isForActionNPCList = true;
    }

}
