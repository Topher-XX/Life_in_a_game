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
    [SerializeField] private int corridorLength;
    [SerializeField] private int corridorHeight;

    [Header("Tiles & Elements")]
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject transitionLevelTrigger;
    [SerializeField] private GameObject playerSpawn;

    Vector3Int posCorridorEnter;
    Vector3Int leftPosCorridorEnter;
    Vector3Int rightPosCorridorEnter;

    Vector3Int posCorridorExit;
    Vector3Int leftPosCorridorExit;
    Vector3Int rightPosCorridorExit;

    static public MapGenerator Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There's more than one instance of MapGenerator on this map !");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (!ceilingSecretRoom.GetComponent<HideSecretRoom>())
        {
            Debug.LogError("The CeilingSecretRoom don't contain script HideSecretRoom !\n" +
                           "Generate Map fail");
            return;
        }

        //Detect doors enter and exit location
        posCorridorEnter = new Vector3Int(mapLength / 2, 0, 0);
        leftPosCorridorEnter = posCorridorEnter + Vector3Int.left * corridorLength / 2;
        rightPosCorridorEnter = posCorridorEnter + Vector3Int.right * corridorLength / 2;

        posCorridorExit = new Vector3Int(mapLength / 2, mapHeight - 1, 0);
        leftPosCorridorExit = posCorridorExit + Vector3Int.left * corridorLength / 2;
        rightPosCorridorExit = posCorridorExit + Vector3Int.right * corridorLength / 2;

        //Create the map's arena
        GenerateFloor(0, 0, mapLength, mapHeight);
        GenerateWall(0, 0, mapLength, mapHeight);
        GenerateCorridorsEnterExit();

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

    /// <summary>
    /// generates doors and corridors for enter and exit of the level. Place transition level too
    /// </summary>
    private void GenerateCorridorsEnterExit()
    {
        //enter corridor
        GenerateFloor(leftPosCorridorEnter.x, leftPosCorridorEnter.y - corridorHeight + 1, corridorLength, corridorHeight);
        GenerateWall(leftPosCorridorEnter.x, leftPosCorridorEnter.y - corridorHeight + 1, corridorLength, corridorHeight);

        for (int i = leftPosCorridorEnter.x + 1; i < rightPosCorridorEnter.x - 1; i++)
        {
            Vector3Int thisPosDoorEnter = new Vector3Int(i, leftPosCorridorEnter.y, 0);
            wallMap.SetTile(thisPosDoorEnter, null);

            Vector3 prefabDoorPos = thisPosDoorEnter + wallMap.tileAnchor;
            Instantiate(door, prefabDoorPos, new Quaternion());

        }

        //exit corridor
        GenerateFloor(leftPosCorridorExit.x, leftPosCorridorExit.y, corridorLength, corridorHeight);
        GenerateWall(leftPosCorridorExit.x, leftPosCorridorExit.y, corridorLength, corridorHeight);

        for (int i = leftPosCorridorExit.x + 1; i < rightPosCorridorExit.x - 1; i++)
        {
            Vector3Int thisPosDoorExit = new Vector3Int(i, leftPosCorridorExit.y, 0);
            wallMap.SetTile(thisPosDoorExit, null);

            Vector3 prefabDoorPos = thisPosDoorExit + wallMap.tileAnchor;
            Instantiate(door, prefabDoorPos, new Quaternion());

        }

        //Place the PlayerSpawn and the TransitionLevelTrigger
        Instantiate(playerSpawn, posCorridorEnter + new Vector3Int(0, -corridorHeight / 2, 0), new Quaternion());
        Instantiate(transitionLevelTrigger, posCorridorExit + new Vector3Int(0, CorridorLength + 2, 0), new Quaternion());

    }

    #region Getter & Setter

    public int CorridorLength
    {
        get { return corridorLength; }
    }
    
    #endregion


}
