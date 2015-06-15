using UnityEngine;
using System.Collections;

public class BellAnimationScript : MonoBehaviour {

    public float speed= .5f;
    public AudioClip bellSound;
    public GameObject torch;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            print("K");
            this.animation["Bell Take 001"].speed = speed;
            this.animation.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void gong()
    {
        Vector3 pos = transform.Find("tower/towerroot/tower1/bell/bellroot").position;

        AudioSource.PlayClipAtPoint(bellSound, Camera.main.transform.position, 0.1f);

        Instantiate(torch, pos, Quaternion.identity);
    }
}
