using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawner : MonoBehaviour
{
    public Monster Monster;
    public int MinMonster;
    public int MaxMonster;
    public float SpawnRadius;
    public float DetectionRadius;

    private List<Monster> SpawnedMonsters = new List<Monster>();
    private Transform Player;

    private void Start()
    {
        Player = P_Movement.instance.transform;
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        int monsterCount = Random.Range(MinMonster, MaxMonster + 1);
        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            var newMonster = Instantiate(Monster, spawnPosition,
                Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));

            SpawnedMonsters.Add(newMonster);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomCircle = Random.insideUnitCircle * SpawnRadius;
        return new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y,
            transform.position.z + randomCircle.z);
    }
    private void Update()
    {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        var getPlayer = distance < DetectionRadius;
        TriggerMonsterCheck(getPlayer);        
    }
    private void TriggerMonsterCheck(bool getPlayer)
    {
        for (int i = 0; i < SpawnedMonsters.Count; ++i)
        {
            if (getPlayer)
            {
                SpawnedMonsters[i].GetPlayer(Player);
            }
            else
            {
                SpawnedMonsters[i].RemovePlayer();
            }
        }
    }

}
