﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catalog : MonoBehaviour
{
    
    public enum Feature { House, Flowers }; // TODO: more

    public Button raiseHouseButton;
    public Button raiseFlowersButton;

    public Text housesText;
    public Text flowersText;

    public Text housesAmountText;
    public Text flowersAmountText;

    public Button[] buttons;
    public Text[] labels;
    public Text[] amounts;

    public int[] costs =
    {
        50, // house
        30 // flowers
    };

    public int[] capacityEffects = {
        5, // house
        0 // flowers
    };

    public int[] pointsPerPersonEffects = {
        0, // house
        3 // flowers
    };

    public int[] timeForPointsEffects = {
        0, // house
        0 // flowers
    };

    public string[][] possibleMessages = {
        Script.house,
        Script.flowers
    };

    public List<int> locked;

    private void Start() {
        buttons = new Button[]{
            raiseHouseButton,
            raiseFlowersButton
        };

        labels = new Text[]{
            housesText,
            flowersText
        };

        amounts = new Text[]{
            housesAmountText,
            flowersAmountText
        };

        locked = new List<int>();
        locked.AddRange((int[])Enum.GetValues(typeof(Feature)));
    }
}
