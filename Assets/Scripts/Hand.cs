using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddCard (Transform card) {
		card.SetParent (transform);
		card.localScale = new Vector3 (1, 1, 1);
	}

	public void DiscardRandomCard () {
		Destroy (transform.GetChild(Random.Range(0,transform.childCount)).gameObject);
	}
}
