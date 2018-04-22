using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dog : MonoBehaviour {

	public bool hasFood;

	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Dog " + (hasFood ? "has food" : "is hungry");
	}
}
