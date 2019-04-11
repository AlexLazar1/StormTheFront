using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleController : MonoBehaviour
{
    //public Board board;
    public Tilemap m_level;

    // Start is called before the first frame update
    void Start()
    {
        //board.Create();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = m_level.WorldToCell(worldPoint);

            TileBase tile = m_level.GetTile(position);
            Debug.Log(position.x+ "-" + position.y);
        }
      
    }
}
