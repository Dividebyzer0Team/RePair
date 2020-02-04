using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalPreset", menuName = "RePair/Animal", order = 10)]
public class AnimalPreset : ScriptableObject
{
	public List <Gene> genes;
    public Gene Head => genes[0];
	public Gene Body => genes[1];
	public Gene Legs => genes[2];
}
