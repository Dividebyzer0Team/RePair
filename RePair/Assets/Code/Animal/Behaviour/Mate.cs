using UnityEngine;
public class Mate : Behaviour //tmp action for mating
{
    Animal m_other;
    public Mate(Animal host, Animal other) {
        m_host = host;
        m_other = other;
        m_name = "Mate";
    }

    float m_mateTime;

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        m_mateTime -= deltaTime;
        if (m_mateTime < 0)
            Stop();
    }

    public override void Start()
    {
        m_mateTime = m_host.GetTrait("matingTime");
        m_started = true;
    }

    public override void Stop() {
        base.Stop();
    }

}

