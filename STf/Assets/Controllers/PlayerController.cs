using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {

            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime,0f,0f));
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f,Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.Equals("castle1_collide"))
        {
            Debug.Log("Castle1");
        }
        if (collision.gameObject.name.Equals("enemy1"))
        {
            EnemyController.StartBattle(1);
        } else if(collision.gameObject.name.Equals("enemy2"))
        {
            EnemyController.StartBattle(2);
        }
    }
}
