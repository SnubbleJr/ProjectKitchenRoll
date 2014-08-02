using UnityEngine;
using System.Collections;
using System.Linq;

public class SweepingTorch : MonoBehaviour
{

    public GameObject torchBG;

    //default values
    private float width = 5f;
    private float height = 25f;
    private bool direction = false;
    /*
     * This will just be a rectangle torch, that will slowly grow in one direction, untill reaching it's max, an emitter will make multiple of 5these and chain them up
     * */

    private float maxSize = 150;
    private float growRate = 0.33f;
    private int castFrequency = 4;
    private float destructionTime = 2f;
    private Vector3 offset;
    private bool spawnAgain = true;
    private bool cone = false;
    private float coneTo, coneFrom;
    private float fadeRate = -1f;

    MainCameraBehaviour mainCamerBehaviour, RTCamerBehaviour;
    RenderTextureGrabber mainCameraGrabber;

    RayCastTorchBackground torchBGScript;

    Mesh mesh = new Mesh();
    Vector3[] maxVecArr;
    Vector3[] vecArr;
    Vector2[] uvArr;
    int[] triArr;

    private float currentSize;

    //TODO: MAKE IT LIKE HOW THE OLD TORCH USED JTO BE: GROWING SLOWLY
    //THEN SEND THE VEC DATA  TO THE BACK GROUND, TO SEW THE OUUTLINE TO MATCH IT

    void Start()
    {
        if (fadeRate < 0)
        {
            setEnergy(2f);
        }

        mainCamerBehaviour = Camera.main.GetComponent<MainCameraBehaviour>();
        mainCameraGrabber = Camera.main.GetComponent<RenderTextureGrabber>();
        offset = DisplayCameraOffset.offset;

        setTexture();

        makeTorch();

        //moving it to the top plane
        offset.z = mainCamerBehaviour.getZOffset();
        transform.position += offset;

        makeBG();
        updateTorch();

        currentSize += growRate;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp (transform.position, transform.position - (new Vector3 (0	, 0, 0.01f)), destructionTime);

        fade();

        updateTorch();
        updateBG();

        if (renderer.material.color.b <= 0.01f)
        {
            Destroy(gameObject);
        }
    }

    void makeTorch()
    {
        currentSize = 0f;

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

        debugDraw(vecArr);
    }

    void OnDestroy()
    {
        DestroyImmediate(mesh);
        DestroyImmediate(renderer.material);
        DestroyImmediate(torchBG);
        mainCamerBehaviour.informTorchDestoryed();
    }

    void setTexture()
    {
        RTCamerBehaviour = mainCameraGrabber.getCurrentCameraBehaviour();

        renderer.material.mainTexture = mainCameraGrabber.getTexture(destructionTime * 1.2f);
    }

    void createArrays()
    {
        maxVecArr = vecArrMake(maxSize);

        vecArr = vecArrMake(1.0f);

        uvArr = uvArrMake(vecArr);

        Vector2[] mazUvArr = uvArrMake(maxVecArr);

        triArr = triArrMake();

    }

    void fade()
    {
        //slowly move back the torch and make it fade, as torches have cut out materials, fade by turning it black

        renderer.material.color = Color.Lerp(renderer.material.color, Color.black, fadeRate);
        torchBG.renderer.material.color = Color.Lerp(renderer.material.color, Color.black, fadeRate);
    }

    Vector3[] vecArrMake(float magnitude)
    {
        //magnitude only effects width here
        //direction dictates which half of the array gets a width value
        Vector3[] vecArr = new Vector3[castFrequency + 1];

        for (int i = 0; i < castFrequency; i++)
        {
            vecArr[i] = (Quaternion.Euler(0, 0, (((coneTo - coneFrom) / castFrequency) * i)) * new Vector3(1f,1f,0));
            //left to right, 1st half
            if (direction)
            {
                if (i < 3 && i != 0)
                {
                    vecArr[i].Scale(new Vector3(width * magnitude, height, 0f));
                }
                else
                {
                    vecArr[i].Scale(new Vector3(0, height, 0f));
                }
            }
            //right to left, 2nd half
            else
            {
                if (i >= 3 || i == 0)
                {
                    vecArr[i].Scale(new Vector3(width * magnitude, height, 0f));
                }
                else
                {
                    vecArr[i].Scale(new Vector3(0, height, 0f));
                }
            }
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

        for (int i = 0; i < vecArr.Length; i++)
        {
            uv[i] = RTCamerBehaviour.uvFromWorldToScreen(transform.TransformPoint(vecArr[i]) - offset);
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

    Vector3[] currentVecArrUpdate(Vector3[] vecArr)
    {
        //updatees the vec3 array given, growing it until it hits maxsize or collision

        for (int i = 0; i < vecArr.Length; i++)
        {
            if ((vecArr[i] * (1 + currentSize)).magnitude < maxVecArr[i].magnitude)
            {
                vecArr[i].Scale(new Vector3 ((1 + currentSize), 1,0));
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

    IEnumerator subTorch(Vector3 location, float size)
    {
        int childFreq = 8;
        if (spawnAgain)
        {
            yield return new WaitForEndOfFrame();
            GameObject torch = Instantiate(this.gameObject, location, transform.rotation) as GameObject;
            RayCastTorch torchScript = torch.GetComponent<RayCastTorch>();
            torchScript.setEnergy(size);
            torchScript.setCastFrequency(childFreq);
            torchScript.setDestructionTime(destructionTime);
            torchScript.setSpawnAgain(false);
        }
    }

    void makeBG()
    {
        torchBG = Instantiate(torchBG, transform.position + new Vector3(0, 0, 0.3f), Quaternion.identity) as GameObject;
        torchBGScript = torchBG.GetComponent<RayCastTorchBackground>();
        torchBG.transform.parent = transform;

        torchBGScript.setMaxSize(maxSize / 10);
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

    public void setEnergy(float rate)
    {
        maxSize = rate;
        fadeRate = Time.deltaTime * (1f / (rate / 10));
    }

    public float getEnergy()
    {
        return fadeRate;
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

    public void setWidth(float w)
    {
        width = w;
    }

    public float getWidth()
    {
        return width;
    }

    public void setHeight(float h)
    {
        height = h;
    }

    public float getHeight()
    {
        return height;
    }

    public void setDirection(bool dir)
    {
        direction = dir;
    }

    public bool getDirection()
    {
        return direction;
    }
}
