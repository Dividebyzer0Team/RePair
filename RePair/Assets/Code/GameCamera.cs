using UnityEngine;

public class GameCamera : MonoBehaviour
{
	public Transform player;
	public float stretchFactor = 0.5f;
	public float easingFactor = 0.017f;

	private Camera cam;

	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		var camPos = cam.transform.position;
		var playerX = player.position.x;
		var mouseWorldX = cam.ScreenToWorldPoint(Input.mousePosition).x;
		camPos.x = Mathf.Lerp(camPos.x, playerX + (mouseWorldX - playerX) * stretchFactor, 0.5f * Time.deltaTime / easingFactor);
		cam.transform.position = camPos;
	}
}
