using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
	ParticleSystem m_particleSystem;

	void Start()
	{
		m_particleSystem = GetComponent <ParticleSystem> ();
	}

	void Update()
	{
		if (!m_particleSystem.IsAlive())
		{
			Destroy(gameObject);
		}
	}
}
