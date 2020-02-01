using UnityEngine;
using System.Collections.Generic;

public class Animal : MonoBehaviour
{
    string m_state; // Строка со стейтом спайна для анимирования
    Dictionary<string, float> m_traits;
    Animator mAnimator; // Класс для анимирования. Если кастомный - надо правильный сделать будет
    float m_age;
    Rigidbody2D m_rigidbody;
    Behaviour m_behaviour;

    //TEMP
    GameObject m_target;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_traits = new Dictionary<string, float> {
                { "speed", 2f },
                { "thinkingTime" , 2f },
                { "wanderDistance" , 5f }
            };
        m_target = GameObject.Find("Target");
        Idle();
        Color[] randomColors = new Color[] { Color.blue, Color.green, Color.red, Color.yellow };
        Color color = randomColors[Random.Range(0, 4)];
        GetComponent<MeshRenderer>().material.color = color;
    }

    void FixedUpdate()
	{
        m_behaviour.Update(Time.deltaTime);
	}

    public float GetTrait(string traitName)
    {
        if (!m_traits.ContainsKey(traitName)) return 0f;
        return m_traits[traitName];
    }

    public void Move(Vector3 position)
    {
        m_state = "walk"; // Когда-то здесь будет еще и fly/jump (вероятно)
        float time = Time.fixedDeltaTime;
        float speed = GetTrait("speed");
        m_rigidbody.velocity = (position - transform.position).normalized * speed;
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
        GameObject newAnimal = Instantiate(this.gameObject);
        newAnimal.transform.position = (this.transform.position + other.transform.position) / 2;

        //Временно красим животных
        Color c1 = GetComponent<MeshRenderer>().material.color;
        Color c2 = other.GetComponent<MeshRenderer>().material.color;
        newAnimal.GetComponent<MeshRenderer>().material.color = new Color((c1.r + c2.r) * 0.5f, (c1.g + c2.g) * 0.5f, (c1.b + c2.b) * 0.5f);
        //creation of new animal
    }
}
