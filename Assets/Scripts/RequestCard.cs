using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestCard : MonoBehaviour {

	public string initialText;
	public string postponeText = "As you wish, your majesty";
	public string agreeText = "As you wish, your majesty";
	public string disagreeText = "As you wish, your majesty";
	public string watText = "Oh, of course, your majesty... (WTF?!)";

	public string[] onStart;
	public string[] onAgree;
	public string[] onDisagree;
	public string[] onPostpone;

	public bool postponable = true;
	public string reply;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string ResolveReply(GameManager game) {
		switch (reply) {
		case "yes":
			game.willHappen = onAgree;
			return agreeText;
		case "no":
			game.willHappen = onDisagree;
			return disagreeText;
		case "later":
			game.willHappen = postponable ? onPostpone : onDisagree;
			return postponeText;
		default:
			return watText;
		}
	}
}
