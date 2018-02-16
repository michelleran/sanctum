using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catalog : MonoBehaviour
{
    
    public enum Feature { House, Flowers, Shrine, Beacon };

    public Stage stage;

    public GameObject[] displays;
    public Button[] buttons;
    public Text[] labels;
    public Text[] amounts;

    public int[] costs = {
        50, // house
        30, // flowers
        100, // shrine
        200 // beacon
    };

    public string[][] possibleMessages = {
        Script.house,
        Script.flowers,
        Script.shrine,
        Script.beacon
    };

    public List<int> locked;

    private void Start() {
        displays = new GameObject[]{
            stage.housesDisplay,
            stage.flowersDisplay,
            stage.shrinesDisplay,
            stage.beaconsDisplay
        };

        buttons = new Button[]{
            stage.raiseHouseButton,
            stage.raiseFlowersButton,
            stage.raiseShrinesButton,
            stage.raiseBeaconsButton
        };

        labels = new Text[]{
            stage.housesText,
            stage.flowersText,
            stage.shrinesText,
            stage.beaconsText
        };

        amounts = new Text[]{
            stage.housesAmountText,
            stage.flowersAmountText,
            stage.shrinesAmountText,
            stage.beaconsAmountText
        };

        locked = new List<int>();
        locked.AddRange((int[])Enum.GetValues(typeof(Feature)));
    }
}
