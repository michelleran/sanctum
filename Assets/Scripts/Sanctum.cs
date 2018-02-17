using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sanctum : MonoBehaviour {

    // REFERENCES

    Stage stage;
    Storyteller storyteller;
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

            foreach (int type in unlockedFeatures) {
                // if NOT a shrine & at shrine limit, and NOT a beacon & at beacon limit...
                if (!(type == (int)Catalog.Feature.Shrine && storyteller.ATTACK_FACTOR >= Storyteller.MAX_ATTACK_FACTOR) &&
                    !(type == (int)Catalog.Feature.Beacon && storyteller.EVENT_FACTOR <= Storyteller.MIN_EVENT_FACTOR)) {

                    storyteller.toggleAvailability(type, catalog.costs[type] <= points);

                }
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

    public Dictionary<int, int> features = new Dictionary<int, int> {
        {(int)Catalog.Feature.House, 0},
        {(int)Catalog.Feature.Orchard, 0},
        {(int)Catalog.Feature.Shrine, 0},
        {(int)Catalog.Feature.Beacon, 0}
    };

    public List<int> unlockedFeatures;
    public List<int> existingFeatures;

	void Start () {
        stage = this.gameObject.GetComponent<Stage>();
        storyteller = this.gameObject.GetComponent<Storyteller>();
        catalog = this.gameObject.GetComponent<Catalog>();

        existingFeatures = new List<int>();
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
    }

    public string killResident() {
        // pick a random person to kill
        Person person = residents[Random.Range(0, residents.Count)];
        person.IsAlive = false;
        residents.Remove(person);
        Population--;
        return person.name;
    }

    public void addFeature(int type) {

        Debug.Log("in addfeature");

        if (features[type] == 0)
            existingFeatures.Add(type);

        features[type] += 1;

        switch (type) {
            case (int)Catalog.Feature.House:
                Capacity += 5;
                break;
            case (int)Catalog.Feature.Orchard:
                PointsPerPerson += 3;
                break;
            case (int)Catalog.Feature.Shrine:
                storyteller.ATTACK_FACTOR++;
                break;
            case (int)Catalog.Feature.Beacon:
                storyteller.EVENT_FACTOR--;
                break;
        }

        Debug.Log("points: " + Points + "; cost: " + catalog.costs[type]);

        Points -= catalog.costs[type];

        Debug.Log("now points are: " + points);

        // double cost
        catalog.costs[type] *= 2;
        storyteller.toggleAvailability(type, catalog.costs[type] <= points);

        // update features display
        catalog.amounts[type].text = "" + features[type];
    }
}
