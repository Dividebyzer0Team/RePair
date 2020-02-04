using System;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Gene", menuName = "RePair/Gene", order = 1)]
public class Gene : ScriptableObject
{
    public enum GeneId
    {
        UNKNOWN = 0,

        GOOSE_HEAD,
        GOOSE_FRONT,
        GOOSE_REAR,
        IBIS_HEAD,
        IBIS_FRONT,
        IBIS_REAR,
        ELEPHANT_HEAD,
        ELEPHANT_FRONT,
        ELEPHANT_REAR,
        GIRAFFE_HEAD,
        GIRAFFE_FRONT,
        GIRAFFE_REAR,
        RHINO_HEAD,
        RHINO_FRONT,
        RHINO_REAR,
        ZEBRA_HEAD,
        ZEBRA_FRONT,
        ZEBRA_REAR,
    }

    [Serializable]
    public struct Trait
    {
        public string name;
        public float factor;
    }

    public GeneId id = GeneId.UNKNOWN;
    public SkeletonDataAsset skeletonAsset;
    public List <Trait> traits;
}
