using UnityEngine;
using System.Collections;

public class ScreenGrabber : MonoBehaviour {
	private bool firstFrameRendered = false;
	private Texture2D frameTexture;
	private float zOffset;
	private float zIncrement = 0.005f;
	private float timeToZReset;
	
	//Takes a picture of where it is, and then applies it to the object
	//due to screen dimensions and game dimensions being different, it has to calcualte what resolution it needs to be
	
	void Start ()
	{
		//frameTexture.alphaIsTransparency = true;
		setZToDefault();
	}

	void Update()
	{
		timeToZReset -= Time.deltaTime;
		if (timeToZReset < 0)
		{
			setZToDefault();
		}

		else
		{
			increaseZ();
		}
	}
	
	void OnPreRender() {
		
		if (firstFrameRendered)
		{	
			Destroy(frameTexture, timeToZReset);
		}
	}

	void OnPostRender() {

		if (firstFrameRendered)
		{	
			frameTexture = new Texture2D((int)camera.pixelWidth, (int)camera.pixelHeight);
			frameTexture.ReadPixels(camera.pixelRect,0,0);
			frameTexture.Apply();
		}
		else
		{
			firstFrameRendered = true;
		}
	}
	
	public Texture getTexture(float torchDestructionTime)
	{	
		setTimer (torchDestructionTime);
		return frameTexture;
	}
	
	public Vector2 uvFromWorldToScreen(Vector3 posistion)
	{
		//offset co ords so it's realitve to the camera
		Vector2 uv;
		Vector3 worldCoOrd;

		worldCoOrd = camera.WorldToScreenPoint(posistion);
		
		uv.x = ((worldCoOrd/camera.pixelWidth).x);
		uv.y = ((worldCoOrd/camera.pixelHeight).y);
		
		return uv;
	}

	void setZToDefault()
	{
		zOffset = 1f;
	}

	void increaseZ()
	{
		zOffset += zIncrement;
	}

	void setTimer(float time)
	{
		if (time > timeToZReset)
		{
			timeToZReset = time;
		}
	}

	public float getZOffset()
	{
		return zOffset;
	}
	
}