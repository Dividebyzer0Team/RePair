using UnityEngine;
public class Behaviour 
{
    protected string m_name;
    protected Animal m_host;
    protected bool m_started;
    public virtual void Start() {}
    public virtual void Stop() {
        m_host.OnActionStop();
    }
    public virtual void Update(float deltaTime)
    {
        if (!m_started)
            Start();
    }

    public string GetName()
    {
        return m_name;
    }
}
