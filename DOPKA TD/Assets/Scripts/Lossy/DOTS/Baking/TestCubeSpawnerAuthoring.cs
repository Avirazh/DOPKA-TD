using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class TestCubeSpawnerAuthoring : MonoBehaviour
    {
        public Transform SpawnPoint;
        public GameObject TestCubePrefab; 
    }

    public class TestCubeSpawnerBaker : Baker<TestCubeSpawnerAuthoring>
    {
        public override void Bake(TestCubeSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TestCubeSpawnerComponent 
            {
                SpawnPointPosition = new float3(authoring.SpawnPoint.position),
                TestCube = GetEntity(authoring.TestCubePrefab, TransformUsageFlags.Dynamic)
            } );
        }
    }
}