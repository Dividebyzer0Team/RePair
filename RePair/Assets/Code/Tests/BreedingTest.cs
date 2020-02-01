using UnityEngine;

public class BreedingTest : MonoBehaviour
{
	public GameObject animalPrefab;

	private float m_animalSpawnRadius;

	void Start()
	{
		m_animalSpawnRadius = GetComponent<RectTransform>().rect.height / 2 * 0.7f;
	}

	public void RandomBreed()
	{
		// spawning new child object
		var child = GameObject.Instantiate(animalPrefab, transform);
		var childTransform = child.GetComponent <RectTransform> ();
		var randomPos = Random.insideUnitCircle.normalized * m_animalSpawnRadius;
		childTransform.localPosition = new Vector3(randomPos.x, randomPos.y, 0);

		// choosing parents
		var allAnimals = GetComponentsInChildren <AnimalTest> ();
		int parent1Index = Random.Range(0, allAnimals.Length - 1), parent2Index = 0;
		do
		{
			parent2Index = Random.Range(0, allAnimals.Length - 1);
		}
		while (parent2Index == parent1Index); // second parent index must be different from first parent index

		AnimalTest parent1 = allAnimals[parent1Index], parent2 = allAnimals[parent2Index];

		// making new genome
		var childGenome = Genome.Breed(parent1.Genome, parent2.Genome);
		child.GetComponent <AnimalTest> ().Init(childGenome.Genes);
	}
}
