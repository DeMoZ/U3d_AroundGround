using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyPrefsPlayer : BodyPrefsAll
{
    public float _rotationSpeed = 200;
    public override float RotationSpeed
    {
        set { _rotationSpeed = value; }
        get { return _rotationSpeed; }
    }

    public float _boostModify = 2;          // модификатор внезапного ускорения
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

        if (_enginePS.Count > 0)
        {
            for (int i = 0; i < _enginePS.Count; i++)
            {
                //_enginePS.Add(_enginePoints[i].GetComponent<ParticleSystem>());
                _engineTR.Add(_enginePS[i].GetComponent<TrailRenderer>());
            }
        }
    }
    public override void Enabler(Vector3 pos, Quaternion rot, bool enabled)   //disabled - отключить меши, коллайдеры и скрипты
    {
        IsUsed = enabled;



        //  // _bodyMove.enabled = enabled;
        //  // _sC.enabled = enabled;

        //  gameObject.SetActive(enabled);
        // _transform.position = pos;
        // _transform.rotation = rot;

        if (enabled)
        {
            _currHp = Hp;
            VirtualRefresh();
        }
        else
        {
            Debug.LogError("Виртуальное уничтожение  " + name);
            PlayDestroyParticles();
            VirtualDestroy();
        }

        gameObject.SetActive(enabled);
    }
    //public override void TailTypes(float b, float g, float r, float w, float y)
    //{
    //    _gm .TailTypes.TailTypes(b, g, r, w, y);
    //}
}
