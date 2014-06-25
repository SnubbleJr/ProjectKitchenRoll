using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    public bool multiplePowerNeed = false;
    public int powerNumberNeeded;

    public OutputType currentState;

    float powerNumber, powerNeeded;

    //testing vecs
    Vector3 open, closed;
    Vector3 openVector = new Vector3(0, 10, 0);

	// Use this for initialization
    void Start()
    {
        closed = transform.position;
        open = transform.position + openVector;

        changeState(GetComponent<OutputComponentBehaviour>().outputState);

        if(multiplePowerNeed)
        powerNeeded = powerNumberNeeded;
	}
	
	// Update is called once per frame
	void Update () {
	}

    void changeState(OutputType state)
    {
        switch (state)
        {            
            case OutputType.ON:
                turnOn();
                break;
            case OutputType.OFF:
                turnOff();
                break;
            case OutputType.OTHER:
                specialCase();
                break;
            default:
                break;
        }
    }

    void turnOn()
    {
        if (multiplePowerNeed)
        {
            if (powerNumber < powerNumberNeeded)
            powerNumber++;

            if (powerNumber != powerNumberNeeded)
            {
                //test Vect
                transform.position = closed + ((powerNumber / powerNeeded) * openVector);

                return;
            }
        }

        transform.position = open;
        currentState = OutputType.ON;
    }

    void turnOff()
    {
        if (multiplePowerNeed)
        {
            if (powerNumber > 0)
                powerNumber--;

            if (powerNumber != 0)
            {
                //test Vect
                transform.position = closed + ((powerNumber / powerNumberNeeded) * openVector);

                return;
            }
        }

        transform.position = closed;
        currentState = OutputType.OFF;
    }

    //other trigger
    void specialCase()
    {
    }

}
