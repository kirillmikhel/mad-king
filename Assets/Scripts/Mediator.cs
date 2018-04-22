using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetRequest(Transform request) {
		request.SetParent (transform);
		request.localScale = new Vector3 (1, 1, 1);
	}

	public RequestCard GetRequest() {
		return GetComponentInChildren<RequestCard> ();
	}

	public void StartRequest () {
		
	}

	public void EndRequest() {
		RequestCard request = GetRequest ();
		if (request.postponable && request.reply == "later") {
			Deck requestDeck = GameObject.Find ("RequestDeck").GetComponent<Deck> ();
			requestDeck.AddCard (request.transform);
			requestDeck.Shuffle ();
		}

		Destroy (request.gameObject);
	}

}
