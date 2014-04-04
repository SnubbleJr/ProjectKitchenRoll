
using UnityEngine;
using System.Collections;
	
public class RayCastTorchBackground : MonoBehaviour {

	LineRenderer BG;

	//default values
	private float maxSize ;
	private int castFrequency;
	private float destructionTime = 2f;
	private Vector3 offset = new Vector3 (0,256,0);
	private bool spawnAgain = true;
	private bool cone = false;
	private float coneFrom, coneTo;
	
	ScreenGrabber mainCameraGrabber;

	float growRateBG;

	Vector3[] vecArr;

	private float currentGrowRate;
	private float currentSize;
	private float timerSize;

	void Start () {


		BG = GetComponent<LineRenderer>();

		growRateBG = 1f;

		BG.SetVertexCount(castFrequency+1);
		BG.SetWidth(maxSize/25, maxSize/25);

		setScale(growRateBG);
	}

	void Update () {
		//Used to be a lot faster, but the slowness is niceish


		//Any linesize above 1.05 is really trippy
		
		//growRateBG *= 0.5f;
		//growRateBG *= 0.7f;
		
		growRateBG *= 1.007f;

		//BG.SetWidth(growRateBG, growRateBG);

		//transform.localScale += new Vector3(growRateBG,growRateBG,growRateBG);
		setScale(growRateBG);
	}

	void setScale (float scale)
	{
		for (int i=0; i < vecArr.Length-1; i++)
		{
			BG.SetPosition(i, (vecArr[i]*scale));
		}
		
		BG.SetPosition(vecArr.Length-1, (vecArr[0]*scale));

	}

	public void setMaxSize(float size)
	{
		maxSize = size;
	}
	
	public float getMaxSize()
	{
		return maxSize;
	}
	
	public void setCastFrequency(int freq)
	{
		castFrequency = freq;
	}
	
	public int getCastFrequency()
	{
		return castFrequency;
	}
	
	public void setDestructionTime(float time)
	{
		destructionTime = time;
	}
	
	public float getDestructionTime()
	{
		return destructionTime;
	}
	
	public void setOffset(Vector3 off)
	{
		offset = off;
	}
	
	public Vector3 getOffset()
	{
		return offset;
	}
	
	public void setSpawnAgain(bool spawn)
	{
		spawnAgain = spawn;
	}
	
	public bool getSpawnAgain()
	{
		return spawnAgain;
	}
	
	public void setCone(bool con)
	{
		cone = con;
	}
	
	public bool getCone()
	{
		return cone;
	}
	
	public void setConeFrom(float from)
	{
		coneFrom = from;
	}
	
	public float getConeFrom()
	{
		return coneFrom;
	}
	
	public void setConeTo(float to)
	{
		coneTo = to;
	}
	
	public float getConeTo()
	{
		return coneTo;
	}
	
	public void setVecArr(Vector3[] arr)
	{
		vecArr = arr;
	}
	
	public Vector3[] getVecArr()
	{
		return vecArr;
	}

}
