using UnityEngine;
using System.Collections;

public class TextureGettest : MonoBehaviour {

	public GameObject cam;

	RenderTextureGrabber RTG;

	// Use this for initialization
	void Start () {
	
		RTG = cam.GetComponent<RenderTextureGrabber>();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump"))
		{
			renderer.material.mainTexture = RTG.getTexture(2f);
		}
	}
}
