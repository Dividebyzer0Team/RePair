using UnityEngine;
using UnityEditor;
using Spine.Unity;
using Spine;

public class SkeletonController : MonoBehaviour
{
    GameObject rear, front, head;
    SkeletonPartController headSkelController, frontSkelController, rearSkelController;

    // "fly", "walk", "idle"
    public void SwitchAnimationState(string animState)
    {   
        foreach (Transform child in gameObject.transform) {
            SkeletonPartController skel = child.gameObject.GetComponent<SkeletonPartController>();
            skel.PlayAnimation(animState);
        }
    }
 
    // "Head", "Front", "Rear"
    // "Zebra", "Rhino", "Jiraffe", "Goose", "Red", "Elephant"
    public void SetBodyPart(string slot, SkeletonDataAsset skelData)
    {
        if (slot == null) {
            SetBodyPart("Head", skelData);
            SetBodyPart("Front", skelData);
            SetBodyPart("Rear", skelData);
            return;
        }

        SkeletonPartController controller = null, childController = null;
        SkeletonAnimation animationComponent = null;
 
        if (slot == "Head") {
            controller = headSkelController;
        } else if (slot == "Front") {
            controller = frontSkelController;
            childController = headSkelController;
            animationComponent = front.GetComponent<SkeletonAnimation>();
        } else if (slot == "Rear") {
            controller = rearSkelController;
            childController = frontSkelController;
            animationComponent = rear.GetComponent<SkeletonAnimation>();
        } else
            Debug.Log("Unknown body part slot: " + slot);

        if (controller != null)
            controller.SetSkeleton(skelData);
        if (childController != null && animationComponent != null)
            childController.OnParentSkeletonChanged(animationComponent);
    }
 
    void Start()
    {
        rear  = gameObject.transform.GetChild(0).gameObject;
        front = gameObject.transform.GetChild(1).gameObject;
        head  = gameObject.transform.GetChild(2).gameObject;
        headSkelController  = head.GetComponent<SkeletonPartController>();
        frontSkelController = front.GetComponent<SkeletonPartController>();
        rearSkelController  = rear.GetComponent<SkeletonPartController>();

        frontSkelController.OnParentSkeletonChanged(rear.GetComponent<SkeletonAnimation>());
        headSkelController.OnParentSkeletonChanged(front.GetComponent<SkeletonAnimation>());
    }
}
