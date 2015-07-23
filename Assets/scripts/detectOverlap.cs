using UnityEngine;
using System.Collections;
// Enable lists
using System.Collections.Generic;

public class detectOverlap : MonoBehaviour {

	/* NB One object _MUST_ have a RigidBody2D for this trigger to work */

	public string overlapObj = null;
	public List<string> overlapList = new List<string>();

	void OnTriggerEnter2D (Collider2D other) {
		overlapObj = other.name;
		if (!overlapList.Contains(overlapObj)) {
			overlapList.Add(overlapObj);
		}
	}

	void OnTriggerExit2D ( Collider2D other ) {
		overlapList.Remove( other.name );
		if (overlapList.Count == 0) {
			overlapObj = null;
		} else {
			/* List is zero-indexed: */
			overlapObj = overlapList[ overlapList.Count - 1 ];
		}
	}

	void Update () {

	}
	void FixedUpdate () {

	}
}
