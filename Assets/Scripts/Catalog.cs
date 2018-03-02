using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catalog : MonoBehaviour
{
    
    public enum Feature { House, Orchard, Shrine, Beacon };

    public Stage stage;

    public GameObject[] displays;
    public Button[] buttons;
    public Text[] labels;
    public Text[] amounts;

    public int[] costs = {
        50, // house
        30, // orchard
        100, // shrine
        200 // beacon
    };

    public string[][] possibleMessages = {
        Script.house,
        Script.orchard,
        Script.shrine,
        Script.beacon
    };

    public List<int> locked;

    private void Start() {
        displays = new GameObject[]{
            stage.housesDisplay,
            stage.orchardsDisplay,
            stage.shrinesDisplay,
            stage.beaconsDisplay
        };

        buttons = new Button[]{
            stage.raiseHouseButton,
            stage.raiseOrchardButton,
            stage.raiseShrinesButton,
            stage.raiseBeaconButton
        };

        labels = new Text[]{
            stage.housesText,
            stage.orchardsText,
            stage.shrinesText,
            stage.beaconsText
        };

        amounts = new Text[]{
            stage.housesAmountText,
            stage.orchardsAmountText,
            stage.shrinesAmountText,
            stage.beaconsAmountText
        };

        locked = new List<int>();
        //locked.AddRange((int[])Enum.GetValues(typeof(Feature)));
        locked.Add((int)Feature.House);
        locked.Add((int)Feature.Orchard);
        locked.Add((int)Feature.Shrine);
        locked.Add((int)Feature.Beacon);
    }
}
