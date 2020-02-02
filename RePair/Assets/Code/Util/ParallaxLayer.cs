using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
	public Vector2 factor;
    Vector2 startPos;
    Vector2 startCameraPos;
	Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
        startCameraPos = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 camOffset = (Vector2)cam.transform.position - startCameraPos;
        transform.position = startPos + new Vector2(camOffset.x * factor.x, camOffset.y * factor.y);
    }
}
