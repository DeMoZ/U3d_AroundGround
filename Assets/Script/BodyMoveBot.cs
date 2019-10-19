using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyMoveBot : BodyMoveAll
{
    public GlobalEnum.PatrolType _patrolType;

    float _startEngineAngle = 30;
    float _currAngleTargetAndUp = 0;  // угол можду направлением джоя/таргетом и трансворм.ап
    float _currAcceleration = 0; // локальное ускорение, используется ботами для прогулки или погони
    BodyPrefsBot _bpBot;            // индивидуальные настройки бота

    Vector2 _randomPatrolTime = new Vector2(3f, 10f);
    float _randomPatrolTimer = 0;
    Vector3 _randomDir = new Vector2(0, 0);
    Vector3 _vectorToPlanet = new Vector2(0, 0);        // вектор от тела на планету

    bool _boost;                         // внезапное ускорение
    //Vector3 _dir = new Vector2(0, 0);

    //LayerMask _targetLayerAsteroid = 1 << 17;    // layer Asteroid
    //LayerMask _targetLayerBot = 1 << 18;    // layer Bot
    //LayerMask _targetLayerBonus = 1 << 19;    // layer Bonus

    [Tooltip("(перед,зад) дистанции риагирования на цели")]
    public Vector2 _viewRadiusAsteroid = new Vector2(3, 2);         // (спереди,сзади) дисстанции замечения цели
    public Vector2 _viewRadiusBot = new Vector2(4, 2);
    public Vector2 _viewRadiusBonus = new Vector2(5, 2);

    public Transform _targetAsteroid;
    public Transform _targetBot;
    public Transform _targetBonus;

    Transform _targetAsteroidEx;
    Transform _targetBotEx;
    Transform _targetBonusEx;

    // убрать из паблика и оставить в GetTarget как локальные
    public List<Transform> _targetsAsteroids;       // от них убегать
    public List<Transform> _targetsBots;            // их атаковать
    public List<Transform> _targetsBonuses;         // за ними бегать

    float _engine = 0;                              // значения двигателя для партиклов и трэйла

    public override void VirtualStart()
    {
        _bpBot = GetComponent<BodyPrefsBot>();
    }

    public override void VirtualFixedUpdate()
    {
        //if (_bp._controlType == GlobalEnum.ControlType.botSelf)// реализуется в общем
        //    UpdateBot(Time.fixedDeltaTime);// реализуется в общем
    }
    public override void UpdateThis(float deltaTime)
    {// _ dir надо получать в зависимости от цели, или рандомной точки
        _targetAsteroid = GetTarget(_targetsAsteroids, _viewRadiusAsteroid.x, _viewRadiusAsteroid.y, GlobalEnum._targetLayerAsteroid);
        _targetBot = GetTarget(_targetsBots, _viewRadiusBot.x, _viewRadiusBot.y, GlobalEnum._targetLayerBot);
        _targetBonus = GetTarget(_targetsBonuses, _viewRadiusBonus.x, _viewRadiusBonus.y, GlobalEnum._targetLayerBonus);

        LongFollowTarget(_targetAsteroid, _targetAsteroidEx, _viewRadiusAsteroid.x + _viewRadiusAsteroid.y);
        LongFollowTarget(_targetBot, _targetBotEx, _viewRadiusBot.x + _viewRadiusBot.y);
        LongFollowTarget(_targetBonus, _targetBonusEx, _viewRadiusBonus.x + _viewRadiusBonus.y);

        _vectorToPlanet = _bp._planet.position - _bp._transform.position;

        if (_targetAsteroid)
        {
            //RunFromAsteroid(deltaTime);
            _currAcceleration = _bp._acceleration;
            UniversalDirection(_bp._transform.position - _targetAsteroid.position, deltaTime);
        }
        else if (_targetBot)
        {
            _currAcceleration = _bp._acceleration;
            UniversalDirection(_targetBot.position - _bp._transform.position, deltaTime);
            //StalkBot(deltaTime);
        }
        else if (_targetBonus)
        {
            _currAcceleration = _bp._acceleration;
            UniversalDirection(_targetBonus.position - _bp._transform.position, deltaTime);
            // StalkBonus(deltaTime);
        }
        else
        {
            _currAcceleration = _bp._acceleration / 3;
            _randomDir = GetRandomDirToMove(deltaTime);


            UniversalDirection(Vector3.Cross(_randomDir, _vectorToPlanet).normalized, deltaTime);
            //Scouting(deltaTime);
        }
        UniversaldUpdate(deltaTime);
    }
    void UniversalDirection(Vector3 dir, float deltaTime)
    {
        _currAcceleration = _bp._acceleration;
        _dir = dir;
        //вектор надо подкрутить до паралели

        float angle = Vector3.Angle(_vectorToPlanet, _dir);
        // нахожу лишнее значение угла
        angle -= 90;
        _dir = Vector3.RotateTowards(_dir, _vectorToPlanet, angle * deltaTime, 0);
        if (Mathf.Abs(_dir.x) > 1 || Mathf.Abs(_dir.y) > 1)
            _dir = _dir.normalized;

        Debug.DrawRay(_bp._transform.position, _dir * 2, Color.yellow);
        _currAngleTargetAndUp = Vector3.Angle(_bp._transform.up, _dir);

        // разворот в сторону dir
        if (Mathf.Abs(_dir.x) > 0 || Mathf.Abs(_dir.y) > 0)
        {
            Quaternion q = Quaternion.LookRotation(_bp._transform.forward, _dir);
            //  _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp._rotationSpeed * deltaTime);
            _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp.RotationSpeed * deltaTime);
        }
    }
    void UniversaldUpdate(float deltaTime)
    {
        _currentVelocity = _bp._rigidbody.velocity;

        float powr = 0;
        float engine = 0;
        powr = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y) ? _dir.x : _dir.y;
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

    Vector3 GetRandomDirToMove(float deltaTime)
    {
        Vector3 dir = _randomDir;// new Vector3( _dir.x, _dir.y,_dir.z); // расчет от тукущего направления

        if (_randomPatrolTimer <= 0)
        {
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
    public override void BoostBody(bool boost)
    {
        _boost = boost;
    }

    Transform GetTarget(List<Transform> targetsList, float viewRadius, float viewRadiusBack, LayerMask layerMask)
    {
        Transform res = null;
        Collider[] dirtyCollisions = Physics.OverlapSphere(_bp._transform.position, viewRadius, layerMask);// 1 << 9);// 9 layerMask - Body

        List<Transform> collisions = new List<Transform>();

        //********** Чистка Коллизий по расстоянию (спереди, сзади)
        for (int i = 0; i < dirtyCollisions.Length; i++)
        {//  при коллизии с ботами, боты из одной группы  не попадут в таргет
            if (dirtyCollisions[i].tag != tag)// обьект из другой группы ботов
            {
                float angle = Vector3.Angle(_bp._transform.up, dirtyCollisions[i].transform.position - _bp._transform.position);
                if (angle <= 90)
                    collisions.Add(dirtyCollisions[i].transform);

                else if (Vector3.Distance(_bp._transform.position, dirtyCollisions[i].transform.position) <= viewRadiusBack)
                    collisions.Add(dirtyCollisions[i].transform);
            }
        }
        //***********

        //*********** Проверка бонусов на состояние в хвосте
        if (layerMask == GlobalEnum._targetLayerBonus) // чистка от бонусов в хвосте
        {
            // пересобрать новый список коллизий c бонусами
            List<Transform> collisionsBonus = new List<Transform>();

            for (int i = 0; i < collisions.Count; i++)
            {
                BodyPrefsBonus bpA = collisions[i].GetComponent<BodyPrefsBonus>().GetLeadBonus();
                // получаю последний бонус из ряда, если ряд (надо проверить что за таргет у него)
                if (bpA._target)// если у бонуса есть цель (бот или игрок), если цель игрок, то бежим за его бонусом )))) (может выставить приоритет? тогда бот быстро найдет игрока)
                {
                    if (bpA._target.gameObject.layer != gameObject.layer || bpA._target.gameObject.tag != gameObject.tag) 
                        // значит бонус летит за игроком и этот бонус в приоритете. (развернувшись на него, нацелимся на игрока)
                    {
                        collisionsBonus.Clear();
                        collisionsBonus.Add(bpA._transform);
                        break;
                    }
                }
                else // крайний бонус, добавляю его в список 
                {
                    collisionsBonus.Add(bpA._transform);
                }
            }
            collisions = collisionsBonus;
        }
        //***********

        targetsList.Clear();
        int closestIndex = -1;
        float closestDistance = -1;
        for (int i = 0; i < collisions.Count; i++)
        {
            Debug.DrawRay(_bp._transform.position, collisions[i].transform.position - _bp._transform.position, Color.black);
            Transform t = collisions[i].transform;
            targetsList.Add(t);

            //сразу сортирую чтобы взять самого ближнего
            float d = Vector3.Distance(_bp._transform.position, t.position);
            if (i == 0)
            {
                closestIndex = i;
                closestDistance = d;// Vector3.Distance(Bp._transform.position, t.position);
            }
            else if (d < closestDistance)
            {
                closestIndex = i;
                closestDistance = d;
            }
        }
        if (closestIndex > -1)
            res = collisions[closestIndex].transform;

        return res;
    }

    // таргет засекает
    void LongFollowTarget(Transform target, Transform targetEx, float longRadius)
    {
        if (targetEx)
            if (target)
            {
                if (targetEx != target)
                    targetEx = target;
            }
            else
            {// проверяю дистанцию до цели
                if (Vector3.Distance(_bp._transform.position, targetEx.position) <= (longRadius))
                    target = targetEx;
                else targetEx = null;
            }
    }
    void EngineRun(float powr)
    {
        //if (_bpBot._enginePoints.Count > 0 && _bpBot._enginePoints[0])
        //{
        //    _bpBot._enginePS[0].emissionRate = (int)(powr * 10);
        //    _bpBot._engineTR[0].time = powr;
        //}
        if (_bpBot._enginePS.Count>0) {
            for (int i = 0; i < _bpBot._enginePS.Count; i++)
                _bpBot._enginePS[i].emissionRate = (int)(powr * 10);
                //_bpBot._enginePS[i].emission.rate = (int)(powr * 10);
            _bpBot._engineTR[0].time = powr;
        }
    }
    /*
        void RunFromAsteroid(float deltaTime)
        {
            Vector3 VToPlanet = _bp._planet.position - _bp._transform.position;
            _currAcceleration = _bp._acceleration;
            _dir = _bp._transform.position - _targetAsteroid.position;
            //вектор надо подкрутить до паралели

            float angle = Vector3.Angle(VToPlanet, _dir);
            // нахожу лишнее значение угла
            angle -= 90;
            _dir = Vector3.RotateTowards(_dir, VToPlanet, angle * deltaTime, 0);
            if (Mathf.Abs(_dir.x) > 1 || Mathf.Abs(_dir.y) > 1)
                _dir = _dir.normalized;

            Debug.DrawRay(_bp._transform.position, _dir * 2, Color.yellow);
            _currAngleTargetAndUp = Vector3.Angle(_bp._transform.up, _dir);

            // разворот в сторону dir
            if (Mathf.Abs(_dir.x) > 0 || Mathf.Abs(_dir.y) > 0)
            {
                Quaternion q = Quaternion.LookRotation(_bp._transform.forward, _dir);
                _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp._rotationSpeed * deltaTime);
            }
        }
        */
    /*
    void StalkBot(float deltaTime)
    {
        Vector3 VToPlanet = _bp._planet.position - _bp._transform.position;
        _currAcceleration = _bp._acceleration;
        _dir = _targetBot.position - _bp._transform.position;
        //вектор надо подкрутить до паралели

        float angle = Vector3.Angle(VToPlanet, _dir);
        // нахожу лишнее значение угла
        angle -= 90;
        _dir = Vector3.RotateTowards(_dir, VToPlanet, angle * deltaTime, 0);
        if (Mathf.Abs(_dir.x) > 1 || Mathf.Abs(_dir.y) > 1)
            _dir = _dir.normalized;

        Debug.DrawRay(_bp._transform.position, _dir * 2, Color.yellow);
        _currAngleTargetAndUp = Vector3.Angle(_bp._transform.up, _dir);

        // разворот в сторону dir
        if (Mathf.Abs(_dir.x) > 0 || Mathf.Abs(_dir.y) > 0)
        {
            Quaternion q = Quaternion.LookRotation(_bp._transform.forward, _dir);
            _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp._rotationSpeed * deltaTime);
        }
    }
    */
    /*
    void StalkBonus(float deltaTime)
    {
        Vector3 VToPlanet = _bp._planet.position - _bp._transform.position;
        _currAcceleration = _bp._acceleration;
        _dir = _targetBonus.position - _bp._transform.position;
        //вектор надо подкрутить до паралели

        float angle = Vector3.Angle(VToPlanet, _dir);
        // нахожу лишнее значение угла
        angle -= 90;
        _dir = Vector3.RotateTowards(_dir, VToPlanet, angle * deltaTime, 0);
        if (Mathf.Abs(_dir.x) > 1 || Mathf.Abs(_dir.y) > 1)
            _dir = _dir.normalized;

        Debug.DrawRay(_bp._transform.position, _dir * 2, Color.yellow);
        _currAngleTargetAndUp = Vector3.Angle(_bp._transform.up, _dir);

        // разворот в сторону dir
        if (Mathf.Abs(_dir.x) > 0 || Mathf.Abs(_dir.y) > 0)
        {
            Quaternion q = Quaternion.LookRotation(_bp._transform.forward, _dir);
            _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp._rotationSpeed * deltaTime);
        }
    }
    */
    /*
    void Scouting(float deltaTime)
    {
        Vector3 VToPlanet = _bp._planet.position - _bp._transform.position;
        // !!! и отполовинить скорость движения
        _currAcceleration = _bp._acceleration / 3;
        _randomDir = GetRandomDirToMove(deltaTime);

        _dir = Vector3.Cross(_randomDir, VToPlanet).normalized;
        _currAngleTargetAndUp = Vector3.Angle(_bp._transform.up, _dir);
        Debug.DrawRay(_bp._transform.position, _dir * 3, Color.blue);


        // разворот в сторону dir
        if (Mathf.Abs(_dir.x) > 0 || Mathf.Abs(_dir.y) > 0)
        {
            Quaternion q = Quaternion.LookRotation(-_dir, _bp._transform.forward);
            _bp._transform.rotation = Quaternion.RotateTowards(_bp._transform.rotation, q, _bp._rotationSpeed * deltaTime);
        }
    }
    */

}
