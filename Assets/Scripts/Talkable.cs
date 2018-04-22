using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talkable : MonoBehaviour {

	public GameObject speechBubble;
	public Text speechText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Say (string text) {
		speechText.text = text;
		speechBubble.SetActive (true);
	}

	public void Silent () {
		speechText.text = "";
		speechBubble.SetActive (false);
	}
}
