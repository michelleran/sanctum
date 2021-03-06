﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storyteller : MonoBehaviour {

	// CONSTANTS

	public const float SECONDS_BETWEEN_MESSAGES = 4.0f;

	public const float GATE_OPEN_DURATION = 20f;
	public const float GATE_COOLDOWN = 15f;

	public const float EVENT_INTERVAL = 5f;

    public const int MIN_EVENT_FACTOR = 1;
    int _EVENT_FACTOR = 6;
    public int EVENT_FACTOR {
        get { return _EVENT_FACTOR; }
        set {
            _EVENT_FACTOR = value;
            if (_EVENT_FACTOR <= MIN_EVENT_FACTOR) // min event factor
                stage.raiseBeaconButton.interactable = false;
        }
    }

    public const int MAX_ATTACK_FACTOR = 7;
    int _ATTACK_FACTOR = 2;
    public int ATTACK_FACTOR {
        get { return _ATTACK_FACTOR; }
        set {
            _ATTACK_FACTOR = value;
            if (_ATTACK_FACTOR >= MAX_ATTACK_FACTOR)
                stage.raiseShrinesButton.interactable = false;
        }
    }

    public const int DEATH_FACTOR = 5;

    public const float REQUEST_INTERVAL = 40f;
    public int REQUEST_FACTOR = 1; // larger over time

    public const float OBSERVANCE_INTERVAL = 60f;
    public const int OBSERVANCE_FACTOR = 3;


    // REFERENCES

    public Archivist archivist;
	public Sanctum sanctum;
    public Stage stage;
    public Catalog catalog;


    // VARIABLES

    bool didFinishTutorial = false;
	bool isTellingStory = false;
    bool isCoolingDown = false; // this refers to gate cooldown before can open again

    int waitingRefugees = 0;

	bool open = false;
    bool Open {
        get { return open; }
        set {
            open = value;

            if (open) {
                for (int i = 0; i < waitingRefugees; i++) { sanctum.addResident(new Person()); }
                waitingRefugees = 0;
                stage.gatesStatusText.text = "open";
            } else {
                stage.gatesStatusText.text = "closed";
            }
        }
    }


	//--- SYSTEM FUNCTIONS ---//

	void Start () {
        if (archivist.saveExists()) {
            didFinishTutorial = true;
            StartCoroutine (restore (archivist.load ()));
        } else {
            beginTutorial();
        }

        stage.restartButton.onClick.AddListener(archivist.restart);
	}

	void Update () {
        if (Open) {
            stage.cooldownIndicator.fillAmount -= Time.deltaTime / GATE_OPEN_DURATION;
        } else if (isCoolingDown) {
            stage.cooldownIndicator.fillAmount -= Time.deltaTime / GATE_COOLDOWN;
        }
	}

    private void OnApplicationPause(bool pause) {
        if (didFinishTutorial)
            archivist.save(open, waitingRefugees);
    }

    private void OnApplicationQuit() {
        if (didFinishTutorial)
            archivist.save(open, waitingRefugees);
    }


    //--- TUTORIAL ---//

    void beginTutorial() {
        stage.cooldownIndicator.fillAmount = 0f; // disable for tutorial
		StartCoroutine (displayMessages (Script.tutorialA)) ;
		StartCoroutine (toggleOption (stage.raiseGatesButton.gameObject, true));
		stage.raiseGatesButton.onClick.AddListener (raiseGates);
	}

	void raiseGates() {
		StartCoroutine (toggleOption (stage.raiseGatesButton.gameObject, false));
        StartCoroutine (displayMessages (Script.tutorialB)) ;
		StartCoroutine (toggleOption (stage.openGatesButton.gameObject, true));
		stage.openGatesButton.onClick.AddListener (openTutorialGates); // later, will assign proper listener
	}

	void openTutorialGates() {
		StartCoroutine (toggleButton (stage.openGatesButton, false)); // no cooldown during tutorial
        StartCoroutine (displayMessages (Script.tutorialC)) ;

        StartCoroutine( toggleOption (stage.topDisplay.gameObject, true));

        sanctum.addResident(new Person());
        sanctum.Points = 50;

        StartCoroutine (unlockFeature((int)Catalog.Feature.House, raiseTutorialHouse, true));
	}

	void raiseTutorialHouse() {
        // circumvent normal process of adding a house
        sanctum.features[(int)Catalog.Feature.House] += 1;
        sanctum.unlockedFeatures.Add((int)Catalog.Feature.House);
        sanctum.existingFeatures.Add((int)Catalog.Feature.House);
        sanctum.Capacity += 4; // tutorial house is different from normal
        sanctum.Points -= 25;
        stage.housesAmountText.text = "1";

        // introduce features display
        StartCoroutine (toggleOption (stage.featuresDisplay.gameObject, true));
        StartCoroutine (toggleOption (catalog.displays[(int)Catalog.Feature.House], true));

        // finish tutorial script
        StartCoroutine (displayMessages (Script.tutorialD)) ;

        stage.gatesStatusText.text = "closed";

        // begin gates cooldown - had previously been deferred
		StartCoroutine (cooldown ());

		// tutorial is finished now, so prepare for proper gameplay...
		stage.openGatesButton.onClick.RemoveAllListeners ();
        catalog.buttons[(int)Catalog.Feature.House].onClick.RemoveAllListeners ();

		stage.openGatesButton.onClick.AddListener (openGates);
        catalog.buttons[(int)Catalog.Feature.House].onClick.AddListener (raiseHouse);

        // begin creating events
		InvokeRepeating ("createEvent", EVENT_INTERVAL, EVENT_INTERVAL);
        InvokeRepeating ("createRequest", REQUEST_INTERVAL, REQUEST_INTERVAL);
        InvokeRepeating ("createObservance", OBSERVANCE_INTERVAL, OBSERVANCE_INTERVAL);

        StartCoroutine (save ());
	}


	//--- GENERAL FUNCTIONS ---//

	IEnumerator displayMessages(params string[] messages) {
		while (isTellingStory)       
			yield return new WaitForSeconds(0.1f);

		isTellingStory = true;
        for (int i = 0; i < messages.Length; i++)
        {
            string text = stage.storyText.text;
            if (text.Length > 3000) {
                var index = text.LastIndexOf(System.Environment.NewLine, System.StringComparison.CurrentCulture);
                text = text.Substring(0, index);
            }

            stage.storyText.text = "\n" + messages[i] + "\n" + text;

            if (i < messages.Length - 1) // if there are any messages left
                yield return new WaitForSeconds(SECONDS_BETWEEN_MESSAGES);
            else
                yield return new WaitForSeconds(SECONDS_BETWEEN_MESSAGES/2);
		}

		isTellingStory = false;
	}

    string pickRandomMessage(string[] messages) {
        return messages[Random.Range(0, messages.Length)];
    }

    void unlockFeature(int type) {
        UnityEngine.Events.UnityAction listener;
        switch (type) {
            case (int)Catalog.Feature.House:
                listener = raiseHouse;
                StartCoroutine(unlockFeature(type, listener));
                break;

            case (int)Catalog.Feature.Orchard:
                listener = raiseOrchard;
                StartCoroutine(unlockFeature(type, listener));
                break;

            case (int)Catalog.Feature.Shrine:
                listener = raiseShrine;
                StartCoroutine(unlockFeature(type, listener));
                break;

            case (int)Catalog.Feature.Beacon:
                listener = raiseBeacon;
                StartCoroutine(unlockFeature(type, listener));
                break;
        }
    }

    IEnumerator unlockFeature(int type, UnityEngine.Events.UnityAction listener, bool isTutorial=false) {
        while (isTellingStory)
            yield return new WaitForSeconds(0.5f);

        catalog.locked.Remove(type);
        catalog.buttons[type].gameObject.SetActive(true);
        catalog.buttons[type].onClick.AddListener(listener);
        catalog.buttons[type].interactable = sanctum.Points >= catalog.costs[type];

        if (!isTutorial) {
            catalog.displays[type].SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(stage.featuresRect);
        }

        sanctum.unlockedFeatures.Add(type);
    }

	IEnumerator toggleOption(GameObject option, bool active) {
		while (isTellingStory)       
			yield return new WaitForSeconds(0.5f);

		option.SetActive (active);
	}

	IEnumerator toggleButton(Button button, bool interactable) {
		while (isTellingStory)       
			yield return new WaitForSeconds(0.5f);

		button.interactable = interactable;
	}

    public void toggleAvailability(int type, bool available) {
        catalog.buttons[type].interactable = available;
    }

    IEnumerator save() {
        while (isTellingStory)
            yield return new WaitForSeconds(0.1f);

        didFinishTutorial = true;

        archivist.save(open, waitingRefugees);
    }

    IEnumerator restore(Record record) {
        // if catalog's arrays haven't yet been initialized, wait
        while (catalog.locked.Count == 0 || catalog.buttons.Length == 0)
            yield return new WaitForSeconds(0.1f);

        // activate displays
        StartCoroutine (toggleOption (stage.topDisplay.gameObject, true));
        StartCoroutine (toggleOption (stage.featuresDisplay.gameObject, true));

        // activate feature-associated UI elements
        // don't need to modify sanctum.unlockedFeatures - handled in unlockFeature
        foreach (int type in record.unlockedFeatures)
            unlockFeature(type);

        // restore story
        stage.storyText.text = record.story;

        // restore waiting refugees
        waitingRefugees = record.waitingRefugees;

        // activate gates button & restore gates status
        Open = record.open;
        StartCoroutine(toggleOption(stage.openGatesButton.gameObject, true));
        if (Open)
            StartCoroutine(countDownToClose());
        else
            StartCoroutine(cooldown());

        // load in data
        sanctum.Population = record.population;
        sanctum.Capacity = record.capacity;

        sanctum.Points = record.points;
        sanctum.PointsPerPerson = record.pointsPerPerson;
        sanctum.TimeForPoints = record.timeForPoints;

        List<Person> residents = new List<Person>(record.residents);
        sanctum.residents = residents;
        restoreResidents(residents);

        catalog.costs = record.costs;

        sanctum.existingFeatures = new List<int>(record.existingFeatures);

        sanctum.features[(int)Catalog.Feature.House] = record.housesAmount;
        sanctum.features[(int)Catalog.Feature.Orchard] = record.orchardsAmount;
        sanctum.features[(int)Catalog.Feature.Shrine] = record.shrinesAmount;
        sanctum.features[(int)Catalog.Feature.Beacon] = record.beaconsAmount;

        // modify factors - this should toggle availability of buttons too
        ATTACK_FACTOR += record.shrinesAmount;
        EVENT_FACTOR -= record.beaconsAmount;

        // have to manually update displays
        stage.housesAmountText.text = "" + record.housesAmount;
        stage.orchardsAmountText.text = "" + record.orchardsAmount;
        stage.shrinesAmountText.text = "" + record.shrinesAmount;
        stage.beaconsAmountText.text = "" + record.shrinesAmount;

        // now resume normal gameplay
        stage.openGatesButton.interactable = false;
        stage.openGatesButton.onClick.AddListener(openGates);

        InvokeRepeating ("createEvent", EVENT_INTERVAL, EVENT_INTERVAL);
        InvokeRepeating ("createRequest", REQUEST_INTERVAL, REQUEST_INTERVAL);
        InvokeRepeating ("createObservance", OBSERVANCE_INTERVAL, OBSERVANCE_INTERVAL);
    }

    void restoreResidents(List<Person> residents) {
        for (int i = 0; i < residents.Count; i++) {
            GameObject obj = new GameObject("Person");
            obj.AddComponent<AudioSource>();
            obj.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/" + Person.notes[residents[i].note]);
            residents[i].obj = obj;
        }
    }

    bool roll(int factor) {
        return Random.Range(0, factor) == 0;
    }


    // ~ EVENTS ~ //

	void createEvent() {
        // first, decide whether to have an event or not
        if (!roll(EVENT_FACTOR))
            return;

        /* possible events:
         *  - refugees arrive
         *      - if gate is open: +population
         *      - if gate is closed: some may die
         *  - monsters attack
         *      - only if gate is open
         *  - a resident peacefully passes away
         */

        // in order of priority...

        if (Open && sanctum.Population > 1 && roll(ATTACK_FACTOR)) {
            // pick random # of people to kill
            int killed = Random.Range(1, sanctum.Population / 2);
            for (int i = 0; i < killed; i++)
                sanctum.killResident();

            string message = killed + pickRandomMessage(Script.casualties);
            if (killed == 1)
                message = pickRandomMessage(Script.casualty);

            StartCoroutine (displayMessages (new string[] { pickRandomMessage(Script.attack), message }));

            return;

        } else if (sanctum.Population > 1 && roll(DEATH_FACTOR)) {
            StartCoroutine(displayMessages(sanctum.killResident() + pickRandomMessage(Script.peacefulDeath)));
            return;
        }

        if (!Open && waitingRefugees > 0) {
            // only kill one at a time - at least, right now
            waitingRefugees -= 1;
            StartCoroutine(displayMessages(pickRandomMessage(Script.death)));
            return;
        }

        if (sanctum.Population + waitingRefugees < sanctum.Capacity) {
            int arrivals = Random.Range(1, (sanctum.Capacity - sanctum.Population - waitingRefugees) / 2);
            waitingRefugees += arrivals;

            string message;
            if (arrivals == 1)
                message = pickRandomMessage(Script.arrival);
            else
                message = arrivals + pickRandomMessage(Script.arrivals);

            StartCoroutine(displayMessages(message));

            if (Open) {
                for (int i = 0; i < waitingRefugees; i++) { sanctum.addResident(new Person()); }
                waitingRefugees = 0;
            }

            return;
        }
	}


    // ~ REQUESTS ~ //

    void createRequest() {
        if (sanctum.Population > 0 && catalog.locked.Count > 0) {
            // request or not?
            if (!roll(REQUEST_FACTOR))
                return;

            // pick someone to serve as requester
            Person person = sanctum.residents[Random.Range(0, sanctum.residents.Count)];

            // request a locked feature, thereby unlocking it
            int i = Random.Range(0, catalog.locked.Count);
            StartCoroutine (displayMessages (person.name + Script.request[catalog.locked[i]]));
            unlockFeature(catalog.locked[i]);

            REQUEST_FACTOR++;
        }
    }


    // ~ OBSERVANCES ~ //

    void createObservance() {
        if (sanctum.Population > 0) {
            // observance or not?
            if (!roll(OBSERVANCE_FACTOR))
                return;

            // pick a person
            int i = Random.Range(0, sanctum.residents.Count);
            Person person = sanctum.residents[i];

            // pick a (present) feature for the observance to revolve around
            int f = Random.Range(0, sanctum.existingFeatures.Count);
            int feature = sanctum.existingFeatures[f];

            // pick a message & append name
            StartCoroutine (displayMessages (person.name + pickRandomMessage(Script.observance[feature])));
        }
    }


	//--- LISTENERS ---//

	void openGates() {
        stage.cooldownIndicator.fillAmount = 1.0f;
		Open = true;
		stage.openGatesButton.interactable = false;

        StartCoroutine (displayMessages (Script.gatesOpen));
		StartCoroutine (countDownToClose ());
	}

	IEnumerator countDownToClose() {
		yield return new WaitForSeconds (GATE_OPEN_DURATION);
		Open = false;

        StartCoroutine(displayMessages (Script.gatesClose));
		StartCoroutine (cooldown ());
	}

	IEnumerator cooldown() {
        stage.cooldownIndicator.fillAmount = 1.0f;
        isCoolingDown = true;

		yield return new WaitForSeconds (GATE_COOLDOWN);

		stage.openGatesButton.interactable = true;
        isCoolingDown = false;
	}

	void raiseHouse() {
        // as a failsafe...
        if (sanctum.Points < catalog.costs[(int)Catalog.Feature.House])
            return;

        sanctum.addFeature ((int)Catalog.Feature.House);
        StartCoroutine (displayMessages (pickRandomMessage(Script.house)));
	}

    void raiseOrchard() {
        if (sanctum.Points < catalog.costs[(int)Catalog.Feature.Orchard])
            return;

        sanctum.addFeature ((int)Catalog.Feature.Orchard);
        StartCoroutine (displayMessages (pickRandomMessage(Script.orchard)));
    }

    void raiseShrine() {
        if (sanctum.Points < catalog.costs[(int)Catalog.Feature.Shrine])
            return;

        sanctum.addFeature ((int)Catalog.Feature.Shrine);
        StartCoroutine (displayMessages (pickRandomMessage(Script.shrine)));
    }

    void raiseBeacon() {
        if (sanctum.Points < catalog.costs[(int)Catalog.Feature.Beacon])
            return;

        sanctum.addFeature ((int)Catalog.Feature.Beacon);
        StartCoroutine (displayMessages (pickRandomMessage(Script.beacon)));
    }
}
