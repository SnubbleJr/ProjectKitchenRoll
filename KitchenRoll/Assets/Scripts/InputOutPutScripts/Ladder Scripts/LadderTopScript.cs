using UnityEngine;
using System.Collections;

public class LadderTopScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Player") {
			if (Input.GetKeyDown(KeyCode.S)) {
				other.transform.position -= new Vector3(0, 10, 0);
			}
		}
	}
}
