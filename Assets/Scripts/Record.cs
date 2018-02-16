using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Record {

    // TODO: probably record # waiting refugees, too?

    public string story;

    public int population;
    public int capacity;

    public int points;
    public int pointsPerPerson;
    public float timeForPoints;

    public Person[] residents;

    public int[] costs;

    public int[] existingFeatures;

    public int housesAmount;
    public int flowersAmount;
    // TODO: more

    public bool open;
    public int waitingRefugees;

    public Record(string story, int population, int capacity, 
                  int points, int pointsPerPerson, float timeForPoints, 
                  Person[] residents, 
                  int[] costs, int[] existingFeatures, 
                  int housesAmount, int flowersAmount, 
                  bool open, int waitingRefugees) {

        this.story = story;

        this.population = population;
        this.capacity = capacity;

        this.points = points;
        this.pointsPerPerson = pointsPerPerson;
        this.timeForPoints = timeForPoints;

        this.residents = residents;

        this.costs = costs;

        this.existingFeatures = existingFeatures;

        this.housesAmount = housesAmount;
        this.flowersAmount = flowersAmount;

        this.open = open;
        this.waitingRefugees = waitingRefugees;
    }
}
