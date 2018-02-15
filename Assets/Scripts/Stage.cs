using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour {

    // story

    public Text storyText;

    // top display

    public Image topDisplay;

    public Text gatesStatusText;

    public Text pointsAmountText;
    public Text pointsRateText;

    public Text populationAmountText;

    // features display

    public ScrollRect featuresDisplay;

    public GameObject housesDisplay;
    public GameObject flowersDisplay;

    public Text housesText;
    public Text flowersText;

    public Text housesAmountText;
    public Text flowersAmountText;

    // buttons

    public Button raiseGatesButton;
    public Button openGatesButton;
    public Image cooldownIndicator;

    public Button raiseHouseButton;
    public Button raiseFlowersButton;
}
