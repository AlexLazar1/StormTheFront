using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BattleController : MonoBehaviour
{
    //public Board board;
    //public Tilemap m_level;
    public GameObject Board;
    private Cell[,] cellsGame;


    public static Cell[,] GetAllChilds(GameObject Go)
    {
        Cell[,] cells; 
        cells = new Cell[8,4];
        int x = 0;
        int y = 0;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            GameObject cellObject = Go.transform.GetChild(i).gameObject;
            list.Add(Go.transform.GetChild(i).gameObject);
            if(y%4 == 0 && y != 0)
            {
                x++;
                y = 0;
            }
            cells[x, y] = new Cell
            {
                x = x,
                y = y,
                go = cellObject
            };
            y++;
            /*cells[x,y].go.AddComponent(typeof(EventTrigger));
            EventTrigger trigger = cells[x,y].go.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => {
                Debug.Log("test");
            });
            trigger.triggers.Add(entry);*/
        }
        return cells;
    }

    // Start is called before the first frame update
    void Start()
    {
        cellsGame = GetAllChilds(Board);

    }

    private Cell findCell(GameObject obj)
    {
        foreach (Cell cell in cellsGame)
        { 
            if (cell.go.Equals(obj))
            {
                return cell;
            }
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Cell cell = findCell(hit.collider.gameObject);
                Debug.Log(cell.x + "-" + cell.y);

            }
        }
    }
}
