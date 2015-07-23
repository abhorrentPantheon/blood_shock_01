using UnityEngine;
using System.Collections;

public class capiExpose : MonoBehaviour {
	
	/* Create CAPI variables 
	 * NB CAPI only supports float numeric types currently
	 */
//	private int _outScore = 0;
//	public int oScore {
	private float _outScore = 0;
	public float oScore {
		get { return _outScore; }
		set { _outScore = value; Capi.set ( "Sim.Score", value ); }
	}
	//private string _outFeed = GameObject.Find("button_done").GetComponent<pathwayAAScore>().outFeed;
	private string _outFeed = "";
	public string oFeed {
		get { return _outFeed; }
		set { _outFeed = value; Capi.set ( "Sim.Feedback", value ); }
	}

	private string _outSave = "";
	public string oSave {
		get { return _outSave; }
		set { _outSave = value; Capi.set ( "Sim.SaveData", value ); }
	}

	// Use this for initialization
	void Start () {

		/* Change output values */
		oScore = this.GetComponent<pathwayAAScore>().outScore;
		oFeed = this.GetComponent<pathwayAAScore>().outFeed;
		oSave = this.GetComponent<pathwayAAScore>().outSave;

		/*
		 * Allow CAPI variables to be seen 
		 * NB: Currently only float, string, bool, string[] supported
		 */ 
		Capi.expose<float> ("Sim.Score", () => { return oScore; }, (value) => { return oScore = value; } );
		Capi.expose<string> ("Sim.Feedback", () => { return oFeed; }, (value) => { return oFeed = value; } );
		Capi.expose<string> ("Sim.SaveData", () => {return oSave; }, (value) => {return oSave = value; } );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
