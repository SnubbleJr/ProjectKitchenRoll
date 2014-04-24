
using UnityEngine;
using System.Collections;
	
public class RayCastTorchBackground : MonoBehaviour {

	private bool alternateVersion;

	LineRenderer BG;

	private float maxSize ;
	private int castFrequency;
	private Vector3 offset;

	float growRateBG;

	Vector3[] vecArr;

	void Start () {

		offset = DisplayCameraOffset.offset;

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
		
		if(alternateVersion)
		{
			growRateBG *= 1.007f;
			setScale(growRateBG);
		}
		else
		{
			setScale();
		}

		//BG.SetWidth(growRateBG, growRateBG);
		//transform.localScale += new Vector3(growRateBG,growRateBG,growRateBG);

	}

	public void setScale(float scale)
	{
		for (int i=0; i < vecArr.Length-1; i++)
		{
			BG.SetPosition(i, (vecArr[i]*scale));
		}
		
		BG.SetPosition(vecArr.Length-1, (vecArr[0]*scale));
		
	}

	public void setScale()
	{
		setScale(1f);
		
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

	public void setOffset(Vector3 off)
	{
		offset = off;
	}
	
	public Vector3 getOffset()
	{
		return offset;
	}

	public void setVecArr(Vector3[] arr)
	{
		vecArr = arr;
	}
	
	public Vector3[] getVecArr()
	{
		return vecArr;
	}

	public void setAlternateVersion(bool AV)
	{
		alternateVersion = AV;
	}
	
	public bool getAlternateVersion()
	{
		return alternateVersion;
	}

}
