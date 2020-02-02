using UnityEngine;

public class Run : Behaviour
{
	enum State
	{
		Landing, Running
	};

	private State m_state;

	private float m_originalSpeed, m_originalGravityScale;
	private float m_direction, m_directionSwitchInterval;
	private float m_runningTime, m_directionSwitchTime;

	public Run(Animal host)
	{
		m_host = host;
		m_name = "Run";
	}

	public override void Start()
	{
		m_originalSpeed = m_host.GetTrait("speed");

		var runningValue = m_host.GetTrait("running");

		var rb = m_host.GetRigidbody();
		m_originalGravityScale = rb.gravityScale;
		rb.gravityScale = runningValue * 2;

		m_runningTime = runningValue;

		m_directionSwitchInterval = GameObject.Find("GameController").GetComponent<GameController>().animalRunningDirectionRandomizeInterval;
		m_directionSwitchTime = m_directionSwitchInterval;
		RandomizeDirection();

		m_state = IsGrounded() ? State.Running : State.Landing;

		m_started = true;
	}

	public override void Stop()
	{
		base.Stop();
		m_host.SetTrait("speed", m_originalSpeed);
		m_host.GetRigidbody().gravityScale = m_originalGravityScale;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		var pos = m_host.transform.position;
		pos.x += m_direction;
		m_host.Move(pos);

		switch (m_state)
		{
			case State.Landing:
				if (IsGrounded())
				{
					m_host.SetTrait("speed", m_host.GetTrait("running"));
					m_state = State.Running;
				}
				break;
			case State.Running:
				m_directionSwitchTime -= deltaTime;
				if (m_directionSwitchTime <= 0)
				{
					RandomizeDirection();
					m_directionSwitchTime = m_directionSwitchInterval;
				}

				m_runningTime -= deltaTime;
				if (m_runningTime <= 0) Stop();
			break;
		}
	}

	bool IsGrounded()
	{
		var hit = Physics2D.Raycast(m_host.transform.position + Vector3.down * 0.05f, Vector2.down, 0.05f);
		return (hit.transform != null);
	}

	void RandomizeDirection()
	{
		m_direction = Random.value > 0.5f ? 1.0f : -1.0f;
	}
}
