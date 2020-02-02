using UnityEngine;
using UnityEditor;
using Spine.Unity;
using Spine;

public class SkeletonController : MonoBehaviour
{
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
    public void SetBodyPart(string slot, string phenotype)
    {
        if (slot == null) {
            SetBodyPart("Head", phenotype);
            SetBodyPart("Front", phenotype);
            SetBodyPart("Rear", phenotype);
            return;
        }
 
        string fname = phenotype.ToLower() + "_" + slot.ToLower() + "_SkeletonData.asset";
        string path = "Assets/BodyParts/" + phenotype + slot + "/" + fname;
        SkeletonDataAsset skelData = (SkeletonDataAsset) AssetDatabase.LoadAssetAtPath("Assets/BodyParts/" + phenotype + slot + "/" + phenotype.ToLower() + "_" + slot.ToLower() + "_SkeletonData.asset", typeof(SkeletonDataAsset));
        if (skelData == null) {
            Debug.Log("Asset " + fname + " not found (" + path + ")");
            return;
        }

        SkeletonPartController skel;
 
        GameObject rear = gameObject.transform.GetChild(0).gameObject;
        GameObject front = gameObject.transform.GetChild(1).gameObject;
        GameObject head = gameObject.transform.GetChild(2).gameObject;
  
        SkeletonPartController headSkelController = head.GetComponent<SkeletonPartController>();
        SkeletonPartController frontSkelController = front.GetComponent<SkeletonPartController>();
        SkeletonPartController rearSkelController = rear.GetComponent<SkeletonPartController>();
 
        if (slot == "Head") {
            skel = headSkelController;
            skel.SetSkeleton(skelData);
        } else if (slot == "Front") {
            skel = frontSkelController;
            skel.SetSkeleton(skelData);
            headSkelController.OnParentSkeletonChanged(front.GetComponent<SkeletonAnimation>());
        } else if (slot == "Rear") {
            skel = rearSkelController;
            skel.SetSkeleton(skelData);
            frontSkelController.OnParentSkeletonChanged(rear.GetComponent<SkeletonAnimation>());
        } else {
            Debug.Log("Unknown body part slot: " + slot);
            return;
        }
    }
}
