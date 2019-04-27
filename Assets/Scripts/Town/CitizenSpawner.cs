﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    [SerializeField] int spawnChance = 60;
    [SerializeField] int citizenDisplayedPerCount = 10;
    [SerializeField] int intervall = 5;

    [SerializeField] CitizenGroup[] citizenGroupPrefabs;
    [SerializeField] GameObject spawnPointHolder;
    [SerializeField] SlaveManager slaveManager;

    SpawnPoint[] spawnPoints;

    void Start()
    {
        spawnPoints = spawnPointHolder.GetComponentsInChildren<SpawnPoint>();
        StartCoroutine(TrySpawn());
    }

    IEnumerator TrySpawn()
    {
        while (true)
        {
            int roll = Random.Range(0, 100);
            if (roll < spawnChance)
            {
                roll = Random.Range(0, spawnPoints.Length);
                if(spawnPoints[roll].CitizenGroup == null)
                {
                    int rollGroup = Random.Range(0, citizenGroupPrefabs.Length);
                    int displayedCitizen = 1;
                    if (rollGroup > citizenDisplayedPerCount)
                    {
                        displayedCitizen = Mathf.RoundToInt(rollGroup * 0.1f);
                    }

                    CitizenGroup newCitizenGroup = Instantiate(citizenGroupPrefabs[rollGroup], spawnPoints[roll].transform);
                    newCitizenGroup.SlaveManager = slaveManager;
                    newCitizenGroup.transform.localPosition = Vector3.zero;
                    newCitizenGroup.spawnPoint = spawnPoints[roll];
                    spawnPoints[roll].CitizenGroup = newCitizenGroup;
                }
            }
            yield return new WaitForSeconds(intervall);
        }
    }
}