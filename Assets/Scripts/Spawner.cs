using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	// public Vector3 gizmoSize;

	public GameObject[] spawnObjects;
	public float spawnInterval;
	// Use this for initialization
	void Start () {
		// start repeatedly spawning random objects from spawnObjects array with given spawnInterval
		InvokeRepeating("SpawnObject", Random.Range(0, spawnInterval), spawnInterval  );		
	}

	void SpawnObject(){

		// little bit of random here
		if (Random.value < 0.75){
			Instantiate( 
				spawnObjects[ Random.Range(0, spawnObjects.Length) ],
				transform
			);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// not used, gizmos testing
	// private void OnDrawGizmos() {
	// 	Gizmos.DrawWireCube(transform.position, gizmoSize);
	// }
}
