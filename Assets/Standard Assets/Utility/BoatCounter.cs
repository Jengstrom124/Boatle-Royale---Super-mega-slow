using System;
using UnityEngine;
using UnityEngine.UI;

public class BoatCounter : MonoBehaviour
{
    public static BoatCounter instance;
    int boatCount;
    Text boatText;

    private void Awake()
    {
        instance = this;
        boatText = GetComponent<Text>();
    }

    public void UpdateBoatCount(int amount)
    {
        boatCount += amount;

        boatText.text = "Boats: " + boatCount;
    }
}
