using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    List<Animal> animals;

    private void Start()
    {
        animals = new List<Animal>();
    }

    void Update()
    { 
        if (Input.GetMouseButtonDown (0)){ 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clicked = hit.transform.gameObject;
                Animal an = clicked.GetComponent<Animal>();
                if (an != null)
                {
                    SelectAnimal(an);
                    Debug.Log("Select animal");
                }
            }
        }
    }

    void SelectAnimal(Animal animal)
    {
        animals.Add(animal);
        if (animals.Count > 1)
        {
            animals[0].OrderMeet(animals[1]);
            animals[1].OrderMeet(animals[0]);
            animals.Clear();
        }
    }
}
