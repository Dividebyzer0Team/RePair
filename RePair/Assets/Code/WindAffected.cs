using UnityEngine;

public class WindAffected : MonoBehaviour
{
	void Start()
	{
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody) {
            GameController game = GameController.GetInstance();
            if (game) {
                Wind windComponent = game.GetComponent<Wind>();
                if (windComponent != null)
                    rigidbody.AddForce(windComponent.GetWindForce());
            }
        }
	}
}
