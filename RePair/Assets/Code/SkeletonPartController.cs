using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine.Unity;

public class SkeletonPartController : MonoBehaviour
{
    public SkeletonDataAsset skeleton;
    
    public void PlayAnimation(string anim)
    {
        SkeletonAnimation skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null) {
            skeletonAnimation.state.SetAnimation(0, anim, true);
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                SkeletonPartController skel = child.GetComponent<SkeletonPartController>();
                skel.PlayAnimation(anim);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        SkeletonAnimation skeletonAnimation = gameObject.AddComponent<SkeletonAnimation>();
        skeletonAnimation.skeletonDataAsset = skeleton;
        skeletonAnimation.Initialize(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
