using UnityEngine;
using System.Collections;

public class BodyMoveGBullet : BodyMoveAll
{
    public GlobalEnum.PatrolType _patrolType;

   // float _currAngleTargetAndUp = 0;  // угол можду направлением джоя/таргетом и трансворм.ап
    float _currAcceleration = 0; // локальное ускорение, используется ботами для прогулки или погони

   // Vector2 _randomPatrolTime = new Vector2(3, 10);
   // float _randomPatrolTimer = 0;
   // Vector3 _randomDir = new Vector2(0, 0);

    bool _boost;                         // внезапное ускорение
    //Vector3 _dir = new Vector2(0, 0);

    BodyPrefsGBullet _bpGBullet;        // частные настройки обьекта

    public override void VirtualStart()
    {
        _bpGBullet = GetComponent<BodyPrefsGBullet>();
        if (_bp._controlType == GlobalEnum.ControlType.botSelf)
            _bp._rigidbody.AddForce(AddRandomForce());
    }
    public override void VirtualFixedUpdate()
    {
        //if (_bp._controlType == GlobalEnum.ControlType.botSelf)// реализуется в общем
        //    UpdateThis(Time.fixedDeltaTime);// реализуется в общем
    }
    //public override void PlaceBody(Vector3 pos, Vector3 rot, bool enabled)
    //{

    //}
    public override void UpdateThis(float deltaTime)
    {
        _currAcceleration = _bp._acceleration / 3;
        /*
        if (_bp._controlType == GlobalEnum.ControlType.botSelf)
        {
            _randomDir = GetRandomDirToMove(deltaTime);
            _dir = _randomDir;
        }
        */

        UniversaldUpdate(deltaTime);
    }
    void UniversaldUpdate(float deltaTime)
    {
        _currentVelocity = _bp._rigidbody.velocity;

        float powr = 0;
        powr = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y) ? _dir.x : _dir.y;
        powr = Mathf.Abs(powr) * deltaTime * _currAcceleration;

        Vector3 velocityNext = _currentVelocity + _currentVelocity.normalized * powr;
        float velocityMagnitudeNext = velocityNext.magnitude;

        if (_bp._speed > velocityMagnitudeNext)
        {
            _currentVelocity = velocityNext;
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
    }

    /*
        Vector3 GetRandomDirToMove(float deltaTime)
        {
            Vector3 dir = _randomDir;// new Vector3( _dir.x, _dir.y,_dir.z); // расчет от тукущего направления
            if (_randomPatrolTimer <= 0)
            {
                //Debug.Log("смена направления");
                _randomPatrolTimer = Random.Range(_randomPatrolTime.x, _randomPatrolTime.y);
                dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            }
            else
            {
                if (_patrolType != GlobalEnum.PatrolType.oneDirection)
                    _randomPatrolTimer -= deltaTime;
            }
            return dir;
        }
        */
    Vector3 GetRandomDirToRotate()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; ;
    }
    Vector3 AddRandomForce()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
    }
    void Update()
    {
        if (_bp.IsUsed)
        {
            _bpGBullet._bulletTimer -= Time.deltaTime;
            if (_bpGBullet._bulletTimer <= 0) { _bp.DestroyReturn(); }
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != _bpGBullet._owner.gameObject)
        {
            Debug.Log("столкновение с обьектом " + collider.name);
            collider.SendMessage("SetDamage", _bpGBullet._bulletDamage, SendMessageOptions.DontRequireReceiver);
            _bp.DestroyReturn();
        }
    }


}
