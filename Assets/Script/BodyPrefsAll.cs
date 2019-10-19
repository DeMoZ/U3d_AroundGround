using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[System.Serializable]
public class BodyPrefsAll : MonoBehaviour
{
    public GlobalEnum.ControlType _controlType;

    public float _currHp;     // текущее колво жизней  
    float _hp = 2;             // постоянное колво жизней (берем каждый рестат)
    public float Hp
    { get { return _hp; } }

    public float _curHeight = 15;
    public float _speed = 5;
    public float _acceleration = 5;

    [HideInInspector]
    public GameObject _gm;
    GMControlDriver _cd;
    [HideInInspector]
    public BodyMoveAll _bodyMove;
    [HideInInspector]
    public Transform _transform, _camera, _planet;
    [HideInInspector]
    public Rigidbody _rigidbody;

    SphereCollider _sC;

    bool _isUsed = false;
    public bool IsUsed
    { set { _isUsed = value; } get { return _isUsed; } }
    int _poolIndex = -1;
    public int PullIndex
    { set { _poolIndex = value; } get { return _poolIndex; } }
    GMPoolAll _pull;
    public GMPoolAll Pull
    { set { _pull = value; } get { return _pull; } }

    //public List<Transform> _enginePoints = new List<Transform>();
    //[HideInInspector]
    public List<ParticleSystem> _enginePS = new List<ParticleSystem>();

    [HideInInspector]
    public List<TrailRenderer> _engineTR = new List<TrailRenderer>();

    public List<ParticleSystem> _deadPoints = new List<ParticleSystem>();

    public ParticleSystem _core = new ParticleSystem();

    void Awake()
    //void Start()
    {
        AwakeToStart();
    }
    void Start()
    {
        if (_enginePS.Count > 0)
        {
            for (int i = 0; i < _enginePS.Count; i++)
            {
                //_enginePS.Add(_enginePoints[i].GetComponent<ParticleSystem>());
                _engineTR.Add(_enginePS[i].GetComponent<TrailRenderer>());
            }
        }

        VirtualStart();
    }

    public void AwakeToStart()
    {
        _transform = transform;
        _gm = GameObject.Find("GameManager");
        _cd = _gm.GetComponent<GMControlDriver>();

        _bodyMove = GetComponent<BodyMoveAll>();
        _sC = GetComponent<SphereCollider>();

        if (!_bodyMove)
        {
            Debug.LogWarning("на теле " + name + " отсутствует скрипт BodyMove. Добавлен автоматический.");
            _bodyMove = gameObject.AddComponent<BodyMoveAll>();
        }
        if (!_sC)
        {
            Debug.LogWarning("на теле " + name + " отсутствует SphereCollider. Добавлен автоматический.");
            _sC = gameObject.AddComponent<SphereCollider>();
        }

        _planet = _cd._planet;

        if (!_planet) _planet = GameObject.FindWithTag("Planet").transform;

        // чтобы обьект появился с жизнями
        if (_currHp == 0)
            _currHp = _hp;
        else _hp = _currHp;

        // if (!_gm || !_cd || !_bodyMove || !_sC || !_planet) return false;
    }

    public Transform GetCamera()
    {// запускается из скрипта BodyMovePlayer
     //  Debug.Log(" 1 " + (_cd == null));
     //  Debug.Log(" 2 " + (_cd._camera == null));
        if (_cd)
            return _cd._camera;
        else
            return null;
    }

    public void SetDamage(float dem)
    {
        _currHp -= dem;
        if (_currHp <= 0)
        {
            // Destroy(_transform);
            DestroyReturn();
        }
    }
    public void DestroyReturn()
    {
        if (PullIndex > -1)
            _pull.ReturnToPool(PullIndex);
        else // если у тела нету индекса пула, значит тело само по себе. Уничтожать
             //  Destroy(gameObject);
            Enabler(new Vector3(0, 0), Quaternion.identity, false);
    }
    public virtual void VirtualDestroy() { }
    public virtual void VirtualRefresh() { }

    /* / DEPRICATED 
        Transform CreateNotCamera()
    {
        GameObject notCams = GameObject.Find("NotCams");
        if (!notCams)
        {// еще не создавалось хранилище для не камер
            notCams = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            notCams.name = "NotCams";
            notCams.GetComponent<Collider>().enabled = false;
            notCams.GetComponent<MeshRenderer>().enabled = false;
        }

        GameObject notCam = GameObject.CreatePrimitive(PrimitiveType.Cube);
        notCam.name = "notCam";
        notCam.GetComponent<Collider>().enabled = false;
        notCam.GetComponent<MeshRenderer>().enabled = false;

        notCam.transform.SetParent(notCams.transform);

        return notCam.transform;
    }
    */
    public virtual float RotationSpeed
    { set { } get { return 0; } }
    //    { return 0; }
    public virtual float BoostModify
    { set { } get { return 0; } }
    public virtual void VirtualStart() { }
    public virtual void RecursionToTailHead(int i) { } // изспользуется в BodyPrefsBonus
    public virtual BodyPrefsBonus GetLeadBonus() { return null; } // изспользуется в BodyPrefsBonus
    //    { return 0; }
    // public virtual float BulletSize
    // { set { } get { return 0; } }
    public virtual void Enabler(Vector3 pos, Quaternion rot, bool enabled)   //disabled - отключить меши, коллайдеры и скрипты
    {
        IsUsed = enabled;

        _currHp = Hp;

        // _bodyMove.enabled = enabled;
        // _sC.enabled = enabled;
        gameObject.SetActive(enabled);
        _transform.position = pos;
        _transform.rotation = rot;

        if (enabled) VirtualRefresh();
        else
        {
            Debug.LogError("Виртуальное уничтожение  " + name);
            if (_deadPoints.Count > 0)
            {
                for (int i = 0; i < _deadPoints.Count; i++)
                {
                    if (_deadPoints[i])
                    {
                        _deadPoints[i].time = 0;
                        _deadPoints[i].Play();
                    }
                }
            }
            VirtualDestroy();
        }

    }
    public void PlayDestroyParticles()
    {
        if (_deadPoints.Count > 0)
        {
            for (int i = 0; i < _deadPoints.Count; i++)
            {
                if (_deadPoints[i])
                {
                    //_deadPoints[i].time = 0;
                    //_deadPoints[i].Play();
                    ParticleSystem ps = Instantiate(_deadPoints[i]);
                    AutodestroyInSeconds ad = ps.gameObject.AddComponent<AutodestroyInSeconds>();
                    ad._time = 2;
                    ps.transform.position = _deadPoints[i].transform.position;
                    ps.time = 0;
                    ps.Play();
                }
            }
        }
    }

    //public void TailTypes(float b, float g, float r, float w, float y) {}
    //public virtual void TailTypes(float b, float g, float r, float w, float y) { }
    
}
