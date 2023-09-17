using UnityEngine;

public class CameraCenterScript : LeadToSurviveGameBaseClass
{

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
        _Tf = transform;
        _Go = gameObject;

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
        Vector3 CamreaPos = _Tf.position;
        if (PlayerTf == null) return;
        if (_camera == null) return;
        if (Vector3.Distance(PlayerTf.position, _Tf.position) > 1)
        {
            CamreaPos = Vector3.MoveTowards(_Tf.position, PlayerTf.position, .8f);
            CamreaPos.y = 0;
            CamreaPos.z = -10;
            _Tf.position = CamreaPos;
            _cameraTf.position = CamreaPos;
        }
    }


}
