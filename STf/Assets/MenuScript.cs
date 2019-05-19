using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public InputField playerName;
    public Text errorField;
    public void newGame()
    {
        string pName = playerName.text;
        if(pName.Length < 1)
        {
            errorField.text = "Name is required!";
            return;
        }
        if(pName.Length > 10)
        {
            errorField.text = "Name too long! (10 chars max)";
            return;
        }
        if(!Regex.IsMatch(pName, @"^[a-zA-Z]+$"))
        {
            errorField.text = "Only letters :)";
            return;
        }
        Hero player = new Hero
        {
            Name = pName,
            ArchersNo = 3,
            SoldiersNo = 10,
            KnightsNo = 2,
            SiegeNo = 5
        };
        int id = player.SaveInDb();
        Debug.Log(id);
        if(id != 0)
        {
            player.id = id;
            Global.Player = player;

            Castle playerCaste = new Castle
            {
                HeroId = id,
                ArchersNo = 5,
                SoldiersNo = 5,
                KnightsNo = 5,
            };

            int castleId = playerCaste.SaveInDb();
            if(castleId != 0)
            {
                SceneManager.LoadScene("MainSceneIG");
            }
            else
            {
                //charsh error ?
            }
        } else
        {
            //charsh
        }

    }
    public void loadGame()
    {

    }
    public void quitGame()
    {
        Debug.Log("wtf");
        Application.Quit();
    }

}
