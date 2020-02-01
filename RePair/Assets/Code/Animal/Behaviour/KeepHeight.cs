using UnityEngine;
public class KeepHeight : Behaviour
{
    public KeepHeight(Animal host) {
        m_host = host;
        m_height = host.transform.position.y;

    }

    float m_height;

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        float deltaHeight = m_height - m_host.transform.position.y;
        if (deltaHeight >= m_host.transform.localScale.y / 2) //Если упали на полроста
        {
            Rigidbody2D rb = m_host.GetRigidbody();
            rb.velocity += Vector2.up * deltaHeight;
        }
    }

    public override void Stop() {
        base.Stop();
    }

}

