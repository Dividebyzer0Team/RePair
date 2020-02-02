using UnityEngine;
public class Die : Behaviour
{
    public Die(Animal host) {
        m_host = host;
        m_name = "Die";
    }

    float m_wanderTime;
    Vector2 m_wanderPosition;
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        m_host.transform.position += Vector3.down * m_host.transform.localScale.y * deltaTime;
    }

    public override void Stop() {
        base.Stop();
    }

}

