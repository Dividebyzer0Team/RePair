using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalPreset", menuName = "RePair/Animal", order = 10)]
public class AnimalPreset : ScriptableObject
{
	public List <Gene> genes;
}
