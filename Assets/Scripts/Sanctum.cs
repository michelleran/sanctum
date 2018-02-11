using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sanctum : MonoBehaviour {
    
    // REFERENCES

    public Storyteller storyteller;
    public Maestro maestro;
    public Catalog catalog;

	public Text populationText;
	public Text pointsText;


	// VARIABLES

	int capacity;
	public int Capacity {
		get { return capacity; }
		set {
			capacity = value;
			populationText.text = "population: " + population + "/" + capacity;
		}
	}
		
	int population;
    public int Population {
        get { return population; }
        set {
            population = value;
            populationText.text = "population: " + population + "/" + capacity;
        }
    }

    public List<Person> residents = new List<Person>();

	int points;
	public int Points {
		get { return points; }
		set {
			points = value;
			pointsText.text = "points: " + points;

            foreach (KeyValuePair<int, int> pair in features) {
                storyteller.toggleAvailability(pair.Key, catalog.costs[pair.Key] <= points);
            }
		}
	}

	public int pointsPerPerson = 5;
	public float timeForPoints = 10f; // seconds between getting points
	float timeSinceLastPoints = 0;

    // note that when a new feature is added, numbers will be altered there - not going to use a property for this
    public Dictionary<int, int> features = new Dictionary<int, int>
    {
        {(int)Catalog.Feature.House, 0},
        {(int)Catalog.Feature.Flowers, 0}
    };

    // TODO: a display to show what features there are

	void Start () {
		Capacity = 1;
        Population = 0;
		Points = 0;
	}

	void Update () {
		timeSinceLastPoints += Time.deltaTime;
		if (timeSinceLastPoints >= timeForPoints) {
			Points += population * pointsPerPerson;
			timeSinceLastPoints = 0;
		}
	}

    public void addResident(Person person) {
        residents.Add(person);
        Population += 1;
        //maestro.conduct(person);
    }

    public string addFeature(int type) {

        features[type] += 1;

        /*Feature feature = new Feature (type); // TODO: don't bother initializing, just make all static variables instead?
        Capacity += feature.capacityEffect;
        pointsPerPerson += feature.pointsPerPersonEffect;
        timeForPoints += feature.timeForPointsEffect;
        Points -= feature.cost;*/

        Capacity += catalog.capacityEffects[type];
        pointsPerPerson += catalog.pointsPerPersonEffects[type];
        timeForPoints += catalog.timeForPointsEffects[type];
        Points -= catalog.costs[type];

        // double cost
        catalog.costs[type] *= 2;
        storyteller.toggleAvailability(type, catalog.costs[type] <= points);

        // pick a random message to show
        int m = Random.Range(0, catalog.possibleMessages[type].Length);
        return catalog.possibleMessages[type][m];
    }
}
