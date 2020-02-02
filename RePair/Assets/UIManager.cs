using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public GameObject logo;
	public GameObject tutorialManager;
	public GameObject badEnding;
	public GameObject goodEnding;
	public GameObject restartText;
    // Start is called before the first frame update
    void Start()
    {
	Invoke("ShowTutor", 6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void ShowTutor()
  {
	tutorialManager.SetActive(true);
  }
}
