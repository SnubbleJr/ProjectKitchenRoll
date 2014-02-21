﻿//NOTE! DO NOT PLACE THIS OUTSIDE OF THE GAME! mAKE SURE THERE IS ALWAYS SOMEWTHING BEHIND IT, NOT JUST EMPTY GAME SPACE!

using UnityEngine;
using System.Collections;

public class RayCastTorchBackground : MonoBehaviour {
	
	public float radius = 1.0f;
	public float vertexCount = 20f;

	//default values
	private float maxSize = 15;
	private float growRate = 0.5f;
	private int castFrequency = 32;
	private float destructionTime = 10f;
	private Vector3 offset = new Vector3 (0,256,0);
	private bool spawnAgain = true;
	private bool cone = false;
	private float coneFrom, coneTo;
	
	ScreenGrabber mainCameraGrabber;
	
	Mesh mesh;
	
	Vector3[] maxVecArr;
	Vector3[] vecArr;
	Vector2[] uvArr;
	int[] triArr;
	
	private float currentGrowRate;
	private float currentSize;
	private float timerSize;
	
	void Start () {
		//NOTE! DO NOT PLACE THIS OUTSIDE OF THE GAME! mAKE SURE THERE IS ALWAYS SOMEWTHING BEHIND IT, NOT JUST EMPTY GAME SPACE!
		/*
		currentSize = 0.1f;
		timerSize = currentSize;
		currentGrowRate = 0f;
		
		//if cone is not set, then make it a circle
		if (!cone)
		{
			coneTo = 360f;
			coneFrom = 0f;
		}
		
		//makes a mesh, of viable size and fedelity
		//it is altered by a ray trace
		
		//getting the texture that will be applyed to it
		mainCameraGrabber = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<ScreenGrabber>();

		//creating core arrays
		vecArr = vecArrMake(1.0f);
		maxVecArr = vecArrMake(maxSize);
		
		uvArr = uvArrMake(vecArr);
		
		triArr = triArrMake();
		
		//moving it to the top plane
		offset.z = mainCameraGrabber.getZOffset ();
		transform.position += offset;
		
		renderer.enabled = true;
		
		make ();*/

		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(c1, c1);
		lineRenderer.SetWidth(0.05f, 0.05f);
		lineRenderer.SetVertexCount((	int)vertexCount+1);
		
		for(int i = 0; i < vertexCount+1; i++)
		{
			float angle = i;
			i = (int)((i / vertexCount) * Mathf.PI * 2);
			Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
			lineRenderer.SetPosition(i, pos);
		}
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//transform.position = Vector3.Lerp (transform.position, transform.position - (new Vector3 (0	, 0, 0.01f)), destructionTime);
		
		if (timerSize >= (maxSize - 1)) {
			//fade ();
		} else {
			currentGrowRate += growRate;
			currentSize += growRate;
			timerSize += currentGrowRate;
			//make ();
		}
	}
	
	void make()
	{
		vecArr = currentVecArrUpdate(vecArr);	
		uvArr = uvArrMake(vecArr);
		mesh = meshMaker(vecArr, uvArr, triArr);
		Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
		
		//debugDraw(vecArr);
	}
	
	void fade()
	{
		//slowly move back the torch and make it fade, as torches have cut out materials, fade by turning it black
		float rate = Time.deltaTime*destructionTime;
		
		renderer.material.color = Color.Lerp(renderer.material.color, Color.black, rate);
		
		if (renderer.material.color.r <= 0.01f)
		{
			Destroy (gameObject);
		}
	}
	
	Vector3[] vecArrMake(float magnitude)
	{
		Vector3[] vecArr = new Vector3[castFrequency+1];
		
		for (int i=0; i<castFrequency; i++)
		{
			vecArr[i] = (Quaternion.Euler(0, 0, (((coneTo - coneFrom)/castFrequency)*i)) * Vector3.up);
			vecArr[i] *= magnitude;
		}
		vecArr[castFrequency] = Vector3.zero;
		
		return vecArr;
	}
	
	Vector2[] uvArrMake(Vector3[] vecArr)
	{
		Vector2[] uv = new Vector2[vecArr.Length];
		
		float f = 0.5f;
		
		for (int i=0; i<vecArr.Length; i++)
		{
			uv[i].x = -((vecArr[i].x/maxSize)*f)+f;
			uv[i].y = ((vecArr[i].y/maxSize)*f)+f;
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
				absorbtionValue = (1 - rayDistance) * ray.collider.gameObject.GetComponent<Absorbtion>().absobtion;
				
				//rebound code
				//StartCoroutine( subTorch(transform.TransformPoint(vecArr[i]*(rayDistance*0.9f)), (vecArr[i].magnitude - ray.distance) * (1 - absorbtionValue)));
				
				if ((rayDistance + absorbtionValue) < 1)
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
	
	/*Vector2[] currentUvArrUpdate(Vector2[] uvArr)
	{
		//updatees the vec2 array given, growing it until it hits maxsize or collision

		float sizeRatio = vecArr[1].magnitude/maxVecArr[1].magnitude;

		for (int i=0; i<uvArr.Length; i++)
		{
			if (uvArr[i].magnitude < maxUvArr[i].magnitude)
			{
				uvArr[i] = coreUvArr[i] * (1 + currentSize);
			}
			else
			{
				uvArr[i] = maxUvArr[i];
			}
			//print(i + ". core: " + coreUvArr[i] + ", current: " + uvArr[i] + ", max: " + maxUvArr[i] + ", ratio: " + sizeRatio);
		}
		return uvArr;
	}*/
	
	Mesh meshMaker(Vector3[] newVertices, Vector2[] newUV, int[] newTriangles)
	{
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;
		mesh.RecalculateNormals();
		return mesh;
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
			torchScript.setOffset (offset);
			torchScript.setSpawnAgain (false);
		}
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
}
