using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityPlacementControllerMode {
    MOVE,
    BUY
}

public class EntityPlacementController : MonoBehaviour
{
    public Vector2Int footprint;

    public MeshRenderer PlaneMeshRenderer;

    public EntityPlacementControllerMode CurrentMode;

    public GameObject attachedEntity;

    public Vector2Int OriginalPosition;

    public Vector2Int CurrentPosition;

    public void AttachEntity(Entity entity) {
        attachedEntity = entity.gameObject;
        entity.gameObject.transform.parent = gameObject.transform;
    }
}
