using UnityEngine;
using System.Collections;

public class DevStaf : MonoBehaviour
{
		public bool _showSensor = true;
		public GameObject[] _holoarr ;
		public GameObject[] _arr;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}


		[ContextMenu("HideShowEnemySensor")]
		void HideShowEnemySensor ()
		{
				//_arr=new GameObject[];
				_arr = GameObject.FindGameObjectsWithTag ("EnemySensor");
				if (_showSensor) {
						foreach (GameObject go in _arr) {
								go.GetComponent<Renderer>().enabled = true;
						}
				} else {
						foreach (GameObject go in _arr) {
								go.GetComponent<Renderer>().enabled = false;
						}
				}
		}




		[ContextMenu("FeelList1")]
		void FeelList1 ()
		{
				_arr = GameObject.FindGameObjectsWithTag ("Halo0(Clone)");
				_arr = GameObject.FindGameObjectsWithTag ("Halo1(Clone)");
				_arr = GameObject.FindGameObjectsWithTag ("Halo2(Clone)");
				_arr = GameObject.FindGameObjectsWithTag ("Halo3(Clone)");
				_arr = GameObject.FindGameObjectsWithTag ("Halo4(Clone)");
				_arr = GameObject.FindGameObjectsWithTag ("Halo5(Clone)");
				_arr = GameObject.FindGameObjectsWithTag ("Halo6(Clone)");


		}
		[ContextMenu("DeleteCurrentHalo2")]
		void DeleteCurrentHalo2 ()
		{
				foreach (GameObject go in _arr) {
						//	Destroy (go.GetComponent ("Halo"));
						//if (go.GetComponent ("Halo") != null) {
						//DestroyImmediate (go);//.GetComponent ("Halo"));
						Destroy (go);
						//}
				}

		}

		[ContextMenu("AddHalo3")]
		void AddHalo3 ()
		{
				foreach (GameObject obj in _arr) {
						for (int i=0; i<_holoarr.Length; i++) {
								GameObject go = (GameObject)Instantiate (_holoarr [i]);
								go.transform.parent = obj.transform;
								go.transform.localPosition = new Vector3 (0, 0, 0);
								go.SetActive (false);
						}
				}
		
		}






}
