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
        var size = GetTrait("size");
				transform.localScale = new Vector3(size, size, 1f);
        Invoke("Die", GetTrait("lifetime"));
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        // ТЕСТОВОЕ
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

    public void Move(Vector3 position)
    {
        if (m_movementMethod == MovementMethod.WALK || m_movementMethod == MovementMethod.FLY)
        {
            m_state = "walk"; // Когда-то здесь будет еще и fly/jump (вероятно)
            float time = Time.fixedDeltaTime;
            float speed = GetTrait("speed");
            m_rigidbody.velocity = (position - transform.position).normalized * speed;
        }
        if (m_movementMethod == MovementMethod.JUMP)
        {

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

    public void Idle()
    {
        m_state = "idle"; // Тут будем рассказывать спайну, что начали стоять тупить
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
