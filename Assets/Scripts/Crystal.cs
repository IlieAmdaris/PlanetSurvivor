﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour {
	public int hitsToDestroy;
	public string color;

	// function to decrease crystal health that is used from animation events
	public void DecreaseHitsToDestroy(){
		hitsToDestroy--;
	}
}
