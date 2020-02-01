using UnityEngine;
public class Wander : Behaviour
{
    public Wander(Animal host) {
        m_host = host;
        m_name = "Wander";
    }

    float m_wanderTime;
    Vector2 m_wanderPosition;
    public override void Update(float deltaTime)
    {

        base.Update(deltaTime);
        m_wanderTime -= deltaTime;
        if (m_wanderTime > 0)
        {
            m_host.Move(m_wanderPosition);
        }
        else
        {
            Stop();
        }
    }

    public override void Start()
    {
        Vector2 wanderDirection = Random.value > 0.5 ? Vector2.left : Vector2.right;
        float hostSize = m_host.transform.localScale.x;
        float wanderDistance = Random.Range(hostSize, hostSize * m_host.GetTrait("wanderDistance"));
        m_wanderTime = wanderDistance / m_host.GetTrait("speed");
        m_wanderPosition = (Vector2)m_host.transform.position + wanderDirection * wanderDistance;
        m_started = true;
    }

    public override void Stop() {
        base.Stop();
    }

}

