using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial class ShooterSystem : SystemBase
{
    private float fireRateTimer = 0;
    private EntityBakeryAuthoring entityBakery;

    [BurstCompile]
    protected override void OnStartRunning()
    {
        entityBakery = GameManager.Instance.GetComponent<EntityBakeryAuthoring>();
    }
    [BurstCompile]
    protected override void OnUpdate()
    {
        float moveSpeed = GameManager.Instance.moveSpeed;
        float heightInput = 0;
        if(Input.GetKey(KeyCode.Q))
        {
            heightInput = -1;
        }
        else if(Input.GetKey(KeyCode.E))
        {
            heightInput = 1;
        }

        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 inputForwardVector = GameManager.Instance.mainCamTransform.right * horizontalAxis;
        Vector3 inputRightVector = GameManager.Instance.mainCamTransform.forward * verticalAxis;

        GameManager.Instance.mainCamTransform.position += (inputForwardVector + inputRightVector + (Vector3.up * heightInput)) * moveSpeed;
        if (Input.GetMouseButton(0))
        {
            fireRateTimer += SystemAPI.Time.DeltaTime;
            if (fireRateTimer < GameManager.Instance.RateOfFire) return;
            fireRateTimer = 0;
            if(GameManager.Instance.useEntity)
            {
                EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(CheckedStateRef.WorldUnmanaged);
                Entity bulletEntity = ecb.Instantiate(SystemAPI.GetSingleton<EntityBakeryComponent>().bullet);
                ecb.AddComponent(bulletEntity, new LocalTransform
                {
                    Position = GameManager.Instance.mainCam.ScreenToWorldPoint(Input.mousePosition),
                    Rotation = GameManager.Instance.mainCamTransform.rotation,
                    Scale = 1
                });
                ecb.AddComponent(bulletEntity, new PhysicsVelocity
                {
                    Linear = GameManager.Instance.mainCamTransform.forward * GameManager.Instance.bulletSpeed,
                    Angular = 0
                });
                ecb.AddComponent(bulletEntity, new LifeTimeComponent
                {
                    lifeTimer = 2f // Set lifetime to 5 seconds
                });
            }
            else
            {
                GameObject.Instantiate(
                    entityBakery.bulletPrefab,
                    GameManager.Instance.mainCam.ScreenToWorldPoint(Input.mousePosition),
                    GameManager.Instance.mainCamTransform.rotation
                ).GetComponent<Rigidbody>().linearVelocity = GameManager.Instance.mainCamTransform.forward * GameManager.Instance.bulletSpeed;
            }
        }
    }
}
