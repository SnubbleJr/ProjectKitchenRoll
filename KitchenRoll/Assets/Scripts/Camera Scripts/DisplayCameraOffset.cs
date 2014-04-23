using UnityEngine;
using System.Collections;

public class DisplayCameraOffset : MonoBehaviour {

	public static Vector3 offset = new Vector3 (0,256,0);

	// Use this for initialization
	void Start () {
	
	}

	void Awake ()
	{
		transform.localPosition = offset;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
