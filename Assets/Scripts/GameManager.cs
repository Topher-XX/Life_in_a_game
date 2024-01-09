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
    public GameObject[] monster;
    public GameObject monstersees;
    private  int[] percentToSpawn;
    private int Maxweight;
    private List<int> percentList;

    private void Awake()
    {
        GMInstance = this;
        percentList = new List<int>();
    }

    private void Start()
    {
        
        Maxweight = 0;
        for(var i = 0; i < monster.Length ; i++)
        {
            var percent = monster[i].GetComponent<Enemy>().baseStats.weight;
            percentList.Add(percent);
           Maxweight +=  percent;
        }
        Debug.Log($"MaxWeight ={Maxweight}");
        var listNbr = 0; 
        GetRandomMonster(out listNbr);
        Debug.Log($"le truc a spawn ={listNbr}");
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
            // % de spawn de monstres
            var RandomMonster = 0;
            GetRandomMonster(out RandomMonster);


            var SpawnPos = new Vector3(0, 0, 0);

            if (avoidCenter)
            {

                RandomPos(placeToSpawn, out SpawnPos);
                if ((SpawnPos.x == placeToSpawn.x) && (SpawnPos.y == placeToSpawn.y))
                {
                    SpawnPos += new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
                    Instantiate(monster[RandomMonster], SpawnPos, Quaternion.identity);
                }
                else
                {

                    Instantiate(monster[RandomMonster], SpawnPos, Quaternion.identity);
                }
            }
            else
            {
                RandomPos(placeToSpawn, out SpawnPos);
                Instantiate(monster[RandomMonster], SpawnPos, Quaternion.identity);
            }
        }
        
    }

    private Vector3 RandomPos(Vector3 SpawnPointCenter, out Vector3 Pos)
    {
        var RandomX = Random.Range(SpawnPointCenter.x - 2, SpawnPointCenter.x + 2);
        var RandomY = Random.Range(SpawnPointCenter.y - 2, SpawnPointCenter.y + 2);
        return Pos = new Vector3(RandomX, RandomY, SpawnPointCenter.z);
        
    }

    public void SpawnSelectEnemy(Vector3 placeToSpawn, int nbrToSpawn,GameObject wantedMonster)
    {

        for (int i = 0; i < nbrToSpawn; i++)
        {
            Debug.Log("SPawn");
        }
    }
    
    private void GetRandomMonster(out int nbrMonster)
    {
        var i = 0;
        var random = Random.Range(1,Maxweight);
        for ( i=0;i<monster.Length;i++) 
        {
            if (random<percentList[i])
            { 
                nbrMonster = i;
                break;
            }
            else
            {
                random -= percentList[i];
            }
        }
        nbrMonster = i;
    }


    
}
