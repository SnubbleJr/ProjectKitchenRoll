using UnityEngine;
using System.Collections;

public class OutputComponentBehaviour : MonoBehaviour {

    public OutputType outputState = OutputType.OFF;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void output(OutputType state)
    {
        outputState = state;
        SendMessage("changeState", state);
    }
}
