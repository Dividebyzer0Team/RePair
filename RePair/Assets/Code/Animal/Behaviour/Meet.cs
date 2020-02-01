using UnityEngine;
public class Meet : Behaviour
{
    Animal m_target;
    public Meet(Animal host, Animal target) {
        m_host = host;
        m_target = target;
        m_matingDistance = m_target.transform.localScale.x / 2 + m_host.transform.localScale.x / 2; // Дистанция траханья
        m_name = "Meet";
    }

    float m_matingDistance;

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        Vector2 toTarget = m_target.transform.position - m_host.transform.position;
        if (toTarget.magnitude <= m_matingDistance)
            Stop();
        m_host.Move(m_target.transform.position);
    }

    public override void Stop() {
        base.Stop();
        m_host.Mate(m_target);
        //m_host.Breed();
    }

}

