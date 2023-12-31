using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public partial class MapGenerator : MonoBehaviour
{
    private int nbSecretRoom;
    //private GameManager gameManager;
    public GameObject truc;

    [Header("Secret Room Parameters")]
    [SerializeField] private int minNbSecretRoom;
    [SerializeField] private int maxNbSecretRoom;
    [SerializeField] private int secretRoomSize;

    [Header("Destructible Wall Parameters")]
    [SerializeField] private GameObject destructibleWall;
    [SerializeField] private int destructibleWallSize;

    [Tooltip("A game object for hides secret rooms. Need contains the script HideSecretRoom for generate map")]
    [SerializeField] private GameObject ceilingSecretRoom;

    [Header("Secret Room Contents Parameters")]
    [SerializeField] private GameObject chest;
    [SerializeField][Range(0, 100)] private int itemProbability;
    [SerializeField][Range(0, 100)] private int mobProbability;
    [SerializeField][Range(0, 100)] private int chestProbability;

    /// <summary>
    /// Generate secret rooms from variable nbSecretRoom
    /// </summary>
    private void GenerateSecretRoom()
    {
        //define a number of secret room to spawn
        nbSecretRoom = Random.Range(minNbSecretRoom, maxNbSecretRoom);


        //determine nb secret room can be placed
        //the nb is dependante of the size's map and the size of a secret room
        //bigger is the map and smaller is a secret room, more secret room are created
        //the enter of a secret room can't be create too close of the maps' corners
        int nbSecretRoomOnALength = mapLength / secretRoomSize;
        int nbSecretRoomOnAHeight = mapHeight / secretRoomSize;

        #region Check if the Number Secret Room is not greater than the map can have.
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
        #endregion

        //All Available Secret Room Position possible
        Dictionary<Vector3Int, OrientationEnterSecretRoom> availableSpotSecretRoom = new Dictionary<Vector3Int, OrientationEnterSecretRoom>();

        #region Get all available room's position
        //Get all available position on top and bottom
        for (int i = 0; i < nbSecretRoomOnALength; i++)
        {
            int xPos = i * secretRoomSize + secretRoomSize / 2;
            
            Vector3Int topPos = new Vector3Int(xPos, mapHeight - 1, 0);
            availableSpotSecretRoom.Add(topPos, OrientationEnterSecretRoom.up);

            Vector3Int bottomPos = new Vector3Int(xPos, 0, 0);
            availableSpotSecretRoom.Add(bottomPos, OrientationEnterSecretRoom.down);
        }

        //Get all available position on left and right
        for (int i = 0; i < nbSecretRoomOnAHeight; i++)
        {
            int yPos = i * secretRoomSize + secretRoomSize / 2;

            Vector3Int leftPos = new Vector3Int(0, yPos, 0);
            availableSpotSecretRoom.Add(leftPos, OrientationEnterSecretRoom.left);

            Vector3Int rightPos = new Vector3Int(mapLength - 1, yPos, 0);
            availableSpotSecretRoom.Add(rightPos, OrientationEnterSecretRoom.right);
        }
        #endregion Get all available room's position

        Dictionary<Vector3Int, OrientationEnterSecretRoom> posSecretsRooms = new Dictionary<Vector3Int, OrientationEnterSecretRoom>();

        //Spawn a number of secret room on availables positions
        for (int i = 0; i < maxSecretRoomPossible; i++)
        {
            int selectedPosIndex = Random.Range(0, availableSpotSecretRoom.Count);

            //Takes the position of the entrance to the secret room and the orientation of this room
            Vector3Int posEnterSecretRoom = availableSpotSecretRoom.ElementAt(selectedPosIndex).Key;
            OrientationEnterSecretRoom orientationEnterSecretRoom = availableSpotSecretRoom.ElementAt(selectedPosIndex).Value;

            #region Determine orientation for the room
            int minXPosRoom;
            int minYPosRoom;
            //Up
            if (orientationEnterSecretRoom == OrientationEnterSecretRoom.up)
            {
                minXPosRoom = posEnterSecretRoom.x - (secretRoomSize / 2);
                minYPosRoom = posEnterSecretRoom.y;
            }
            //Down
            else if (orientationEnterSecretRoom == OrientationEnterSecretRoom.down)
            {
                minXPosRoom = posEnterSecretRoom.x - (secretRoomSize / 2);
                minYPosRoom = posEnterSecretRoom.y - secretRoomSize + 1;
            }
            //Left
            else if (orientationEnterSecretRoom == OrientationEnterSecretRoom.left)
            {
                minXPosRoom = posEnterSecretRoom.x - secretRoomSize + 1;
                minYPosRoom = posEnterSecretRoom.y - (secretRoomSize / 2);
            }
            //Right
            else
            {
                minXPosRoom = posEnterSecretRoom.x;
                minYPosRoom = posEnterSecretRoom.y - (secretRoomSize / 2);
            }
            #endregion
            
            #region Place Tiles and Destructible Wall
            //Create floor
            GenerateFloor(minXPosRoom, minYPosRoom, secretRoomSize, secretRoomSize);

            //Create Wall
            GenerateWall(minXPosRoom, minYPosRoom, secretRoomSize, secretRoomSize);

            //Remove walls for place destructible wall
            wallMap.SetTile(posEnterSecretRoom, null);
            int destructibleWallLength = 0;
            if (destructibleWallSize < secretRoomSize)
            {
                destructibleWallLength = (destructibleWallSize - 1) / 2;
            }
            else
            {
                Debug.LogWarning("destructibleWallSize is larger than secretRoomSize ! \n" +
                                 "Please, reduce destructibleWallSize or increase secretRoomSize.\n" +
                                 "destructibleWallSize is set to 1 for this session.");
            }
            if (orientationEnterSecretRoom is OrientationEnterSecretRoom.up or OrientationEnterSecretRoom.down)
            {
                for (int j = 1; j <= destructibleWallLength; j++)
                {
                    wallMap.SetTile(posEnterSecretRoom + Vector3Int.left * j, null);
                    wallMap.SetTile(posEnterSecretRoom + Vector3Int.right * j, null);
                }

                //Place destructible wall
                Vector3 posDestructibleWall = new Vector3(wallMap.tileAnchor.x, wallMap.tileAnchor.y, 0)
                                              + posEnterSecretRoom;
                Quaternion destrutibelWallRotation = Quaternion.Euler(0, 0, 0);
                Instantiate(destructibleWall, posDestructibleWall, destrutibelWallRotation);
            }
            else if (orientationEnterSecretRoom is OrientationEnterSecretRoom.left or OrientationEnterSecretRoom.right)
            {
                for (int j = 1; j <= destructibleWallLength; j++)
                {
                    wallMap.SetTile(posEnterSecretRoom + Vector3Int.up * j, null);
                    wallMap.SetTile(posEnterSecretRoom + Vector3Int.down * j, null);
                }

                //Place destructible wall
                Vector3 posDestructibleWall = new Vector3(wallMap.tileAnchor.x, wallMap.tileAnchor.y, 0)
                                              + posEnterSecretRoom;
                Quaternion destrutibelWallRotation = Quaternion.Euler(0, 0, 90);
                Instantiate(destructibleWall, posDestructibleWall, destrutibelWallRotation);
            }
            

            
            #endregion

            posSecretsRooms.Add(posEnterSecretRoom, orientationEnterSecretRoom);
            availableSpotSecretRoom.Remove(posEnterSecretRoom);
        }

        #region  Fill all secrets rooms with a item, a mob or a chest and add a ceiling for hide them
        for (int i = 0; i < posSecretsRooms.Count; i++)
        {
            Vector3 posSecretRoomCenter = GetSecretRoomCenter(posSecretsRooms.ElementAt(i).Value, 
                                                            posSecretsRooms.ElementAt(i).Key);

            

            int maxRandom = itemProbability + mobProbability + chestProbability;
            int random = Random.Range(0, maxRandom);

            #region Spawn a item, a mob or a chest based on the random
            if (random < itemProbability) //Item
            {
                ItemSpawnerManager.instance.SpawnSpecificNbItems(1, posSecretRoomCenter, 0, new Quaternion());
            }
            else if (random < itemProbability + mobProbability) //Mob
            {
                Debug.Log("Spawn a enemy");
                
                // GameManager.GMInstance.SpawnRandomEnemy(posSecretRoomCenter, 2, false);
                
                
            }
            else //Chest
            {
                Instantiate(chest, posSecretRoomCenter, new Quaternion());
            }

            GameObject ceiling = Instantiate(ceilingSecretRoom, posSecretRoomCenter, new Quaternion());
            ceiling.GetComponent<HideSecretRoom>().SetCeilingSize(secretRoomSize-2, secretRoomSize-2);

            #endregion
        }
        #endregion



    }

    public Vector3 GetSecretRoomCenter(OrientationEnterSecretRoom orientationEnterSecretRoom, Vector3 posSecretRoomEnter)
    {
        Vector3 posSecretRoomCenter = new Vector3(0, 0, 0);

        switch (orientationEnterSecretRoom)
        {
            //Up
            case OrientationEnterSecretRoom.up:
                posSecretRoomCenter.x = floorMap.tileAnchor.x;
                posSecretRoomCenter.y = secretRoomSize / 2 + floorMap.tileAnchor.y;
                break;

            //Down
            case OrientationEnterSecretRoom.down:
                posSecretRoomCenter.x = floorMap.tileAnchor.x;
                posSecretRoomCenter.y = -secretRoomSize / 2 + floorMap.tileAnchor.y;
                break;

            //Left
            case OrientationEnterSecretRoom.left:
                posSecretRoomCenter.x = -secretRoomSize / 2 + floorMap.tileAnchor.x;
                posSecretRoomCenter.y = floorMap.tileAnchor.y;
                break;

            //Right
            case OrientationEnterSecretRoom.right:
                posSecretRoomCenter.x = secretRoomSize / 2 + floorMap.tileAnchor.x;
                posSecretRoomCenter.y = floorMap.tileAnchor.y;
                break;

            default:
                Debug.Log("A Secret room has not Orientation when place secret room contain! ");
                break;
        }
        return posSecretRoomCenter += posSecretRoomEnter;
    }


}
