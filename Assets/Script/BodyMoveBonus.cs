using UnityEngine;
using System.Collections;

public class BodyMoveBonus : BodyMoveAll
{
    BodyPrefsBonus _bpBonus;

    public GlobalEnum.PatrolType _patrolType;

    float _currAcceleration = 0; // локальное ускорение, используется ботами для прогулки или погони

    Vector2 _randomPatrolTime = new Vector2(3, 10);
    float _randomPatrolTimer = 0;
    Vector3 _randomDir = new Vector2(0, 0);
    Vector3 _vectorToPlanet = new Vector2(0, 0);

    public override void VirtualStart()
    {
        _bpBonus = GetComponent<BodyPrefsBonus>();
        _bp._rigidbody.AddForce(AddRandomForce());

        // _randomDir = GetRandomDirToMove();
    }


    public override void VirtualFixedUpdate()
    {

    }
    public override void UpdateThis(float deltaTime)
    {
        if (_bpBonus._target)
        {
            BonMove(deltaTime);
        }
        else
        {
            BonMoveAsAsteroid(deltaTime);
        }
    }
    void BonMove(float deltaTime)
    {
        _currentVelocity = _bp._rigidbody.velocity;
        Vector3 velocityNext = new Vector2(0, 0);
        float powr = 0, angle = 0, breakingModify = 1;


        _vectorToPlanet = _bp._planet.position - _bp._transform.position;
        _dir = _bpBonus._target._transform.position - _bpBonus._transform.position;
        //вектор надо подкрутить до паралели
        angle = Vector3.Angle(_vectorToPlanet, _dir);
        // нахожу лишнее значение угла
        angle -= 90;
        _dir = Vector3.RotateTowards(_dir, _vectorToPlanet, angle * deltaTime, 0);
        if (Mathf.Abs(_dir.x) > 1 || Mathf.Abs(_dir.y) > 1)
            _dir = _dir.normalized;
        Debug.DrawRay(_bp._transform.position, _dir * 2, Color.yellow);

        powr = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y) ? _dir.x : _dir.y;
        powr = Mathf.Abs(powr) * deltaTime * _currAcceleration;

        float dist = Vector3.Distance(_bp._transform.position, _bpBonus._target._transform.position);

        //***8
        velocityNext = _currentVelocity + _dir * powr;
        // разворачиваю вектор ускорения на таргет(чтобы хвост не заносило)
        velocityNext = Vector3.RotateTowards(velocityNext, _dir, deltaTime, 0);
        //***

        if (_bpBonus._target.gameObject.layer == gameObject.layer) // если летим за бонусом
        {
            if (dist > 0.5f)// жестко держать расстояние
            {
                Vector3 vBod = (_bp._transform.position - _bpBonus._target._transform.position).normalized;
                Ray ray = new Ray(_bpBonus._target._transform.position, vBod);
                Vector3 point = ray.GetPoint(0.5f);
                _bp._transform.position = point;
            }
            if (dist >= 0.2f)// надо мягко держать расстояние
            {
                if (velocityNext.magnitude <= _bpBonus._target._speed)
                    _currentVelocity = velocityNext;
            }
            else // прекратить движение
            {
                breakingModify = 10;
            }
        }
        else // если  летим за ботом
        {
            if (dist > 1.5f)// жестко держать расстояние
            {
                Vector3 vBod = (_bp._transform.position - _bpBonus._target._transform.position).normalized;
                Ray ray = new Ray(_bpBonus._target._transform.position, vBod);
                Vector3 point = ray.GetPoint(1.5f);
                _bp._transform.position = point;
            }
            if (dist >= 1f)// надо мягко держать расстояние
            {
                if (velocityNext.magnitude <= _bpBonus._target._speed)
                    _currentVelocity = velocityNext;
            }
            else // прекратить движение
            {
                breakingModify = 10;
            }

        }

        //**************************************
        Breaking(breakingModify * deltaTime);
        Vector3 VToPlanet = (_bp._planet.position - _bp._transform.position);
        // получаю угол между вектором на планету и скоростью
        angle = Vector3.Angle(VToPlanet, _currentVelocity);
        // нахожу лишнее значение угла
        angle -= 90;
        // поворот скорости до значения 90 градусов к вектору на планету
        _currentVelocity = Vector3.RotateTowards(_currentVelocity, VToPlanet, angle * deltaTime, 0);
        _bp._rigidbody.velocity = _currentVelocity;
        Debug.DrawRay(_bp._transform.position, _bp._rigidbody.velocity, Color.red);
        //**************************
        FixBodyPos();

    }
    void BonMoveAsAsteroid(float deltaTime)
    {
        _currAcceleration = _bp._acceleration / 3;
        _randomDir = GetRandomDirToMove(deltaTime);
        _dir = _randomDir;

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
    Vector3 GetRandomDirToMove(float deltaTime)
    {
        Vector3 dir = _randomDir;// new Vector3( _dir.x, _dir.y,_dir.z); // расчет от тукущего направления
                                 // _randomPatrolTimer -= deltaTime;
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
        Vector3 ret = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
        // Vector3 VToPlanet = (_bp._planet.position - _bp._transform.position);
        Vector3 VToPlanet = (new Vector3(0, 0) - transform.position);
        // получаю угол между вектором на планету и скоростью
        float a = Vector3.Angle(VToPlanet, ret);
        // нахожу лишнее значение угла
        a -= 90;
        // поворот скорости до значения 90 градусов к вектору на планету
        ret = Vector3.RotateTowards(ret, VToPlanet, a, 0);
        return ret;
    }
    //********

    void OnTriggerEnter(Collider collider)
    {
        string testStr1 = "", testStr2 = "";
        bool testTarg = false;//, testFTarg = false; 
        testStr1 = ("1)  триггер; bonus = " + name + "  collider = " + collider.name + "   target = " + _bpBonus._target + "   follower =" + _bpBonus._follower);

        testTarg = _bpBonus._target;


        //Debug.LogWarning("1)  триггер; bonus = "+name+"  collider = "+collider.name+"   target = "+_bpBonus._target+"   follower ="+_bpBonus._follower);
        if (collider.gameObject.layer != gameObject.layer)// если это не бонус (столкновение с бонусом не дает реакции)
        {
            _bp.PlayDestroyParticles();
            _bpBonus.RecursionToTailHead(0);// тут запустить рекурсивную функцию, которая дойдет до головы хвоста(если она есть)(прыгая по таргетам)(target.target.target.target.....)
            if (_bpBonus._target)
            {
                BodyPrefsBonus bpb = _bpBonus._target.GetComponent<BodyPrefsBonus>();
                if (bpb)
                {
                    bpb._follower = null;
                }

            }
            _bpBonus._target = null;
            // и сообщит номер элемента, который надо отцеплять
            _bpBonus.RemoveFollowersTarget();
            BodyTail bT = collider.GetComponent<BodyTail>();
            if (bT) // коллайдер может собрать бонус в хвост
            {
                // Debug.Log("добавлен в хвост    ");               
                //_bpBonus.RemoveFollowersTarget();
                bT.AddFollower(_bpBonus);
            }
            else
            {
                //Destroy(_bp._transform);

                // запустить процесс пересчета для хвоста
                //_bpBonus.RecountForPossibleTail(1); // один бон то есть

                //_bpBonus.RemoveFollowersTarget();
                //_bpBonus._target = null;
            }
        }
        //testStr2= ("2)  триггер; bonus = " + name + "  collider = " + collider.name + "   target = " + _bpBonus._target + "   follower =" + _bpBonus._follower);
        // Debug.LogWarning("2)  триггер; bonus = " + name + "  collider = " + collider.name + "   target = " + _bpBonus._target + "   follower =" + _bpBonus._follower);
        if (testTarg && !_bpBonus._target)
        {
            testStr2 = ("2)  триггер; bonus = " + name + "  collider = " + collider.name + "   target = " + _bpBonus._target + "   follower =" + _bpBonus._follower);
            Debug.Log(testStr1);
            Debug.Log(testStr2);
        }
    }


}
