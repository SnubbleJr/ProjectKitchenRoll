using UnityEngine;
using System.Collections;

public class MainCameraBehaviour : MonoBehaviour {

	private Texture2D frameTexture;
	private float zOffset;
	private float zIncrement = 0.005f;
	private float timeToZReset;
    private int torchCount = 0;

	//Everything for the Main camera without actually getting thge texture

	void Start ()
	{
		//frameTexture.alphaIsTransparency = true;
		setZToDefault();
	}
	
	void Update () {
        if (torchCount <= 0)
		{
			setZToDefault();
		}
		
		else
		{
			increaseZ();
		}
	}

	public Vector2 uvFromWorldToScreen(Vector3 posistion)
	{
		//offset co ords so it's realitve to the camera
		Vector2 uv;

		Vector3 worldCoOrd = camera.WorldToViewportPoint(posistion);

        uv.x = worldCoOrd.x;
        uv.y = worldCoOrd.y;
		return uv;
	}
	
	void setZToDefault()
	{
		zOffset = 1f;
        torchCount = 0;
	}
	
	void increaseZ()
	{
		zOffset += zIncrement;
	}
	
	public float getZOffset()
	{
        torchCount++;
		return zOffset;
	}

    public void informTorchDestoryed()
    {
        torchCount--;
    }

}
