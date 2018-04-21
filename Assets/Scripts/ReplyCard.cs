using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplyCard : MonoBehaviour {

	public string text;
	public string action;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string getAction(int kingsMood) {
		switch (kingsMood) {
		case -1:
			return "no";
			break;
		case 0:
			return "later";
			break;
		case 1:
			return "yes";
			break;
		default:
			return "wat";
			break;
		}
	}

	public string getAction() {
		return action;
	}
}
