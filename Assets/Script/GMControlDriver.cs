using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GMControlDriver : MonoBehaviour
{
    //public Transform _body; //!!!!! при смене тела, важно обнулить тело и только потом получить новое
    public BodyPrefsPlayer _body;
    [Header("камера добавиться автоматически если пусто")]
    public Transform _camera;//!!!!! при смене камеры, важно обнулить тело и только потом получить новое
    public Transform _planet;


    // Transform _bodyEx; // для определения, что тело сменили
    BodyPrefsPlayer _bodyEx;
    BodyPrefsAll _bp;      // скрипт настроек тела, буду посылать в изолированые области, чтобы они знали, с кем имеют дело

    bool _hadBody;          // тело было
    bool _hadCamera;       // камера была

    // bool _singleInUse = false;      // какая то область исключающая касания других в использовании

   // [HideInInspector]
    //public List<MyIsolatedArea> _areas = new List<MyIsolatedArea>(); // удалить


    //public Transform _actionArea;
    //bool _actionIsUsed = false;
    //Image _actionImage;
    //Color _aColor;

    //public Transform _jumpArea;
    //bool _jumpIsUsed = false;
    //Image _jumpImage;
    //Color _jColor;

    // Transform _lookArea;
    bool _lookIsUsed = false;
    Vector2 _lookAxis = new Vector2(0, 0);
    public Vector2 LookAxis
    {
        set { _lookAxis = value; }
        get
        {
            if (_lookIsUsed) return _lookAxis;
            else return new Vector2(0, 0);
        }
    }

   // public Transform _joyArea;
    bool _joyIsUsed = false;
    Vector2 _joyAxis = new Vector2(0, 0);
    public Vector2 JoyAxis
    {
        set { _joyAxis = value; }
        get
        {
            if (_joyIsUsed) return _joyAxis;
            else return new Vector2(0, 0);
        }
    }

    void Awake()
    {
        if (!_camera) _camera = Camera.main.transform;
        //if (!_planet) _planet.position = new Vector3(0, 0);
    }
    void Start()
    {

        // кеширую цвета экшен и джамп зон. чтобы при смене прозрачности вернуться на предыдущую прозрачность
        //if (_actionArea != null)
        //{
        //    _actionImage = _actionArea.GetComponent<Image>();
        //    _aColor = _actionImage.color;
        //}
        //if (_jumpArea != null)
        //{
        //    _jumpImage = _jumpArea.GetComponent<Image>();
        //    _jColor = _jumpImage.color;
        //}


    }

    void Update()
    {

        if ((_body != null && !_hadBody) || (_body != _bodyEx))
        {
            _hadBody = true;
            _bodyEx = _body;
            _bp = _body.GetComponent<BodyPrefsAll>();
            // _bodyName = _body.ToString();
            //посылаю эвент, что управляемое тело получено
            // EventManager.TriggerEvent("GotBody");
            ActionAreaEnabler();
        }
        if (_body == null && _hadBody)
        {
            _hadBody = false;
            _bp = null;
            // _bodyName = null;
            //посылаю эвент, что управляемое тело потеряно
            //  EventManager.TriggerEvent("LostBody");
        }

        if (_camera != null && !_hadCamera)
        {
            _hadCamera = true;
            //посылаю эвент, что управляемое тело получено
            //  EventManager.TriggerEvent("GotCamera");
        }
        if (_camera == null && _hadCamera)
        {
            _hadCamera = false;
            //посылаю эвент, что управляемое тело потеряно
            //  EventManager.TriggerEvent("LostCamera");
        }


#if UNITY_EDITOR
        // EditorControl();
#endif

#if UNITY_ANDROID
        // AndroidControl();
#endif




        // FadeActionArea(_jumpIsUsed);
        // FadeJumpArea(_actionIsUsed);
    }

    /*
    void EditorControl()
    {
        // мышку с компа всегда пишу в тач дата 1  _touchData[1]
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {// закеширую положение мышки
            Vector2 mPos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                //проверяю поподание в изолированые области
                for (int i = 0; i < _areas.Count; i++)
                {
                    if (_areas[i].isActiveAndEnabled && _areas[i].TouchInArea(mPos))
                    {
                        if (!_areas[i].inTouch)
                        {
                            if (!_singleInUse || (_singleInUse && !_areas[i].SingleTouchArea))
                            {
                                //---припарка с вращением камерой и новыми патернами
                                if (i == 3 && (_actionIsUsed || _jumpIsUsed))
                                    break;

                                //end-----припарка

                                if (_areas[i].SingleTouchArea)
                                    _singleInUse = true;
                                _areas[i].TouchStart(1, mPos);
                            }
                        }
                        // остановим for если зона уже используется, или начала использоваться                         
                        break;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {   // если отпустили мышку
                for (int i = 0; i < _areas.Count; i++)
                {
                    if (_areas[i]._fid == 1)
                    {
                        //if (_areas[i]._singleTouchArea)
                        if (_areas[i].SingleTouchArea)
                            _singleInUse = false;
                        _areas[i].TouchEnd(mPos);
                        break;
                    }
                }

            }
            else if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < _areas.Count; i++)
                {
                    if (_areas[i]._fid == 1)
                    {
                        _areas[i].TouchMove(mPos);
                    }
                }
            }
        }
    }
    */
    /*
    void AndroidControl()
    {
        Touch[] myTouches = Input.touches;
        foreach (Touch touch in myTouches)
        {
            int fid = touch.fingerId;
            Vector2 mPos = touch.position;
            switch (touch.phase)
            {
                case TouchPhase.Began:

                    for (int i = 0; i < _areas.Count; i++)
                    {
                        if (_areas[i].TouchInArea(mPos))
                        {
                            if (!_areas[i].inTouch)
                            {
                                if (!_singleInUse || (_singleInUse && !_areas[i].SingleTouchArea))
                                {
                                    //---припарка с вращением камерой и новыми патернами
                                    if (i == 3 && (_actionIsUsed || _jumpIsUsed))
                                        break;

                                    //end-----припарка
                                    if (_areas[i].SingleTouchArea)
                                        _singleInUse = true;
                                    _areas[i].TouchStart(fid, mPos);
                                }
                            }
                            // остановим for если зона уже используется, или начала использоваться                         
                            break;
                        }
                    }
                    break;
                case TouchPhase.Canceled:

                case TouchPhase.Ended:
                    for (int i = 0; i < _areas.Count; i++)
                    {
                        if (_areas[i]._fid == fid)
                        {
                            // if (_areas[i]._singleTouchArea)
                            if (_areas[i].SingleTouchArea)
                                _singleInUse = false;
                            _areas[i].TouchEnd(mPos);
                            break;
                        }
                    }
                    break;
                default:// TouchPhase.Moved, TouchPhase.Stationary
                    for (int i = 0; i < _areas.Count; i++)
                    {
                        if (_areas[i]._fid == fid)
                        {
                            _areas[i].TouchMove(mPos);
                        }
                    }
                    break;
            }
        }
    }

    */


    void FadeActionArea(bool boo)
    {
        //Color co = _actionImage.color;
        //if (boo)
        //{
        //    co.a -= 10 * Time.deltaTime;
        //    co.a = Mathf.Clamp(co.a, 0, _aColor.a);
        //    _actionImage.color = co;
        //}
        //else if (!_actionIsUsed && co.a < _aColor.a)
        //{
        //    co.a += 10 * Time.deltaTime;
        //    co.a = Mathf.Clamp(co.a, 0, _aColor.a);
        //    _actionImage.color = co;
        //}
    }
    void FadeJumpArea(bool boo)
    {
        //Color co = _jumpImage.color;
        //if (boo)
        //{
        //    co.a -= 10 * Time.deltaTime;
        //    co.a = Mathf.Clamp(co.a, 0, _jColor.a);
        //    _jumpImage.color = co;
        //}
        //else if (!_jumpIsUsed && co.a < _jColor.a)
        //{
        //    co.a += 10 * Time.deltaTime;
        //    co.a = Mathf.Clamp(co.a, 0, _jColor.a);
        //    _jumpImage.color = co;
        //}
    }

    public void TestExitGame()
    {
        Application.Quit();
    }

    //public void UsingAction(bool boo)
    //{
    //    _actionIsUsed = boo;
    //}
    //public void UsingJump(bool boo)
    //{
    //    _jumpIsUsed = boo;
    //}
    public void ActionAreaEnabler()
    {
        //_actionArea.gameObject.SetActive(_bp.Armed);
    }
    //public void UsingLook(bool boo)
    //{
    //    _lookIsUsed = boo;
    //}
    public float GetMouse(string str)
    {
        float rezult = 0;
        switch (str)
        {
            case "Horizontal":
                //rezult = (Mathf.Abs(_touchJoyValue.x) > Mathf.Abs(_keyboartJoyValue.x)) ? _touchJoyValue.x : _keyboartJoyValue.x;
                rezult = LookAxis.x;
                break;
            case "Vertical":
                // rezult = (Mathf.Abs(_touchJoyValue.y) > Mathf.Abs(_keyboartJoyValue.y)) ? _touchJoyValue.y : _keyboartJoyValue.y;
                rezult = LookAxis.y;
                break;
            case "MouseWheel":// пока нет наэкранного элемента управления на среднюю кнопку мыши
                rezult = Input.GetAxis("Mouse ScrollWheel");
                break;
        }
        return rezult;
    }
    public void UsingJoy(bool boo)
    {
        _joyIsUsed = boo;
    }

    public float GetAxis(string str)
    {
        Vector2 _keyboartJoyValue = new Vector2(0, 0);
        float rezult = 0;

        if (true)//(_fixKeyboard)
        {//значение клавиатуры принимает (-1;0;1) резко
            _keyboartJoyValue.x = Input.GetAxisRaw("Horizontal");
            _keyboartJoyValue.y = Input.GetAxisRaw("Vertical");
        }
        //else
        //{// стандартная клавиатура применяет сглаживание значения.			
        //    _keyboartJoyValue.x = Input.GetAxis("Horizontal");
        //    _keyboartJoyValue.y = Input.GetAxis("Vertical");
        //}
        switch (str)
        {
            case "Horizontal":
                //rezult = JoyAxis.x;
                rezult = (Mathf.Abs(JoyAxis.x) > Mathf.Abs(_keyboartJoyValue.x)) ? JoyAxis.x : _keyboartJoyValue.x;
                break;
            case "Vertical":
                //rezult = JoyAxis.y;
                rezult = (Mathf.Abs(JoyAxis.y) > Mathf.Abs(_keyboartJoyValue.y)) ? JoyAxis.y : _keyboartJoyValue.y;
                break;
        }

        return rezult;
    }
}


