using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLevel : MonoBehaviour
{
    [SerializeField] private PlayerDatas playerDatas;
    private int nbLevelFinished = 0;

    private void Start()
    {
        Vector2 boxCollider2DSize = new Vector2(MapGenerator.Instance.CorridorLength, 3);

        GetComponent<BoxCollider2D>().size = boxCollider2DSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(nbLevelFinished >= 3)
            {
                playerDatas.levelFinished = 0;
                SceneManager.LoadScene("BossArena");
            }
            else if (SceneManager.GetActiveScene().name == "ArenaLevel")
            {
                playerDatas.levelFinished++;
                SceneManager.LoadScene("Hub");
            }
            else if (SceneManager.GetActiveScene().name == "Hub")
            {
                SceneManager.LoadScene("ArenaLevel");
            }
        }
    }
}
