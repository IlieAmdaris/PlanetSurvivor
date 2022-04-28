using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHole : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		bool isGameOver = GameObject.Find("Knight").GetComponent<KnightBehaviour>().isGameOver;
        if (isGameOver)
        {
			return;
        }
		// planet is kinda self-regenerating, the holes are getting smaller with time, if too small - the hole disappears, 
		// for simplicity holes have no physics logic, only the graphic representation
		transform.localScale *= 0.9992f;
		if (transform.localScale.x < 0.1f) Destroy(gameObject);
	}
}
