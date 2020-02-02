using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject animalBase;
	public float animalHungerFactor = 1.0f;
	public float animalFeedingFactor = 1.0f;
	public float animalRunningDirectionRandomizeInterval = 0.25f;

	static GameController instance;
	public static GameController GetInstance()
	{
		return GameController.instance;
	}

	private void Awake()
	{
		GameController.instance = this;
	}
}
