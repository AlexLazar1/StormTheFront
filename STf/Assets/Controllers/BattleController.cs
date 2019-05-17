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
    public Sprite knight;
    public Text dialog;
    private SpriteRenderer spriteRenderer;
    private Cell[,] cellsGame;
    private Hero hero1, hero2;
    private bool IsSpriteSelected = false;
    private Cell LastSelectedCell = null;


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
        //LoadHeroes();
        SetTroopAtPosition(0, 1, knight);


    }

    public void SetTroopAtPosition(int x,int y,Sprite sprite,bool flipX = true)
    {
        cellsGame[x, y].troop = new GameObject("Troop", typeof(SpriteRenderer));
        SpriteRenderer troopRenderer = cellsGame[0, 1].troop.GetComponent<SpriteRenderer>();
        troopRenderer.sprite = knight;
        troopRenderer.flipX = flipX;
        cellsGame[x, y].troop.transform.parent = cellsGame[0, 1].go.transform;
        cellsGame[x, y].troop.transform.localPosition = new Vector3(0, 0, -1);
        cellsGame[x, y].troop.transform.localScale = new Vector3(0.4f, 0.4f, 1);

    }
    public void ClearTroopAtPosition(int x,int y)
    {
        cellsGame[x, y].troop = null;
        Destroy(cellsGame[x, y].go.transform.GetChild(0));
    }

    private void SwapCellTroop(Cell cell1,Cell cell2)
    {
        cell2.troop = cell1.troop;
        cell2.troop.transform.parent = cell2.go.transform;
        cell2.troop.transform.localPosition = new Vector3(0, 0, -1);
        cell1.troop = null;
        //Destroy(cell1.go.transform.GetChild(0));
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
        IsSpriteSelected = false;

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
                if (IsSpriteSelected == false)
                {
                    if(cell.troop == null)
                    {
                        return;
                    }
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
                    LastSelectedCell = cell;
                    IsSpriteSelected = true;
                } else
                {
                    if(cell.selected == true)
                    {
                        UnselectAllCells();
                        return;
                    } else if (cell.troop == null)
                    {
                        SwapCellTroop(LastSelectedCell, cell);
                        UnselectAllCells();
                    }
                }

            }
        }
    }
}
