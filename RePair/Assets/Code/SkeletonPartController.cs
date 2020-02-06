using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine;
using Spine.Unity;

public class SkeletonPartController : MonoBehaviour
{

    public SkeletonDataAsset skeleton;
    
    Vector3 initialLocalPos;
    Vector2 initialStackPointLocalPos;
    Bone stackPoint;
 
    public void SetSkeleton(SkeletonDataAsset newSkeleton)
    {
        SkeletonAnimation skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null) {
            string anim = skeletonAnimation.AnimationName;
            skeletonAnimation.ClearState();

            skeletonAnimation.skeletonDataAsset = newSkeleton;
            skeletonAnimation.Initialize(true);

            skeleton = newSkeleton;
            skeletonAnimation.AnimationName = anim;
        }
    }
 
    public void OnParentSkeletonChanged(SkeletonAnimation parentSkeletonAnimation)
    {
        if (parentSkeletonAnimation != null) {
            stackPoint = parentSkeletonAnimation.skeleton.FindBone("stack_point");
            if (stackPoint != null) {
                initialStackPointLocalPos = new Vector2(stackPoint.WorldX, stackPoint.WorldY);
            }
        }
    }
 
    public void PlayAnimation(string anim)
    {
        SkeletonAnimation skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null) {
            skeletonAnimation.AnimationName = anim;
        }
    }

    void Start()
    {
        Vector3 localPos = gameObject.transform.localPosition;
        initialLocalPos = new Vector3(localPos.x, localPos.y, localPos.z);
    }

    public void Update()
    {
        gameObject.transform.localPosition = new Vector3(initialLocalPos.x, initialLocalPos.y, initialLocalPos.z);
    }

    public void LateUpdate()
    {
        if (stackPoint != null) {
            Vector3 localPos = gameObject.transform.localPosition;
            float worldXDiff = stackPoint.WorldX - initialStackPointLocalPos.x;
            float worldYDiff = stackPoint.WorldY - initialStackPointLocalPos.y;
            gameObject.transform.localPosition = new Vector3(localPos.x + worldXDiff, localPos.y + worldYDiff, localPos.z);
        }
    }
}
