using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int happiness = 70;
	public int population = 100;
	public int army = 50;
	public int workers = 20;

	public int food = 500;
	public int gold = 300;
	public int weapons = 50;
	public int buildingResources = 100;

	public int daysToWin = 30;
	public int currentDay = 0;
	public int dayToLoose1stCard = 10;
	public int dayToLoose2ndCard = 20;

	public string[] willHappen;

	private Deck requestDeck = null;
	private Deck replyDeck = null;
	private Hand hand = null;

	public Mediator mediator;
	public King king;
	public Jest jest;
	public Dog dog;

	private Text UIDaysLeft = null;
	private Text UIHappiness = null;
	private Text UIProblems = null;

	public Image UINight;
	public Text UIStatsFood = null;
	public Text UIStatsGold = null;
	public Text UIStatsWeapons = null;
	public Text UIStatsBuildingResources = null;
	public Text UIStatsPopulation = null;
	public Text UIStatsArmy = null;
	public Text UIStatsWorkers = null;

	// Use this for initialization
	void Start () {
		requestDeck = GameObject.Find ("RequestDeck").GetComponent<Deck> ();
		replyDeck = GameObject.Find ("ReplyDeck").GetComponent<Deck> ();
		hand = GameObject.Find ("Hand").GetComponent<Hand> ();

		UIDaysLeft = GameObject.Find ("DaysLeft/Text").GetComponent<Text> ();
		UIHappiness = GameObject.Find ("Happiness/Text").GetComponent<Text> ();
		UIProblems = GameObject.Find ("Problems").GetComponent<Text> ();

		NextDay ();
	}

	// Update is called once per frame
	void Update () {

		UIDaysLeft.text = (daysToWin - currentDay + 1).ToString();
		UIHappiness.text = (happiness).ToString();

		string problemsText = "";

		if (food < 0)
			problemsText += "Hunger\n";
		if (gold < 0)
			problemsText += "Need more gold\n";
		if (army < population / 4)
			problemsText += "Small army\n";
		if (weapons < army)
			problemsText += "Not enough weapons\n";
		if (buildingResources < 0)
			problemsText += "Need more building resources\n";

		UIProblems.text = problemsText;

		UIStatsFood.text = "Food: " + food + " (" + GetFoodChange() + ")";
		UIStatsGold.text = "Gold: " + gold + " (" + GetGoldChange() + ")";
		UIStatsWeapons.text = "Weapons: " + weapons;
		UIStatsBuildingResources.text = "Building materials: " + buildingResources + " (" + GetBuildingResourcesChange() + ")";
		UIStatsPopulation.text = "Population: " + population + " (" + GetPopulationChange() + ")";
		UIStatsArmy.text = "Army: " + army;
		UIStatsWorkers.text = "Workers: " + workers;
	}

	public void NextDay () {
		currentDay++;

		UpdateStats ();

		ResolveStatsChange (willHappen);
		willHappen = new string[0];

		CheckWinConditions ();

		if (currentDay == dayToLoose1stCard || currentDay == dayToLoose2ndCard) {
			hand.DiscardRandomCard ();
		}

		hand.AddCard (replyDeck.DrawCard ());

		dog.hasFood = Random.Range (0, 100) > 50;
		jest.isSad = Random.Range (0, 100) > 50;

		mediator.SetRequest(requestDeck.DrawCard ());

		StartCoroutine (Day());
	}

	private void CheckWinConditions () {
		if (happiness <= 0) {
			// TODO: Show failure screen
		}

		if (currentDay >= daysToWin) {
			// TODO: Show victory screen
		}
	}

	private IEnumerator Day () {
 		// Fade in		
		while(UINight.color.a > 0) {
			Color nightColor = UINight.color;
			yield return new WaitForSeconds (Time.deltaTime);
			nightColor.a -= Time.deltaTime;
			UINight.color = nightColor;
		}
		UINight.gameObject.SetActive (false);

		yield return new WaitForSeconds (1);
		RequestCard request = mediator.GetRequest ();

		mediator.GetComponent<Talkable>().Say ("Your Majesty, " + request.initialText);
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

			king.GetComponent<Talkable> ().Silent ();

			bool yesJest = Random.Range (0, 100) > 50;

			jest.GetComponent<Talkable> ().Say (jest.isSad ? "Whatever... " + (yesJest ? "Yes" : "No") : "Absolutely yes!");

			mediator.GetRequest ().reply = jest.isSad ? (yesJest ? "yes" : "no") : "yes";
			break;
		case "ask_dog":
			king.GetComponent<Talkable> ().Say ("Ask my dog");

			yield return new WaitForSeconds (3);

			king.GetComponent<Talkable>().Silent ();

			dog.GetComponent<Talkable> ().Say (dog.hasFood ? "Hmph hmph... (later)" : "Rrrrr! (no)");

			mediator.GetRequest ().reply = dog.hasFood ? "later" : "no";
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

		yield return new WaitForSeconds (1);

		// Fade out
		UINight.gameObject.SetActive (true);
		while(UINight.color.a < 1) {
			Color nightColor = UINight.color;
			yield return new WaitForSeconds (Time.deltaTime);
			nightColor.a += Time.deltaTime;
			UINight.color = nightColor;
		}

		yield return new WaitForSeconds (1);

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
			case "workers":
				workers += value;
				break;
			case "weapons":
				weapons += value;
				break;
			default:
				break;
			}
		}
	}

	private int GetFoodChange () {
		return (int)(- population - army - workers) / 2;
	}

	private int GetGoldChange () {
		return - army - workers + (int) population / 4;
	}

	private int GetBuildingResourcesChange () {
		return workers;
	}

	private int GetPopulationChange() {
		return (int)(food/100);
	}

	private void UpdateStats () {
		food += GetFoodChange ();
		gold += GetGoldChange ();
		buildingResources += GetBuildingResourcesChange();

		population += GetPopulationChange ();

		happiness += 
			(food < 0 ? -5 : 2) // food availablity
			+ (army < population / 4 ? -5 : 2) // safety
			+ (gold < 0 ? -5 : 0) // salary
			+ (weapons < army ? - (int) (army - weapons) / 4 : 0) // weapons availablity
			;

		happiness = Mathf.Clamp (happiness, 0, 100);
	}
}
