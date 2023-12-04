using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum OrientationEnterSecretRoom
{
    up, down, left, right
}

public class MapGenerator : MonoBehaviour
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

    [Header("Secret Room Parameters")]
    [SerializeField] private int nbSecretRoom;
    [SerializeField] private int secretRoomSize;
    [SerializeField] private GameObject[] ObjectInRoom;

    private void Start()
    {
        GenerateFloor();
        GenerateWall();
        GenerateSecretRoom();
    }

    private void GenerateFloor()
    {
        //Generate floor in a rectangular form with a size of mapLength x mapHeight
        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Vector3Int position = new Vector3Int(i - mapLength/2, j - mapHeight/2, 0);
                floorMap.SetTile(position, floorTile);
            }
        }
    }

    private void GenerateWall()
    {
        //Generate wall on the top and bottow of the map
        for (int i = 0; i < mapLength; i++)
        {
            Vector3Int downPosition = new Vector3Int(i - mapLength / 2, -mapHeight / 2, 0);
            wallMap.SetTile(downPosition, wallTile);

            Vector3Int upPosition = new Vector3Int(i - mapLength / 2, mapHeight / 2 - 1, 0);
            wallMap.SetTile(upPosition, wallTile);
        }

        //Generate wall on left and right of the map
        for (int i = 0; i < mapHeight; i++)
        {
            Vector3Int leftPosition = new Vector3Int(-mapLength / 2, i - mapHeight / 2, 0);
            wallMap.SetTile(leftPosition, wallTile);

            Vector3Int rightPosition = new Vector3Int(mapLength / 2 - 1, i - mapHeight / 2, 0);
            wallMap.SetTile(rightPosition, wallTile);
        }
    }

    private void GenerateSecretRoom()
    {
        //determine nb secret room can ba placed
        //the nb is dependante of the size's map and the size of a secret room
        //bigger is the map and smaller is a secret room, more secret room are created
        //the enter of a secret room can't be create too close of the maps' corners
        int nbSecretRoomOnALength = mapLength / secretRoomSize;
        int nbSecretRoomOnAHeight = mapHeight / secretRoomSize;

        //Check if the Number Secret Room is not greater than the map can have.
        int maxSecretRoomPossible = nbSecretRoom;
        if (maxSecretRoomPossible > (nbSecretRoomOnAHeight + nbSecretRoomOnALength) * 2)
        {
            maxSecretRoomPossible = (nbSecretRoomOnAHeight + nbSecretRoomOnALength) * 2;

            Debug.LogWarning("the number Secret Room requested if greater than " +
                             "the map can contain !\n" +
                             "Reduce the Number of Secret Room requested or extend the map's size.\n" +
                             "Number Secret Room is Created with the maximum possible : " +
                             maxSecretRoomPossible);

        }

        //All Available Secret Room Position possible
        Dictionary<Vector3Int, OrientationEnterSecretRoom> availableSpotSecretRoom = new Dictionary<Vector3Int, OrientationEnterSecretRoom>();
        
        //Get all available position on top and bottom
        for (int i = 0; i < nbSecretRoomOnALength; i++)
        {
            int xPos = i * secretRoomSize - mapLength / 2 + secretRoomSize / 2;
            
            Vector3Int topPos = new Vector3Int(xPos, -mapHeight / 2, 0);
            availableSpotSecretRoom.Add(topPos, OrientationEnterSecretRoom.up);

            Vector3Int bottomPos = new Vector3Int(xPos, mapHeight / 2 - 1, 0);
            availableSpotSecretRoom.Add(bottomPos, OrientationEnterSecretRoom.down);
        }

        //Get all available position on left and right
        for (int i = 0; i < nbSecretRoomOnAHeight; i++)
        {
            int yPos = i * secretRoomSize - mapHeight / 2 + secretRoomSize / 2;

            Vector3Int leftPos = new Vector3Int(-mapLength/2, yPos, 0);
            availableSpotSecretRoom.Add(leftPos, OrientationEnterSecretRoom.left);

            Vector3Int rightPos = new Vector3Int(mapLength/2 - 1, yPos, 0);
            availableSpotSecretRoom.Add(rightPos, OrientationEnterSecretRoom.right);
        }


        //Spawn a number of secret room on availables positions
        for (int i = 0; i < maxSecretRoomPossible; i++)
        {
            int selectedPosIndex = Random.Range(0, availableSpotSecretRoom.Count);
            
            Vector3Int posEnterSecretRoom = availableSpotSecretRoom.ElementAt(selectedPosIndex).Key;
            OrientationEnterSecretRoom orientationEnterSecretRoom =
                availableSpotSecretRoom.ElementAt(selectedPosIndex).Value;


            int minXPosRoom;
            int maxXPosRoom;
            int minYPosRoom;
            int maxYPosRoom;

            //determine orientation for the room
            //Up
            if (orientationEnterSecretRoom == OrientationEnterSecretRoom.up)
            {
                minXPosRoom = posEnterSecretRoom.x - (secretRoomSize / 2);
                maxXPosRoom = posEnterSecretRoom.x + (secretRoomSize / 2);
                minYPosRoom = posEnterSecretRoom.y - (secretRoomSize - 1);
                maxYPosRoom = posEnterSecretRoom.y;
            }
            //Down
            else if (orientationEnterSecretRoom == OrientationEnterSecretRoom.down)
            {
                minXPosRoom = posEnterSecretRoom.x - (secretRoomSize / 2);
                maxXPosRoom = posEnterSecretRoom.x + (secretRoomSize / 2);
                minYPosRoom = posEnterSecretRoom.y;
                maxYPosRoom = posEnterSecretRoom.y + (secretRoomSize - 1);
            }
            //Left
            else if (orientationEnterSecretRoom == OrientationEnterSecretRoom.left)
            {
                minXPosRoom = posEnterSecretRoom.x - (secretRoomSize - 1);
                maxXPosRoom = posEnterSecretRoom.x;
                minYPosRoom = posEnterSecretRoom.y - (secretRoomSize / 2);
                maxYPosRoom = posEnterSecretRoom.y + (secretRoomSize / 2);
            }
            //Right
            else
            {
                minXPosRoom = posEnterSecretRoom.x;
                maxXPosRoom = posEnterSecretRoom.x + (secretRoomSize - 1);
                minYPosRoom = posEnterSecretRoom.y - (secretRoomSize / 2);
                maxYPosRoom = posEnterSecretRoom.y + (secretRoomSize / 2);
            }

            //Create floor
            for (int j = minXPosRoom; j <= maxXPosRoom; j++)
            {
                for (int k = minYPosRoom; k <= maxYPosRoom; k++)
                {
                    Vector3Int pos = new Vector3Int(j, k, 0);
                    floorMap.SetTile(pos, floorTile);
                }
            }

            //Create Wall
            for (int j = minXPosRoom; j <= maxXPosRoom; j++)
            {
                Vector3Int downPosition = new Vector3Int(j, minYPosRoom, 0);
                wallMap.SetTile(downPosition, wallTile);

                Vector3Int upPosition = new Vector3Int(j, maxYPosRoom, 0);
                wallMap.SetTile(upPosition, wallTile);
            }

            for (int j = minYPosRoom; j <= maxYPosRoom; j++)
            {
                Vector3Int leftPosition = new Vector3Int(minXPosRoom, j, 0);
                wallMap.SetTile(leftPosition, wallTile);

                Vector3Int rightPosition = new Vector3Int(maxXPosRoom, j, 0);
                wallMap.SetTile(rightPosition, wallTile);
            }

            wallMap.SetTile(posEnterSecretRoom, destructibleWallTile);

            availableSpotSecretRoom.Remove(posEnterSecretRoom);

        }
        

    }
}
