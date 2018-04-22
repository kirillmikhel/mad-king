using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int happiness = 70;
	public int population = 100;
	public int army = 50;

	public int food = 500;
	public int gold = 300;
	public int weapons = 50;
	public int buildingResources = 100;

	public int daysToWin = 30;
	public int currentDay = 0;
	public int dayToLoose1stCard = 15;
	public int dayToLoose2ndCard = 25;

	public string[] willHappen;

	public bool jestIsSad = false;
	public bool dogHasFood = false;

	private Deck requestDeck = null;
	private Deck replyDeck = null;
	private Hand hand = null;

	public Mediator mediator;
	public King king;
	public Jest jest;
	public Dog dog;

	private Text UIDaysLeft = null;
	private Text UIHappiness = null;

	public Text UIStatsFood = null;
	public Text UIStatsGold = null;
	public Text UIStatsWeapons = null;
	public Text UIStatsBuildingResources = null;
	public Text UIStatsPopulation = null;
	public Text UIStatsArmy = null;

	// Use this for initialization
	void Start () {
		requestDeck = GameObject.Find ("RequestDeck").GetComponent<Deck> ();
		replyDeck = GameObject.Find ("ReplyDeck").GetComponent<Deck> ();
		hand = GameObject.Find ("Hand").GetComponent<Hand> ();

		UIDaysLeft = GameObject.Find ("DaysLeft/Text").GetComponent<Text> ();
		UIHappiness = GameObject.Find ("Happiness/Text").GetComponent<Text> ();

		NextDay ();
	}

	// Update is called once per frame
	void Update () {

		UIDaysLeft.text = (daysToWin - currentDay).ToString();
		UIHappiness.text = (happiness).ToString();

		UIStatsFood.text = "Food: " + food;
		UIStatsGold.text = "Gold: " + gold;
		UIStatsWeapons.text = "Weapons: " + weapons;
		UIStatsBuildingResources.text = "Building resources: " + buildingResources;
		UIStatsPopulation.text = "Population: " + population;
		UIStatsArmy.text = "Army: " + army;
	}

	public void NextDay () {
		currentDay++;

		// TODO: Update stats

		ResolveStatsChange (willHappen);
//		willHappen = [];

		CheckWinConditions ();

		hand.AddCard (replyDeck.DrawCard ());

		dogHasFood = Random.Range (0, 100) > 50;
		jestIsSad = Random.Range (0, 100) > 50;

		mediator.SetRequest(requestDeck.DrawCard ());

		StartCoroutine (Day());
	}

	private void CheckWinConditions () {
		if (happiness < 30) {
			// TODO: Show failure screen
		}

		if (currentDay >= daysToWin) {
			// TODO: Show victory screen
		}
	}

	private IEnumerator Day () {
		RequestCard request = mediator.GetRequest ();

		mediator.GetComponent<Talkable>().Say ("Your majesty, " + request.initialText);
		yield return new WaitUntil (() => mediator.GetRequest ().reply != "");
		mediator.GetComponent<Talkable>().Silent ();

		switch (mediator.GetRequest().reply) {
		case "toss_a_coin":
			king.GetComponent<Talkable> ().Say ("* tossing a coin *");

			yield return new WaitForSeconds (2);

			bool yes = Random.Range (0, 100) > 50;

			king.GetComponent<Talkable> ().Say (yes ? "Yes" : "No");

			mediator.GetRequest ().reply = yes ? "yes" : "no";
			break;
		case "ask_jest":
			king.GetComponent<Talkable> ().Say ("Ask my jest");

			yield return new WaitForSeconds (3);

			king.GetComponent<Talkable>().Silent ();

			jest.GetComponent<Talkable> ().Say (jestIsSad ? "Whatever... (later)" : "Absolutely yes!");

			mediator.GetRequest ().reply = jestIsSad ? "later" : "yes";
			break;
		case "ask_dog":
			king.GetComponent<Talkable> ().Say ("Ask my dog");

			yield return new WaitForSeconds (3);

			king.GetComponent<Talkable>().Silent ();

			dog.GetComponent<Talkable> ().Say (dogHasFood ? "Hmph hmph... (later)" : "Rrrrr! (no)");

			mediator.GetRequest ().reply = dogHasFood ? "later" : "no";
			break;
		case "yes":
			king.GetComponent<Talkable> ().Say ("Yes");
			break;
		case "no":
			king.GetComponent<Talkable> ().Say ("No");
			break;
		case "later":
			king.GetComponent<Talkable> ().Say ("Later");
			break;
		default:
			king.GetComponent<Talkable> ().Say ("Later");
			break;
		}

		yield return new WaitForSeconds (3);

		// Shut up everyone
		king.GetComponent<Talkable>().Silent ();
		jest.GetComponent<Talkable>().Silent ();
		dog.GetComponent<Talkable>().Silent ();

		mediator.GetComponent<Talkable>().Say (request.ResolveReply(this));
		yield return new WaitForSeconds (3);
		mediator.GetComponent<Talkable>().Silent ();
		mediator.EndRequest ();

		yield return new WaitForSeconds (3);

		NextDay ();
	}

	public void PlayCard (ReplyCard card) {
		mediator.GetRequest ().reply = card.GetAction ();
		Destroy (card.gameObject);
	}

	private void ResolveStatsChange (string[] statsChange) {
		foreach (string happening in statsChange) {
			string[] happeningPair = happening.Split (':');

			int value = int.Parse (happeningPair [1]);

			switch (happeningPair[0]) {
			case "food":
				food += value;
				break;
			case "buildingResources":
				buildingResources += value;
				break;
			case "gold":
				gold += value;
				break;
			case "population":
				population += value;
				break;
			case "army":
				army += value;
				break;
			case "weapons":
				weapons += value;
				break;
			default:
				break;
			}
		}
	}
}
