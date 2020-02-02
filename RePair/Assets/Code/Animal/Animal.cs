using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	public AnimalPreset preset;

	GameObject m_animalBase;
	Genome m_genome;
	string m_state; // Строка со стейтом спайна для анимирования
	Dictionary<string, float> m_traits;
	Rigidbody2D m_rigidbody;
	Behaviour m_behaviour;
	SkeletonController m_view;
	bool m_orientation; // true = left

	enum MovementMethod
	{
		WALK,
		JUMP,
		FLY
	}

	MovementMethod m_movementMethod = MovementMethod.WALK;
	// ТЕСТОВОЕ

	public Genome GetGenome()
	{
		return m_genome;
	}

	private void InitFromPreset()
	{
		m_genome = new Genome(preset.genes);
		InitAnimal();
	}

	public void Inherit(Animal parent1, Animal parent2)
	{
		m_genome = Genome.Breed(parent1.GetGenome(), parent2.GetGenome());
		InitAnimal();

	}

	private void InitAnimal()
	{
		Debug.Log("InitAnimal");
		m_traits = m_genome.GetAllTraits();
		m_traits["age"] = 0f;
		float canFly = GetTrait("flying") - GetTrait("size");
		if (canFly > 0)
		{
			m_traits["flyingHeight"] = canFly * 5f;
			m_movementMethod = MovementMethod.FLY;
			m_rigidbody.gravityScale = 0f;
		}

		//Invoke("Die", GetTrait("lifetimme"));
		float size = GetTrait("size");
        m_view.SetBodyPart("Head", m_genome.Head.animalName);
        m_view.SetBodyPart("Front", m_genome.Body.animalName);
        m_view.SetBodyPart("Rear", m_genome.Legs.animalName);
        m_view.enabled = false;
        m_view.enabled = true;
        //GameObject legsGO = Instantiate(m_genome.Legs.representation, m_view.transform);
        //legsGO.transform.localScale = new Vector2(size * 0.1f, size * 0.1f);
        //GameObject bodyGO = Instantiate(m_genome.Body.representation, m_view.transform);
        //bodyGO.transform.localScale = new Vector2(size * 0.1f, size * 0.1f);
        //GameObject headGO = Instantiate(m_genome.Head.representation, m_view.transform);
        //headGO.transform.localScale = new Vector2(size * 0.1f, size * 0.1f);
        transform.localScale = new Vector3(size, size, 1f);
	}


	void Die()
	{
		Destroy(this.gameObject);
	}

	void Awake()
	{
		m_animalBase = GameObject.Find("GameController").GetComponent<GameController>().animalBase;
		m_view = GetComponentInChildren<SkeletonController>();
		m_rigidbody = GetComponent<Rigidbody2D>();
		if (preset != null)
			InitFromPreset();
        Invoke("Idle", 3f);
	}

	void FixedUpdate()
	{
		if (m_behaviour != null)
            m_behaviour.Update(Time.fixedDeltaTime);
		m_traits["age"] += Time.fixedDeltaTime;
        SetOrientation(m_rigidbody.velocity.x > 0);
    }

	public float GetTrait(string traitName)
	{
		if (!m_traits.ContainsKey(traitName)) return 0f;
		return m_traits[traitName];
	}

	public void SetOrientation(bool orientation)
	{
		if (m_orientation != orientation)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		m_orientation = orientation;
	}

	public void Move(Vector3 position)
	{
		float time = Time.fixedDeltaTime;
		float speed = GetTrait("speed");
		if (m_movementMethod == MovementMethod.WALK)
		{
			SetState("walk");
			m_rigidbody.velocity = new Vector2(Mathf.Sign(position.x - transform.position.x) * speed, m_rigidbody.velocity.y);

		}
		if (m_movementMethod == MovementMethod.FLY)
		{
			SetState("fly");
			m_rigidbody.velocity = (position - transform.position).normalized * speed;
		}
		SetOrientation(m_rigidbody.velocity.x > 0);
	}

	public void OnActionStop()
	{
		if (m_behaviour is Idle)
			Decide();
		else
			Idle();
	}

	public Behaviour GetBehaviour()
	{
		return m_behaviour;
	}

	void SetState(string state)
	{
		if (m_state == state)
			return;
		m_state = state;
		m_view.SwitchAnimationState(m_state);
	}

	public void Idle()
	{
		SetState("idle");
		m_behaviour = new Idle(this);
	}

	public void Decide()
	{
		m_behaviour = new Wander(this);
	}

	public void OrderMeet(Animal other)
	{
		m_behaviour = new Meet(this, other);
	}

	public void Mate(Animal other)
	{
		Idle();
		other.Idle();
		GameObject newAnimalGO = Instantiate(m_animalBase);
		newAnimalGO.transform.position = (this.transform.position + other.transform.position) / 2;
		Animal newAnimal = newAnimalGO.GetComponent<Animal>();
		newAnimal.Inherit(this, other);
	}

	public Rigidbody2D GetRigidbody()
	{
		return m_rigidbody;
	}
}
