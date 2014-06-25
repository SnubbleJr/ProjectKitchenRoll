using UnityEditor;
using UnityEngine;
using System.Collections;

public class InputManagerBehaviour : MonoBehaviour {

    public logicComponent[] logic;

    private logicComponent[] logicCopy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        validateLogic();
	}

    //errors if logic is bad, else fires off output if ok
    void validateLogic()
    {
        
        for (int i = 0; i < logic.Length; i++)
        {
            for (int j = 0; j < logic[i].inputOutputComponents.Length; j++)
            {
                sendOutput(logic[i].inputOutputComponents[j]);
            }
        }
    }

    //returns true if all conditions for this inpput are met
    bool evaluateIOC(InputComponent[] inComp)
    {
        bool result = false;

        for (int i=0; i < inComp.Length; i++)
        {
            result = inComp[i].input.GetComponent<InputComponentBehaviour>().isActive;

            //invert if not is set
            if (inComp[i].not)
            {
                result = !result;
            }

            if (!result)
            {
                return false;
            }
        }
        return true;
    }

    //only send output if it already hasn't been sent
    void sendOutput(InputOutputComponent IOC)
    {
        if (evaluateIOC(IOC.inputComponents))
        {
            if (IOC.outputState != IOC.getLastSent())
            {
                IOC.output.SendMessage("output", IOC.outputState);
                IOC.setLastSent(IOC.outputState);
            }
        }
        else
        {
            if (IOC.defaultState != IOC.getLastSent())
            {
                IOC.output.SendMessage("output", IOC.defaultState);
                IOC.setLastSent(IOC.defaultState);
            }
        }
    }
    
}

[System.Serializable]
public class InputComponent
{
    public bool not;
    public GameObject input;
}

[System.Serializable]
public class InputOutputComponent
{
    public InputComponent[] inputComponents;
    public GameObject output;
    public OutputType outputState;
    public OutputType defaultState = OutputType.OFF;

    private OutputType lastSentOutput;

    public OutputType getLastSent()
    {
        return lastSentOutput;
    }

    public void setLastSent(OutputType LSO)
    {
        lastSentOutput = LSO;
    }
}

[System.Serializable]
public class logicComponent
{
    public string comment;
    public InputOutputComponent[] inputOutputComponents;
}