using UnityEngine;
using System.Collections;

public class answerAssigned : MonoBehaviour {

	public bool ansLock = false;

	/* If there aren't any overlapping objects, ansLock cannot be true */
	void Update () {
		if (this.GetComponent<detectOverlap>().overlapObj == null) {
			ansLock = false;
		}
	}
}
