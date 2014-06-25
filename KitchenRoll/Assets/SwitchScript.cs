using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        changeState(GetComponent<InputComponentBehaviour>().isActive);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void changeState(bool active)
    {
        if (active)
            renderer.material.color = Color.yellow;
        else
            renderer.material.color = Color.blue;
    }
}
