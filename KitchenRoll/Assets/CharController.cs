using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

    bool inTrigger = false;
    Collider activeCollider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Use"))
        {
            if (inTrigger)
            {
                activeCollider.gameObject.SendMessage("interact");
            }
            else
            {
                print("nothing there!");
            }
        }
	
	}

    void OnTriggerEnter(Collider collision)
    {
        inTrigger = true;
        activeCollider = collision;
    }

    void OnTriggerExit()
    {
        inTrigger = false;
    }
}
