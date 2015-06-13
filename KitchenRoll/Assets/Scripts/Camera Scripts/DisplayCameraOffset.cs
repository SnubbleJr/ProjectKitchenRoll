using UnityEngine;
using System.Collections;

public class DisplayCameraOffset : MonoBehaviour {

    public int YOffset = 255;
    public static Vector3 offset;

	// Use this for initialization
	void Start () {
	}

	void Awake ()
	{
        offset = new Vector3(0, (float)YOffset, 0);
		transform.localPosition = offset;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
