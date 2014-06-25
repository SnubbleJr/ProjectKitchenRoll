using UnityEngine;
using System.Collections;

public class InputComponentBehaviour : MonoBehaviour {

    public bool isActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void interact()
    {
        isActive = !isActive;
        SendMessage("changeState", isActive);
    }
}
