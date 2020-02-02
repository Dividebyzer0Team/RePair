using UnityEngine;

public class Wind : MonoBehaviour
{
    public float min;
    public float max;
    public float changeInterval;
    
    private float timeSinceLastChange;
    private float m_windX;

    public Vector3 GetWindForce()
    {
        return new Vector3(m_windX, 0.0f, 0.0f);
    }

	void Start()
	{
        WindsAreChangin();
	}

    void Update()
    {
        if (changeInterval < float.Epsilon)
            return;

        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange > changeInterval) {
            WindsAreChangin();
        }
    }
    
    private void WindsAreChangin()
    {
        m_windX = Random.Range(min, max);
        timeSinceLastChange = 0.0f;
    }
}
