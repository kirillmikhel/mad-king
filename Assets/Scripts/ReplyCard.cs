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
		case 0:
			return "later";
		case 1:
			return "yes";
		default:
			return "wat";
		}
	}

	public string getAction() {
		return action;
	}
}
