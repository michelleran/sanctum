using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sanctum : MonoBehaviour {

    // REFERENCES

    Stage stage;
    Storyteller storyteller;
    Maestro maestro;
    Catalog catalog;

    /*public Text pointsAmountText;
    public Text pointsRateText;
    public Text populationAmountText;*/


	// VARIABLES

	int capacity;
	public int Capacity {
		get { return capacity; }
		set {
			capacity = value;
			stage.populationAmountText.text = population + "/" + capacity;
		}
	}
		
	int population;
    public int Population {
        get { return population; }
        set {
            population = value;
            stage.populationAmountText.text = population + "/" + capacity;
            stage.pointsRateText.text = "+ " + (pointsPerPerson * population) + "/" + timeForPoints + " sec";
        }
    }

    public List<Person> residents = new List<Person>();

	int points;
	public int Points {
		get { return points; }
		set {
			points = value;
			stage.pointsAmountText.text = "" + points;

            foreach (KeyValuePair<int, int> pair in features) {
                storyteller.toggleAvailability(pair.Key, catalog.costs[pair.Key] <= points);
            }
		}
	}

	int pointsPerPerson = 5;
    public int PointsPerPerson {
        get { return pointsPerPerson; }
        set {
            pointsPerPerson = value;
            stage.pointsRateText.text = "+ " + (pointsPerPerson * population) + "/" + timeForPoints + " sec";
        }
    }

	float timeForPoints = 10f; // seconds between getting points
    public float TimeForPoints {
        get { return timeForPoints; }
        set {
            timeForPoints = value;
            stage.pointsRateText.text = "+ " + (pointsPerPerson * population) + "/" + timeForPoints + " sec";
        }
    }

	float timeSinceLastPoints = 0;

    public Dictionary<int, int> features = new Dictionary<int, int>();

    public List<int> existingFeatures;

	void Start () {
        stage = this.gameObject.GetComponent<Stage>();
        storyteller = this.gameObject.GetComponent<Storyteller>();
        maestro = this.gameObject.GetComponent<Maestro>();
        catalog = this.gameObject.GetComponent<Catalog>();

        existingFeatures = new List<int>();
		Capacity = 1;
        Population = 0;
		Points = 0;

        features = new Dictionary<int, int> {
            {(int)Catalog.Feature.House, 0},
            {(int)Catalog.Feature.Flowers, 0}
        };
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
    }

    public string addFeature(int type) {

        if (features[type] == 0)
            existingFeatures.Add(type);

        features[type] += 1;

        Capacity += catalog.capacityEffects[type];
        PointsPerPerson += catalog.pointsPerPersonEffects[type];
        TimeForPoints += catalog.timeForPointsEffects[type];
        Points -= catalog.costs[type];

        // double cost
        catalog.costs[type] *= 2;
        storyteller.toggleAvailability(type, catalog.costs[type] <= points);

        // update features display
        catalog.amounts[type].text = "" + features[type];

        // pick a random message to show
        int m = Random.Range(0, catalog.possibleMessages[type].Length);
        return catalog.possibleMessages[type][m];
    }
}
