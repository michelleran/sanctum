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
    public RectTransform featuresRect;

    public GameObject housesDisplay;
    public GameObject orchardsDisplay;
    public GameObject shrinesDisplay;
    public GameObject beaconsDisplay;

    public Text housesText;
    public Text orchardsText;
    public Text shrinesText;
    public Text beaconsText;

    public Text housesAmountText;
    public Text orchardsAmountText;
    public Text shrinesAmountText;
    public Text beaconsAmountText;

    // buttons

    public Button raiseGatesButton;
    public Button openGatesButton;
    public Image cooldownIndicator;

    public Button raiseHouseButton;
    public Button raiseOrchardButton;
    public Button raiseShrinesButton;
    public Button raiseBeaconButton;

    public Button restartButton;
}
