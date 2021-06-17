using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Level_0_Taining_0 : Level
{

    public Transform _targetForArrow;
    public float angle = 0;
    public float product = 0;
    public float _playerHP = 0;
    //public override void VirtualAwake() { }
    //public override void VirtualStart() { }
    //public override void VirtualUpdate() { }
    public class Step
    {
        // процессы (деплоинг)
        // тайминги (посекундное переключение событий...)

    }
    public Image _targetArrow;
    public Text _tipsBox;
    string _deadMSG = "ЧТО ПРОИСХОДИТ?!? КАК ЭТО?!? НЕ ПОНИМАЮ!?! \n Мне нужно время, чтобы решить, что будет дальше... Прости";
    List<string> _tips = new List<string> {
        "Коснись, отмеченной части экрана, \n и води пальцем, не отрывая, \n или жми стрелки на клавиатуре",   //0
        " ",                                                                //1 3 ШТУКИ СТОЯЩИХ, 
        "Собери Энергетические Артефакты",                                  //2
        " ",                                                                //3 ПОТОМ ОДИН двигающийся, 
        "А этот убегает, собери его тоже",                                  //4 потом еще один 
        "ты проделал большую работу, молодец!",                             //5 3 сек 
        "как ты заметил, в нашем мире совсем нет опасностей",               //6 3 сек
        "поэтому у нас нет никакого оружия",                                //7 3 сек
        "я даже не знаю, что означает это слово",                           //8 3 сек
        " ",                                                                //9 деплой астероида
        "ой, смотри какой большой! Собери его тоже.",                       //10        
        //"Ты Можешь помочь Существу в решить задачу. Простомри рекламу, пожалуйста"
        "что же ты, поторопись",                                            //11
        "я очень хочу исследовать этот артефакт",                           //12
        "Никогда таких не видел таких огромных",                            //13
        "Я уже представляю, как заглядываю внутрь этого арефакта",          //14

        "Может быть тебе не хватает скорости?",
        "Я добавлю тебе экстренное ускорение",
        "ТАДАМ!",
        "Используй с умом. Я не очень люблю высокую скорость."
    };
    public float _levelHeight = 15;
    public Image _pictureTouchJoy;
    Color _picTJColor;                   // -цвет дефолтный
    float _fadeTime = 1f;
    float _fadeTimer = 0;
    bool _fadeWay = false;      // направление фейда



    float _stepTimer = 5f;         // первый тренеровочный таймер, на управление
    int steps = 0;              // переключатель шагов по сценарию

    float _nextTime = 3;        // для единого управление временем сделующего таймера(для пропусков)

    // [Tooltip("первые 5 - бонусы. Потом астероиды")]
    //public List<Transform> _bonuses = new List<Transform>();
    public List<BodyPrefsBonus> _bonuses = new List<BodyPrefsBonus>();
    public List<BodyPrefsAll> _asteroids = new List<BodyPrefsAll>();

    List<BodyPrefsBonus> bTargets = new List<BodyPrefsBonus>(); // массив целей для стрелки


    public override void VirtualAwake() { }
    public override void VirtualStart()
    {
        // previoius sets for colors
        if (_pictureTouchJoy)
        {
            _picTJColor = _pictureTouchJoy.color;
        }



        // показываю сообщение о потрогать левую часть экрана и показываю джойстик
        if (_tipsBox)
        {
            _tipsBox.gameObject.SetActive(true);
        }
        if (_pictureTouchJoy)
        {
            _pictureTouchJoy.gameObject.SetActive(true);
        }


    }

    void FadeJoyPlace()
    {
        _fadeTimer -= Time.deltaTime;
        if (_fadeTimer <= 0)
        {
            _fadeTimer = _fadeTime;
            _fadeWay = !_fadeWay;
        }

        if (!_pictureTouchJoy) return;
        Color co = _pictureTouchJoy.color;
        if (_fadeWay)
        {
            co.a -= Time.deltaTime;
            co.a = Mathf.Clamp(co.a, 0, _picTJColor.a);
            _pictureTouchJoy.color = co;
        }
        else// if (co.a < _jColor.a)
        {
            co.a += Time.deltaTime;
            co.a = Mathf.Clamp(co.a, 0, _picTJColor.a);
            _pictureTouchJoy.color = co;
        }
    }

    public override void VirtualUpdate()
    {
        if (!_tipsBox)
        {
            Debug.Log("UI Text не задан в скрипте сценария");
            return;
        }
        _playerHP = GmCD._body._currHp;
        if (GmCD._body._currHp <= 0)
        {
            if (_tipsBox.text != _deadMSG)
            {
                Debug.LogWarning("наступило усовие смерти");
                _tipsBox.text = _deadMSG;
            }
        }
        else
        {
            switch (steps)
            {
                case 0:
                    _tipsBox.text = _tips[0];
                    Step0_ControlTraining();
                    break;

                case 1:// instantiate bonuses
                    _tipsBox.text = _tips[1];
                    Step1_Deploy3Bonuses();
                    break;
                case 2: // wait till collect bonuses
                    _tipsBox.text = _tips[2];
                    _targetForArrow = GetBonusTarget(bTargets);
                    Step2_CollectTraining();
                    break;

                case 3: // deploy one more movein bonus
                    _tipsBox.text = _tips[3];
                    Step3_DeployFastBonus();
                    break;

                case 4: // wait to collect fast bonus
                    _tipsBox.text = _tips[4];
                    _targetForArrow = GetBonusTarget(bTargets);
                    Step4_CollectingTraining2();
                    break;

                case 5://
                    NextStepInTimer(6, _nextTime, _tips[6]);
                    break;

                case 6:
                    NextStepInTimer(7, _nextTime, _tips[7]);
                    break;
                case 7:
                    NextStepInTimer(8, _nextTime, _tips[8]);
                    break;
                case 8:
                    NextStepInTimer(9, 0, _tips[9]);
                    break;
                case 9:
                    //выполняем разово деплой астероида
                    Step9_DeployAsteroid();// тут целью задаю этот астероид
                    _nextTime = 6;

                    NextStepInTimer(10, _nextTime, _tips[10]);
                    break;
                case 10:
                    NextStepInTimer(11, _nextTime, _tips[11]);
                    break;
                case 11:
                    NextStepInTimer(12, _nextTime, _tips[12]);
                    break;
                case 12:
                    NextStepInTimer(13, _nextTime, _tips[13]);
                    break;
                case 13:
                    NextStepInTimer(14, _nextTime, _tips[14]);
                    break;
                case 14:
                    //   // на последней строчке бесконечно повторяю предыдущую
                    //  NextStepInTimer(steps, _nextTime, _tips[15]);// верменно повторяюсь на этой позиции
                    break;
            }
        }
        TargetArrow(_targetForArrow);
    }
    void NextStepInTimer(int nextStep, float nextTime, string nextTip) // переход на другой step после отсчета времени
    {
        //_playerHP = GmCD._body._currHp;
        //if (GmCD._body._currHp > 0) // || GmCD._body.gameObject.activeSelf == true)
        //{
        //    _stepTimer -= Time.deltaTime;
        //    if (_stepTimer <= 0)
        //    {
        //        steps = nextStep;
        //        _stepTimer = nextTime;
        //        _tipsBox.text = nextTip;
        //    }
        //}
        //else
        //{
        //    Debug.LogError("наступило усовие смерти");
        //    if (_tipsBox.text != _deadMSG)
        //        _tipsBox.text = _deadMSG;
        //}

        _stepTimer -= Time.deltaTime;
        if (_stepTimer <= 0)
        {
            steps = nextStep;
            _stepTimer = nextTime;
            _tipsBox.text = nextTip;
        }
    }

    void Step0_ControlTraining()
    {
        bool fade = GetPlayerActivity();
        if (!fade)
        {
            Color co = _pictureTouchJoy.color;
            co.a = 0;
            _pictureTouchJoy.color = co;

            // запускаю таймер первой подсказки(5 секунд), после этого вторая подсказка,
            _stepTimer -= Time.deltaTime;
            if (_stepTimer < 0)
            {
                steps = 1;
            }
        }

        if (fade)
            FadeJoyPlace();
    }

    bool GetPlayerActivity()
    {
        bool boo = true;
        Touch[] myTouches = Input.touches;
        foreach (Touch touch in myTouches)
        {
            if (touch.phase == TouchPhase.Began)// &&touch.position)
            { boo = false; }
            if (touch.phase == TouchPhase.Ended)
            { boo = true; }
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        { boo = false; }
        else { boo = true; }

        if (Input.GetMouseButton(0))
        { boo = false; }
        // if (Input.GetMouseButtonUp(0))
        //{ fade = true; }  
        return boo;
    }
    void Step1_Deploy3Bonuses()
    {
        // deploy three bonuses close to fly with no speed(How)
        //Transform b0 = Instantiate(_objects[0]);
        //Transform b1 = Instantiate(_objects[1]);
        //Transform b2 = Instantiate(_objects[2]);
        BodyPrefsBonus b0 = Instantiate(_bonuses[0]).GetComponent<BodyPrefsBonus>();
        BodyPrefsBonus b1 = Instantiate(_bonuses[0]).GetComponent<BodyPrefsBonus>();
        BodyPrefsBonus b2 = Instantiate(_bonuses[0]).GetComponent<BodyPrefsBonus>();

        b0._speed = 0;
        b1._speed = 0;
        b2._speed = 0;

        //// count positions depend on camera
        //b0._transform.position = new Vector3(0, 0);
        //b1._transform.position = new Vector3(0, 0);
        //b2._transform.position = new Vector3(0, 0);

        // посылаю луч из центра до самолета, разварачиваю градусов на 30 и деплою туда бонус
        Vector3 vBod = (GmCD._body._transform.position - GmCD._planet.position).normalized;
        Vector3 vBodRotated, point;
        Ray ray;

        vBodRotated = Quaternion.AngleAxis(15, GmCD._body._transform.up) * vBod;
        ray = new Ray(GmCD._planet.position, vBodRotated);
        point = ray.GetPoint(_levelHeight);
        b0._transform.position = point;
        b0._typeColor = GlobalEnum.BonusColor._white;

        vBodRotated = Quaternion.AngleAxis(15, GmCD._body._transform.right) * vBod;
        ray = new Ray(GmCD._planet.position, vBodRotated);
        point = ray.GetPoint(_levelHeight);
        b1._transform.position = point;
        //b1._typeColor = GlobalEnum.BonusColor._white;
        b1._typeColor = GlobalEnum.BonusColor._green;

        vBodRotated = Quaternion.AngleAxis(-15, GmCD._body._transform.right) * vBod;
        ray = new Ray(GmCD._planet.position, vBodRotated);
        point = ray.GetPoint(_levelHeight);
        b2._transform.position = point;
        //b2._typeColor = GlobalEnum.BonusColor._white;
        b2._typeColor = GlobalEnum.BonusColor._blue;

        steps = 2;

        bTargets.Add(b0);
        bTargets.Add(b1);
        bTargets.Add(b2);
    }
    void Step2_CollectTraining()
    {
        if (GmHud.TailLenght >= 3)
        {
            steps = 3;
        }
    }
    void Step3_DeployFastBonus()
    {
        BodyPrefsBonus b3 = Instantiate(_bonuses[3]).GetComponent<BodyPrefsBonus>();
        b3._speed = 3;
        b3._typeColor = GlobalEnum.BonusColor._white;

        // посылаю луч из центра до самолета, разварачиваю градусов на 30 и деплою туда бонус
        Vector3 vBod = (GmCD._body._transform.position - GmCD._planet.position).normalized;
        Vector3 vBodRotated, point;
        Ray ray;

        vBodRotated = Quaternion.AngleAxis(20, GmCD._body._transform.up) * vBod;
        ray = new Ray(GmCD._planet.position, vBodRotated);
        point = ray.GetPoint(_levelHeight);
        b3._transform.position = point;
        //bTargets.Clear();
        bTargets.Add(b3);

        steps = 4;
    }
    void Step4_CollectingTraining2()
    {
        if (GmHud.TailLenght >= 4)
        {
            NextStepInTimer(5, _nextTime, _tips[5]);
        }
    }

    void Step9_DeployAsteroid()
    {
        BodyPrefsAll asteroid =
        Instantiate(_asteroids[0]);
        //_asteroids[0]._speed = 3;
        asteroid._speed = 3;

        // посылаю луч из центра до самолета, разварачиваю градусов на 30 и деплою туда бонус
        Vector3 vBod = (GmCD._body._transform.position - GmCD._planet.position).normalized;
        Vector3 vBodRotated, point;
        Ray ray;

        vBodRotated = Quaternion.AngleAxis(20, -GmCD._body._transform.up) * vBod;
        ray = new Ray(GmCD._planet.position, vBodRotated);
        point = ray.GetPoint(_levelHeight);
        //_asteroids[0]._transform.position = point;
        asteroid._transform.position = point;
        _targetForArrow = asteroid._transform;

    }

    void TargetArrow(Transform target)
    {

        if (_targetArrow)
        {
            if (target)
            {
                if (target.gameObject.activeSelf && Vector3.Distance(GmCD._body._transform.position, target.position) >= 3)
                {
                    // _targetArrow.gameObject.SetActive(true);
                    FadeUiImage(_targetArrow, Time.deltaTime);
                    Transform _aTransform = _targetArrow.transform;
                    Vector2 t = Camera.main.WorldToScreenPoint(target.position);
                    Vector2 ar = _aTransform.position;//pos v3 -> v2

                    Vector2 relative = (t - ar).normalized;
                    angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
                    _aTransform.eulerAngles = new Vector3(0, 0, -angle);
                }
                else
                {
                    FadeUiImage(_targetArrow, -Time.deltaTime);
                    //_targetArrow.gameObject.SetActive(false);
                }
            }
            else
            {
                //_targetArrow.gameObject.SetActive(false);
                FadeUiImage(_targetArrow, -Time.deltaTime);

            }
        }
    }
    Transform GetBonusTarget(List<BodyPrefsBonus> targets)
    {
        float distance = 100000;
        Transform res = null;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] && targets[i].gameObject.activeSelf && targets[i]._target == null)
            {
                float d = Vector3.Distance(GmCD._body._transform.position, targets[i]._transform.position);
                if (d < distance)
                {
                    distance = d;
                    res = targets[i]._transform;
                }
            }
        }
        return res;
    }


    void FadeUiImage(Image image, float deltaTime)
    {

        Color co = image.color;
        co.a += deltaTime;
        co.a = Mathf.Clamp(co.a, 0, 1);

        image.color = co;
    }
}
