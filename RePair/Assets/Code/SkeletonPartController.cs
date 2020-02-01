using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine;
using Spine.Unity;

public class SkeletonPartController : MonoBehaviour
{

    public SkeletonDataAsset skeleton;
    
    public void PlayAnimation(string anim)
    {
        SkeletonAnimation skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null) {
            // run animation
            skeletonAnimation.state.SetAnimation(0, anim, true);

            // follow parent's stack point
            GameObject parent = gameObject.transform.parent.gameObject;
            if (parent != null) {
                SkeletonAnimation parentSkeletonAnimation = parent.GetComponent<SkeletonAnimation>();
                if (parentSkeletonAnimation != null) {
                    Bone stackPoint = parentSkeletonAnimation.skeleton.FindBone("stack_point");
                    if (stackPoint != null) {
                        BoneFollower boneFollower = gameObject.GetComponent<BoneFollower>();
                        boneFollower.SkeletonRenderer = parentSkeletonAnimation;
                        boneFollower.SetBone("stack_point");
                        boneFollower.Initialize();
                    }
                }
            }

            // recursively call other body parts
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
