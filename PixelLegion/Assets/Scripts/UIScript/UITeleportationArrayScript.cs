using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 傳送陣
/// </summary>
public class UITeleportationArrayScript : MonoBehaviour
{
    private AsyncOperation Ao;

    private void Awake()
    {
        
    }
    /// <summary>
    /// 傳送至哪個場景
    /// </summary>
    /// <param name="_sceneName">場景名稱</param>
    public void TeleportationScene(string _sceneName)
    {
        Ao = SceneManager.LoadSceneAsync(_sceneName);
        Ao.allowSceneActivation = true;
    }
}
