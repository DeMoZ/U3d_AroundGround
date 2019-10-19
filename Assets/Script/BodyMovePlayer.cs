using UnityEngine;
using System.Collections;

public class BodyMovePlayer : BodyMoveAll
{
    // float velocityMagnitude; // -скорость
    [Tooltip("двигатель всегда на максимум")]
    public bool _maxPower = true;       // для простоты управления двигатель на максимуме(без учета значения на курсоре)
    float _startEngineAngle = 30;
    float _currAngleTargetAndUp = 0;  // угол можду направлением джоя/таргетом и трансворм.ап
    float _currAcceleration = 0; // локальное ускорение, используется ботами для прогулки или погони
    BodyPrefsPlayer _bpPlayer;        // частные настройки обьекта

    bool _boost;                         // внезапное ускорение
                                         // Vector3 _dir = new Vector2(0, 0);
    float _engine = 0;                  // значение нагрузки на двигатель для трэйла и частиц
    public override void VirtualStart()
    {
        //if (_controlType == ControlType.player)
        //// Debug.Log("1");
        //_bp = (BodyPrefsAll)GetComponent<BodyPrefsBot>();
        _bpPlayer = GetComponent<BodyPrefsPlayer>();
        //   _bp._camera = _bp.GetCamera();
        //   Debug.Log(_bp._camera = null);
        // else
        // {
        //_camera = CreateNotCamera();
        // }
    }

    public override void UpdateThis(Vector3 dir)
    {
        //if (_bp._camera == null)
        //{
        //    _bp._camera = _bp.GetCamera();
        //    if (_bp._camera == null)
        //        return;
        //}

        //Debug.Log("2");
        _currAcceleration = _bp._acceleration;
        _dir = dir;
        _currAngleTargetAndUp = Vector3.Angle(_bp._transform.up, _bp._camera.TransformDirection(_dir));

        // разворот в сторону dir
        if (Mathf.Abs(_dir.x) > 0 || Mathf.Abs(_dir.y) > 0)
        {
            Quaternion q = Quaternion.LookRotation(-_bp._camera.TransformDirection(_dir.normalized), _bp._transform.forward);
            //_bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp._rotationSpeed * Time.fixedDeltaTime);
            _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp.RotationSpeed * Time.fixedDeltaTime);
        }
        UniversaldUpdate(Time.fixedDeltaTime);
        //FixCamPos();
        //_bp._camera.LookAt(_bp._planet, _bp._camera.up);
    }

    void UniversaldUpdate(float deltaTime)
    {
        _currentVelocity = _bp._rigidbody.velocity;
        float powr = 0;
        float engine = 0;

        powr = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y) ? _dir.x : _dir.y;

        if (_maxPower) { powr = 1; }// для простоты управления двигатель на максимуме(без учета значения на курсоре)

        engine = Mathf.Abs(powr);
        _engine = Mathf.Lerp(_engine, 0, 10 * deltaTime);
        powr = Mathf.Abs(powr) * deltaTime * _currAcceleration;

        if (_currAngleTargetAndUp >= _startEngineAngle)
            EngineRun(_engine);

        Vector3 velocityNext = _currentVelocity + _bp._transform.up * powr;
        float velocityMagnitudeNext = velocityNext.magnitude;

        if (_bp._speed > velocityMagnitudeNext)
        {
            if (_currAngleTargetAndUp <= _startEngineAngle) // если угол между куда смотрит и transform up не определенного, чтоб включить двигатель
            {
                _currentVelocity = velocityNext;
                _engine = Mathf.Lerp(_engine, engine, 10 * deltaTime);
                EngineRun(_engine);
            }
        }
        if (_boost)
        {// внезапное ускорение/ так же увелличивается максимальная скорость
            if ((_bp._speed + _bp.BoostModify) > velocityMagnitudeNext)
            {
                _currentVelocity = _currentVelocity + _bp._transform.up * deltaTime * _currAcceleration * _bp.BoostModify;
                _engine = Mathf.Lerp(_engine, 3, 10 * deltaTime);
                EngineRun(3);
            }
        }

        //**************************************
        Breaking(deltaTime);
        Vector3 VToPlanet = (_bp._planet.position - _bp._transform.position);
        // получаю угол между вектором на планету и скоростью
        float angle = Vector3.Angle(VToPlanet, _currentVelocity);
        // нахожу лишнее значение угла
        angle -= 90;
        // поворот скорости до значения 90 градусов к вектору на планету
        _currentVelocity = Vector3.RotateTowards(_currentVelocity, VToPlanet, angle * deltaTime, 0);
        _bp._rigidbody.velocity = _currentVelocity;
        Debug.DrawRay(_bp._transform.position, _bp._rigidbody.velocity, Color.red);
        //**************************

        FixBodyPos();
        _bp._transform.LookAt(_bp._planet, _bp._transform.up);

    }
    // ABILS
    public override void BoostBody(bool boost)
    {
        _boost = boost;
    }
    public override void GBulletBody()
    {
        BodyPrefsAll bpA = _bpPlayer._pullGBulet.GetFromPool();
        //bpA.Enabler(_bp._transform.position, _bp._transform.rotation, true);
        bpA.Enabler(_bpPlayer._shootPoints[0].position, _bp._transform.rotation, true);
        BodyPrefsGBullet bpGBullet = bpA.GetComponent<BodyPrefsGBullet>();
        bpGBullet._bulletTimer = bpGBullet._bulletTime;
        bpGBullet._owner = _bp._transform;
        //***      
        Vector3 velocity = _bp._transform.up * Vector3.Dot(_bp._transform.up, _bp._rigidbody.velocity);
        bpA._bodyMove.AddDirections(velocity);
        bpA._rigidbody.angularVelocity = bpA._bodyMove._dir.normalized * bpA._speed * 2;// вращение снаряда

    }
    void EngineRun(float powr)
    {
        //if (_bpPlayer._enginePoints.Count > 0 && _bpPlayer._enginePoints[0])
        //{            
        //    //ParticleSystem ps = _bpPlayer._enginePoints[0].GetComponent<ParticleSystem>();
        //    //ps.emissionRate = (int)(powr * 10);
        //    //TrailRenderer tr = _bpPlayer._enginePoints[0].GetComponent<TrailRenderer>();
        //    //tr.time = powr/2;

        //    _bpPlayer._enginePS[0].emissionRate = (int)(powr * 10);
        //    _bpPlayer._engineTR[0].time = powr ;
        //}

        //for (int i = 0; i < _bpPlayer._enginePoints.Count; i++)
        //{
        //    if (_bpPlayer._enginePoints[i])
        //    {
        //        _bpPlayer._enginePS[i].emissionRate = (int)(powr * 10);
        //        _bpPlayer._engineTR[i].time = powr;
        //    }
        //}

        for (int i = 0; i < _bpPlayer._enginePS.Count; i++)
        {
            if (_bpPlayer._enginePS[i])
            {
                _bpPlayer._enginePS[i].emissionRate = (int)(powr * 10);


            //    var rate =                _bpPlayer._enginePS[i].emission;
            ////var em=_bpPlayer._enginePS[i].emission;

            //    rate.constantMin = (float)(powr * 10);

            //    _bpPlayer._engineTR[i].time = powr;
            }
        }



    }
}
