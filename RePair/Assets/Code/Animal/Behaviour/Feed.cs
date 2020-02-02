using UnityEngine;

public class Feed : Behaviour
{
	enum State
	{
		Moving, Feeding
	};

	private Transform m_targetFood;
	private State m_state;
	private float m_feedingFactor;

	public Feed(Animal host)
	{
		m_host = host;
		m_name = "Feed";
	}

	public override void Start()
	{
		var allFood = GameObject.FindGameObjectsWithTag("Food");
		if (allFood.Length > 0)
		{
			m_targetFood = allFood[Random.Range(0, allFood.Length - 1)].transform;
			m_feedingFactor = GameObject.Find("GameController").GetComponent<GameController>().animalFeedingFactor;
			m_state = State.Moving;
			m_started = true;
		}
		else
		{
			Stop();
		}
	}

	public override void Stop()
	{
		base.Stop();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		switch (m_state)
		{
			case State.Moving:
				m_host.Move(m_targetFood.position);
				if ((m_host.transform.position - m_targetFood.position).magnitude < Mathf.Abs(m_host.transform.localScale.x))
				{
					m_state = State.Feeding;
				}
			break;
			case State.Feeding:
				// slowing down
				m_host.GetRigidbody().velocity = Vector2.Lerp(m_host.GetRigidbody().velocity, Vector2.zero, deltaTime);

				var fullness = m_host.GetTrait("stomachFullness");
				fullness += m_feedingFactor * deltaTime;
				m_host.SetTrait("stomachFullness", fullness);
				if (fullness >= m_host.GetTrait("stomachSize"))
				{
					Stop();
				}
			break;
		}
	}
}
