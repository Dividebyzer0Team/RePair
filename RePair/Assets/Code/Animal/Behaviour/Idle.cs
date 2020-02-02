using UnityEngine;
public class Idle : Behaviour
{
    public Idle (Animal host) {
        m_host = host;
        m_name = "Idle";
    }

    float m_idleTime;

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        Vector2 velocity = m_host.GetRigidbody().velocity;
        if (velocity.magnitude > 0.1f)
            m_host.GetRigidbody().velocity = Vector2.Lerp(velocity, Vector2.zero, deltaTime / 5f); //Потихоньку тормозимс на айдле
        else
            m_host.GetRigidbody().velocity = Vector2.zero;
        m_idleTime -= deltaTime;
        if (m_idleTime < 0)
            Stop();
    }

    public override void Start()
    {
        m_idleTime = m_host.GetTrait("thinkingTime") * Random.Range(0.9f, 1.1f);
        m_started = true;
    }

    public override void Stop() {
        base.Stop();
    }

}

