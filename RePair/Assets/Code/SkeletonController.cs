﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SkeletonController : MonoBehaviour
{
    public void SwitchAnimationState(string animState)
    {
        GameObject back = gameObject.transform.GetChild(0).gameObject;
        SkeletonPartController backskel = back.GetComponent<SkeletonPartController>();
        backskel.PlayAnimation(animState, null);

        GameObject front = gameObject.transform.GetChild(1).gameObject;
        SkeletonPartController frontskel = front.GetComponent<SkeletonPartController>();
        frontskel.PlayAnimation(animState, back.GetComponent<SkeletonAnimation>());
        
        GameObject head = gameObject.transform.GetChild(2).gameObject;
        SkeletonPartController headskel = head.GetComponent<SkeletonPartController>();
        headskel.PlayAnimation(animState, front.GetComponent<SkeletonAnimation>());
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
