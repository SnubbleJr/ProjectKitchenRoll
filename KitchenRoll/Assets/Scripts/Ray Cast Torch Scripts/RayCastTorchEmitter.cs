using UnityEngine;
using System.Collections;

public class RayCastTorchEmitter : MonoBehaviour {
	
	//default values
	public GameObject childTorch;
	public float torchesPerSecond = 1f;
	public float delay = 0f;
	public float energy = 5;
	public int castFrequency = 64;
	public bool cone;
	public float coneTo, coneFrom;

	private float range;
	private GameObject player;
	private bool firstFrameRendered = false;	

	// Use this for initialization
	void Start () {
		range = 75;	
		player = GameObject.FindGameObjectWithTag ("Player");
		InvokeRepeating("emitTorch", delay, (1/torchesPerSecond));
	}

	// Update is called once per frame
	void Update () {

	}

	void emitTorch()
	{
		if (Vector3.Distance(player.transform.position, transform.position) < range)
		{
			if (firstFrameRendered)
			{
				GameObject torch = Instantiate (childTorch, transform.position + new Vector3 (0, 0, 0), transform.rotation) as GameObject;
				RayCastTorch torchScript = torch.GetComponent<RayCastTorch>();
                torch.transform.parent = transform;
				torchScript.setEnergy (energy);
				torchScript.setCastFrequency (castFrequency);
				torchScript.setCone (cone);
				torchScript.setConeFrom (coneFrom);
				torchScript.setConeTo (coneTo);
			}
			else
			{
				firstFrameRendered = true;
			}
		}
	}
}
