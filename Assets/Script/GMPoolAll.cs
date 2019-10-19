using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GMPoolAll : MonoBehaviour
{
    public int _poolSize = 10;
    public GameObject _prefab;

    //List<Transform> _pool = new List<Transform>();
    List<BodyPrefsAll> _pool = new List<BodyPrefsAll>();
    List<int> _poolFree = new List<int>();
    Transform _parent;
    // выдаю свободный обьект из пула
    //public Transform GetFromPool()
    //{ return null; }
    //float _timeForBotDelta = 0;                                             // время на начало расчета апдейта бота    

    void Start()
    {
        GameObject go = new GameObject();
        _parent = go.transform;
        go.name = GetType().ToString();

        SetPool(_poolSize);
        //StartCoroutine(PoolFixedUpdate());
    }
    public BodyPrefsAll GetFromPool()
    {
        if (_pool.Count < 1) return null;
        int index = 0;
        for (int i = _poolFree.Count - 1; i >= 0; i--)
        {
            if (!_pool[_poolFree[i]].IsUsed)
            {
                index = _poolFree[i];
                _poolFree.RemoveAt(i);
                return _pool[index];
            }
        }
        SetPool(1);
        index = _poolFree[_poolFree.Count - 1];
        _poolFree.RemoveAt(_poolFree.Count - 1);
        return _pool[index];
    }
    public void ReturnToPool(int index)
    {
        _pool[index].IsUsed = false;
        _pool[index].Enabler(new Vector3(0, 0), Quaternion.identity, false);
        _pool[index]._rigidbody.velocity = new Vector3(0, 0);
        _poolFree.Add(index);
        //Debug.Log("возвращен индекс в пул");
    }
    // при старtе создаю пул обьектов с выключенными параметрами
    void SetPool(int numb)
    {
        if (!_prefab) { Debug.Log("в пуле " + this + " не задан префаб"); return; }
        for (int i = 0; i < numb; i++)
        {
            GameObject go = Instantiate(_prefab);
            go.name = _prefab.name;
            BodyPrefsAll bpAll = go.GetComponent<BodyPrefsAll>();
            bpAll.Enabler(new Vector3(0, 0), Quaternion.identity, false);
            bpAll._transform.SetParent(_parent);
            //bpAll._controlType = GlobalEnum.ControlType.botManager;
            bpAll._controlType = GlobalEnum.ControlType.botSelf;

            _pool.Add(bpAll);
            bpAll.PullIndex = _pool.Count - 1;
            //bpAll._rigidbody.velocity = new Vector3(0, 0);

            bpAll.Pull = this as GMPoolAll;
            _poolFree.Add(_pool.Count - 1);
        }

    }
    IEnumerator PoolFixedUpdate()
    {
        int a = 0;
        float b = 0;
        while (true)
        {
            // записываю время начала обработки массивов
            float timeForBotDelta = Time.time;
            for (int i = _pool.Count - 1; i >= 0; i--)
            {
                a = i / 5;
                b = i / 5;
                if (i > 0 && a == b)
                //if (i > 0 && (int)i / 5 == i / 5f)
                {
                    //yield return new WaitForFixedUpdate();
                    yield return null;
                }
                //if (_pool[i].IsUsed)
                _pool[i]._bodyMove.UpdateBot(Time.time - timeForBotDelta + Time.deltaTime);
            }
            yield return null;
            //yield return new WaitForEndOfFrame();
            //yield return new WaitForFixedUpdate();
        }
    }

}
