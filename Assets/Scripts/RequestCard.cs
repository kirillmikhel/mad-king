using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestCard : MonoBehaviour {

	public string initialText;
	public bool postponable = true;
	public string postponeText = "As you wish, your majesty";
	public string agreeText = "As you wish, your majesty";
	public string disagreeText = "As you wish, your majesty";

	public string[] agreeResults;
	public string[] disagreeResults;
	public string[] postponeResults;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ResolveReply(string reply, GameManager gameManager) {
		//TODO: Resolve reply
	}
}
