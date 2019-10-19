using UnityEngine;
using System.Collections;

public class MyIsolatedLookArea : MyIsolatedArea
{
    public override void On_Awake() { }
    public override void OnStarted() { }
    public override void OnMoved() { }
    public override void OnEnded() { }

    //public float _deltaAcceleration = 0;
    //public bool _deltaAcceleration;
    //public Vector2 _touchPosDeltaEx = new Vector2();
    public override void OnUpdate()
    {
        ////if (Mathf.Abs( _deltaAcceleration)>0)
        //if (_deltaAcceleration)
        //{
        //    //_touchPosDelta.x = Mathf.Lerp(_touchPosDelta.x, _cd.GetMouse("Horizontal") , _deltaAcceleration * Time.deltaTime);
        //    //_touchPosDelta.y = Mathf.Lerp(_touchPosDelta.y, _cd.GetMouse("Vertical"), _deltaAcceleration * Time.deltaTime * 2);
        //    // _touchPosDelta.x = Mathf.Lerp(_touchPosDelta.x, _cd.GetMouse("Horizontal"), Time.deltaTime);
        //    // _touchPosDelta.y = Mathf.Lerp(_touchPosDelta.y, _cd.GetMouse("Vertical"), Time.deltaTime);
        //    _touchPosDeltaEx.x = Mathf.Lerp(_touchPosDeltaEx.x, _touchPosDelta.x, Time.deltaTime);
        //    _touchPosDeltaEx.y = Mathf.Lerp(_touchPosDeltaEx.y, _touchPosDelta.y, Time.deltaTime);
        //    _cd.LookAxis = _touchPosDeltaEx;
        //}
        //else {
         _cd.LookAxis = _touchPosDelta;
        //}
    }
}
