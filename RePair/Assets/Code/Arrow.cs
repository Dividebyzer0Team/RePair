using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum WhatToMake
    {
        LOVE,
        WAR
    }
    public WhatToMake whatToMake;
    public delegate void OnCollisionWithAnimalHandler(Animal animal, WhatToMake whatToMake);
	public OnCollisionWithAnimalHandler OnCollisionWithAnimal;

	private Rigidbody2D m_rigidbody;

	void Start()
	{
		m_rigidbody = GetComponent <Rigidbody2D> ();
	}

	void Update()
	{
		var rotation = new Vector3(0, 0, -Vector2.SignedAngle(m_rigidbody.velocity.normalized, Vector2.right) - 90);
		transform.rotation = Quaternion.Euler(rotation);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var animal = collision.transform.GetComponent <Animal> ();
		if (animal) OnCollisionWithAnimal(animal, whatToMake);
		Destroy(gameObject);
	}
}
