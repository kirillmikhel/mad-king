using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public Night Night;
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

		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}

		UIDaysLeft.text = DaysLeft().ToString();
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

		UIStatsFood.text = "Food: " + food + " (" + (GetFoodChange() > 0 ? "+" : "") + GetFoodChange() + ")";
		UIStatsGold.text = "Gold: " + gold + " (" + (GetGoldChange() > 0 ? "+" : "") + GetGoldChange() + ")";
		UIStatsWeapons.text = "Weapons: " + weapons;
		UIStatsBuildingResources.text = "Building materials: " + buildingResources + " (" + (GetBuildingResourcesChange() > 0 ? "+" : "") + GetBuildingResourcesChange() + ")";
		UIStatsPopulation.text = "Population: " + population + " (" + (GetPopulationChange() > 0 ? "+" : "") + GetPopulationChange() + ")";
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
			SceneManager.LoadScene ("gameover");
		}

		if (currentDay >= daysToWin) {
			SceneManager.LoadScene ("victory");
		}
	}

	private IEnumerator Day () {
		yield return StartCoroutine(Night.ShowStatsUpdated(this));
		yield return StartCoroutine(Night.FadeIn());

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

		yield return StartCoroutine(Night.FadeOut());
		yield return StartCoroutine(Night.ShowStatsFutureUpdate(this));

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

	public int GetFoodChange () {
		return (int)(- population - army - workers) / 3;
	}

	public int GetGoldChange () {
		return - army - workers + (int) population / 4;
	}

	public int GetBuildingResourcesChange () {
		return workers;
	}

	public int GetWeaponsChange () {
		return 0;
	}

	public int GetPopulationChange() {
		return (int)(food/100);
	}

	public int GetArmyChange () {
		return 0;
	}

	public int GetWorkersChange () {
		return 0;
	}

	public int GetHappinessChange() {
		return (food < 0 ? -5 : 2) // food availablity
			+ (army < population / 4 ? -3 : 2) // safety
			+ (gold < 0 ? -5 : 0) // salary
			+ (weapons < army ? - (int) (army - weapons) / 6 : 0) // weapons availablity
			;
	}

	private void UpdateStats () {
		int newFood =  food + GetFoodChange ();
		int newGold = gold + GetGoldChange ();
		int newBuildingResources = buildingResources + GetBuildingResourcesChange();

		int newPopulation = population + GetPopulationChange ();

		int newHappiness = happiness + GetHappinessChange ();

		newHappiness = Mathf.Clamp (newHappiness, 0, 100);

		// Apply changes
		food = newFood;
		gold = newGold;
		buildingResources = newBuildingResources;
		population = newPopulation;
		happiness = newHappiness;
	}

	public int DaysLeft() {
		return daysToWin - currentDay + 1;
	}
}
