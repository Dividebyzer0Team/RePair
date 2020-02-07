using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public GameObject logo;
	public GameObject tutorialManager;
	public GameObject goodEnding;
	public GameObject badEnding;

	void Start()
	{
		logo.SetActive(true);
		Invoke("ShowTutor", 6f);
	}

	void Update()
	{

	}

	void ShowTutor()
	{
		tutorialManager.SetActive(true);
		Invoke("StartGame", 7f);
		Invoke("ShowEndMentor", (float) GameController.GetInstance().levelTimeout);
	}

	void RestartGame()
	{
		SceneManager.LoadScene("Game");
	}

	void StartGame()
	{
		GameController.GetInstance().StartGame();
	}

	void ShowEndMentor()
	{
		GameController game = GameController.GetInstance();
		WinCondition winCond = game.GetComponent<WinCondition>();

		if (winCond == null || winCond.Satisfies())
			goodEnding.SetActive(true);
		else
			badEnding.SetActive(true);

		Invoke("RestartGame", 10f);
	}
}
