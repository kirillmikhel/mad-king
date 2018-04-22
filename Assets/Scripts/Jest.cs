using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jest : MonoBehaviour {

	public bool isSad;

	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Jest " + (isSad ? "is sad" : "is happy");
	}
}
