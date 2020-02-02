using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject loveArrowPrefab;
    public GameObject warArrowPrefab;
    public float arrowForceFactor = 1.0f;
	public float chargeForceFactor = 1.0f;
	public float chargeMaxTime = 1.5f;
	public bool debugBreeding = false;

	private Animal m_queuedAnimal;
	private float m_shotCharge, m_shotChargeTime;

	void Start()
	{
	}

	void Update()
	{
		HandleBreeding();
	}

	void HandleBreeding()
	{
		if (debugBreeding)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.collider != null)
				{
					Animal animal = hit.transform.GetComponent<Animal>();
					if (animal != null)
					{
						OnAnimalHit(animal, Arrow.WhatToMake.LOVE);
					}
				}
			}
		}
		else
		{
			if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
			{
				m_shotChargeTime += Time.deltaTime;
				if (m_shotChargeTime <= chargeMaxTime)
				{
					m_shotCharge += chargeForceFactor * Time.deltaTime;
				}
			}
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
        bool loveArrow = Input.GetMouseButtonUp(0);
        GameObject prefab = loveArrow ? loveArrowPrefab : warArrowPrefab;
        var dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
				var arrowGO = Instantiate(prefab, transform.position, Quaternion.identity);
				var arrowRB = arrowGO.GetComponent<Rigidbody2D>();
				arrowRB.AddForce(dir * (arrowForceFactor + m_shotCharge), ForceMode2D.Impulse);
				m_shotCharge = m_shotChargeTime = 0;
				var arrowBeh = arrowGO.GetComponent<Arrow>();
				arrowBeh.OnCollisionWithAnimal += OnAnimalHit;
			}
		}
	}

	void OnAnimalHit(Animal animal, Arrow.WhatToMake whatToMake)
	{
        if (whatToMake == Arrow.WhatToMake.LOVE)
        {
            if (!m_queuedAnimal || m_queuedAnimal.IsDead())
            {
                m_queuedAnimal = animal;
            }
            else if (animal != m_queuedAnimal) // cannot select same animal twice
            {
                m_queuedAnimal.OrderMeet(animal);
								m_queuedAnimal.gameObject.AddComponent<LineRenderer>();
                animal.OrderMeet(m_queuedAnimal);
                m_queuedAnimal = null;
            }
        }
        else
        {
            animal.Die();
        }

    }
}
