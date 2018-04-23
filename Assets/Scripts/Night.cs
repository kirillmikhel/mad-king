using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Night : MonoBehaviour {

	public GameObject UIStatsUpdate;

	// Use this for initialization
	void Start () {
		UIStatsUpdate.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator FadeIn() {
		Image UINight = GetComponent<Image> ();

		while(UINight.color.a > 0) {
			Color nightColor = UINight.color;
			yield return new WaitForSeconds (Time.deltaTime);
			nightColor.a -= Time.deltaTime;
			UINight.color = nightColor;
		}
		gameObject.SetActive (false);
	}

	public IEnumerator FadeOut() {
		Image UINight = GetComponent<Image> ();

		gameObject.SetActive (true);
		while(UINight.color.a < 1) {
			Color nightColor = UINight.color;
			yield return new WaitForSeconds (Time.deltaTime);
			nightColor.a += Time.deltaTime;
			UINight.color = nightColor;
		}
	}

	public void HideStatsUpdate() {
		UIStatsUpdate.SetActive (false);
	}

	public IEnumerator ShowStatsFutureUpdate(GameManager game) {
		
		UIStatsUpdate.transform.Find ("Day").GetComponent<Text> ().text = game.DaysLeft() + " days left";

		UIStatsUpdate.transform.Find ("Stats").GetComponent<Text> ().text = ""
			+ string.Format ("Food: {0} ({1})\n", game.food, (game.GetFoodChange() > 0 ? "+" : "") + game.GetFoodChange())
			+ string.Format ("Gold: {0} ({1})\n", game.gold, (game.GetGoldChange() > 0 ? "+" : "") + game.GetGoldChange())
			+ string.Format ("Building materials: {0} ({1})\n", game.buildingResources, (game.GetBuildingResourcesChange() > 0 ? "+" : "") + game.GetBuildingResourcesChange())
			+ string.Format ("Weapons: {0} ({1})\n", game.weapons, (game.GetWeaponsChange() > 0 ? "+" : "") + game.GetWeaponsChange())
			+ string.Format ("Population: {0} ({1})\n", game.population, (game.GetPopulationChange() > 0 ? "+" : "") + game.GetPopulationChange())
			+ string.Format ("Army: {0} ({1})\n", game.army, (game.GetArmyChange() > 0 ? "+" : "") + game.GetArmyChange())
			+ string.Format ("Workers: {0} ({1})\n\n", game.workers, (game.GetWorkersChange() > 0 ? "+" : "") + game.GetWorkersChange())
			+ string.Format ("Happiness: {0} ({1})", game.happiness, (game.GetHappinessChange() > 0 ? "+" : "") + game.GetHappinessChange())
			;
		
		UIStatsUpdate.SetActive (true);

		yield return new WaitForSeconds (3);

		HideStatsUpdate ();

	}

	public IEnumerator ShowStatsUpdated(GameManager game) {
		UIStatsUpdate.transform.Find ("Day").GetComponent<Text> ().text = game.DaysLeft() + " days left";

		UIStatsUpdate.transform.Find ("Stats").GetComponent<Text> ().text = ""
			+ string.Format ("Food: {0}\n", game.food)
			+ string.Format ("Gold: {0}\n", game.gold)
			+ string.Format ("Building materials: {0}\n", game.buildingResources)
			+ string.Format ("Weapons: {0}\n", game.weapons)
			+ string.Format ("Population: {0}\n", game.population)
			+ string.Format ("Army: {0}\n", game.army)
			+ string.Format ("Workers: {0}\n\n", game.workers)
			+ string.Format ("Happiness: {0}", game.happiness)
			;

		UIStatsUpdate.SetActive (true);

		yield return new WaitForSeconds (3);

		HideStatsUpdate ();
	}
}
