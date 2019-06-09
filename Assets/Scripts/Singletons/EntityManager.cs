using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    public List<Entity> entities;

    public GameObject GameboardEntitiesContainer;

    #region Entity Placement Controller
    public GameObject EntityPlacementControllerPrefab;

    private EntityPlacementController _entityPlacementController;

    public void InteractWithEntityPlacementController(Entity entity) {
        if(_entityPlacementController == null ) {
            if(entity is Building) {
                var buildingEntity = (entity as Building);
                if(buildingEntity.data.state == BuildingState.CONSTRUCTION) {
                    return;
                }
                SpawnEntityPlacementController();
                _entityPlacementController.footprint = buildingEntity.data.footprint;
                _entityPlacementController.PlaneMeshRenderer.sharedMaterial.SetInt("_SelectedCellX", _entityPlacementController.footprint.y - 1);
                _entityPlacementController.OriginalPosition = buildingEntity.GridPosition;
                MoveEntityPlaceControllerTo(buildingEntity.GridPosition);
                _entityPlacementController.AttachEntity(buildingEntity);
                entities.Remove(buildingEntity);
            }
        }
    }

    public void PlaceEntityPlacementController() {
        if(_entityPlacementController != null) {
            var goEntityComp =  _entityPlacementController.attachedEntity.GetComponent<Building>();
            SpawnBuildingVisual(goEntityComp, _entityPlacementController.CurrentPosition);
            goEntityComp.GridPosition = _entityPlacementController.CurrentPosition;
            goEntityComp.Setup();
            entities.Add(goEntityComp);
            SetLayerRecursive(_entityPlacementController.attachedEntity, GameboardEntitiesContainer.layer);
            RemoveEntityPlacementController();
        }
    }

    public void CancelEntityPlacementController() {
        if(_entityPlacementController != null) {
            var goEntityComp =  _entityPlacementController.attachedEntity.GetComponent<Building>();
            SpawnBuildingVisual(goEntityComp, _entityPlacementController.OriginalPosition);
            goEntityComp.GridPosition = _entityPlacementController.OriginalPosition;
            goEntityComp.Setup();
            entities.Add(goEntityComp);
            SetLayerRecursive(_entityPlacementController.attachedEntity, GameboardEntitiesContainer.layer);
            RemoveEntityPlacementController();
        }
    }

    public bool IsValidEntityPlaceControllerPosition(Vector2Int gridXY) {
        bool retVal = true;
        if(_entityPlacementController == null) {
            return false;
        }
        foreach (var entity in entities)
        {
            if(entity is Building && retVal) {
                var data = (entity as Building).data;
                
                int currentCheckX, currentCheckY;
                for(int i = 0; i < _entityPlacementController.footprint.y && retVal; i++) {
                    currentCheckX = gridXY.x + i;
                    for(int j = 0; j < _entityPlacementController.footprint.x; j++) {
                        currentCheckY = gridXY.y + j;
                        
                        if(entity.GridPosition.x <= currentCheckX && entity.GridPosition.x + data.footprint.y > currentCheckX) {
                            if(entity.GridPosition.y <= currentCheckY && entity.GridPosition.y + data.footprint.x > currentCheckY) {
                                retVal = false;
                                break;
                            }
                        }
                        if(GameboardManager.Instance.IsOutOfBounds(new Vector2Int(currentCheckX, currentCheckY))) {
                            retVal = false;
                        }
                    }
                }
            }
        }
        return retVal;
    }

    public void MoveEntityPlaceControllerTo(Vector2Int gridXY) {
        if(_entityPlacementController != null) {

            Vector3 newPos = new Vector3(0f, 1f, 0f);
            
            newPos.x += (gridXY.x * Gameboard.SQUARE_SIZE);
            newPos.z += (gridXY.y * Gameboard.SQUARE_SIZE) * -1;
            newPos.x += (Gameboard.SQUARE_SIZE/2) * _entityPlacementController.footprint.y;
            newPos.z += ((Gameboard.SQUARE_SIZE/2) * _entityPlacementController.footprint.x * -1);
            _entityPlacementController.PlaneMeshRenderer.transform.localScale = new Vector3(_entityPlacementController.footprint.y, 1f, _entityPlacementController.footprint.x);
            _entityPlacementController.gameObject.transform.position = newPos;

            _entityPlacementController.PlaneMeshRenderer.sharedMaterial.SetInt("_GridSizeX", _entityPlacementController.footprint.y);
            _entityPlacementController.PlaneMeshRenderer.sharedMaterial.SetInt("_GridSizeY", _entityPlacementController.footprint.x);
            TooltipsManager.Instance.ShowMoveTooltip(newPos);
            _entityPlacementController.CurrentPosition = gridXY;
        }
    }

    public void SpawnEntityPlacementController() {
        if(_entityPlacementController == null) {
            var epcGO = Instantiate(EntityPlacementControllerPrefab);
            _entityPlacementController = (epcGO.GetComponent<EntityPlacementController>());
        }
    }

    public void RemoveEntityPlacementController() {
        if(_entityPlacementController != null) {
            Destroy(_entityPlacementController.gameObject);
        }
    }
    #endregion

    private void Awake() {
        Instance = this;
        entities = new List<Entity>();

        GameModeManager.OnGameModeChanged += CancelEntityPlacementController;
    }

    private void Start() {
        if(DataManager.Instance.LoadedEntitiesData[0] is BuildingData) {
            PlaceNewBuildingAt(DataManager.Instance.LoadedEntitiesData[0] as BuildingData, Vector2Int.one);
        }
    }

    private void PlaceNewBuildingAt(BuildingData buildingData, Vector2Int gridXY) {
        var go = new GameObject();
        var goEntityComp =  go.AddComponent<Building>();
        goEntityComp.GridPosition = gridXY;
        goEntityComp.data = buildingData as BuildingData;
        SpawnBuildingVisual(goEntityComp, goEntityComp.GridPosition);
        goEntityComp.Setup();
        entities.Add(goEntityComp);
        SetLayerRecursive(go, GameboardEntitiesContainer.layer);
    }

    private void SpawnBuildingVisual(Building entity, Vector2Int gridXY) {
            var data = entity.data;
            
            if(entity.VisualPrefab == null) {
                entity.VisualPrefab = Instantiate(data.visualPrefab);
            }

            entity.gameObject.name = data.visualPrefab.name;
            entity.gameObject.transform.parent = GameboardEntitiesContainer.transform;
            entity.VisualPrefab.transform.parent = entity.gameObject.transform;

            float entityX = Gameboard.SQUARE_SIZE * gridXY.x;
            float entityZ = Gameboard.SQUARE_SIZE * gridXY.y * -1;

            entity.gameObject.transform.position = new Vector3(entityX, 0f, entityZ);
    }

    public Entity ReturnEntityAt(Vector2Int checkPos) {
        foreach (var entity in entities)
        {
            if(entity is Building) {
                var data = (entity as Building).data;
                
                if(entity.GridPosition.x <= checkPos.x && entity.GridPosition.x + data.footprint.y > checkPos.x) {
                    if(entity.GridPosition.y <= checkPos.y && entity.GridPosition.y + data.footprint.x > checkPos.y) {
                        return entity;
                    }
                }
            }
        }
        return null;
    }

    public Entity CheckEntityTouched(Vector2Int gridTouch) {
        return ReturnEntityAt(gridTouch);
    }

    private void SetLayerRecursive(GameObject go, int newLayer) {
        go.layer = newLayer;
        foreach (var child in go.transform)
        {
            SetLayerRecursive((child as Transform).gameObject, newLayer);
        }
    }

    private void OnDestroy() {
        Instance = null;
        GameModeManager.OnGameModeChanged -= CancelEntityPlacementController;
    }
}
