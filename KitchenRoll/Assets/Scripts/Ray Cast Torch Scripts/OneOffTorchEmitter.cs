using UnityEngine;
using System.Collections;

public class OneOffTorchEmitter : MonoBehaviour {

    public GameObject childTorch;
    public float width = 5f, height = 25f;
    public bool forward = false; //true = move left to right, false = opposite
    public float delay = 0f;
    public float energy = 5;
    public AudioClip sound;

    private int castFrequency = 4;
    private bool firstFrameRendered = false;

    void OnTriggerEnter()
    {
        Invoke("emitTorch", delay);
        AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, 0.5f);
    }

    void emitTorch()
    {
        if (firstFrameRendered)
        {
            GameObject torch = Instantiate(childTorch, transform.position + new Vector3(width*-2,0,0), transform.rotation) as GameObject;
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
            emitTorch();
        }
        this.enabled = false;
    }
}
