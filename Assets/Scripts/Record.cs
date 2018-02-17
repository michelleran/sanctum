using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Record {
    public string story;

    public int population;
    public int capacity;

    public int points;
    public int pointsPerPerson;
    public float timeForPoints;

    public Person[] residents;

    public int[] costs;

    public int[] unlockedFeatures;
    public int[] existingFeatures;

    public int housesAmount;
    public int orchardsAmount;
    public int shrinesAmount;
    public int beaconsAmount;

    public bool open;
    public int waitingRefugees;

    public Record(string story, int population, int capacity, 
                  int points, int pointsPerPerson, float timeForPoints, 
                  Person[] residents, int[] costs, 
                  int[] existingFeatures, int[] unlockedFeatures,
                  int housesAmount, int orchardsAmount, 
                  int shrinesAmount, int beaconsAmount,
                  bool open, int waitingRefugees) {

        this.story = story;

        this.population = population;
        this.capacity = capacity;

        this.points = points;
        this.pointsPerPerson = pointsPerPerson;
        this.timeForPoints = timeForPoints;

        this.residents = residents;

        this.costs = costs;

        this.unlockedFeatures = unlockedFeatures;
        this.existingFeatures = existingFeatures;

        this.housesAmount = housesAmount;
        this.orchardsAmount = orchardsAmount;
        this.shrinesAmount = shrinesAmount;
        this.beaconsAmount = beaconsAmount;

        this.open = open;
        this.waitingRefugees = waitingRefugees;
    }
}
