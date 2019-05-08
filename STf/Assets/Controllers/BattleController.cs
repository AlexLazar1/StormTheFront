using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BattleController : MonoBehaviour
{
    //public Board board;
    //public Tilemap m_level;
    public GameObject Board;
    public Sprite selected;
    public Sprite unselected;
    public Text dialog;
    private SpriteRenderer spriteRenderer;
    private Cell[,] cellsGame;
    private Hero hero1, hero2;


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
                go = cellObject,
                selected = false,
                walkable = true,
            };
            y++;
        }
        return cells;
    }

    // Start is called before the first frame update
    void Start()
    {
        cellsGame = GetAllChilds(Board);
        LoadHeroes();

    }

    private Cell FindCell(GameObject obj)
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
    private void UnselectAllCells()
    {
        foreach(Cell cell in cellsGame)
        {
            cell.selected = false;
            spriteRenderer = cell.go.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = unselected;
        }

    }

    public void LoadHeroes()
    {
        hero1 = Hero.GetPlayer(Global.Player.id);
        //hero1.printHero();
        hero2 = Global.Enemy;
        //hero2.printHero();
        dialog.text = string.Format("{0}: Prepare to die, {1}!",hero1.Name,hero2.Name);
        Invoke("ClearDialog", 3.0f);

    }
    void ClearDialog()
    {
        dialog.text = "";
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
                Cell cell = FindCell(hit.collider.gameObject);
                Debug.Log(cell.x + "-" + cell.y);
                if (cell.selected == true || cell.walkable == false)
                {
                    Debug.Log("selected");
                    return;
                }
                UnselectAllCells();
                spriteRenderer = cell.go.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = selected;
                cell.selected = true;

            }
        }
    }
}
