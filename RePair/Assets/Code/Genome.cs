﻿using System.Collections.Generic;
using UnityEngine;

public class Genome
{
	private List <Gene> m_genes;
	private List <Gene> m_inactiveGenes;
    private int activeDnaId = 0;

	// all genes and individual parts as read-only properties
	public List <Gene> Genes => m_genes;
	public Gene Head => m_genes[0];
	public Gene Body => m_genes[1];
	public Gene Legs => m_genes[2];

    public int ActiveDnaId => activeDnaId;

	private Genome()
	{
		m_genes = new List <Gene> ();
		m_inactiveGenes = new List <Gene> ();
	}

	public Genome(List <Gene> genes)
	{
		m_genes = new List <Gene> (genes);
		// at first generation inactive genes are the same as active
		m_inactiveGenes = new List <Gene> (genes);
        activeDnaId = CalculateDnaId(m_genes);
	}

	public static Genome Breed(Genome parent1, Genome parent2)
	{
		var child = new Genome();
		var allGenes = new List <List <Gene>> { parent1.m_genes, parent1.m_inactiveGenes, parent2.m_genes, parent2.m_inactiveGenes };

		// need to find min gene set length
		var minGeneSetLength = int.MaxValue;
		foreach (var genes in allGenes)
		{
			if (genes.Count < minGeneSetLength) minGeneSetLength = genes.Count;
		}

		// combining genes randomly
		for (int i = 0; i < minGeneSetLength; ++i)
		{
			var activeGeneSetIndex = Random.Range(0, allGenes.Count - 1);
			child.m_genes.Add(allGenes[activeGeneSetIndex][i]);
			var inactiveGeneSetIndex = 0;
			do
			{
				inactiveGeneSetIndex = Random.Range(0, allGenes.Count - 1);
			}
			while (inactiveGeneSetIndex == activeGeneSetIndex); // inactive gene index should be different from active
			child.m_inactiveGenes.Add(allGenes[inactiveGeneSetIndex][i]);
		}

        child.activeDnaId = CalculateDnaId(child.m_genes);
		return child;
	}

    public static int CalculateDnaId(List<Gene> genes)
    {
        int result = 0;
        foreach (Gene gene in genes) {
            if (gene.id != Gene.GeneId.UNKNOWN)
                result |= (1 << (int) gene.id);
        }
        return result;
    }

	public Dictionary <string, float> GetAllTraits()
	{
		var allTraits = new Dictionary <string, float> ();

		foreach (var gene in m_genes)
		{
			foreach (var trait in gene.traits)
			{
				if (allTraits.ContainsKey(trait.name))
				{
					allTraits[trait.name] += trait.factor;
				}
				else
				{
					allTraits[trait.name] = trait.factor;
				}
			}
		}

		// simply dividing summed traits by 3 for now
		var keys = new List <string> (allTraits.Keys);
		foreach (var k in keys)
		{
			allTraits[k] = allTraits[k];
		}

		return allTraits;
	}
}
