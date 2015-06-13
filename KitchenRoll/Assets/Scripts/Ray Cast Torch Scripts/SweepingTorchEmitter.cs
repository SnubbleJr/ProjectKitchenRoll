using UnityEngine;
using System.Collections;

public class SweepingTorchEmitter : MonoBehaviour
{

    //default values
    public GameObject childTorch;
    public float width = 5f, height = 25f;
    public bool forward = false; //true = move left to right, false = opposite
    public float torchesPerSecond = 1f;
    public float delay = 0f;
    public float energy = 5;
    private int castFrequency = 4;

    private float range;
    private GameObject player;
    private bool firstFrameRendered = false;

    // Use this for initialization
    void Start()
    {
        range = 75;
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("emitTorch", delay, (1 / torchesPerSecond));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void emitTorch()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < range)
        {
            if (firstFrameRendered)
            {
                GameObject torch = Instantiate(childTorch, transform.position, transform.rotation) as GameObject;
                SweepingTorch torchScript = torch.GetComponent<SweepingTorch>();
                torch.transform.parent = transform;
                torchScript.setWidth(width);
                torchScript.setHeight(height);
                torchScript.setDirection(forward);
                torchScript.setEnergy(energy);
                torchScript.setCastFrequency(castFrequency);
            }
            else
            {
                firstFrameRendered = true;
            }
        }
    }
}
