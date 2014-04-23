using UnityEngine;
using System.Collections;

public class TargetFrameRate : MonoBehaviour {

	public int FPS;

	void Awake() {
		Application.targetFrameRate = FPS;
	}

}
