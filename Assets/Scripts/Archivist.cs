using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Archivist : MonoBehaviour {

    public Stage stage;
    public Sanctum sanctum;
    public Catalog catalog;

    public void save(bool open, int waitingRefugees) {
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.IPhonePlayer)
            return;

        Record record = new Record(stage.storyText.text, sanctum.Population, sanctum.Capacity,
                                   sanctum.Points, sanctum.PointsPerPerson, sanctum.TimeForPoints,
                                   sanctum.residents.ToArray(), catalog.costs, 
                                   sanctum.existingFeatures.ToArray(), sanctum.unlockedFeatures.ToArray(),
                                   sanctum.features[(int)Catalog.Feature.House], 
                                   sanctum.features[(int)Catalog.Feature.Orchard],
                                   sanctum.features[(int)Catalog.Feature.Shrine],
                                   sanctum.features[(int)Catalog.Feature.Beacon],
                                   open, waitingRefugees);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/record.gd");
        bf.Serialize(file, record);
        file.Close();
    }

    public bool saveExists() {
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.IPhonePlayer)
            return false;

        return File.Exists(Application.persistentDataPath + "/record.gd");
    }

    public Record load() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/record.gd", FileMode.Open);
        Record record = (Record)bf.Deserialize(file);
        file.Close();
        return record;
    }

    public void restart() {
        if (saveExists())
            File.Move(Application.persistentDataPath + "/record.gd", Application.persistentDataPath + "/old.gd"); // TODO: if there's already an old.gd, will throw error

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
