using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SkeletonController : MonoBehaviour
{
    public void SwitchAnimationState(string animState)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            SkeletonPartController skel = child.GetComponent<SkeletonPartController>();
            skel.PlayAnimation(animState);
        }
    }
 
    // Start is called before the first frame update
    void Start()
    {
        SwitchAnimationState("walk");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
