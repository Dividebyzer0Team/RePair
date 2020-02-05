using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool success = false;

        GameController game = GameController.GetInstance();
        if (game.winCondition.species != null) {
            Transform view = gameObject.transform.Find("CombinedView"); // better instantiate
            if (view != null) {
                SkeletonController skelController = view.gameObject.GetComponent<SkeletonController>();
                skelController.SetBodyPart("Head", game.winCondition.species.Head.skeletonAsset);
                skelController.SetBodyPart("Front", game.winCondition.species.Body.skeletonAsset);
                skelController.SetBodyPart("Rear", game.winCondition.species.Legs.skeletonAsset);
                UpdateHud();
                success = true;
            }
        }
        
        if (!success)
            gameObject.SetActive(false);
    }

  public void UpdateHud()
  {
      GameController game = GameController.GetInstance();

      if (!gameObject.activeSelf || game.winCondition.DnaId < 0)
          // not watching anything
          return;

      int watchCount = 0;
      foreach (GameObject animalGO in game.animals) {
          Animal animal = animalGO.GetComponent<Animal>();
          if (game.winCondition.DnaId == animal.ActiveDnaId && !animal.IsDead())
              watchCount += 1;
      }

      TextMesh textMesh = gameObject.GetComponent<TextMesh>();
      textMesh.text = "x" + watchCount;
  }
}
