using UnityEngine;
using System.Collections;

public class SlowlyRotate : MonoBehaviour {

	public Camera cam;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Time.deltaTime*10, 0, 0);
		transform.Rotate(0, Time.deltaTime*10, 0, Space.World);
	}
}
