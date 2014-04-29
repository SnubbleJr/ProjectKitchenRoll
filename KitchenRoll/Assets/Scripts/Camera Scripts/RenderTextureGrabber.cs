using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderTextureGrabber : MonoBehaviour {

	[System.Serializable]
	public class RT
	{
		public RenderTexture renderTexture;
		public bool locked;

		public RT()
		{
			renderTexture = new RenderTexture(1024,1024,24);
			locked = false;
		}

	}

	private List<RT> RTList;
	private int RTIndex, lockCount;

	// Use this for initialization
	void Start () {

		RTList = new List<RT>();
		RTIndex = 0;
		lockCount = 0;

	}

	void OnPreRender () {

		//If there are no unlocked RTs
		if(RTList.Count <= lockCount)
		{
			RTList.Add(new RT());
		}
		cycleRT();
	}

	// Update is called once per frame
	void Update () {

	}

	void cycleRT()
	{
		RTIndex = getNextUnlocked();
		RTSetActive();
	}

	void RTSetActive()
	{
		RenderTexture.active = RTList[RTIndex].renderTexture;
	}

	public RenderTexture getTexture(float torchDestructionTime)
	{
		lockRT(RTIndex);
		StartCoroutine(unlockRT(RTIndex, torchDestructionTime));
		return RTList[RTIndex].renderTexture;
	}

	int getNextUnlocked()
	{
		int index = 0;

		for (int i=0; i < RTList.Count; i++)
		{
			if (RTList[i].locked == false)
			{
				index = i;
				return index;
			}
		}
		return index;
	}

	void lockRT(int index)
	{		
		RTList[index].locked = true;
		lockCount++;
	}

	IEnumerator unlockRT(int index, float time)
	{
		yield return new WaitForSeconds(time);
		RTList[index].locked = false;
		lockCount--;
	}

}
