using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
	void Start()
	{
		bool success = false;
		GameController game = GameController.GetInstance();
		WinCondition winCondition = null;
		if (game != null)
			winCondition = game.GetComponent<WinCondition>();

		if (winCondition != null) {
			foreach (WinCondition.SpeciesComparisonExpr speciesExpr in winCondition.allOfThese) {
				AnimalPreset species = speciesExpr.species;
				if (species != null) {
					Transform view = gameObject.transform.Find("CombinedView"); // better instantiate
					if (view != null) {
						SkeletonController skelController = view.gameObject.GetComponent<SkeletonController>();
						skelController.SetBodyPart("Head", species.Head.skeletonAsset);
						skelController.SetBodyPart("Front", species.Body.skeletonAsset);
						skelController.SetBodyPart("Rear", species.Legs.skeletonAsset);
						UpdateHud();
						success = true;
					}
				}
			}
		}

		if (!success)
			gameObject.SetActive(false);
	}

	public void UpdateHud()
	{
		GameController game = GameController.GetInstance();

		if (game == null || !gameObject.activeSelf)
			// not watching anything
			return;

		WinCondition winCondition = game.GetComponent<WinCondition>();

		foreach (WinCondition.SpeciesComparisonExpr speciesExpr in winCondition.allOfThese) {
			int watchCount = 0; // replace with a map DnaId -> widget

			foreach (GameObject animalGO in game.animals) {
				Animal animal = animalGO.GetComponent<Animal>();
				if (speciesExpr.DnaId == animal.ActiveDnaId && !animal.IsDead())
					watchCount += 1;

				TextMesh textMesh = gameObject.GetComponent<TextMesh>();
				textMesh.text = "x" + watchCount;
			}
		}
	}
}
