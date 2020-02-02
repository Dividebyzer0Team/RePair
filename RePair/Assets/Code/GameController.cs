using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
  public GameObject animalBase;
  public float animalHungerFactor = 1.0f;
  public float animalFeedingFactor = 1.0f;
  public float animalRunningDirectionRandomizeInterval = 0.25f;
  public bool gameStarted;
  public GameObject gameBounds;

  public List<GameObject> animals;
  [System.Serializable]
  public struct AnimalSpawn
  {
	public GameObject prefab;
	public int amount;
  }
  public List<AnimalSpawn> spawns;

  static GameController instance;
  public static GameController GetInstance()
  {
	return GameController.instance;
  }

  private void Awake()
  {
	GameController.instance = this;
	animals = new List<GameObject>();
	SpawnAnimals();
  }

  public void Genocide()
  {
		foreach (GameObject animalGO in animals)
		{
		  Destroy(animalGO);
		}
		animals.Clear();
  }

  public void SpawnAnimals()
  {
		gameStarted = true;
		foreach (AnimalSpawn spawn in spawns)
		{
			for (int i = 1; i<= spawn.amount; i++)
		  {
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
