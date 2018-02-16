using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catalog : MonoBehaviour
{
    
    public enum Feature { House, Flowers }; // TODO: more

    public Stage stage;

    public GameObject[] displays;
    public Button[] buttons;
    public Text[] labels;
    public Text[] amounts;

    public int[] costs =
    {
        50, // house
        30 // flowers
    };

    public string[][] possibleMessages = {
        Script.house,
        Script.flowers
    };

    public List<int> locked;

    private void Start() {
        displays = new GameObject[]{
            stage.housesDisplay,
            stage.flowersDisplay
        };

        buttons = new Button[]{
            stage.raiseHouseButton,
            stage.raiseFlowersButton
        };

        labels = new Text[]{
            stage.housesText,
            stage.flowersText
        };

        amounts = new Text[]{
            stage.housesAmountText,
            stage.flowersAmountText
        };

        locked = new List<int>();
        locked.AddRange((int[])Enum.GetValues(typeof(Feature)));
    }
}
