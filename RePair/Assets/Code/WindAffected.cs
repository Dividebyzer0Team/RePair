using System;
using UnityEngine;

public class WindAffected : MonoBehaviour
{
	void Start()
	{
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null) {
            GameController game = GameController.GetInstance();
            if (game) {
                Wind windComponent = game.GetComponent<Wind>();
                if (windComponent != null)
                    rigidbody.AddForce(windComponent.GetWindForce());
            }
        }
	}

	void Update()
	{
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            GameController game = GameController.GetInstance();
            if (game) {
                Wind windComponent = game.GetComponent<Wind>();
                if (windComponent != null) {
                    float windX = windComponent.GetWindForce().x;
                    float windMag = Math.Abs(windX);
                    float windDir = windX/windMag;
                    float horizontalSpeed = (windMag - 0.0f) / (80.0f - 0.0f) * (4.0f - 0.0f) + 0.0f;

                    var psProps = particleSystem.main;
                    float simulationSpeed = (windMag - 0.0f) / (80.0f - 0.0f) * (4.0f - 1.0f) + 1.0f;
                    psProps.simulationSpeed = simulationSpeed;
                    
                    float startSpeed = UnityEngine.Random.Range(horizontalSpeed, horizontalSpeed);
                    psProps.startSpeed = startSpeed * windDir;
                }
            }
        }
    }
}
