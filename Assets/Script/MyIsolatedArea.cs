using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyIsolatedArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent _startTouchEvent;
    public UnityEvent _endTouchEvent;
    // public UnityEvent _sendTouchValue;

    public bool _inTouch;

    // Vector2 _sendValue = new Vector2(0, 0);

    public PointerEventData _pointerData;







    [HideInInspector]
    public Transform _transform;                              // указать сюда экшен зону (элемент UI)
    //************************************
    [HideInInspector]
    public GMControlDriver _cd;

    // [HideInInspector]
    // public BodyPrefs _bp;


    public Vector2 _touchPosStart = new Vector2(0, 0);          // начальное положение
                                                                // [HideInInspector]
    public Vector2 _touchPosCurrent = new Vector2(0, 0);        // текущее положение
                                                                // [HideInInspector]
                                                                // public Vector2 _touchPosEnd = new Vector2(0, 0);            // конечное положение
                                                                // [HideInInspector]
    public Vector2 _touchPosDelta = new Vector2(0, 0);          // дельта положение

    // void Awake()
    void Start()
    {
        _transform = transform;
        _cd = GameObject.Find("GameManager").GetComponent<GMControlDriver>();
        On_Awake();
    }

    public virtual void On_Awake() { }


    public void OnPointerDown(PointerEventData data)
    {
       // Debug.Log("тач по зоне");
        if (_startTouchEvent != null)
            _startTouchEvent.Invoke();

        _pointerData = data;
        _inTouch = true;
        _touchPosStart = data.position;
        _touchPosCurrent = data.position;
        OnStarted();
    }

    //works when release pattern btn
    public void OnPointerUp(PointerEventData data)
    {
        if (_endTouchEvent != null)
            _endTouchEvent.Invoke();

        _inTouch = false;
        _touchPosDelta = new Vector2(0, 0);
        OnEnded();
    }

    public void Update()
    {

        if (_inTouch)
        {
            //Debug.Log("Update in touch");
            _touchPosDelta = _touchPosCurrent - _pointerData.position;
            _touchPosCurrent = _pointerData.position;
        }

        OnUpdate();
    }

    virtual public void OnStarted() { }
    virtual public void OnMoved() { }
    virtual public void OnEnded() { }
    virtual public void OnUpdate() { }

    /*
    public override void On_Awake() { }
    public override void OnStarted() { }
    public override void OnMoved() { }
    public override void OnEnded() { }
    public override void OnUpdate() { }
    */
}
