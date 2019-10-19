using UnityEngine;
using System.Collections;

public class MyIsolatedAreaJoyArea : MyIsolatedArea
{

    public Transform _circle;
    public Transform _stick;
    public bool _fixKeyboard = true;
    public bool _showJoyOnStart = false;

    Vector2 _touchJoyValue = new Vector2(0, 0);          // значение экранного джоя
    //Vector2 _keyboartJoyValue = new Vector2(0, 0);       // значение клавиатурного джоя
    Vector2 _joyDirection = new Vector2(0, 0);                  // (для расчетов) направление стика
    public float _joyDistance = 0;                                     // (для расчетов) дистанция стика

    public float _joyCircleDiametr = 0;                                 // диаметры палки и круга
    public float _joyCircleRadius = 0;                                  // (расчитаный) радиус круга

    public override void On_Awake()
    {
        //Debug.Log("0 настройка параметров джоя");
        if (_circle != null || _stick != null)
            SetJoy();
        else
        {
            Debug.LogWarning("укажи круг и палку джоя в инспекторе к скрипту джойстика");
        }
    }
    void SetJoy()
    {
        //Debug.Log("1 настройка параметров джоя " + _circle.localToWorldMatrix);
        _joyCircleDiametr = _circle.localToWorldMatrix[0] * _circle.GetComponent<RectTransform>().rect.height;
        _joyCircleRadius = _joyCircleDiametr / 2;
        if (!_showJoyOnStart)
        {
            _circle.gameObject.SetActive(false);
            _stick.gameObject.SetActive(false);
        }
    }

    public override void OnStarted()
    {
        //Debug.Log("тач по джою");
        _circle.gameObject.SetActive(true);
        _stick.gameObject.SetActive(true);
        //_circle.gameObject.SetActive(false);
        //_stick.gameObject.SetActive(false); странно, но вообще отменяет отображение джоя, хотя на старте горит


    }

    public override void OnEnded()
    {
        _circle.gameObject.SetActive(false);
        _stick.gameObject.SetActive(false);
        _touchJoyValue = new Vector2(0, 0);
    }

    Vector2 MakeJoy(Vector2 direct)
    {
        direct.x = -1 * direct.x / _joyCircleRadius;
        direct.y = -1 * direct.y / _joyCircleRadius;

        direct.x = (Mathf.Abs(direct.x) > 1) ? Mathf.Sign(direct.x) : direct.x;
        direct.y = (Mathf.Abs(direct.y) > 1) ? Mathf.Sign(direct.y) : direct.y;

        return direct;
    }

    public override void OnUpdate()
    {
        // Debug.Log("1 Update in touch");
        if (_inTouch)
        {
            // Debug.Log("2 Update in touch");
            _joyDirection = _touchPosStart - _touchPosCurrent;
            _joyDistance = _joyDirection.magnitude;

            if (_joyDistance > _joyCircleRadius) // двигаю стик за гругом, если тот уходит за расстояние радиуса
                _touchPosStart -= _joyDirection.normalized * (_joyDistance - _joyCircleRadius);

            _touchJoyValue = MakeJoy(_joyDirection);

            _cd.JoyAxis = _touchJoyValue;

            _circle.position = _touchPosStart;
            _stick.position = _touchPosCurrent;
        }
    }

    /// <summary>
    /// получить параметры джойстика
    /// </summary>
    /// 
    //public float GetAxis(string str)
    //{
    //    float rezult = 0;
    //    Vector2 _keyboartJoyValue = new Vector2(0, 0);
    //    if (_fixKeyboard)
    //    {//значение клавиатуры принимает (-1;0;1) резко
    //        _keyboartJoyValue.x = Input.GetAxisRaw("Horizontal");
    //        _keyboartJoyValue.y = Input.GetAxisRaw("Vertical");
    //    }
    //    else
    //    {// стандартная клавиатура применяет сглаживание значения.			
    //        _keyboartJoyValue.x = Input.GetAxis("Horizontal");
    //        _keyboartJoyValue.y = Input.GetAxis("Vertical");
    //    }
    //    switch (str)
    //    {
    //        case "Horizontal":
    //            rezult = (Mathf.Abs(_touchJoyValue.x) > Mathf.Abs(_keyboartJoyValue.x)) ? _touchJoyValue.x : _keyboartJoyValue.x;
    //            break;
    //        case "Vertical":
    //            rezult = (Mathf.Abs(_touchJoyValue.y) > Mathf.Abs(_keyboartJoyValue.y)) ? _touchJoyValue.y : _keyboartJoyValue.y;
    //            break;               
    //    }
    //    return rezult;
    //}

}
