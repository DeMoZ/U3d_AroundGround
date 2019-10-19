using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyPrefsBot : BodyPrefsAll
{

    //  [Tooltip("только для управляемых обьектов")]
    public float _rotationSpeed = 200;
    //  [Tooltip("только для управляемых обьектов")]
    public float _boostModify = 2;          // модификатор внезапного ускорения
    public override float RotationSpeed
    {
        set { _rotationSpeed = value; }
        get { return _rotationSpeed; }
    }

    public override float BoostModify
    {
        set { _boostModify = value; }
        get { return _boostModify; }
    }

    public List<Transform> _shootPoints = new List<Transform>();
    [HideInInspector]
    public GMPoolGBullet _pullGBulet;

    //public List<Transform> _enginePoints = new List<Transform>();
    //[HideInInspector]
    //public List<ParticleSystem> _enginePS = new List<ParticleSystem>();
    //[HideInInspector]
    //public List<TrailRenderer> _engineTR = new List<TrailRenderer>();

    public override void VirtualStart()
    {
        _pullGBulet = _gm.GetComponent<GMPoolGBullet>();

        //if (_enginePoints.Count > 0)
        //{
        //    for (int i = 0; i < _enginePoints.Count; i++)
        //    {
        //        _enginePS.Add(_enginePoints[i].GetComponent<ParticleSystem>());
        //        _engineTR.Add(_enginePoints[i].GetComponent<TrailRenderer>());
        //    }
        //}

        //if (_enginePS.Count > 0)
        //{
        //    for (int i = 0; i < _enginePS.Count; i++)
        //    {
        //        //_enginePS.Add(_enginePoints[i].GetComponent<ParticleSystem>());
        //        _engineTR.Add(_enginePS[i].GetComponent<TrailRenderer>());
        //    }
        //}
    }
}
