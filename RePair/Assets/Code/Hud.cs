using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
	public GameObject widgetPrefab;
	private SortedDictionary<int, GameObject> m_widgets = new SortedDictionary<int, GameObject>();
	// FIXME better move to another place
	private Dictionary<int, AnimalPreset> m_species = new Dictionary<int, AnimalPreset>();

	private const float WIDGET_SIZE = 4.0f;

	private void InitSpeciesWidgetMap()
	{
		GameController game = GameController.GetInstance();
		WinCondition winCondition = null;

		m_widgets.Clear();
		m_species.Clear();

		if (game != null)
			winCondition = game.GetComponent<WinCondition>();

		if (winCondition == null)
			return;

		int speciesNum = 0;
		foreach (WinCondition.SpeciesComparisonExpr speciesExpr in winCondition.allOfThese) {
			AnimalPreset species = speciesExpr.species;
			if (species == null || m_widgets.ContainsKey(speciesExpr.DnaId))
				continue;

			GameObject widget = Instantiate(widgetPrefab, new Vector3(0.0f, 0.0f, 1.0f), Quaternion.identity, gameObject.transform);
			widget.transform.localPosition = new Vector3(-29.0f + WIDGET_SIZE * speciesNum, 13.0f, 1.0f);
			m_widgets.Add(speciesExpr.DnaId, widget);
			m_species.Add(speciesExpr.DnaId, speciesExpr.species);
			++speciesNum;
		}
	}

	void Start()
	{
		InitSpeciesWidgetMap();

		if (m_widgets.Count <= 0) {
			gameObject.SetActive(false);
			return;
		}

		foreach (KeyValuePair<int, GameObject> entry in m_widgets) {
			GameObject widget = entry.Value;
			// FIXME better instantiate inside widget
			Transform view = widget.transform.Find("CombinedView");
			if (view == null) {
				Debug.Log("Warning: Could not find view game object in a widget");
				continue;
			}

			SkeletonController skelController = view.gameObject.GetComponent<SkeletonController>();
			if (skelController == null) {
				Debug.Log("Warning: SkeletonController component not found in the widget view");
				continue;
			}

			AnimalPreset species;
			if (!m_species.TryGetValue(entry.Key, out species)) {
				Debug.Log("Warning: AnimalPreset not found for the HUD, DNA ID: " + entry.Key);
				continue;
			}

			skelController.SetBodyPart("Head", species.Head.skeletonAsset);
			skelController.SetBodyPart("Front", species.Body.skeletonAsset);
			skelController.SetBodyPart("Rear", species.Legs.skeletonAsset);
		}

		UpdateHud();
	}

	public void UpdateHud()
	{
		GameController game = GameController.GetInstance();
		if (game == null || !gameObject.activeSelf)
			return;

		Dictionary<int, int> animalCounters = new Dictionary<int, int>();

		foreach (GameObject animalGO in game.animals) {
			Animal animal = animalGO.GetComponent<Animal>();
			if (animal.ActiveDnaId != 0 && !animal.IsDead()) {
				if (!animalCounters.ContainsKey(animal.ActiveDnaId))
					animalCounters[animal.ActiveDnaId] = 1;
				else
					++animalCounters[animal.ActiveDnaId];
			}
		}

		foreach (KeyValuePair<int, GameObject> entry in m_widgets) {
				TextMesh textMesh = entry.Value.GetComponent<TextMesh>();
				int animalCounter;
				animalCounters.TryGetValue(entry.Key, out animalCounter);
				textMesh.text = "x" + animalCounter;
		}
	}
}
