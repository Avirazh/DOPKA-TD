using Unity.Entities;
using UnityEngine;
using Lossy.DOTS.Components;

namespace Lossy.DOTS.Baking
{
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public Transform SpawnerTransform;
        public float Timer;
    }
    public class SpawnerBaker : Baker<SpawnerAuthoring> 
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new SpawnerComponent()
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                SpawnPosition = authoring.SpawnerTransform.position
            });
            AddComponent(entity, new UnitSpawnTimerComponent()
            {
                Timer = authoring.Timer
            });
        }
    }
}