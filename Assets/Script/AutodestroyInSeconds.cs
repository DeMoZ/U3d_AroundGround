using UnityEngine;
using System.Collections;

public class AutodestroyInSeconds : MonoBehaviour
{
    public float _time = 10;
    // Use this for initialization
    void Start()
    {
        Invoke("AutodestroyInSecconds", _time);
    }
    void AutodestroyInSecconds()
    {
        Destroy(gameObject);
    }
}
