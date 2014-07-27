using UnityEngine;
using System.Collections;

public class RayCastTorchBackground1 : MonoBehaviour {

    //default values
    private float maxSize = 15;
    private float growRate = 0.01f;
    private int castFrequency = 64;
    private float destructionTime = 2f;
    private Vector3 offset;
    private bool spawnAgain = true;
    private bool cone = false;
    private float coneTo, coneFrom;

    Mesh mesh;
    Vector3[] maxVecArr;
    Vector3[] vecArr;
    Vector2[] uvArr;
    int[] triArr;

    private float currentGrowRate;
    private float currentSize;
    private float timerSize;

    void Start()
    {
        mesh = new Mesh();
        makeTorch();
        //updateTorch();
    }

    //TODO: MAKE IT LIKE HOW THE OLD TORCH USED JTO BE: GROWING SLOWLY
     
    // Update is called once per frame
    void Update()
    {
     //   updateTorch();
        //transform.position = Vector3.Lerp (transform.position, transform.position - (new Vector3 (0	, 0, 0.01f)), destructionTime);

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
        //uvArr = uvArrMake(vecArr);
        meshUpdate(vecArr, uvArr, triArr);

        //Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);

        //debugDraw(vecArr);
    }

    void createArrays()
    {
        maxVecArr = vecArrMake(maxSize);
        maxVecArr = collisionChecking(maxVecArr);

        vecArr = vecArrMake(1f);

        uvArr = uvArrMake(vecArrMake(maxSize));

        Vector2[] mazUvArr = uvArrMake(vecArrMake(maxSize));

        triArr = triArrMake();

    }

    Vector3[] vecArrMake(float magnitude)
    {
        Vector3[] vecArr = new Vector3[castFrequency + 1];

        for (int i = 0; i < castFrequency; i++)
        {
            vecArr[i] = (Quaternion.Euler(0, 0, (((coneTo - coneFrom) / castFrequency) * i)) * Vector3.up);
            vecArr[i] *= magnitude;
        }

        if (cone)
        {
            vecArr[0] = Vector3.zero;
        }

        vecArr[castFrequency] = Vector3.zero;

        return vecArr;
    }

    Vector2[] uvArrMake(Vector3[] vecArr)
    {
        Vector2[] uv = new Vector2[vecArr.Length];

        float scale = 0.5f;

        for (int i = 0; i < vecArr.Length; i++)
        {
            uv[i].x = (vecArr[i].x * scale) + scale;
            uv[i].y = (vecArr[i].y * scale) + scale;
        }
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

    Vector3[] collisionChecking(Vector3[] vecArr)
    {
        //gives back the maximum didstance each vector can travel, and then converts them into global vectors.
        RaycastHit ray;
        float rayDistance, absorbtionValue;
        for (int i = 0; i < vecArr.Length; i++)
        {
            if (Physics.Raycast(transform.position, vecArr[i], out ray, maxSize))
            {
                rayDistance = ray.distance / maxSize;
                try
                {
                    absorbtionValue = ray.collider.gameObject.GetComponent<Absorbtion>().absobtion;
                }
                catch
                {
                    absorbtionValue = 0f;
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

        for (int i = 0; i < vecArr.Length; i++)
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
}
