using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapConverter : MonoBehaviour
{
    public Tilemap Map2D;  // 2D Tilemap
    public GameObject[] Objects3D;  // 3D 世界中的物体
    public TileBase tileToPlace;  // 2D Tilemap 上的瓦片类型
    public GameObject objectPrefab;  // 3D 世界中物体的预制体

    [Tooltip("Z = 0")]
    public Vector3Int Map2DPositionOffset;

    private Dictionary<Vector3Int, GameObject> Map2DTo3D = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        Sync3DTo2D();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Sync2DTo3D();
        }
    }

    public void Sync3DTo2D()
    {
        foreach (GameObject obj in Objects3D)
        {
            Vector3 objPosition = obj.transform.position;
            Vector3Int tilemapPos = WorldToTilemapPosition(objPosition);

            Map2D.SetTile(tilemapPos, tileToPlace);

            if (!Map2DTo3D.ContainsKey(tilemapPos))
            {
                Map2DTo3D.Add(tilemapPos, obj);
            }
        }
    }

    public void Sync2DTo3D()
    {
        foreach (var position in Map2D.cellBounds.allPositionsWithin)
        {
            Vector3Int localPosition = new Vector3Int(position.x, position.y, position.z);
            if (Map2D.HasTile(localPosition))
            {
                if (!Map2DTo3D.ContainsKey(localPosition))
                {
                    Vector3 worldPosition = TilemapToWorldPosition(localPosition);
                    GameObject obj = Instantiate(objectPrefab, worldPosition, Quaternion.identity);
                    Map2DTo3D.Add(localPosition, obj);
                }
            }
            else if (Map2DTo3D.ContainsKey(localPosition))
            {
                // Remove 3D Objects that no longer exist
                Destroy(Map2DTo3D[localPosition]);
                Map2DTo3D.Remove(localPosition);
            }
        }
    }

    Vector3Int WorldToTilemapPosition(Vector3 worldPosition)
    {
        return Map2D.WorldToCell(new Vector3(worldPosition.x, worldPosition.z, 0) + Map2DPositionOffset);
    }

    Vector3 TilemapToWorldPosition(Vector3Int tilemapPosition)
    {
        Vector3 worldPosition = Map2D.GetCellCenterWorld(tilemapPosition - Map2DPositionOffset);
        return new Vector3(worldPosition.x, 0, worldPosition.y);  
    }
}