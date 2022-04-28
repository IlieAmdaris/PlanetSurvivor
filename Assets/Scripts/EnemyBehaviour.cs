using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	private GameObject planet;
	private bool isGameOver; 

	public float enemySpeed;
	private bool pulledOut = false;

	public GameObject groundHole;
	// Use this for initialization
	void Start () {
		// accessing the planet gameobject
		isGameOver = GameObject.Find("Knight").GetComponent<KnightBehaviour>().isGameOver;

		planet = GameObject.Find("Planet");
;		// launch repeatet call of LeaveGroundHole function with given time interval
		InvokeRepeating("LeaveGroundHole", 0, 0.5f);
	}

	// the effect of "biting into the planet" - enemy leaves spritemask objects that look like holes in the planet sprite
	public void LeaveGroundHole(){
		GameObject newObject = Instantiate(groundHole, transform.position, transform.rotation);
		newObject.transform.parent = gameObject.transform.parent;
	}

	public void PullOut(){
		pulledOut = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver)
        {
			GameObject.FindObjectOfType<KnightBehaviour>().DecrementHealth();
			return;
        }

		// a bit of linear algebra -- determining direction to planet to know where to move
		Vector3 directionToPlanet = (planet.transform.position - transform.position).normalized;
		float distanceToCore = Vector3.Distance(planet.transform.position, transform.position);
		
		// 3 cases:
		// 1: if enemy is too away from the core, or pulled out -- it flies away from the planet into the space and then gets destroyed
		// 2: else if enemy is too close to the core it damages the planet and decrements health slidebar
		// 3: esle enemy uses its default beahviour which is gradually moving towards the core
		if (distanceToCore > 3.3f || pulledOut) {
			transform.position -= directionToPlanet * enemySpeed * 50f;
			transform.Rotate(new Vector3(0, 0, 10f));
			// enemy should be destroyed in 3 seconds for optimization purposes, and it is out of camera view anyway by that time!!!!!!!
			Destroy(gameObject, 3f);			
		} else if (distanceToCore < 0.3f){ 
			GameObject.FindObjectOfType<KnightBehaviour>().DecrementHealth();
		}
		else {
			transform.position += directionToPlanet * enemySpeed;
		}
		isGameOver = GameObject.Find("Knight").GetComponent<KnightBehaviour>().isGameOver;




	}
}
