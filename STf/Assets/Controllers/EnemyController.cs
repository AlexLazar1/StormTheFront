using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private static Hero enemy;

    public static void StartBattle(int enemyNumber)
    {
        if(enemyNumber == 1)
        {
            enemy = new Hero
            {
                Name = "DeathRiser",
                ArchersNo = 4,
                SoldiersNo = 6,
                SiegeNo = 0,
                KnightsNo = 1
            };
            Global.Enemy = enemy;
            SceneManager.LoadScene("BattleScene");
        }
        if(enemyNumber == 2)
        {
            enemy = new Hero
            {
                Name = "Knight of Doom",
                ArchersNo = 2,
                SoldiersNo = 5,
                SiegeNo = 0,
                KnightsNo = 5
            };
            Global.Enemy = enemy;
            SceneManager.LoadScene("BattleScene");
        }
    }
}
