using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplyCard : MonoBehaviour {

	public string text;
	public string action;

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (Play);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetAction(int kingsMood) {
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

	public string GetAction() {
		return action;
	}

	public void Play() {
		GameObject.Find ("GameManager").GetComponent<GameManager> ().PlayCard (this);
	}
}
