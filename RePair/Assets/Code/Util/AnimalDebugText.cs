using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDebugText : MonoBehaviour
{
    Animal m_animal;
    TextMesh m_text;
    // Start is called before the first frame update
    void Start()
    {
        m_animal = GetComponentInParent<Animal>();
        m_text = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        m_text.text = m_animal.GetBehaviour().GetName();
    }
}
