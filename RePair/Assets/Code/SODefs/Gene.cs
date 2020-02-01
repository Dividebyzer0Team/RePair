using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gene", menuName = "RePair/Gene", order = 1)]
public class Gene : ScriptableObject
{
	[Serializable]
	public struct Trait
	{
		public string name;
		public float factor;
	}

	[Tooltip("Пока плэйсхолдер, мы не знаем как именно будет визуализироваться ген")]
	public Sprite representation;
	public List <Trait> traits;
}
