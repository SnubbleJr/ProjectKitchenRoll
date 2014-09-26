using UnityEngine;
using System.Collections;

public class torchUVTestScript : MonoBehaviour {

    public float inner = -0.5f;
    public float outer = 0f;

    public float innerGrowRate = 0.6f;
    public float outerGrowRate = 0.6f;
    public float innerThreshold = 0f;
    public float outerThreshold = 0f;

    float innerTemp, outerTemp;

	// Use this for initialization
	void Start () {
        innerTemp = inner;
        outerTemp = outer;
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

        renderer.material.SetFloat("_InnerRadius", inner);
        renderer.material.SetFloat("_OuterRadius", outer);
        renderer.material.SetFloat("_Hardness", (1 - inner + innerTemp));
	}
}
