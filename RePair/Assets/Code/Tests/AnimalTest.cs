using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalTest : MonoBehaviour
{
	public Animal animal;

	private Genome m_genome;
	private Text m_text;

	public Genome Genome => m_genome;

	void Start()
	{
		if (animal) Init(animal.genes);
	}

	public void Init(List <Gene> genes)
	{
		m_genome = new Genome(genes);
		m_text = GetComponentInChildren<Text>();
		Print();
	}

	public void Print()
	{
		m_text.text = name + "\n=======\n";
		
		var head = m_genome.Head;
		m_text.text += "Head:\n";
		foreach (var trait in head.traits)
		{
			m_text.text += "- " + trait.name + "\n";
		}

		var body = m_genome.Body;
		m_text.text += "Body:\n";
		foreach (var trait in body.traits)
		{
			m_text.text += "- " + trait.name + "\n";
		}

		var legs = m_genome.Legs;
		m_text.text += "Legs:\n";
		foreach (var trait in legs.traits)
		{
			m_text.text += "- " + trait.name + "\n";
		}
	}
}
