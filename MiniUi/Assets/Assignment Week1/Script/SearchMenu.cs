using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchMenu : MonoBehaviour
{

    public string searchName;

    public bool isFind;

    public GameObject inputField;
    public GameObject textDisplay;

    public void listName()
    {
        searchName = inputField.GetComponent<Text>().text;

        if(isFind)
        {

            if (searchName == "Game")
            {
                textDisplay.GetComponent<Text>().text = "[ <color=green>Game</color> ] " + "is found.";
            }

            else
            {
                textDisplay.GetComponent<Text>().text = "[ <color=red>unknown</color> ] " + "is not found.";
            }

            if (searchName == "Test")
            {
                textDisplay.GetComponent<Text>().text = "[ <color=green>Test</color> ] " + "is found.";
            }

            if(searchName == "Roblox")
            {
                textDisplay.GetComponent<Text>().text = "[ <color=green>Roblox</color> ] " + "is found.";
            }

            if(searchName == "Unity")
            {
                textDisplay.GetComponent<Text>().text = "[ <color=green>Unity</color> ] " + "is found.";
            }

            if (searchName == "Pornhub")
            {
                textDisplay.GetComponent<Text>().text = "[ <color=green>Pornhub</color> ] " + "Error.org";
            }

        }

    }

}
