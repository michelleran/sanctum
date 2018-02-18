using System.Collections;
using System.Collections.Generic;

public class Script {
    // TUTORIAL
    public static string[] tutorialA = {
        "though your link to the outside is a tenuous one, you've heard of what's happened; you've heard of the blight and its grim work.",
        "maybe you can't call down lightning, smite foes, bend the wind and seas, but surely there's something you can do to help.",
        "anything."
    };

    public static string[] tutorialB = {
        "at a thought, gates form and find footing in the mist.",
        "now, if only because you have no other choice, you wait.",
        "it's not long before a stranger straggles up to the gate.",
        "all skin and bones; no, there's yet some spark, to have made it this far."
    };

    public static string[] tutorialC = {
        "the gates open.",
        "walking as if in a dream, the stranger stumbles through and only then wakes, and gazes around with eyes weary and wide.",
        "gratitude suffuses you, abruptly enough to startle",
        "but after surprise comes a strength you've sorely missed."
    };

    public static string[] tutorialD = {
        "it takes some time, but you recall the shape of dwellings, the proper comforts of a place of refuge.",
        "the air and earth together breathe one into form and the stranger enters, and rests.",
        "the gates close. they, too, tire.",
        "now, once more, you wait."
    };

    // FEATURES
    public static string[] house = {
        "a dwelling forms and waits patiently for some breath to stir its air."
    };

    public static string[] orchard = {
        "an orchard stretches up from the earth and swells with fruit."
    };

    public static string[] shrine = {
        "a shrine forms; quiet, yet determined."
    };

    public static string[] beacon = {
        "a beacon rises, its gleam piercing the night."
    };

    // EVENTS
    public static string[] gatesOpen = {
        "the gates open."
    };

    public static string[] gatesClose = {
        "the gates close."
    };

    public static string[] attack = {
        "monsters come snarling through the open gates, howling for blood.",
        "teeth and claws flash and tear and stain the ground red.",
        "the attack ends as swiftly as it started; the stench of death permeates the air."
    };

    public static string[] casualties = {
        " lives are snuffed out. the stars seem more numerous that night."
    };

    public static string[] casualty = {
        "thankfully, only one life is lost."
    };

    public static string[] arrival = {
        "a worn traveler staggers through the mist."
    };

    public static string[] arrivals = {
        " wanderers, faces pale and drawn, trudge up to the gates." // number will be appended
    };

    public static string[] death = {
        "untreated wounds take their toll; there is one less soul waiting at the gates.",
        "illness strikes and pries a soul away before it could know rest.",
        "the cold of the dead night bears another grave before the gates.",
        "a monster seizes upon an easy prey; blood stains the feet of the gates."
    };

    public static string[] peacefulDeath = {
        " quietly passes away in the night."
    };

    public static string[] request = { // name will be appended
        "", // house - blank b/c will never be requested
        " misses the sweetness of fresh fruit.", // orchard
        " thinks a shrine might help protect the sanctum.", // shrine
        " wishes for a light to guide others in need here." // beacon
    };

    // note: feature-agnostic observances will be lumped under houses
    public static Dictionary<int, string[]> observance = new Dictionary<int, string[]>
    {
        { (int)Catalog.Feature.House, new string[] { " repairs a damaged roof.", " mends clothes.", " reads a book.", " picks flowers.", " weaves a flower crown.", " takes a nap amidst the flowers." } },
        { (int)Catalog.Feature.Orchard, new string[] { " picks fruits.", " walks amongst the trees.", " dozes off beneath a tree." } }
    };
}
