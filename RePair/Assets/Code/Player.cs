using UnityEngine;
using Spine.Unity;

public class Player : MonoBehaviour
{
	public GameObject loveArrowPrefab;
	public GameObject warArrowPrefab;
	public GameObject arrowChargeEffect;
	public GameObject matingReadyEffect;
	public float arrowForceFactor = 1.0f;
	public float chargeForceFactor = 1.0f;
	public float chargeMaxTime = 1.5f;
	public bool debugBreeding = false;

	private Animal m_queuedAnimal;
	private float m_shotCharge, m_shotChargeTime;
	private GameObject m_curArrowChargeEffect;
	private GameObject m_view;
	private SkeletonAnimation m_animation;
	private Transform m_aim;

	void Start()
	{
		m_view = transform.Find("View").gameObject;
		m_animation = GetComponentInChildren <SkeletonAnimation> ();
		m_aim = m_view.transform.Find("SkeletonUtility-SkeletonRoot/root/aim_bone");
	}

	void Update()
	{
		HandleBreeding();
	}

	void HandleBreeding()
	{
		if (!GameController.GetInstance().gameStarted) return;

		var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldMousePosition.z = 0;

		if ((transform.position - worldMousePosition).magnitude > transform.localScale.x * 2)
		{
			m_aim.transform.position = worldMousePosition;
		}

		if (debugBreeding)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);
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
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				if (m_curArrowChargeEffect) Destroy(m_curArrowChargeEffect);
				m_curArrowChargeEffect = Instantiate(arrowChargeEffect, transform);
			}
			if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
			{
				var toMouse = (worldMousePosition - transform.position).normalized;
				var rotation = new Vector3(0, 0, -Vector2.SignedAngle(toMouse.normalized, Vector2.right));
				m_curArrowChargeEffect.transform.rotation = Quaternion.Euler(rotation);
				
				m_shotChargeTime += Time.deltaTime;
				if (m_shotChargeTime <= chargeMaxTime)
				{
					m_shotCharge += chargeForceFactor * Time.deltaTime;
				}
			}
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
				Destroy(m_curArrowChargeEffect);
				m_curArrowChargeEffect = null;

				m_animation.AnimationState.SetAnimation(0, "shoot", false);
				m_animation.AnimationState.AddAnimation(0, "prepare", false, 0);
				m_animation.AnimationState.AddAnimation(0, "idle", true, 0);

				bool loveArrow = Input.GetMouseButtonUp(0);
        GameObject prefab = loveArrow ? loveArrowPrefab : warArrowPrefab;
        var dir = (worldMousePosition - transform.position).normalized;
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
            if (animal.IsFertile())
            {
                if (!m_queuedAnimal || m_queuedAnimal.IsDead())
                {
                    m_queuedAnimal = animal;
                    Instantiate(matingReadyEffect, m_queuedAnimal.transform);
                }
                else if (animal != m_queuedAnimal) // cannot select same animal twice
                {
                    Instantiate(matingReadyEffect, animal.transform);
                    m_queuedAnimal.OrderMeet(animal);
                    //m_queuedAnimal.gameObject.AddComponent<LineRenderer>();
                    animal.OrderMeet(m_queuedAnimal);
                    m_queuedAnimal = null;
                }
            }
		}
		else
		{
			animal.Die();
		}
	}
}
