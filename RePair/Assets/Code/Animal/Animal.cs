using UnityEngine;
using System.Collections.Generic;

public class Animal : MonoBehaviour
{
    Genome m_genome;
    string m_state; // Строка со стейтом спайна для анимирования
    Dictionary<string, float> m_traits;
    Animator mAnimator; // Класс для анимирования. Если кастомный - надо правильный сделать будет
    float m_age;
    Rigidbody2D m_rigidbody;
    Behaviour m_behaviour;
    SkeletonController m_view;
    bool m_orientation = true; // true = left

    // ТЕСТОВОЕ
    public AnimalPreset preset;
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
        Color c1 = parent1.GetComponent<MeshRenderer>().material.color;
        Color c2 = parent2.GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = new Color((c1.r + c2.r) * 0.5f, (c1.g + c2.g) * 0.5f, (c1.b + c2.b) * 0.5f);
        InitAnimal();

    }

    private void InitAnimal()
    {
        m_traits = m_genome.GetAllTraits();
        m_traits["age"] = 0f;
        float canFly = GetTrait("flying") - GetTrait("size");
        if (canFly > 0)
        {
            m_traits["flyingHeight"] = canFly * 5f;
            m_movementMethod = MovementMethod.FLY;
            m_rigidbody.gravityScale = 0f;
        }

        Invoke("Die", GetTrait("lifetime"));
        float size = GetTrait("size");
        GameObject legsGO = GameObject.Instantiate(m_genome.Legs.representation, m_view.transform);
        legsGO.transform.localScale = new Vector2(size * 0.1f, size * 0.1f);
        GameObject bodyGO = GameObject.Instantiate(m_genome.Body.representation, legsGO.transform);
        bodyGO.transform.localScale = new Vector2(size * 0.1f, size * 0.1f);
        GameObject headGO = GameObject.Instantiate(m_genome.Head.representation, bodyGO.transform);
        headGO.transform.localScale = new Vector2(size * 0.1f, size * 0.1f);
        transform.localScale = new Vector3(size, size, 1f);

    }


    void Die()
    {
        Destroy(this.gameObject);
    }

    void Awake()
    {
        m_view = GetComponentInChildren<SkeletonController>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        if (preset != null)
            InitFromPreset();
        Idle();
    }

    void FixedUpdate()
	{
        m_behaviour.Update(Time.fixedDeltaTime);
        m_traits["age"] += Time.fixedDeltaTime;
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
        //m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y); //Пока что обнуляем горзонтальную скорость
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
        GameObject newAnimalGO = Instantiate(this.gameObject);
        newAnimalGO.transform.position = (this.transform.position + other.transform.position) / 2;
        Animal newAnimal = newAnimalGO.GetComponent<Animal>();
        newAnimal.Inherit(this, other);

        //Временно красим животных
       
    }

    public Rigidbody2D GetRigidbody()
    {
        return m_rigidbody;
    }
}
