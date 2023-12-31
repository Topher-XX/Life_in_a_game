using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

//Use for determine the door's orientation for secrets rooms
public enum OrientationEnterSecretRoom
{
    up, down, left, right
}

public partial class MapGenerator : MonoBehaviour
{
    [Header("Tile Map")]
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap wallMap;

    [Header("Map Parameters")]
    [SerializeField] private int mapLength;
    [SerializeField] private int mapHeight;

    [Header("Map Sprites")]
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile destructibleWallTile;

    private void Start()
    {
        if (!ceilingSecretRoom.GetComponent<HideSecretRoom>())
        {
            Debug.LogError("The CeilingSecretRoom don't contain script HideSecretRoom !\n" +
                           "Generate Map fail");
            return;
        }
        

        //Create the map's arena
        GenerateFloor(0, 0, mapLength, mapHeight);
        GenerateWall(0, 0, mapLength, mapHeight);

        GenerateSecretRoom();
    }

    /// <summary>
    /// generates floors based on position and room size
    /// </summary>
    /// <param name="startXCoordinate"></param>
    /// <param name="startYCoordinate"></param>
    /// <param name="length"></param>
    /// <param name="height"></param>
    private void GenerateFloor(int startXCoordinate, int startYCoordinate, int length, int height)
    {
        int endXCoordiante = startXCoordinate + length - 1;
        int endYCoordiante = startYCoordinate + height - 1;

        //Generate floor in a rectangular form with a size of length x height
        for (int i = startXCoordinate; i <= endXCoordiante; i++)
        {
            for (int j = startYCoordinate; j <= endYCoordiante; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                floorMap.SetTile(position, floorTile);
            }
        }
    }

    /// <summary>
    /// generates walls based on position and room size
    /// </summary>
    /// <param name="startXCoordinate"></param>
    /// <param name="startYCoordinate"></param>
    /// <param name="length"></param>
    /// <param name="height"></param>
    private void GenerateWall(int startXCoordinate, int startYCoordinate, int length, int height)
    {
        int endXCoordinate = startXCoordinate + length - 1;
        int endYCoordinate = startYCoordinate + height - 1;

        //Generate wall on the top and bottow of the map
        for (int i = startXCoordinate; i <= endXCoordinate; i++)
        {
            Vector3Int downPosition = new Vector3Int(i, startYCoordinate, 0);
            wallMap.SetTile(downPosition, wallTile);

            Vector3Int upPosition = new Vector3Int(i, endYCoordinate, 0);
            wallMap.SetTile(upPosition, wallTile);
        }

        //Generate wall on left and right of the map
        for (int i = startYCoordinate; i <= endYCoordinate; i++)
        {
            Vector3Int leftPosition = new Vector3Int(startXCoordinate, i, 0);
            wallMap.SetTile(leftPosition, wallTile);

            Vector3Int rightPosition = new Vector3Int(endXCoordinate, i, 0);
            wallMap.SetTile(rightPosition, wallTile);
        }
    }



}
