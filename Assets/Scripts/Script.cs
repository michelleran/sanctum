using System.Collections;
using System.Collections.Generic;

public class Script {
    // TUTORIAL
    public static string[] tutorialA = {
        "you are alone. you want to help."
    };

    public static string[] tutorialB = {
        "gates rise. you wait.",
        "a stranger arrives."
    };

    public static string[] tutorialC = {
        "the gates open.",
        "the stranger enters.",
        "the stranger's gratitude fills you with strength."
    };

    public static string[] tutorialD = {
        "you raise a house.",
        "the gates close."
    };

    // FEATURES
    public static string[] house = {
        "a house rises."
    };

    public static string[] flowers = {
        "flowers rise."
    };

    // EVENTS
    public static string[] gatesOpen = {
        "the gates open."
    };

    public static string[] gatesClose = {
        "the gates close."
    };

    public static string[] attack = {
        "monsters attack."
    };

    public static string[] casualties = {
        "# people were killed."
    };

    public static string[] arrival = {
        "a refugee arrives."
    };

    public static string[] arrivals = {
        "refugees arrive."
    };

    public static string[] death = {
        "a refugee dies of wounds.",
        "a refugee dies of illness.",
        "a refugee dies of cold.",
        "a refugee is slain by a monster."
    };

    public static string[] request = { // name will be appended
        "", // house - blank b/c will never be requested
        " misses the scent of flowers." // flowers
    };

    public static Dictionary<int, string[]> observance = new Dictionary<int, string[]>
    {
        { (int)Catalog.Feature.House, new string[] { " repairs a damaged roof.", " mends clothes.", " reads a book." } },
        { (int)Catalog.Feature.Flowers, new string[] { " picks flowers.", " weaves a flower crown.", " takes a nap amidst the flowers." } }
    };
}