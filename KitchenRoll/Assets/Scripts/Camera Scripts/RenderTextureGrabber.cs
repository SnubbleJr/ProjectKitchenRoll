using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderTextureGrabber : MonoBehaviour {

    //Each RT has:
    //A Texture, that is cloned when asked for
    //A Camera, which give accurate uv conversions
    //A Lock, which stops the texture from updating
	[System.Serializable]
	public class RT
	{
		public RenderTexture renderTexture;
        public GameObject RTCamera;
		public bool locked;

		public RT()
        {
			renderTexture = new RenderTexture(1024,1024,-1);
			locked = false;
		}

	}

    public GameObject rtCamera;

	private List<RT> RTList;
    private List<GameObject> cameraList;
	private int RTIndex, lockCount;
    
	// Use this for initialization
	void Start () {

		RTList = new List<RT>();
        cameraList = new List<GameObject>();
		RTIndex = 0;
		lockCount = 0;

	}

	void OnPreRender () {

		//If there are no unlocked RTs
		if(RTList.Count <= lockCount)
		{
			RTList.Add(new RT());
            cameraList.Add(Instantiate(rtCamera, transform.position, transform.rotation) as GameObject);
		}
		cycleRT();
	}

	// Update is called once per frame
	void Update () {
	}

	void cycleRT()
	{
		RTIndex = getNextUnlocked();
		RTSetActive(RTIndex);
	}

	void RTSetActive(int index)
	{
        cameraList[index].transform.position = transform.position;
		RenderTexture.active = RTList[index].renderTexture;
	}

	public RenderTexture getTexture(float torchDestructionTime)
	{
		lockRT(RTIndex);
		StartCoroutine(unlockRT(RTIndex, torchDestructionTime));
		return RTList[RTIndex].renderTexture;
	}

    public MainCameraBehaviour getCurrentCameraBehaviour()
    {
        return cameraList[RTIndex].GetComponent<MainCameraBehaviour>();
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
        if (RTList[index].locked != true)
        {
            RTList[index].locked = true;
            lockCount++;
        }
	}

	IEnumerator unlockRT(int index, float time)
	{
		yield return new WaitForSeconds(time);
		RTList[index].locked = false;
		lockCount--;
	}

    void deactivateRT()
    {
        RenderTexture.active = null;
    }

}
