﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] Text productionCount;
    [SerializeField] Text foodNeededCount;
    [SerializeField] Toggle extraFoodForWorker;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] CollectorManager resourcesManager;
    [SerializeField] CollectorManager farmManager;
    [SerializeField] SlaveManager slaveManager;
    [SerializeField] int foodNeededPerWorker;
    [SerializeField] int extraFoodMultiplier = 2;
    [SerializeField] int foodNeededPerWorkingWorkerNormal = 2;
    [SerializeField] int foodNeedFor10CitizenSpawns;
    [SerializeField] int minDyingPerc = 50;
    [SerializeField] int maxDyingPerc = 90;

    int foodNeeded;
    int foodProduction;
    int foodNeededForSpawning;

    public void AddFoodNeededForSpawning (int citizensSpawend)
    {
        int citizenGroupSize = Mathf.RoundToInt(citizensSpawend / 10);
        if (citizenGroupSize < 1)
        {
            foodNeededForSpawning += foodNeedFor10CitizenSpawns;
        }
        else
        {
            foodNeededForSpawning = citizenGroupSize * foodNeedFor10CitizenSpawns;
        }
    }


    public void CalculateFood(int foodProduction)
    {
        int foodNeededPerWorkingWorker = foodNeededPerWorkingWorkerNormal;
        if (extraFoodForWorker.isOn)
        {
            foodNeededPerWorkingWorker = foodNeededPerWorkingWorkerNormal * extraFoodMultiplier;
        }
        foodNeeded = foodNeededForSpawning;
        int nextNeededFood = farmManager.Worker * foodNeededPerWorkingWorker;
        if (nextNeededFood < farmManager.Material - foodNeeded)
        {
            int roll = Random.Range(minDyingPerc, maxDyingPerc);
            int dyingWorker = farmManager.Worker - Mathf.RoundToInt((farmManager.Material - foodNeeded) / foodNeededPerWorkingWorker);
            dyingWorker = Mathf.RoundToInt(dyingWorker * (roll / 100));
            farmManager.RemoveWorker(dyingWorker);
        }
        nextNeededFood = (resourcesManager.Worker + buildingManager.Worker) * foodNeededPerWorkingWorker;
        if (nextNeededFood < farmManager.Material - foodNeeded)
        {
            int roll = Random.Range(minDyingPerc, maxDyingPerc);
            int dyingWorker = (resourcesManager.Worker + buildingManager.Worker) - Mathf.RoundToInt((farmManager.Material - foodNeeded) / foodNeededPerWorkingWorker);
            dyingWorker = Mathf.RoundToInt((dyingWorker * (roll / 100))/2);
            resourcesManager.RemoveWorker(dyingWorker);
            buildingManager.RemoveWorker(dyingWorker);
        }
        foodNeeded += nextNeededFood;
        nextNeededFood = slaveManager.SlaveCount * foodNeededPerWorker;
        if (nextNeededFood < farmManager.Material - foodNeeded)
        {
            int roll = Random.Range(minDyingPerc, maxDyingPerc);
            int dyingWorker = slaveManager.SlaveCount - Mathf.RoundToInt((farmManager.Material - foodNeeded )/ foodNeededPerWorkingWorker);
            dyingWorker = Mathf.RoundToInt((dyingWorker * (roll / 100)));
            slaveManager.RemoveSlaves(dyingWorker);
        }
        foodNeeded += nextNeededFood;
        if (foodNeeded > farmManager.Material)
        {
            farmManager.RemoveMaterial(farmManager.Material);
        }
        else
        {
            farmManager.RemoveMaterial(foodNeeded);
        }
        foodNeededCount.text = "Demand: " + foodNeeded;
        productionCount.text = "production: " + foodProduction;
    }
}