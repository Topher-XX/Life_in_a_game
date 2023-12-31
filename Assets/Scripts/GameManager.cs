using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{//[]
    public static GameManager GMInstance;
    public So_Scorer so_Scorer;
    private float score;
    private float highScore;
    private float lastScore;
    public float multiplier = 1;
    public GameObject[] monsters;

    private void Awake()
    {
        GMInstance = this;
        
    }


    public void SetScore(float score )
    {
        so_Scorer.currentScore += score * multiplier;
        if (so_Scorer.HighScore < so_Scorer.currentScore)
        {
            SetHighScore();
        }
    }

    public void SetHighScore()
    {
        so_Scorer.HighScore = so_Scorer.currentScore;
    }

    public void SpawnRandomEnemy(Vector3 placeToSpawn ,int nbrToSpawn,bool avoidCenter)
    {
        for (int i = 0; i < nbrToSpawn; i++)
        {
            Debug.Log("SPawn");

            // je vais le changer  le randomMonster pour une fonction avec des Proba
            var RandomMonster = Random.Range(0, monsters.Length - 1);


            var SpawnPos = new Vector3(0, 0, 0);

            if (avoidCenter)
            {

                RandomPos(placeToSpawn, out SpawnPos);
                if ((SpawnPos.x == placeToSpawn.x) && (SpawnPos.y == placeToSpawn.y))
                {
                    SpawnPos += new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
                    Instantiate(monsters[RandomMonster], SpawnPos, Quaternion.identity);
                }
                else
                {

                    Instantiate(monsters[RandomMonster], SpawnPos, Quaternion.identity);
                }
            }
            else
            {
                RandomPos(placeToSpawn, out SpawnPos);
                Instantiate(monsters[RandomMonster], SpawnPos, Quaternion.identity);
            }
        }
        
    }

    private Vector3 RandomPos(Vector3 SpawnPointCenter, out Vector3 Pos)
    {
        var RandomX = Random.Range(SpawnPointCenter.x - 2, SpawnPointCenter.x + 2);
        var RandomY = Random.Range(SpawnPointCenter.y - 2, SpawnPointCenter.y + 2);
        return Pos = new Vector3(RandomX, RandomY, SpawnPointCenter.z);
        
    }

    public void SpawnSelecEnemy(Vector3 placeToSpawn, int nbrToSpawn)
    {

    }

}
