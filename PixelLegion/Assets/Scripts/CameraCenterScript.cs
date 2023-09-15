using UnityEngine;

public class CameraCenterScript : MonoBehaviour
{

    public Transform _tf;
    /// <summary>
    /// 攝影機
    /// </summary>
    public Camera _camera;
    public Transform _cameraTf;
    /// <summary>
    /// 遊戲管理器
    /// </summary>
    public GameManager GameManagerScript;
    public Transform PlayerTf;
    public LayerMask playerLayerMask;
    private void Awake()
    {
        _tf = transform;
        _camera = Camera.main;
        _cameraTf = _camera.transform;
        GameManagerScript = FindObjectOfType<GameManager>();
        if (GameManagerScript != null)
        {
            GameManagerScript.CameraCenterScript = this;
        }
    }

    public void GotoPlyer()
    {
        Vector3 CamreaPos = _tf.position;
        if (PlayerTf == null) return;
        if (_camera == null) return;
        if (Vector3.Distance(PlayerTf.position, _tf.position) > 1)
        {
            CamreaPos = Vector3.MoveTowards(_tf.position, PlayerTf.position, .8f);
            CamreaPos.y = 0;
            CamreaPos.z = -10;
            _tf.position = CamreaPos;
            _cameraTf.position = CamreaPos;
        }
    }


}
