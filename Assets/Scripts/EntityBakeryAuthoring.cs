using Unity.Entities;
using UnityEngine;

public struct EntityBakeryComponent : IComponentData
{
    public Entity bullet;
}

class EntityBakeryAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
}

class EntityBakeryAuthoringBaker : Baker<EntityBakeryAuthoring>
{
    public override void Bake(EntityBakeryAuthoring authoring)
    {
        Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
        AddComponent(entity, new EntityBakeryComponent
        {
            bullet = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic)
        });
    }
}
