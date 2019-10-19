using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetGenerator : MonoBehaviour
{
    public List<Transform> _planetParts = new List<Transform>();

    List<Vector3> _partRotations = new List<Vector3>
    {
        new Vector3(0,0,0),
        new Vector3(90,0,0),
        new Vector3(180,0,0),
        new Vector3(270,0,0),
        new Vector3(0,90,0),
        new Vector3(0,270,0)
    };

    List<float> _zRotation = new List<float>
    {
        0,90,180,270
    };

    public List<SpherePart> _planet;
    [System.Serializable]
    public class SpherePart
    {
        public bool _freese;
        public Transform _part;
        public Vector3 _rot;

        public SpherePart(Transform parent, Transform part, Vector3 rot)
        {
            _freese = false;
            _part = Instantiate(part);
            _part.parent = parent;
            _part.position = new Vector3(0, 0, 0);
            _rot = rot;
            _part.eulerAngles = _rot;
        }
    }
    // Use this for initialization
    void Start()
    {
        //if (_planetParts.Count > 0)
        //{
        //    for (int i = 0; i < _partRotations.Count; i++)
        //    {
        //        Transform goT = Instantiate(_planetParts[Random.Range(0, _planetParts.Count)]);
        //        goT.parent = gameObject.transform;
        //        goT.position = new Vector3(0, 0, 0);

        //        // goT.eulerAngles = _partRotations[i];

        //        float zRot = _zRotation[Random.Range(0, _zRotation.Count)];
        //        Vector3 rot = _partRotations[i];
        //        rot = new Vector3(rot.x, rot.y, zRot);
        //        goT.eulerAngles = rot;
        //    }
        //}
        BuildSphere();
    }

    //[MenuItem("MyMenu/Do Something")]
    //[AddComponentMenu("aaa")]
    [ContextMenu("BuildSphere")]
    public void BuildSphere()
    {
        if (_planetParts.Count > 0)
        {
            for (int i = 0; i < _partRotations.Count; i++)
            {


                //Transform goT = Instantiate(_planetParts[Random.Range(0, _planetParts.Count)]);
                //goT.parent = gameObject.transform;
                //goT.position = new Vector3(0, 0, 0);

                //float zRot = _zRotation[Random.Range(0, _zRotation.Count)];
                //Vector3 rot = _partRotations[i];
                //rot = new Vector3(rot.x, rot.y, zRot);
                //goT.eulerAngles = rot;

                //float zRot = _zRotation[Random.Range(0, _zRotation.Count)];


                SpherePart sp;//= CreateSpherePart(i);

                if (_planet.Count < i + 1)
                {
                    sp = CreateSpherePart(i);
                    _planet.Add(sp);
                }
                else
                {
                    if (_planet[i] == null || _planet[i]._part == null || !_planet[i]._freese)
                    {
                        sp = CreateSpherePart(i);
                        if (_planet[i]._part != null)
                        {
                            DestroyImmediate(_planet[i]._part.gameObject);
                        }
                        _planet[i] = sp;
                    }
                }
                // если часть защищена от изменений, то доворачиваю ее по z на велличину(на случай, если изменялось значение)
                if (_planet[i]._freese)
                {
                    Vector3 rot = _partRotations[i];
                    rot = new Vector3(rot.x, rot.y, _planet[i]._rot.z);
                    _planet[i]._part.eulerAngles = rot;
                }

            }
        }
    }
    SpherePart CreateSpherePart(int i)
    {

        Vector3 rot = _partRotations[i];
        rot = new Vector3(rot.x, rot.y, _zRotation[Random.Range(0, _zRotation.Count)]);
        SpherePart sp = new SpherePart(gameObject.transform, _planetParts[Random.Range(0, _planetParts.Count)], rot);
        return sp;
    }
}
