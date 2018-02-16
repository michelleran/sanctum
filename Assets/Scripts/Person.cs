using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person {

    public static string[] notes = { "bb", "c2", "eb", "f2", "g2" };

    const int MAX_INTERVAL = 12; // temp
    const int MIN_INTERVAL = 2; // temp

    public string name = "X"; // TODO: give people names

    public int note;
    public int interval;

    bool isAlive;
    public bool IsAlive {
        get { return isAlive; }
        set {
            isAlive = value;
            if (!isAlive) { 
                Debug.Log ("about to destroy a person gameobject");
                Object.Destroy(obj); 
            }
        }
    }

    [System.NonSerialized]
    public GameObject obj;

    public Person() {
        note = Random.Range(0, notes.Length);
        interval = Random.Range(MIN_INTERVAL, MAX_INTERVAL+1);
        isAlive = true;

        obj = new GameObject("Person");
        obj.AddComponent<AudioSource>();
        obj.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/" + notes[note]);
    }
}