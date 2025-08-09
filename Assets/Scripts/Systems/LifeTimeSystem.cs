using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public struct LifeTimeComponent : IComponentData
{
    public float lifeTimer;
}

[BurstCompile]
partial struct LifeTimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new LifeTimeJob 
        { 
            DeltaTime = SystemAPI.Time.DeltaTime, 
            parallelWriter = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter() 
        }.ScheduleParallel(state.Dependency);
    }
}
[BurstCompile]
public partial struct LifeTimeJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter parallelWriter;
    [ReadOnly] public float DeltaTime;
    public void Execute([ChunkIndexInQuery] int queryIndex, in Entity entity, ref LifeTimeComponent lifeTime)
    {
        lifeTime.lifeTimer -= DeltaTime;
        if (lifeTime.lifeTimer <= 0f)
        {
            parallelWriter.DestroyEntity(queryIndex, entity);
        }
    }
}
