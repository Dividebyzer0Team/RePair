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
        if (game.watchSpecies != null) {
            Transform view = gameObject.transform.Find("CombinedView"); // better instantiate
            if (view != null) {
                SkeletonController skelController = view.gameObject.GetComponent<SkeletonController>();
                skelController.SetBodyPart("Head", game.watchSpecies.Head.skeletonAsset);
                skelController.SetBodyPart("Front", game.watchSpecies.Body.skeletonAsset);
                skelController.SetBodyPart("Rear", game.watchSpecies.Legs.skeletonAsset);
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

      if (!gameObject.activeSelf || game.WatchSpeciesDnaId < 0)
          // not watching anything
          return;

      int watchCount = 0;
      foreach (GameObject animalGO in game.animals) {
          Animal animal = animalGO.GetComponent<Animal>();
          if (game.WatchSpeciesDnaId == animal.ActiveDnaId && !animal.IsDead())
              watchCount += 1;
      }

      TextMesh textMesh = gameObject.GetComponent<TextMesh>();
      textMesh.text = "x" + watchCount;
  }
}
