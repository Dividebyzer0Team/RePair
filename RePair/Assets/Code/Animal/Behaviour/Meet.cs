using UnityEngine;
public class Meet : Behaviour
{
    Animal m_target;
    float m_power = 4f;
    float m_time = 0f;
    public Meet(Animal host, Animal target) {
        m_host = host;
        m_target = target;
        
        m_matingDistance = (m_host.GetTrait("currentSize") + m_target.GetTrait("currentSize")) * 0.5f;
        m_name = "Meet";
    }

    float m_matingDistance;

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        m_time += deltaTime;
        if (m_target == null || m_target.IsDead())
        {
            m_host.Idle();
            return;
        }
        Vector2 toTarget = m_target.transform.position - m_host.transform.position;
        if (toTarget.magnitude <= m_matingDistance)
            Stop();
        m_host.GetRigidbody().velocity = toTarget * m_time * m_power;
    }

    public override void Stop() {
        base.Stop();
        m_host.Mate(m_target);
        //m_host.Breed();
    }

}

