using UnityEngine;
using System.Collections;

public class RayCastTorchRipple : MonoBehaviour {
    
    //simply a a big plane with the torch's material, that uses the torch uv script to make a ring
    //will it fade?

    private int castFrequency = 4;

    Mesh mesh;
    Vector3[] vecArr;
    Vector2[] uvArr;
    int[] triArr;

	// Use this for initialization
    void Start()
    {
        makeTorch();
	}

    void makeTorch()
    {
        createArrays();
        
        meshUpdate(vecArr, uvArr, triArr);
        Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void createArrays()
    {
        vecArr = vecArrMake();

        uvArr = uvArrMake(vecArr);

        triArr = triArrMake();
    }

    Vector3[] vecArrMake()
    {
        //magnitude only effects width here
        //direction dictates which half of the array gets a width value
        Vector3[] vecArr = new Vector3[castFrequency + 1];

        float z = Camera.main.nearClipPlane;

        for (int i = 0; i < castFrequency; i++)
        {
            vecArr[i] = (Quaternion.Euler(0, 0, ((360f / castFrequency) * i)) * new Vector3(-1f, 1f, 0));
        }

        vecArr[0] = new Vector3(-100,-100,0);
        vecArr[1] = new Vector3(-100,100,0);
        vecArr[2] = new Vector3(100,100,0);
        vecArr[3] = new Vector3(100,-100,0);

        vecArr[castFrequency] = new Vector3(0,0,0);

        return vecArr;
    }

    Vector2[] uvArrMake(Vector3[] vecArr)
    {
        //i think it's just -.5,-.5 to .5,.5
        Vector2[] uv = new Vector2[vecArr.Length];

        uv[0] = new Vector2(0f, 0f);
        uv[1] = new Vector2(0f, 1f);
        uv[2] = new Vector2(1f, 1f);
        uv[3] = new Vector2(1f, 0f);

        uv[castFrequency] = new Vector2(0.5f, 0.5f);
        return uv;
    }

    int[] triArrMake()
    {
        int[] tri = new int[castFrequency * 3];
        int count = 0;

        for (int i = 0; i < tri.Length - 3; i += 3)
        {
            tri[i + count] = castFrequency;
            count++;
            tri[i + count] = (int)(i / 3);
            count++;
            tri[i + count] = 1 + ((int)(i / 3));
            count = 0;
        }
        tri[tri.Length - 3] = castFrequency;
        tri[tri.Length - 2] = castFrequency - 1;
        tri[tri.Length - 1] = 0;
        return tri;
    }

    public void meshUpdate(Vector3[] newVertices, Vector2[] newUV, int[] newTriangles)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
    }

    void debugDraw(Vector3[] vecArr)
    {
        //testing fucntion that shouldn't be used in game, simply displays the vectors in a nice polygon

        Vector3 zero = transform.TransformPoint(Vector3.zero);
        int length = vecArr.Length - 2;

        for (int i = 0; i < castFrequency - 1; i++)
        {
            Debug.DrawLine(transform.TransformPoint(vecArr[i]), transform.TransformPoint(vecArr[i + 1]), Color.red);
            Debug.DrawLine(zero, transform.TransformPoint(vecArr[i]), Color.white);
        }

        Debug.DrawLine(transform.TransformPoint(vecArr[0]), transform.TransformPoint(vecArr[length]), Color.red);
        Debug.DrawLine(zero, transform.TransformPoint(vecArr[length]), Color.white);
    }

    public void setMaterial(Texture tex)
    {
        renderer.material.mainTexture = tex;
        
        renderer.enabled = true;
    }
}