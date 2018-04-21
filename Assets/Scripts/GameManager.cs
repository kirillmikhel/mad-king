using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public int kingsMood = 0;

	private Deck requestDeck = null;

	// Use this for initialization
	void Start () {
		requestDeck = GameObject.Find ("RequestDeck").GetComponent<Deck> ();
	}

	// Update is called once per frame
	void Update () {


	}

	public void NextDay () {
		currentDay++;

		// TODO: Update stats

		CheckWinConditions ();

		// TODO: Get new Request
	
		kingsMood = Random.Range (-1, 2);

		// TODO: Update hand and draw a card
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
