using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject animalBase;
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