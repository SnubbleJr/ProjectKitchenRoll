using UnityEngine;
using System.Collections;

public class torchUVTestScript : MonoBehaviour {

    public float inner = -0.2f;
    public float outer = 0f;

    public float innerGrowRate = 0.2f;
    public float outerGrowRate = 1f;
    public float innerThreshold = 0f;
    public float outerThreshold = 1f;

    float innerTemp, outerTemp;
    Vector4 distort;

	// Use this for initialization
	void Start () {
        innerTemp = inner;
        outerTemp = outer;

        distort = renderer.material.GetVector("_Distort");
	}
	
	// Update is called once per frame
    void Update()
    {
        if (outer > innerThreshold)
        {
            inner += innerGrowRate*Time.deltaTime/2;
        }

        if (outer < outerThreshold || outerThreshold == 0)
        {
            outer += outerGrowRate * Time.deltaTime;
        }

        renderer.material.SetVector("_Distort", distort);
        renderer.material.SetFloat("_InnerRadius", inner);
        renderer.material.SetFloat("_OuterRadius", outer);
        renderer.material.SetFloat("_Hardness", (1 - inner + innerTemp));
	}

    public void setDistort(Vector2 vec)
    {   
        distort = new Vector4(vec.x, vec.y, distort.z, distort.w);
    }
}
