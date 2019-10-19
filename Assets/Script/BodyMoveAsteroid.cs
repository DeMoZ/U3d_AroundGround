using UnityEngine;
using System.Collections;

public class BodyMoveAsteroid : BodyMoveAll
{
    public GlobalEnum.PatrolType _patrolType;

    float _currAcceleration = 0; // локальное ускорение, используется ботами для прогулки или погони

    Vector2 _randomPatrolTime = new Vector2(3, 10);
    float _randomPatrolTimer = 0;
    Vector3 _randomDir = new Vector2(0, 0);

                                         // Vector3 _dir = new Vector2(0, 0);

    public override void VirtualStart()
    {
        _bp._rigidbody.AddForce(AddRandomForce());
    }
    public override void VirtualFixedUpdate()
    {
        //if (_bp._controlType == GlobalEnum.ControlType.botSelf)// реализуется в общем
        //    UpdateThis(Time.fixedDeltaTime);// реализуется в общем
    }
    public override void UpdateThis(float deltaTime)
    {

        _currAcceleration = _bp._acceleration / 3;
        _randomDir = GetRandomDirToMove(deltaTime);
        _dir = _randomDir;

        //FixCamPos();
        //_bp._camera.LookAt(_bp._planet, _bp._camera.up);
        UniversaldUpdate(deltaTime);
    }
    void UniversaldUpdate(float deltaTime)
    {
        _currentVelocity = _bp._rigidbody.velocity;

        float powr = 0;
        powr = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y) ? _dir.x : _dir.y;
        powr = Mathf.Abs(powr) * deltaTime * _currAcceleration;

        Vector3 velocityNext =  _currentVelocity + _currentVelocity.normalized * powr;
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
        // _bp._transform.LookAt(_bp._planet, _bp._transform.up);

    }

    Vector3 GetRandomDirToMove(float deltaTime)
    {
        Vector3 dir = _randomDir;// new Vector3( _dir.x, _dir.y,_dir.z); // расчет от тукущего направления

        if (_randomPatrolTimer <= 0)
        {
            //Debug.Log("смена направления");
            _randomPatrolTimer = Random.Range(_randomPatrolTime.x, _randomPatrolTime.y);
            dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)/*, Random.Range(0f, 1f)*/).normalized;
            //_randomRot = _bp._transform.localEulerAngles;// test
        }
        else
        {
            if (_patrolType != GlobalEnum.PatrolType.oneDirection)
                _randomPatrolTimer -= deltaTime;
        }

        return dir;
    }
    Vector3 AddRandomForce()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.LogWarning("1)  триггер; bonus = "+name+"  collider = "+collider.name+"   target = "+_bpBonus._target+"   follower ="+_bpBonus._follower);
        //if (collider.gameObject.layer != gameObject.layer)// если это не астероид (столкновение с астероидом не дает реакции)
        //{

        //}
        collider.GetComponent<BodyPrefsAll>().SetDamage(1);
        Debug.Log("asteroid hit "+collider.name);
       
    }
}