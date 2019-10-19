using UnityEngine;
using System.Collections;

public class BodyMoveAll : MonoBehaviour
{
  //  [HideInInspector]
    public Vector3 _dir = new Vector3(0, 0);

    //float _startEngineAngle = 30;
    //float _currAngleTargetAndUp = 0;  // угол можду направлением джоя/таргетом и трансворм.ап
    //float _currAcceleration = 0; // локальное ускорение, используется ботами для прогулки или погони

    //Vector2 _randomPatrolTime = new Vector2(3, 10);
    //float _randomPatrolTimer = 0;
    //Vector3 _randomDir = new Vector2(0, 0);

    //bool _boost;                         // внезапное ускорение
    [HideInInspector]
    public BodyPrefsAll _bp;
    [HideInInspector]
    public
    Vector3 _currentVelocity = new Vector2(0, 0);// текущая скорость, относительно положения камеры
    [HideInInspector]
    public
    float _break = 0.5f;

    //Vector3 _dir = new Vector2(0, 0);

    Transform localNotCamera;
    //void Start()
    void Awake()
    {
        _bp = GetComponent<BodyPrefsAll>();
        //_bp = (BodyPrefsAll)GetComponent<BodyPrefsBot>();
        // Prefs p = (Prefs)GetComponent<PrefsBot>();

        _bp._rigidbody = GetComponent<Rigidbody>();

        if (!_bp._rigidbody)
        {
            Debug.LogWarning("на теле " + name + " отсутствует скрипт Rigidbody. Добавлен автоматический.");
            _bp._rigidbody = gameObject.AddComponent<Rigidbody>();
            _bp._rigidbody.useGravity = false;
        }
        // _bp._rigidbody.velocity = new Vector3(0, 0);
        VirtualStart();
    }
    public virtual void VirtualStart() { }
    public void Breaking(float deltaTime)
    {
        // торможение каждый кадр
        //_currentVelocity = Vector3.Lerp(_currentVelocity, new Vector2(0, 0), _break * Time.deltaTime);
        _currentVelocity = Vector3.Lerp(_currentVelocity, new Vector2(0, 0), _break * deltaTime);
    }
    void FixedUpdate()
    {
        if (_bp._controlType == GlobalEnum.ControlType.botSelf)
            UpdateThis(Time.fixedDeltaTime);

        VirtualFixedUpdate();
    }
    public virtual void VirtualFixedUpdate() { }

    public void UpdatePlayer(Vector3 dir)
    {
        UpdateThis(dir);
    }
    public void UpdateBot(float deltaTime)
    {
        if (!_bp) return;
        UpdateThis(deltaTime);
    }
    public virtual void UpdateThis(Vector3 dir) { }
    public virtual void UpdateThis(float deltaTime) { }



    public void FixBodyPos()
    {
        //****************** позиция считается норм
        //Vector3 vBod = (_bp._transform.position - _bp._planet.position).normalized;
        //Ray ray = new Ray(_bp._planet.position, vBod);
        //Vector3 point = ray.GetPoint(_bp._curHeight);
        //_bp._transform.position = point;
        //******************
        Vector3 vBod = (_bp._transform.position - _bp._planet.position).normalized;
        Ray ray = new Ray(_bp._planet.position, vBod);
        Vector3 point = ray.GetPoint(_bp._curHeight);

        point = Vector3.Lerp(_bp._transform.position, point,  Time.fixedDeltaTime);

        _bp._transform.position = point;


    }
    public void FixCamPos()
    {
        //****************** позиция считается норм
        // Vector3 vCam = (_bp._transform.position - _bp._planet.position).normalized;
        // Ray ray = new Ray(_bp._transform.position, vCam);
        // Vector3 point = ray.GetPoint(7);
        // _bp._camera.position = point;
        //******************
        //****************** позиция считается норм
        Vector3 vCam = (_bp._transform.position - _bp._planet.position).normalized;
        Ray ray = new Ray(_bp._transform.position, vCam);
        Vector3 point = ray.GetPoint(7);

        point = Vector3.Lerp(_bp._camera.position, point, 10 * Time.deltaTime);
        _bp._camera.position = point;
        //******************
    }



    public void Boost(bool boost)
    {
        //_boost = boost;
        BoostBody(boost);
    }
    public virtual void BoostBody(bool boost) { }
    public void GBullet()
    {
        GBulletBody();
    }
    public virtual void GBulletBody() { }
    public void AddDirections(Vector3 velocity)
    {
        // задаю направление вектора движения
        _dir = new Vector3(0, 1, 0);
        _dir = _bp._bodyMove.transform.TransformDirection(_dir);
        //**************************
        // задаю начальную скорость выстрела (скорость пули + скорость стреляющего(скорость forward))
        _bp._rigidbody.velocity = velocity + _bp._transform.up * _bp._speed;


    }
}
