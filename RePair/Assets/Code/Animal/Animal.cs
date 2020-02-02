using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	public enum MovementMethod
	{
		WALK,
		JUMP,
		FLY
	}

	public AnimalPreset preset;

	GameObject m_animalBase;
	Genome m_genome;
	string m_state; // Строка со стейтом спайна для анимирования
	Dictionary<string, float> m_traits;
	Rigidbody2D m_rigidbody;
	Behaviour m_behaviour;
	SkeletonController m_view;
	float m_orientation; // +1 = right
	float m_hungerFactor;
	bool m_active;
<<<<<<< HEAD
=======
    bool m_dead;
	enum MovementMethod
	{
		WALK,
		JUMP,
		FLY
	}


>>>>>>> 853900d3faedf14f04eba03549c225e2f92fc122
	MovementMethod m_movementMethod = MovementMethod.WALK;

	public Genome GetGenome()
	{
		return m_genome;
	}

	private void InitFromPreset()
	{
		m_genome = new Genome(preset.genes);
		Invoke("InitAnimal", 0.1f);
	}

	public void Inherit(Animal parent1, Animal parent2)
	{
		m_genome = Genome.Breed(parent1.GetGenome(), parent2.GetGenome());
		Invoke("InitAnimal", 0.1f);
	}

	private void InitAnimal()
	{
		m_traits = m_genome.GetAllTraits();
		float canFly = GetTrait("flying") - GetTrait("size");
		if (canFly > 0)
		{
			m_traits["flyingHeight"] = canFly * 5f;
			m_movementMethod = MovementMethod.FLY;
			m_rigidbody.gravityScale = 0f;
		}
        m_traits["age"] = 0f;
        m_traits["currentSize"] = GetTrait("size") * 0.5f;
        float size = GetTrait("size") * 0.5f;
		m_view.SetBodyPart("Head", m_genome.Head.animalName);
		m_view.SetBodyPart("Front", m_genome.Body.animalName);
		m_view.SetBodyPart("Rear", m_genome.Legs.animalName);
		transform.localScale = new Vector3(size, size, 1f);
		m_active = true;
		Invoke("Die", GetTrait("lifetime"));

		if (m_traits.ContainsKey("stomachSize"))
		{
			m_traits["stomachFullness"] = m_traits["stomachSize"];
			m_hungerFactor = GameObject.Find("GameController").GetComponent<GameController>().animalHungerFactor;
		}
	}

    public bool IsDead()
    {
        return m_dead;
    }

    public void Die()
	{
        m_rigidbody.velocity = Vector2.zero;
        m_dead = true;
        GetComponent<Collider2D>().enabled = false;
        m_rigidbody.gravityScale = 0;
        m_behaviour = new Die(this);
        Invoke("DestroySelf", 10f);
		//Destroy(gameObject);
	}

    void ManageAge(float deltaTime)
    {
        m_traits["age"] += deltaTime;
        float age = GetTrait("age");
        float size = GetTrait("size");

        float lifeProgess = age / GetTrait("lifetime");
        float sizeMod = Mathf.Min(1f, 0.5f + lifeProgess * 2.5f);
        m_traits["currentSize"] = sizeMod * size;
        float currentSize = sizeMod * size;
        transform.localScale = new Vector3(m_orientation * currentSize, currentSize, 1f);
    }

    void DestoySelf()
    {
        Destroy(gameObject);
    }

    void Awake()
	{
		m_animalBase = GameObject.Find("GameController").GetComponent<GameController>().animalBase;
		m_view = GetComponentInChildren<SkeletonController>();
		m_rigidbody = GetComponent<Rigidbody2D>();
		if (preset != null)
			InitFromPreset();
		Invoke("Idle", 0.3f);
	}

    void UpdateSize()
    {

    }

    void FixedUpdate()
	{
        if (!m_active)
            return;
        if (m_behaviour != null) m_behaviour.Update(Time.fixedDeltaTime);
        m_orientation = Mathf.Sign(-m_rigidbody.velocity.x);
        if (m_orientation == 0f)
            m_orientation = 1f;
        ManageAge(Time.fixedDeltaTime);
		// hunger
		if (m_traits.ContainsKey("stomachSize"))
		{
			m_traits["stomachFullness"] -= m_hungerFactor * Time.fixedDeltaTime;
		}
	}

	public float GetTrait(string traitName)
	{
		if (!m_traits.ContainsKey(traitName)) return 0f;
		return m_traits[traitName];
	}

	public void SetTrait(string traitName, float traitValue)
	{
		m_traits[traitName] = traitValue;
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
		bool behaviorSelected = false; // needed for potentially multiple behavior selections (to be implemented)
		if (m_traits.ContainsKey("stomachSize") && m_traits["stomachFullness"] <= 0)
		{
			m_behaviour = new Feed(this);
			behaviorSelected = true;
		}
		if (!behaviorSelected && m_traits.ContainsKey("running") && m_traits.ContainsKey("panic") &&
		    Random.value < m_traits["panic"])
		{
			m_behaviour = new Run(this);
			behaviorSelected = true;
		}
		if (!behaviorSelected)
		{
			m_behaviour = new Wander(this);
		}
	}

	public void OrderMeet(Animal other)
	{
		m_behaviour = new Meet(this, other);
	}

	public void Mate(Animal other)
	{
        other.Idle();
        GameObject newAnimalGO = Instantiate(GameController.GetInstance().animalBase);   
        newAnimalGO.transform.position = (transform.position + other.transform.position) / 2;
        Animal newAnimal = newAnimalGO.GetComponent<Animal>();
        newAnimal.Inherit(this, other);
        PushParent(other, newAnimal);
        PushParent(this, newAnimal);
    }

  public void PushParent(Animal parent, Animal child)
  {
      parent.GetRigidbody().velocity = Vector2.zero;
      Vector3 force = new Vector2(Mathf.Sign(parent.transform.position.x - child.transform.position.x), 1f) * 100f;
      parent.GetRigidbody().AddForce(force);
  }

	public Rigidbody2D GetRigidbody()
	{
		return m_rigidbody;
	}

	public void SetMovementMethod(MovementMethod method)
	{
		m_movementMethod = method;
	}
}
