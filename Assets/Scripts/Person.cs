using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person {

    public static string[] notes = { "bb", "c2", "eb", "f2", "g2" };

    [System.NonSerialized]
    public static string[] names = {
        "Aaron", "Aaryn", "Abel", "Aditya", "Alfonso", "Aiden", "Akan", "Alan", "Alec", "Alexei", "Alhaji", 
        "Alieu", "Allan", "Amaan", "Amir", "Amro", "Andrei", "Antoni", "Apisai", "Arann", "Archie", "Arian", 
        "Arjuna", "Armen", "Aron", "Arthur", "Arunas", "Asfhan", "Ashton", "Asif", "Athol", "Austen", "Aydan", 
        "Ayman", "Azlan", "Barath",  "Bayley", "Berkay", "Blake", "Bowen", "Brooke", "Bruin", "Bryce", "Bryn", 
        "Cade", "Caedan", "Caelen", "Caidyn", "Cairn", "Cale", "Caley", "Callan", "Calum", "Camron", "Carlo", 
        "Carson", "Casper", "Cavan", "Ceiron", "Charlie", "Cian", "Cinar", "Clarke", "Clyde", "Codi", "Coel", 
        "Cole", "Colt", "Conli", "Conlyn", "Connar", "Conrad", "Corben", "Corrie", "Curtis", "Dale", "Damien", 
        "Dane", "Dara", "Darius", "Dean", "Declan", "Devan", "Dion", "Eassan", "Edwyn", "Eidhan", "Elisau", 
        "Emil", "Enis", "Enzo", "Eonan", "Eric", "Essa", "Evann", "Famara", "Fikret", "Finnen", "Fionn", "Florin", 
        "Fyfe", "Glen", "Graeme", "Hadyn", "Hector", "Hendri", "Heyden", "Idris", "Ilyaas", "Inan", "Irvine", 
        "Isaiah", "Isaac", "Jace", "Jadyn", "Jaheim", "Jaida", "Jaime", "Jakob", "Jameil", "Johann", "Jordon", 
        "Joris", "Joseph", "Joshua", "Jude", "Kaden", "Kael", "Kain", "Kairn", "Kaleem", "Kallan", "Kalvyn", 
        "Kamran", "Kaydin", "Keane", "Keavan", "Keelan", "Keilan", "Keiron", "Kellen", "Kelvin", "Kendyn", 
        "Kenton", "Kenzie", "Kevin", "Khalan", "Khizar", "Kiefer", "Kieran", "Kiern", "Kinnon", "Kirwin", "Kjae", 
        "Kobi", "Kodi", "Kogan", "Konar", "Korben", "Korrin", "Krish", "Kyral", "Kyro", "Lauren", "Leno", "Leon", 
        "Levon", "Lewis", "Liam", "Logan", "Lorcan", "Lucas", "Luis", "Lyall", "Maciej", "Mahdi", "Malo", "Marcel", 
        "Marcos", "Mario", "Marko", "Marlon", "Mason", "Matas", "Matt", "Mayeul", "Mehraz", "Mikael", "Milo", 
        "Mirza", "Modu", "Morgan", "Musa", "Mylo", "Neco", "Nial", "Nico", "Nikash", "Niraj", "Noel", "Norrie", 
        "Oban", "Odin", "Oran", "Orran", "Oryn", "Ossian", "Owais", "Ozzy", "Paolo", "Pascoe", "Quinn", "Rafael", 
        "Raheem", "Rajan", "Ramit", "Rana", "Rasul", "Rayden", "Rayyan", "Reean", "Reese", "Reice", "Reily", "Remo", 
        "Reng", "Reuben", "Rheo", "Rhoan", "Rhuan", "Rhyan", "Rico", "Rihan", "Riley", "Roan", "Robin", "Rohaan", 
        "Rokas", "Roray", "Roshan", "Rowan", "Sabeen", "Sahaib", "Samir", "Savin", "Sethu", "Shae", "Shaun", "Shay", 
        "Shayne", "Sheigh", "Silas", "Sofia", "Sohan", "Stefan", "Struan", "Sudais", "Surien", "Tait", "Talon", 
        "Tanvir", "Tayo", "Tegan", "Temba", "Terry", "Thiago", "Titi", "Tobias", "Tomas", "Torrin", "Umair", 
        "Veeran", "Vinnie", "Wesley", "Xander", "Yahya", "Yann", "Yasir", "Yusef"
    };

    const int MAX_INTERVAL = 12; // temp
    const int MIN_INTERVAL = 2; // temp

    public string name = "X";

    public int note;
    public int interval;

    bool isAlive;
    public bool IsAlive {
        get { return isAlive; }
        set {
            isAlive = value;
            if (!isAlive)
                Object.Destroy(obj);
        }
    }

    [System.NonSerialized]
    public GameObject obj;

    public Person() {
        note = Random.Range(0, notes.Length);
        name = names[Random.Range(0, names.Length)];
        interval = Random.Range(MIN_INTERVAL, MAX_INTERVAL+1);
        isAlive = true;

        obj = new GameObject("Person");
        obj.AddComponent<AudioSource>();
        obj.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/" + notes[note]);
    }
}