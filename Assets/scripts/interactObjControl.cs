using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class interactObjControl : MonoBehaviour {

	private Color mouseOverColor = new Color (0.93f, 0.93f, 0.93f, 1.0f);
	private Color originalColor;
	public Vector3 initLoc; //pub
	public bool atHome = true; //pub/priv?
	public bool atDest = false; //pub
	private bool dragging = false; 
	private float distance;
	//private string chkCont = null;
	private bool lockAns = false;

	float _x;
	public float x {
		get { return _x; }
		set { _x = value; }
	}

	float _y;
	public float y {
		get { return _y; }
		set { _y = value; }
	}

	void overCorrectType ( string chkStr ) {
		string chkCont = null;
		switch (chkStr) {
		case ("box"):
			chkCont = "socket";
			break;
		case ("arrow"):
			chkCont = "target";
			break;
		default:
			Debug.Log("No type of object referenced. Did you click on anything?");
			break;
		}
		if (chkCont == "socket" || chkCont == "target") {
			List<string> ovList = this.GetComponent<detectOverlap>().overlapList;
			string ovObjNm = ovList.Find(listOvObj => listOvObj.Contains( chkCont ) );
			if (ovObjNm != null) {
				var ovObjAA = GameObject.Find(ovObjNm).GetComponent<answerAssigned>();
			
				/* If the list isn't empty, the overlap is with a socket/target and there is
				 * no object at the destination already, move it there */
				if (ovList != null && ovObjNm != null && !ovObjAA.ansLock) {
					this.transform.position = GameObject.Find(ovObjNm).transform.position;
//					/* Make sure obj at original z-val */
					ovObjAA.ansLock = true;
					this.lockAns = true;
					this.atDest = true;
					transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, 0);

				} else {
					this.atDest = false;
					this.lockAns = false;
				}
			/* Without both of these, it will stick if it has previously been assigned */ 
			} else {
				this.atDest = false;
				this.lockAns = false;
			}
		}

	}

	void OnMouseEnter() {

		if (!GameObject.Find("button_done").GetComponent<doneButton>().endSim && !this.lockAns ) {
			this.GetComponent<Renderer>().material.color = mouseOverColor;
		}
	}
	
	void OnMouseExit() {
		if (!GameObject.Find("button_done").GetComponent<doneButton>().endSim) {
			this.GetComponent<Renderer> ().material.color = originalColor;
		}
	}

	void OnMouseDown() {

		if (!GameObject.Find("button_done").GetComponent<doneButton>().endSim) {
			distance = Vector3.Distance(transform.position, Camera.main.transform.position);
			dragging = true;
		}
	}

	void OnMouseUp() {
		if (!GameObject.Find("button_done").GetComponent<doneButton>().endSim) {
			dragging = false;

			/* Detect if at a destination */
			if (this.name.Contains("box")) {
				overCorrectType("box");
			}
			if (this.name.Contains("arrow")) {
				overCorrectType("arrow");
			}

			/* Reset z-val to original */
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, 0 );
		}
//		string dl = "H:"+atHome.ToString() +" D:"+ atDest.ToString();
//		Debug.Log (dl);
	}

	void Start () {
		originalColor = this.GetComponent<Renderer> ().material.color;
		initLoc = this.transform.position;
	}
	
	void FixedUpdate () {
		/* NB The name "button_done" here must be an exact match */
		if (!GameObject.Find("button_done").GetComponent<doneButton>().endSim) {
			if (dragging) {

				this.GetComponent<Renderer>().material.color = mouseOverColor;

//				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//				Vector3 rayPoint = ray.GetPoint(distance);
//				x = rayPoint.x;
//				y = rayPoint.y;
				Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 mousePos2D = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
				x = mousePos2D.x;
				y = mousePos2D.y;
				this.transform.position = new Vector3 (x, y, -1);
			}
			if ( !atHome && !atDest ) {
				float spd = 100 * Time.deltaTime;
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.initLoc, spd);
			}
			if ( this.transform.position != initLoc ) {
				atHome = false;
			} else {
				atHome = true;
			}
		}
	}
}
