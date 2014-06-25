using UnityEngine;
using System.Collections;
using System.Linq;

public class RayCastTorch : MonoBehaviour {

	public GameObject torchBG;

	public bool alternateVersion = false;

	//default values
	private float maxSize = 15;
	private float growRate = 0.01f;
	private int castFrequency = 64;
	private float destructionTime = 2f;
	private Vector3 offset;
	private bool spawnAgain = true;
	private bool cone = false;
	private float coneTo, coneFrom;

    MainCameraBehaviour mainCamerBehaviour, RTCamerBehaviour;
	RenderTextureGrabber mainCameraGrabber;

	RayCastTorchBackground torchBGScript;

    double uvVecRatioX, uvVecRatioY, uvVecRatioMaxX, uvVecRatioMaxY;

	Mesh mesh = new Mesh();	
	Vector3[] maxVecArr;
	Vector3[] vecArr;
	Vector2[] uvArr;
	int[] triArr;

	private float currentGrowRate;
	private float currentSize;
	private float timerSize;
	
	void Start () {

        mainCamerBehaviour = Camera.main.GetComponent<MainCameraBehaviour>();
		mainCameraGrabber = Camera.main.GetComponent<RenderTextureGrabber>();
        offset = DisplayCameraOffset.offset;

        setTexture();

		makeTorch();

		//moving it to the top plane
		offset.z = mainCamerBehaviour.getZOffset ();
		transform.position += offset;

        makeBG();
		updateTorch();
 
	}
	
	// Update is called once per frame
	void Update ()
	{
		//transform.position = Vector3.Lerp (transform.position, transform.position - (new Vector3 (0	, 0, 0.01f)), destructionTime);

		if (timerSize >= (maxSize - 1)) {
			fade ();
		} else {
			currentGrowRate += growRate;
			currentSize += growRate;
			timerSize += currentGrowRate;

            if (!alternateVersion)
            {
                updateTorch();
                updateBG();
            }
		}
	
	}

	void makeTorch()
	{
		currentSize = 0.1f;
		timerSize = currentSize;
		currentGrowRate = 0f;
		
		//if cone is not set, then make it a circle
		if (!cone)
		{
			coneTo = 360f;
			coneFrom = 0f;
		}

		createArrays();
		
		renderer.enabled = true;

		meshUpdate(vecArr, uvArr, triArr);
		Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
	}

	void updateTorch()
	{
        vecArr = currentVecArrUpdate(vecArr);
        uvArr = uvArrMake(vecArr);
        meshUpdate(vecArr, uvArr, triArr);

		//Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);

		//debugDraw(vecArr);
	}

	void OnDestroy() {
		DestroyImmediate(mesh);
		DestroyImmediate(renderer.material);
		DestroyImmediate(torchBG);
	}

	void setTexture()
    {
        RTCamerBehaviour = mainCameraGrabber.getCurrentCameraBehaviour();

		renderer.material.mainTexture = mainCameraGrabber.getTexture(destructionTime*1.25f);
		mainCamerBehaviour.setTimer(destructionTime*1.25f);
	}

	void createArrays()
	{
		maxVecArr = vecArrMake(maxSize);	
		maxVecArr = collisionChecking(maxVecArr);

		if(alternateVersion)
		{
			vecArr = maxVecArr;	
		}
		else
		{
			vecArr = vecArrMake(1.0f);
		}

		uvArr = uvArrMake(vecArr);

        int i = 1;

        Vector2[] mazUvArr = uvArrMake(maxVecArr);

        uvVecRatioX = uvArr[i].x / vecArr[i].x;
        uvVecRatioY = uvArr[i].y / vecArr[i].y;
        uvVecRatioMaxX = mazUvArr[i].x / maxVecArr[i].x;
        uvVecRatioY = mazUvArr[i].y / maxVecArr[i].y;

		triArr = triArrMake();

	}

	void fade()
	{
		//slowly move back the torch and make it fade, as torches have cut out materials, fade by turning it black
		float rate = Time.deltaTime*destructionTime;

		renderer.material.color = Color.Lerp(renderer.material.color, Color.black, rate);
		torchBG.renderer.material.color = Color.Lerp(renderer.material.color, Color.black, rate);

		Destroy(gameObject, destructionTime);
	}

	Vector3[] vecArrMake(float magnitude)
	{
		Vector3[] vecArr = new Vector3[castFrequency+1];
		
		for (int i=0; i<castFrequency; i++)
		{
			vecArr[i] = (Quaternion.Euler(0, 0, (((coneTo - coneFrom)/castFrequency)*i)) * Vector3.up);
			vecArr[i] *= magnitude;
		}

		if (cone)
		{
			vecArr[0] =  Vector3.zero;
		}

		vecArr[castFrequency] = Vector3.zero;
		
		return vecArr;
	}

	Vector2[] uvArrMake(Vector3[] vecArr)
	{
		Vector2[] uv = new Vector2[vecArr.Length];

		for (int i=0; i<vecArr.Length; i++)
		{
			uv[i] = RTCamerBehaviour.uvFromWorldToScreen(transform.TransformPoint(vecArr[i])-offset);
		}
		return uv;
	}

	int[] triArrMake()
	{
		int[] tri = new int[castFrequency*3];
		int count = 0;

		for (int i=0; i<tri.Length-3; i+=3)
		{	
			tri[i+count] = castFrequency;
			count++;
			tri[i+count] = (int)(i/3);
			count++;
			tri[i+count] = 1+((int)(i/3));
			count = 0;
		}
		tri[tri.Length-3] = castFrequency;
		tri[tri.Length-2] = castFrequency-1;
		tri[tri.Length-1] = 0;
		return tri;
	}

	Vector3[] collisionChecking(Vector3[] vecArr)
	{
		//gives back the maximum didstance each vector can travel, and then converts them into global vectors.
		RaycastHit ray;
		float rayDistance, absorbtionValue; 
		for (int i=0; i<vecArr.Length; i++)
		{
			if (Physics.Raycast (transform.position, vecArr[i], out ray, maxSize))
			{
				rayDistance = ray.distance/maxSize;
				try
				{
					absorbtionValue = ray.collider.gameObject.GetComponent<Absorbtion>().absobtion;
				}
				catch 
				{
					absorbtionValue = 1f;
				}
				
				absorbtionValue *= (1 - rayDistance);

				//rebound code
				//StartCoroutine( subTorch(transform.TransformPoint(vecArr[i]*(rayDistance*0.9f)), (vecArr[i].magnitude - ray.distance) * (1 - absorbtionValue)));

				if ((rayDistance + absorbtionValue) < 1f)
				{
					vecArr[i] *= (rayDistance + absorbtionValue);
				}
			}
		}
		return vecArr;
	}

	Vector3[] currentVecArrUpdate(Vector3[] vecArr)
	{
		//updatees the vec3 array given, growing it until it hits maxsize or collision

		for (int i=0; i<vecArr.Length; i++)
		{
			if ((vecArr[i] * (1 + currentSize)).magnitude < maxVecArr[i].magnitude)
			{
				vecArr[i] *= (1 + currentSize);
			}
			else
			{
				vecArr[i] = maxVecArr[i];
			}
		}
		return vecArr;
	}
    
	public void meshUpdate(Vector3[] newVertices, Vector2[] newUV, int[] newTriangles)
	{
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;
		mesh.RecalculateNormals();
	}

	void debugDraw(Vector3[] vecArr)
	{
		//testing fucntion that shouldn't be used in game, simply displays the vectors in a nice polygon

		Vector3 zero = transform.TransformPoint (Vector3.zero);
		int length = vecArr.Length - 2;

		for (int i=0; i<castFrequency-1; i++)
		{
			Debug.DrawLine(transform.TransformPoint(vecArr[i]), transform.TransformPoint(vecArr[i+1]), Color.red);
			Debug.DrawLine(zero, transform.TransformPoint(vecArr[i]), Color.white);
		}

		Debug.DrawLine(transform.TransformPoint(vecArr[0]), transform.TransformPoint(vecArr[length]), Color.red);
		Debug.DrawLine(zero, transform.TransformPoint(vecArr[length]), Color.white);

	}

	IEnumerator subTorch(Vector3 location, float size)
	{
		int childFreq = 8;
		if (spawnAgain)
		{
			yield return new WaitForEndOfFrame();
			GameObject torch = Instantiate (this.gameObject, location, transform.rotation) as GameObject;
			RayCastTorch torchScript = torch.GetComponent<RayCastTorch>();
			torchScript.setMaxSize (size);
			torchScript.setGrowRate (growRate);
			torchScript.setCastFrequency (childFreq);
			torchScript.setDestructionTime (destructionTime);
			torchScript.setSpawnAgain (false);
		}
	}
	
	void makeBG()
	{
		torchBG = Instantiate(torchBG, transform.position, Quaternion.identity) as GameObject;
		torchBGScript = torchBG.GetComponent<RayCastTorchBackground>();
        torchBG.transform.parent = transform;

		torchBGScript.setAlternateVersion(!alternateVersion);
		torchBGScript.setMaxSize(maxSize);
		torchBGScript.setCastFrequency(castFrequency);
		torchBGScript.setVecArr(vecArr);

		updateBG();
	}

	void updateBG()
	{
        //torchBGScript.setCurrentSize(currentSize);
	}

	public Mesh getMesh()
	{
		return mesh;
	}

	public void setMaxSize(float size)
	{
		maxSize = size;
	}
	
	public float getMaxSize()
	{
		return maxSize;
	}
			
	public void setGrowRate(float rate)
	{
		growRate = rate;
	}

	public float getGrowRate()
	{
		return growRate;
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

	public void setTorchBG(GameObject BG)
	{
		torchBG = BG;
	}
	
	public GameObject getTorchBG()
	{
		return torchBG;
	}
}
