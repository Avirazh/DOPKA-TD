using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct SpawnerAspect : IAspect
    {
        public readonly Entity entity;

        private readonly RefRO<SpawnerComponent> _spawner;
        private readonly RefRW<UnitSpawnTimerComponent> _unitSpawnTimer;

        public Entity Prefab => _spawner.ValueRO.Prefab;
        public float3 SpawnPosition => _spawner.ValueRO.SpawnPosition;
        public float TimeToNextSpawn 
        {
            get => _unitSpawnTimer.ValueRO.TimeToNextSpawn;
            set => _unitSpawnTimer.ValueRW.TimeToNextSpawn = value;
        }
        public float Timer => _unitSpawnTimer.ValueRO.Timer;
    }
}