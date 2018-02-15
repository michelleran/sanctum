using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storyteller : MonoBehaviour {

	// CONSTANTS

	public const float SECONDS_BETWEEN_MESSAGES = 0.5f; // short for testing purposes

	public const float GATE_OPEN_DURATION = 20f; // temp
	public const float GATE_COOLDOWN = 4f; // temp

	public const float EVENT_INTERVAL = 5f; // temp
    public const int EVENT_FACTOR = 10; // temp; larger = smaller likelihood of event
    public const int ATTACK_FACTOR = 3; // temp
    public const int DEATH_FACTOR = 5; // temp

    public const float REQUEST_INTERVAL = 10f; // temp
    public const int REQUEST_FACTOR = 3; // temp

    public const float OBSERVANCE_INTERVAL = 20f; // temp
    public const int OBSERVANCE_FACTOR = 5; // temp


	// REFERENCES

	public Sanctum sanctum;
    public Catalog catalog;

    public Image topDisplay;
    public ScrollRect featuresDisplay;

	public Text storyText;
    RectTransform storyRect;

    public Text gatesStatusText;

    public Text housesAmountText;
    public Text flowersAmountText;

	public Button raiseGatesButton;
	public Button openGatesButton;

    public Image cooldownIndicator;


	// VARIABLES

	bool isTellingStory = false;
    bool isCoolingDown = false; // this refers to gate cooldown before can open again

    int waitingRefugees = 0;

	bool open = false;
    bool Open {
        get { return open; }
        set {
            open = value;
            for (int i = 0; i < waitingRefugees; i++) { sanctum.addResident(new Person()); }
            waitingRefugees = 0;
            // TODO: display a message
        }
    }


	//--- SYSTEM FUNCTIONS ---//

	void Start () {
        storyRect = storyText.GetComponent<RectTransform>();
		beginTutorial ();
	}

	void Update () {
        if (Open) {
            cooldownIndicator.fillAmount -= Time.deltaTime / GATE_OPEN_DURATION;
        } else if (isCoolingDown) {
            cooldownIndicator.fillAmount -= Time.deltaTime / GATE_COOLDOWN;
        }
	}


    //--- TUTORIAL ---//

    void beginTutorial() {
        cooldownIndicator.fillAmount = 0f; // disable for tutorial
		StartCoroutine (displayMessages (Script.tutorialA)) ;
		StartCoroutine (toggleOption (raiseGatesButton.gameObject, true));
		raiseGatesButton.onClick.AddListener (raiseGates);
	}

	void raiseGates() {
		StartCoroutine (toggleOption (raiseGatesButton.gameObject, false));
        StartCoroutine (displayMessages (Script.tutorialB)) ;
		StartCoroutine (toggleOption (openGatesButton.gameObject, true));
		openGatesButton.onClick.AddListener (openTutorialGates); // later, will assign proper listener
	}

	void openTutorialGates() {
		StartCoroutine (toggleButton (openGatesButton, false)); // no cooldown during tutorial
        StartCoroutine (displayMessages (Script.tutorialC)) ;

        StartCoroutine( toggleOption (topDisplay.gameObject, true));

        sanctum.addResident(new Person());
        sanctum.Points = 50;

        StartCoroutine (unlockFeature((int)Catalog.Feature.House, raiseTutorialHouse, true));
	}

	void raiseTutorialHouse() {
        // circumvent normal process of adding a house
        sanctum.features[(int)Catalog.Feature.House] += 1;
        sanctum.existingFeatures.Add((int)Catalog.Feature.House);
        sanctum.Capacity += 4; // tutorial house is different from normal
        sanctum.Points -= 25;
        housesAmountText.text = "1";

        // introduce features display
        StartCoroutine (toggleOption (featuresDisplay.gameObject, true));
        StartCoroutine (toggleOption (catalog.displays[(int)Catalog.Feature.House], true));

        // finish tutorial script
        StartCoroutine (displayMessages (Script.tutorialD)) ;

        gatesStatusText.text = "closed";

        // begin gates cooldown - had previously been deferred
		StartCoroutine (cooldown ());

		// tutorial is finished now, so prepare for proper gameplay...
		openGatesButton.onClick.RemoveAllListeners ();
        catalog.buttons[(int)Catalog.Feature.House].onClick.RemoveAllListeners ();

		openGatesButton.onClick.AddListener (openGates);
        catalog.buttons[(int)Catalog.Feature.House].onClick.AddListener (raiseHouse);

        // begin creating events
		InvokeRepeating ("createEvent", EVENT_INTERVAL, EVENT_INTERVAL);
        InvokeRepeating ("createRequest", REQUEST_INTERVAL, REQUEST_INTERVAL);
        InvokeRepeating ("createObservance", OBSERVANCE_INTERVAL, OBSERVANCE_INTERVAL);
	}


	//--- GENERAL FUNCTIONS ---//

	IEnumerator displayMessages(string[] messages) {
		while (isTellingStory)       
			yield return new WaitForSeconds(0.1f);

		isTellingStory = true;
		foreach (string message in messages) {
            storyText.text = "\n" + message + "\n" + storyText.text;
            yield return new WaitForSeconds (SECONDS_BETWEEN_MESSAGES);
		}

		isTellingStory = false;
	}

    void unlockFeature(int type) {
        UnityEngine.Events.UnityAction listener;
        switch (type) {
            case (int)Catalog.Feature.House:
                listener = raiseHouse;
                StartCoroutine(unlockFeature(type, listener));
                break;

            case (int)Catalog.Feature.Flowers:
                listener = raiseFlowers;
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

        if (!isTutorial) {
            catalog.displays[type].SetActive(true);
        }
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

    bool roll(int factor) {
        return Random.Range(0, factor) == 0;
    }


    // ~ EVENTS ~ //

	void createEvent() {
        // first, decide whether to have an event or not
        if (!roll(EVENT_FACTOR))
            return;

        /* possible events: (TODO: more?)
         *  - refugees arrive
         *      - if gate is open: +population
         *      - if gate is closed: some may die
         *  - monsters attack
         *      - only if gate is open
         */

        // in order of priority...

        if (Open) {
            // monsters may or may not attack
            if (sanctum.Population > 0 && roll(ATTACK_FACTOR)) {
                Debug.Log("monsters are gonna attack!");
                // pick random people to kill
                // TODO: alternatively, just shave off the 1st/last n residents where n is a random number
                int killed = 0;
                List<Person> residents = new List<Person>(sanctum.residents);
                foreach (Person person in residents) {
                    if (person.IsAlive && roll(DEATH_FACTOR)) {
                        person.IsAlive = false; // TODO: make a killResident function?
                        sanctum.residents.Remove(person);
                        killed++;
                    }
                } // TODO: problem: no one might die...

                sanctum.Population -= killed;

                // pick random message
                string attackMessage = Script.attack[Random.Range(0, Script.attack.Length)];
                string casualtiesMessage = Script.casualties[Random.Range(0, Script.casualties.Length)];
                casualtiesMessage = casualtiesMessage.Replace("#", "" + killed); // TODO: deal w/ plurals

                StartCoroutine (displayMessages (new string[] { attackMessage, casualtiesMessage }));

                return;
            }
        }

        if (!Open && waitingRefugees > 0)
        {
            // only kill one at a time - at least, right now
            waitingRefugees -= 1;

            string message = Script.death[Random.Range(0, Script.death.Length)];
            StartCoroutine(displayMessages(new string[] { message }));

            return;
        }

        if (sanctum.Population + waitingRefugees < sanctum.Capacity) {
            int arrivals = Random.Range(1, sanctum.Capacity - sanctum.Population - waitingRefugees + 1);
            waitingRefugees += arrivals;

            string message;
            if (arrivals == 1) {
                message = Script.arrival[Random.Range(0, Script.arrival.Length)];
            } else {
                message = Script.arrivals[Random.Range(0, Script.arrivals.Length)];
            }

            StartCoroutine(displayMessages(new string[] { message }));

            if (Open) {
                for (int i = 0; i < waitingRefugees; i++) { sanctum.addResident(new Person()); }
                waitingRefugees = 0;
            }

            return;
        }

        // TODO: show different messages for arrivals while gate is open vs. while gate is closed?
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
            StartCoroutine (displayMessages (new string[] { person.name + Script.request[catalog.locked[i]] }));
            unlockFeature(catalog.locked[i]);
        }
    }


    // ~ OBSERVANCES ~ //

    void createObservance() {
        if (sanctum.Population > 0) { // TODO: are there any feature-agnostic observances?
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
            int m = Random.Range(0, Script.observance[feature].Length);
            StartCoroutine (displayMessages (new string[] { person.name + Script.observance[feature][m] }));
        }
    }


	//--- LISTENERS ---//

	void openGates() {
        cooldownIndicator.fillAmount = 1.0f;
		Open = true;
		openGatesButton.interactable = false;
        gatesStatusText.text = "open";

        StartCoroutine (displayMessages (Script.gatesOpen));
		StartCoroutine (countDownToClose ());
	}

	IEnumerator countDownToClose() {
		yield return new WaitForSeconds (GATE_OPEN_DURATION);
		Open = false;
        gatesStatusText.text = "closed";

        StartCoroutine(displayMessages (Script.gatesClose));
		StartCoroutine (cooldown ());
	}

	IEnumerator cooldown() {
        cooldownIndicator.fillAmount = 1.0f;
        isCoolingDown = true;

		yield return new WaitForSeconds (GATE_COOLDOWN);

		openGatesButton.interactable = true;
        isCoolingDown = false;
	}

	void raiseHouse() {
        string message = sanctum.addFeature ((int)Catalog.Feature.House);
        StartCoroutine (displayMessages (new string[] { message }));
	}

    void raiseFlowers() {
        string message = sanctum.addFeature((int)Catalog.Feature.Flowers);
        StartCoroutine(displayMessages(new string[] { message }));
    }
}
