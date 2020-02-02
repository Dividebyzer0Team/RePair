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
        SkeletonPartController skel;
        SkeletonAnimation parentSkelAnim;
 
        GameObject rear = gameObject.transform.GetChild(0).gameObject;
        GameObject front = gameObject.transform.GetChild(1).gameObject;
        GameObject head = gameObject.transform.GetChild(2).gameObject;
 
        if (slot == "Head") {
            skel = head.GetComponent<SkeletonPartController>();
            parentSkelAnim =  front.GetComponent<SkeletonAnimation>();
        } else if (slot == "Front") {
            skel = front.GetComponent<SkeletonPartController>();
            parentSkelAnim =  rear.GetComponent<SkeletonAnimation>();
        } else if (slot == "Rear") {
            skel = rear.GetComponent<SkeletonPartController>();
            parentSkelAnim = null;
        } else if (slot == null) {
            SetBodyPart("Head", phenotype);
            SetBodyPart("Front", phenotype);
            SetBodyPart("Rear", phenotype);
            return;
        } else {
            Debug.Log("Unknown body part slot: " + slot);
            return;
        }

        string fname = phenotype.ToLower() + "_" + slot.ToLower() + "_SkeletonData.asset";
        string path = "Assets/BodyParts/" + phenotype + slot + "/" + fname;
        SkeletonDataAsset skelData = (SkeletonDataAsset) AssetDatabase.LoadAssetAtPath("Assets/BodyParts/" + phenotype + slot + "/" + phenotype.ToLower() + "_" + slot.ToLower() + "_SkeletonData.asset", typeof(SkeletonDataAsset));
        if (skelData == null) {
            Debug.Log("Asset " + fname + " not found (" + path + ")");
            return;
        }

        skel.SetSkeleton(skelData, parentSkelAnim);
    }
 
    // Start is called before the first frame update
    void Start()
    {
        SetBodyPart(null, "Red");
        SwitchAnimationState("walk");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
