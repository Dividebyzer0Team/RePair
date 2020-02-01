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

        float hostSize = m_host.GetTrait("size");
        float flyingHeight = m_host.GetTrait("flyingHeight");

        float wanderDistance = Random.Range(hostSize, hostSize * m_host.GetTrait("wanderDistance"));
        Vector2 wanderVector = wanderDirection * wanderDistance;
        if (flyingHeight > 0)
        {
            Debug.Log(flyingHeight);
            float targetHeightOffset = Random.Range(-hostSize, hostSize);

            float targetHeight = flyingHeight + targetHeightOffset;
            Debug.Log(targetHeight);
            float verticalDirection = targetHeight - m_host.transform.position.y;
            wanderVector.y = verticalDirection;
        }
        m_wanderPosition = (Vector2)m_host.transform.position + wanderVector;
        m_wanderTime = wanderVector.magnitude / m_host.GetTrait("speed");
        m_started = true;
    }

    public override void Stop() {
        base.Stop();
    }

}

