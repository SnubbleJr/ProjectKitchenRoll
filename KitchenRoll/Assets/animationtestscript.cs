using UnityEngine;
using System.Collections;

public class animationtestscript : MonoBehaviour {

    protected Animator animator;
    
    void Start () 
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Ladder Interact", false);   
    }
    
    void Update () 
    {
        if(animator)
        {
            //get the current state
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
            //if we're in "Run" mode, respond to input for jump, and set the Jump parameter accordingly. 
            if(stateInfo.nameHash == Animator.StringToHash("Base Layer"))
            {
                if(Input.GetButtonDown("Use")) 
                    animator.SetBool("Ladder Interact", true );
            }

            float v = Input.GetAxisRaw("Vertical");
            
            //set event parameters based on user input
            animator.SetInteger("Direction", (int)v);

            print((int)v);
        }       
    }        
}
