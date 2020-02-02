using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public GameObject logo;
	public GameObject tutorialManager;
	public GameObject goodEnding;
    // Start is called before the first frame update
    void Start()
    {
			logo.SetActive(true);
			Invoke("ShowTutor", 6f);
	
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void ShowTutor()
  {
	tutorialManager.SetActive(true);
	Invoke("StartGame", 7f);
	Invoke("ShowEndMentor", 300f);
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
	goodEnding.SetActive(true);
	Invoke("RestartGame", 10f);
  }
}
