using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* MonoBehaviour to finalise the simulation */
public class doneButton : MonoBehaviour {

	//GameObject.Find("Score").GetComponent<Text>().text = outScore.ToString();
	private Color origColor;
	private Color mouseOverColor = new Color( 0.93f, 0.93f, 0.93f);
	private Color doneColor = new Color (0.75f, 0.75f, 0.75f);
	public bool endSim = false;

	/* Have a locally locked thing to hold the mutable value */
//	int _finScore;
//	public int finScore {
//		get { return _finScore; }
//		set { _finScore = value; Capi.set("Done.FinScore", value); }
//	}
//
//	string _feedBack;
//	public string feedBack {
//		get { return _feedBack; }
//		set { _feedBack = value; Capi.set("Done.FeedBack", value); }
//	}

	// Use this for initialization
	void Start () {
//		finScore = 0;
		origColor = this.GetComponent<Renderer>().material.color;
	}

	void OnMouseEnter() {
		/* Change material when hover */
		if ( !endSim ) {
			this.GetComponent<Renderer>().material.color = mouseOverColor;
		}
	}

	void OnMouseExit() {
		/* Change material back when no more */
		if ( !endSim ) {
			this.GetComponent<Renderer>().material.color = origColor;
		}
	}

	void OnMouseDown() {
		/* Run the Score script */
		this.GetComponent<pathwayAAScore>().calculateScore();
		//int oSc = this.GetComponent<pathwayAAScore>().outScore;
		float oSc = this.GetComponent<pathwayAAScore>().outScore;
		string oFd = this.GetComponent<pathwayAAScore>().outFeed;
		// string oSv = this.GetComponent<pathwayAAScore>().outSave;
//		Debug.Log ("outScore: " + oSc);
//		Debug.Log ("outFeed: " + oFd);
		
		this.GetComponent<capiExpose>().oScore = oSc;
		this.GetComponent<capiExpose>().oFeed = oFd;
		//this.GetComponent<capiExpose>().oSave = oSv;

		/* Darken all objects */
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("boxes");
		GameObject[] arrows = GameObject.FindGameObjectsWithTag("arrows");
		GameObject[] mobElem = new GameObject[ boxes.Length + arrows.Length ];
		boxes.CopyTo(mobElem, 0);
		arrows.CopyTo(mobElem, boxes.Length);
		foreach ( GameObject mobE in mobElem ) {
			mobE.GetComponent<Renderer>().material.color = doneColor;
		}


		/* Stop time - this locks all objects to current location */
		Time.timeScale = 0;
		endSim = true;
	}

	// Update is called once per frame
	void Update () {

	}
}
