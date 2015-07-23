using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This script must be higher in the heirarchy than doneButton.cs, otherwise it
 * will be called afterwards, meaning that the outScore will be 0 on click, as 
 * it will not have been updated prior to ending the simulation. Setting it to 
 * run beforehand means that when the sim is ended, this script has already 
 * been processed.
 */

public class pathwayAAScore : MonoBehaviour {

	/* Currently CAPI only supports float, not int */
	//public int outScore = 0;
	public float outScore = 0;
	public string outFeed = "";
	public string outSave = "";
		
	private int ansMissing = 0;
	private int arrMissing = 0;
	
	private Dictionary <string, string> ansAlias = new Dictionary<string, string> {
		{"box.001", "Total Peripheral Resistance" },
		{"box.002", "Sympathetic Activity" },
		{"box.003", "Fluid; Interstitium to Plasma" },
		{"box.004", "Parasympathetic Activity" },
		{"box.005", "Heart Rate" },
		{"box.006", "Plasma Volume" },
		{"box.007", "Capillary Blood Pressure" },
		{"box.008", "Baroreceptor Activity" },
		{"box.009", "Haematocrit" }
	};

	private List<string> socks = new List<string> {
		"socket.000", "socket.001", "socket.002", "socket.003", "socket.004", "socket.005", 
		"socket.006", "socket.007", "socket.008", "socket.009", "socket.010"
	};
	
	private Dictionary<string, string[]> ansCorrect = new Dictionary<string, string[]> {
		{"socket.001" , new[] {
				"box.007", 
				"Capillary Blood Pressure", 
				"target.001", 
				"decrease"} 
		},
		{"socket.002" , new[] {
				"box.008", 
				"Baroreceptor Activity", 
				"target.002", 
				"decrease"} 
		},
		{"socket.003" , new[] {
				"box.003", 
				"Fluid Interstitium to Plasma", 
				"target.003", 
				"increase"} 
		},
		{"socket.004" , new[] {
				"box.002", 
				"Sympathetic Activity", 
				"target.004", 
				"increase"} 
		},
		{"socket.005" , new[] {
				"box.004", 
				"Parasympathetic Activity", 
				"target.005", 
				"decrease"} 
		},
		{"socket.006" , new[] {
				"box.009", 
				"Haematocrit", 
				"target.006", 
				"decrease"} 
		},
		{"socket.007" , new[] {
				"box.006", 
				"Plasma Volume", 
				"target.007", 
				"increase"} 
		},
		{"socket.008" , new[] {
				"box.001", 
				"Total Peripheral Resistance", 
				"target.008", 
				"increase"} 
		},
		{"socket.009" , new[] {
				"box.005", 
				"Heart Rate", 
				"target.009", 
				"increase"} 
		},
		{"socket.000" , new[] {
				"static.000", 
				"Mean Arterial Pressure (top)", 
				"target.000", 
				"decrease"} 
		},
		{"socket.010" , new[] {
				"static.010", 
				"Mean Arterial Pressure (bottom)", 
				"target.010", 
				"increase"} 
		}
	};

	private GameObject goGet(string goNm) {
		return GameObject.Find (goNm);
	}

	void ansCheck(string goName) {

		string[] dObs = ansCorrect[goName];
		string oOb = null;

		if (dObs[0].Contains("static")) {
			oOb = dObs[0];
			//Debug.Log ("Static - " + oOb);
		} 
		if ( !dObs[0].Contains("static") ) {
			string tmp = goGet( goName ).GetComponent<detectOverlap>().overlapObj;
			if (tmp != "") {
				oOb = tmp;
			}
		}


		/*
		 * 		Scoring
		 */

		/* If socket has correct box, and isn't MAP, inc score */
		if (oOb != "" && !dObs[0].Contains("static") && oOb == dObs[0] ) {
			outScore += 1;
		}

		/* If arrow is correct direction, inc score */
		string oAr = null;
		if (oOb != null) {
			oAr = goGet(dObs[2]).GetComponent<detectOverlap>().overlapObj;
		}
		if ( oAr != null && oAr.Contains(dObs[3]) ) {
			outScore += 1;
		} 
		if (oAr == null) {
			arrMissing += 1;
		}
		/*
		 * 		Feedback
		 */

		/* If box is correct but arrow isn't */
		if ( oOb != null && oOb == dObs[0] && !oAr.Contains(dObs[3]) ) {
			outFeed = outFeed + " - The direction of " + dObs[1] // change to var
				+ " is incorrect.\n";
		}

		/* If box is incorrect */
		if ( oOb != null && oOb != dObs[0] ) {
			outFeed = outFeed + " - " + ansAlias[oOb] // change to var
			+ " is incorrect.\n";
		}

		if ( oOb == null) {
			ansMissing += 1;
		}
		/*
		 * TODO: Do we need to be able to differentiate between incorrect box, and incorrect box AND arrow?
		 */
	}

	public void calculateScore() {
		foreach (string sck in socks) {
			ansCheck(sck);
		}
		/* Prepend missing info to outFeed */
		if (arrMissing != 0) {
			if (arrMissing ==1 ) {
				outFeed = " - There is 1 arrow slot that has not been filled in.\n" + outFeed;
			} else {
				outFeed = " - There are " + arrMissing.ToString() + 
					" arrow slots that have not been filled in.\n" + outFeed;
			}
			
		}
		if (ansMissing != 0) {
			if (ansMissing == 1) {
				outFeed = " - There is 1 word answer slot that has not been filled in.\n" + outFeed;
			} else {
				outFeed = " - There are " + ansMissing.ToString() + 
					" word answer slots that have not been filled in.\n" + outFeed;
			}
		}
//		Debug.Log(outScore);
//		Debug.Log(outFeed);
	}

	public void calcSave() {
		// calculate save data string

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
