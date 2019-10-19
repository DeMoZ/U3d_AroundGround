using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    GameObject _gm;
    GMControlDriver _gmCD;
    GMMove _gmMove;
    public GMControlDriver GmCD
    {
        get { return _gmCD; }
    }

    GMHud _gmHud;
    public GMHud GmHud
    {
        get { return _gmHud; }
    }

    // от этого класса унаследуется все сценарии уровней
    void Awake()
    {
        _gm = GameObject.Find("GameManager");

        if (!_gm)
        {
            Debug.LogWarning("не найден обьек GameManager. Сценарий уровня отключен");
            return;
        }
        _gmCD = _gm.GetComponent<GMControlDriver>();
        _gmHud = _gm.GetComponent<GMHud>();
        _gmMove = _gm.GetComponent<GMMove>();

        VirtualAwake();
    }
    public virtual void VirtualAwake() { }

    void Start()
    {
        if (!_gm) return;
        _gmMove._allowMove = true;

        VirtualStart();
    }
    public virtual void VirtualStart() { }

    void Update()
    {
        if (!_gm) return;
        VirtualUpdate();
    }
    public virtual void VirtualUpdate() { }
}
