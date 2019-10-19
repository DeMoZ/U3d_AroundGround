using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    [Tooltip("если не укзать обьект, то возьмется gameObject")]
    public Transform _transform;
    public Vector3 _rot = new Vector3(0, 0);
    void Start()
    {
        if (!_transform)
        {
            _transform = transform;
        }
        _rot = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _rot = _rot.normalized;
    }

    void FixedUpdate()
    {
        if (_transform)
        {
            //_rot *= 100 * Time.fixedDeltaTime;
            //Quaternion q = Quaternion.Euler(new Vector3(_transform.rotation.eulerAngles.x + _rot.x,
            //    _transform.rotation.eulerAngles.y + _rot.y,
            //    _transform.rotation.eulerAngles.z + _rot.z));//FromToRotation(_transform.up, _rot * Time.fixedDeltaTime / 100);
            //_transform.rotation = q;

            _transform.Rotate(_rot, 100 * Time.fixedDeltaTime);

        }
    }
}
