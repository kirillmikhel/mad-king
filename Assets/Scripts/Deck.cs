using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Shuffle ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Shuffle () {
		for(int i = 0; i < transform.childCount; i++) {
			Transform card = transform.GetChild (Random.Range (0, transform.childCount));
			card.SetParent (null);
			card.SetParent (transform);
		}
	}

	public Transform DrawCard() {
		return transform.GetChild (0);
	}
}
