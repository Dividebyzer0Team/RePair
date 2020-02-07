using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
  public enum ComparisonOperator
  {
    EQUAL,
    LESS,
    GREATER,
    LESS_OR_EQUAL,
    GREATER_OR_EQUAL
  }

  [System.Serializable]
  public class SpeciesComparisonExpr
  {
    public AnimalPreset species;
    public ComparisonOperator comparator;
    public int amount;

    private int dnaId = -1;
    public int DnaId {
        get {
            if (species != null && dnaId < 0)
                dnaId = Genome.CalculateDnaId(species.genes);
            return dnaId;
        }
    }

		public bool Evaluate(int otherDnaId, int otherAmount)
		{
			if (otherDnaId != dnaId)
				return false;
			
			switch (comparator) {
				case ComparisonOperator.EQUAL:
					return otherAmount == amount;
					break;
				case ComparisonOperator.LESS:
					return otherAmount < amount;
					break;
				case ComparisonOperator.LESS_OR_EQUAL:
					return otherAmount <= amount;
					break;
				case ComparisonOperator.GREATER:
					return otherAmount > amount;
					break;
				case ComparisonOperator.GREATER_OR_EQUAL:
					return otherAmount >= amount;
					break;
			}

			return true;
		}

		public bool Evaluate(Dictionary<int, int> animalCounters)
		{
			int amount;

			if (!animalCounters.TryGetValue(dnaId, out amount))
				amount = 0;

			return Evaluate(dnaId, amount);
		}
  }

	public List<SpeciesComparisonExpr> allOfThese;

	public bool Satisfies()
	{
		Dictionary<int, int> animalCounters = CountAnimals();
		foreach (SpeciesComparisonExpr expr in allOfThese) {
			if (!expr.Evaluate(animalCounters))
				return false;
		}

		return true;
	}

	public Dictionary<int, int> CountAnimals()
	{
		Dictionary<int, int> animalCounters = new Dictionary<int, int>();
		GameController game = GameController.GetInstance();

		if (game == null)
			return animalCounters;

		foreach (GameObject animalGO in game.animals) {
			Animal animal = animalGO.GetComponent<Animal>();
			if (animal.ActiveDnaId != 0 && !animal.IsDead()) {
				if (!animalCounters.ContainsKey(animal.ActiveDnaId))
					animalCounters[animal.ActiveDnaId] = 1;
				else
					++animalCounters[animal.ActiveDnaId];
			}
		}

		return animalCounters;
	}
}
