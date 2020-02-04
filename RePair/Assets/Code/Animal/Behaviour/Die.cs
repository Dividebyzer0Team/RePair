using UnityEngine;
public class Die : Behaviour
{
    public Die(Animal host) {
        m_host = host;
        m_name = "Die";

        GameController.GetInstance().UpdateSpeciesWatch();
    }

    float m_wanderTime;
    Vector2 m_wanderPosition;
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        m_host.transform.position += Vector3.down * deltaTime;
        if (m_host.transform.localScale.y > 0.15f)
            m_host.transform.localScale += new Vector3(0.0f, -0.02f * m_host.transform.localScale.y, 0.0f);
        else
            m_host.SetVisibility(false);
    }

    public override void Stop() {
        base.Stop();
    }

}

