using UnityEngine;
using System.Collections;

public class RayCasTorchEmitter : MonoBehaviour {
	
	//default values
	public GameObject childTorch;
	public float torchesPerSecond = 1f;
	public float maxSize = 5;
	public float growRate = 0.01f;
	public int castFrequency = 64	;
	public float destructionTime = 3f;
	public Vector3 offset = new Vector3 (0,256,0);
	public bool cone;
	public float coneFrom, coneTo;

	private float range;
	private GameObject player;
		
	// Use this for initialization
	void Start () {
		range = 75;	
		player = GameObject.FindGameObjectWithTag ("Player");
		InvokeRepeating("emitTorch", 2, (1/torchesPerSecond));
	}

	// Update is called once per frame
	void Update () {

	}

	void emitTorch()
	{
		if (Vector3.Distance(player.transform.position, transform.position) < range)
		{
			GameObject torch = Instantiate (childTorch, transform.position + new Vector3 (0, 0, 0), transform.rotation) as GameObject;
			RayCastTorch torchScript = torch.GetComponent<RayCastTorch>();
			torchScript.setMaxSize (maxSize);
			torchScript.setGrowRate (growRate);
			torchScript.setCastFrequency (castFrequency);
			torchScript.setDestructionTime (destructionTime);
			torchScript.setOffset (offset);
			torchScript.setCone (cone);
			torchScript.setConeFrom (coneFrom);
			torchScript.setConeTo (coneTo);
		}
	}
}
