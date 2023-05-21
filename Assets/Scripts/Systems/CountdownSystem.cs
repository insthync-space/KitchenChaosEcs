using Unity.Entities;
using Unity.Mathematics;

partial class CountdownSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        // var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        // var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState, ref CountdownToStartComponent countdownToStart) => {
                if (gameState.GameState != GameState.CountdownToStart) {
                    return;
                }
                
                countdownToStart.Timer += dt;
                if (countdownToStart.Timer < countdownToStart.Goal) {
                    return;
                }

                countdownToStart.Timer = 0f;
                gameState.GameState = GameState.GamePlaying;
                // ecb.SetComponentEnabled<CountdownToStartComponent>(entity, false);
                // ecb.SetComponentEnabled<CountdownFinishedComponent>(entity, true);
            }).Schedule();
        
        //ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
partial class CountdownUISystem : SystemBase {
    protected override void OnUpdate() {
        
        foreach (var (countdownToStart, gameState) in SystemAPI.Query<CountdownToStartComponent,GameStateComponent>()) {
            if (gameState.GameState != GameState.CountdownToStart) {
                GameCountdownUI.Instance.Stop();
            }
            else {
                GameCountdownUI.Instance.SetText(
                    math.ceil(countdownToStart.Goal - countdownToStart.Timer).ToString()
                );
            }
        }

        // foreach (var (finished, entity) in SystemAPI
        //              .Query<CountdownFinishedComponent>()
        //              .WithEntityAccess()
        //          ) {
        //     GameCountdownUI.Instance.Stop();
        //     SystemAPI.SetComponentEnabled<CountdownFinishedComponent>(entity, false);
        // }
        
    }
}