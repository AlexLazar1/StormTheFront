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
                ArchersNo = 3,
                SoldiersNo = 2,
                SiegeNo = 0,
                KnightsNo = 1
            };
            Global.Enemy = enemy;
            SceneManager.LoadScene("BattleScene");
        }
        if(enemyNumber == 2)
        {
            Global.Enemy = enemy;
            enemy = new Hero
            {
                Name = "Knight of Doom",
                ArchersNo = 1,
                SoldiersNo = 1,
                SiegeNo = 0,
                KnightsNo = 5
            };
            SceneManager.LoadScene("BattleScene");
        }
    }
}
