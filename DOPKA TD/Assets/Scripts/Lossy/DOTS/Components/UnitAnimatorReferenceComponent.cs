using Lossy.Animation;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Components
{
    public class UnitAnimatorReferenceComponent : ICleanupComponentData
    {
        public UnitAnimatorController AnimatorController;
    }
}
