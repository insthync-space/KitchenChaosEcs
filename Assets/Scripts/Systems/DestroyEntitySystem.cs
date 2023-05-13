using Unity.Entities;
using UnityEngine;

partial struct DestroyEntitySystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transformGO, entity) in SystemAPI
                     .Query<GameObjectTransformComponent>()
                     .WithEntityAccess()
                     .WithAll<MustBeDestroyedComponent>()
                 ) {
            if (transformGO.Transform.gameObject != null) {
                GameObject.Destroy(transformGO.Transform.gameObject);
            }
            ecb.DestroyEntity(entity);
        }
        
        foreach (var (mustBeDestroyed, entity) in SystemAPI
                     .Query<MustBeDestroyedComponent>()
                     .WithEntityAccess()
                     .WithNone<GameObjectTransformComponent>()
                ) {
            ecb.DestroyEntity(entity);
        }
    }
}