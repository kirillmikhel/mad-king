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

	public bool jestIsSad = false;
	public bool dogHasFood = false;

	private Deck requestDeck = null;
	private Deck replyDeck = null;
	private Hand hand = null;

	private Mediator mediator;

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

		CheckWinConditions ();

		hand.AddCard (replyDeck.DrawCard ());

		dogHasFood = Random.Range (0, 100) > 50;
		jestIsSad = Random.Range (0, 100) > 50;

		mediator.SetRequest(requestDeck.DrawCard ());
		mediator.StartRequest ();
	}

	private void CheckWinConditions () {
		if (happiness < 30) {
			// TODO: Show failure screen
		}

		if (currentDay >= daysToWin) {
			// TODO: Show victory screen
		}
	}
}
