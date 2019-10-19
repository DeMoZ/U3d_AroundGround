using UnityEngine;
using System.Collections;

public class BodyPrefsGBullet : BodyPrefsAll
{
    public float _bulletSize = 1;
    public float _bulletDamage = 1;
    public float _bulletTime = 5;
    public float _sinkRadius = 2;
    public float _sinkForce = 2;
    public float _sinkTime = 1;
    [HideInInspector]
    public float _bulletTimer = 0;
    //[HideInInspector]
    public Transform _owner;

    public override void VirtualDestroy()
    {
        _owner = null;
    }
    public override void VirtualRefresh()
    {
        _owner = null;

    }
    //public override float BulletSize
    //{
    //    set { _bulletSize = value; }
    //    get { return _bulletSize; }
    //}

}
