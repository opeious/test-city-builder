using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData {    
    public Vector2Int footprint;

    public GameObject visualPrefab;

    public string entityName;
}

public class Entity : MonoBehaviour
{
    public Vector2Int GridPosition;

    public EntityData data;

    public GameObject VisualPrefab;

    public virtual void OnTouch() {
        
    }
}
