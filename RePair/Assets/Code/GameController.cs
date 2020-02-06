using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
  [System.Serializable]
  public struct AnimalSpawn
	{
		public GameObject prefab;
		public int amount;
  }

  public GameObject animalBase;
	public GameObject gameBounds;
	public GameObject matingEffect;
	public float animalHungerFactor = 1.0f;
	public float animalFeedingFactor = 1.0f;
	public float animalRunningDirectionRandomizeInterval = 0.25f;
	public bool gameStarted;
	public float defaultFertilityAge = 15.0f;
	public float defaultMatingCooldown = 6.0f;

	public List<GameObject> animals;
	public List<AnimalSpawn> spawns;

	private static GameController instance;

	public static GameController GetInstance()
	{
		return GameController.instance;
	}

	private void Awake()
	{
		GameController.instance = this;
		animals = new List<GameObject>();

		SpawnAnimals();
		StartGame();
	}

  public void Genocide()
  {
		foreach (GameObject animalGO in animals)
			Destroy(animalGO);

		animals.Clear();
  }

  public void SpawnAnimals()
  {
		gameStarted = true;

		foreach (AnimalSpawn spawn in spawns) {
			for (int i = 1; i<= spawn.amount; i++) {
				GameObject animalGO = Instantiate(spawn.prefab);
				animalGO.transform.position = new Vector2(Random.Range(-gameBounds.transform.localScale.x / 2, gameBounds.transform.localScale.x / 2), 0);
		  }
		}
  }

	public void StartGame()
  {
		gameStarted = true;
  }

  public void EndGame()
  {
		gameStarted = false;
		Genocide();
  }
}
