using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadGameScript : MonoBehaviour
{
    public Button Load;
    public Dropdown playerDropdown;


    // Start is called before the first frame update
    void Start()
    {
        playerDropdown.AddOptions(Hero.GetAllHeroNames());
        Load.onClick.AddListener(() =>
        {
            string name = playerDropdown.options[playerDropdown.value].text;
            Hero player = Hero.GetByName(name);
            if(player != null)
            {
                Global.Player = player;
                SceneManager.LoadScene("MainSceneIG");
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
