using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class TestCubeSpawnerMonoBehaviour : MonoBehaviour
    {
        public Transform SpawnPoint;
        public GameObject TestCubePrefab; 
    }

    public class TestCubeSpawnerBaker : Baker<TestCubeSpawnerMonoBehaviour>
    {
        public override void Bake(TestCubeSpawnerMonoBehaviour authoring)
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