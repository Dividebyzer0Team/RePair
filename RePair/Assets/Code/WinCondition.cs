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
  }

	public List<SpeciesComparisonExpr> allOfThese;
}
