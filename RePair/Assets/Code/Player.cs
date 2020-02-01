using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject arrow;
	public float arrowForceFactor = 1.0f;
	public float chargeForceFactor = 1.0f;
	public float chargeMaxTime = 1.5f;

	private Animal m_queuedAnimal;
	private float m_shotCharge, m_shotChargeTime;

	void Start()
	{
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			m_shotChargeTime += Time.deltaTime;
			if (m_shotChargeTime <= chargeMaxTime)
			{
				m_shotCharge += chargeForceFactor * Time.deltaTime;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			var arrowGO = GameObject.Instantiate(arrow);
			var dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
			var arrowRB = arrowGO.GetComponent <Rigidbody2D> ();
			arrowRB.AddForce(dir * (arrowForceFactor + m_shotCharge), ForceMode2D.Impulse);
			m_shotCharge = m_shotChargeTime = 0;
			var arrowBeh = arrowGO.GetComponent <Arrow> ();
			arrowBeh.OnCollisionWithAnimal += OnAnimalHit;
		}
	}

	void OnAnimalHit(Animal animal)
	{
		if (!m_queuedAnimal)
		{
			m_queuedAnimal = animal;
		}
		else if (animal != m_queuedAnimal) // cannot select same animal twice
		{
			m_queuedAnimal.OrderMeet(animal);
			animal.OrderMeet(animal);
			m_queuedAnimal = null;
		}
	}
}
