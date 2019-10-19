using UnityEngine;
using System.Collections;

public class GMMove : MonoBehaviour// ControlGetter
{
    public bool _allowMove;
    public Vector3 _camForward = new Vector3(0, 0, 0);         // перед камеры, без учета поворота по X
                                                               // Vector3 _bodyForward = new Vector3(0, 0, 0);

    public Vector3 _moveDirection = new Vector2(0, 0);     // направление движения

    public Vector3 _camPos = new Vector2(0, 0);
    public GMControlDriver _cd;
    //public Transform _body;
    public BodyPrefsPlayer _body;
    public Transform _camera;
    public BodyPrefsAll _bp;

    void Start()
    {
        _cd = GetComponent<GMControlDriver>();
        _body = _cd._body;
        if (_body) _bp = _body.GetComponent<BodyPrefsAll>();

        _camera = _cd._camera;

    }

    void Update()
    {

        // if (_body == null || _camera == null)
        if (!_body || !_camera) return;

        if (_body != _cd._body)
        {
            _body = _cd._body;
            _bp = _body.GetComponent<BodyPrefsAll>();
        }

        _moveDirection.x = _cd.GetAxis("Horizontal");
        _moveDirection.y = _cd.GetAxis("Vertical");
        // _moveDirection.z = 0;


        //_bp._bodyMove.UpdatePlayer(_moveDirection, _camForward);


    }
    void FixedUpdate()
    {
        if (!_body || !_camera) return;

        CameraPos();

        if (!_allowMove || _bp._currHp <= 0)
        {
            _bp._rigidbody.velocity = new Vector3(0, 0);
        }
        else
        {
            _bp._bodyMove.UpdatePlayer(_moveDirection);
        }
    }
    void CameraPos()
    {
        if (_bp._camera == null)
        {
            _bp._camera = _bp.GetCamera();
            if (_bp._camera == null)
                return;
        }
        _bp._bodyMove.FixCamPos();
        _bp._camera.LookAt(_bp._planet, _bp._camera.up);
    }

    #region abils

    public void AbillBoost(bool boost)
    {
        _bp._bodyMove.Boost(boost);
    }
    public void AbillGBoolet()//(bool boost)
    {
        _bp._bodyMove.GBullet();
    }

    #endregion

}
