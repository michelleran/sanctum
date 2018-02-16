using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class Archivist : MonoBehaviour {

    public Stage stage;
    public Sanctum sanctum;

    public void save(bool open, int waitingRefugees) {
        Record record = new Record(stage.storyText.text, sanctum.Population, sanctum.Capacity,
                                   sanctum.Points, sanctum.PointsPerPerson, sanctum.TimeForPoints,
                                   sanctum.residents.ToArray(), sanctum.existingFeatures.ToArray(),
                                   sanctum.features[(int)Catalog.Feature.House], 
                                   sanctum.features[(int)Catalog.Feature.Flowers], 
                                   open, waitingRefugees);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/record.gd");
        bf.Serialize(file, record);
        file.Close();
    }

    public bool saveExists() {
        return File.Exists(Application.persistentDataPath + "/record.gd");
    }

    public Record load() {
        Debug.Log(Application.persistentDataPath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/record.gd", FileMode.Open);
        return (Record)bf.Deserialize(file);
    }
}
