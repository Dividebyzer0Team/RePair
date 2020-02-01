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
    
    public void PlayAnimation(string anim, SkeletonAnimation parentSkeletonAnimation)
    {
        // move to SetBodyPart
        //Debug.Log(">>> 1 " + gameObject.name);
        SkeletonAnimation skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null) {
            //Debug.Log(">>> 2 " + gameObject.name);
            // run animation
            skeletonAnimation.state.SetAnimation(0, anim, true);

            Vector3 localPos = gameObject.transform.localPosition;
            initialLocalPos = new Vector3(localPos.x, localPos.y, localPos.z);

            // follow parent's stack point
            if (parentSkeletonAnimation != null) {
                //Debug.Log(">>> 3 " + gameObject.name);
                stackPoint = parentSkeletonAnimation.skeleton.FindBone("stack_point");
                if (stackPoint != null) {
                    //Debug.Log(">>> 4 " + gameObject.name);
                    initialStackPointLocalPos = new Vector2(stackPoint.WorldX, stackPoint.WorldY);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {
        gameObject.transform.localPosition = new Vector3(initialLocalPos.x, initialLocalPos.y, initialLocalPos.z);
    }

    // Update is called once per frame
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
