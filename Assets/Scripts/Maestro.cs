using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : MonoBehaviour {
    public Sanctum sanctum;

	void Start () {
        StartCoroutine(conduct());
	}

    public IEnumerator conduct() {
        while (true) {
            if (sanctum.residents.Count == 0)
                yield return new WaitForSeconds(1f);

            List<Person> residents = new List<Person>(sanctum.residents);

            int longest = 0;
            foreach (Person person in residents) {
                if (person.interval > longest) {
                    longest = person.interval;
                }

                StartCoroutine(play(person));
            }

            yield return new WaitForSeconds(longest);
        }
    }

    IEnumerator play(Person person) {
        yield return new WaitForSeconds(person.interval);

        if (person.IsAlive && person.obj != null)
        {
            person.obj.GetComponent<AudioSource>().Play();
            Debug.Log("playing a note lala: " + person.note + " @ " + person.interval);
        }
    }
}
