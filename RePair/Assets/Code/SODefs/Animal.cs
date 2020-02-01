using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "RePair/Animal", order = 10)]
public class Animal : ScriptableObject
{
	public List <Gene> genes;
}
