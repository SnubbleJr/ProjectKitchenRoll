using UnityEngine;
using System.Collections;

public class LadderBottomScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Player") {
			Debug.Log ("hi player, im the bottom of the ladder");
			if (Input.GetKeyDown(KeyCode.W)) {
				other.transform.position = new Vector3(44, -1, -10);
			}
		}
	}
}
