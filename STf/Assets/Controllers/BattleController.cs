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
    public Sprite soldierSprite;
    public Sprite knightSprite;
    public Sprite archerSprite;
    public Text dialog;
    private SpriteRenderer spriteRenderer;
    private Cell[,] cellsGame;
    private Hero hero1, hero2;
    private bool IsSpriteSelected = false;
    private Cell LastSelectedCell = null;
    private Troop Archer = new Troop("Archer", 14, 2, 1);
    private Troop Knight = new Troop("Knight", 25, 3, 3);
    private Troop Soldier = new Troop("Soldier", 30, 2, 1);
    private List<Cell> movable = new List<Cell>();



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

    public void SetWalkable()
    {
        cellsGame[3, 0].walkable = false;
        cellsGame[3, 1].walkable = false;
        cellsGame[4, 1].walkable = false;
        cellsGame[4, 0].walkable = false;
        cellsGame[6, 3].walkable = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        cellsGame = GetAllChilds(Board);
        SetWalkable();
        LoadHeroes();
        if(hero1.ArchersNo > 0)
        {
            SetTroopAtPosition(0, 0, archerSprite,"Archer");

        }
        if (hero1.KnightsNo > 0)
        {
            SetTroopAtPosition(0, 1, knightSprite,"Knight");

        }
        if (hero1.SoldiersNo > 0)
        {
            SetTroopAtPosition(0, 2, soldierSprite,"Soldier");
        }

        if (hero2.ArchersNo > 0)
        {
            SetTroopAtPosition(7, 1, archerSprite,"Archer",false,false);

        }
        if (hero2.KnightsNo > 0)
        {
            SetTroopAtPosition(7, 2, knightSprite,"Knight",false,false);

        }
        if (hero2.SoldiersNo > 0)
        {
            SetTroopAtPosition(7, 3, soldierSprite,"Soldier",false,false);

        }

        //SetTroopAtPosition(0, 1, knight);



    }

    public void SetTroopAtPosition(int x,int y,Sprite sprite,string troopName,bool flipX = true,bool playerTroop = true)
    {
        cellsGame[x, y].troop = new GameObject("Troop", typeof(SpriteRenderer));
        SpriteRenderer troopRenderer = cellsGame[x, y].troop.GetComponent<SpriteRenderer>();
        troopRenderer.sprite = sprite;
        troopRenderer.flipX = flipX;
        cellsGame[x, y].troop.transform.parent = cellsGame[x, y].go.transform;
        cellsGame[x, y].troop.transform.localPosition = new Vector3(0, 0, -1);
        cellsGame[x, y].troop.transform.localScale = new Vector3(40, 40, 1);
        cellsGame[x, y].troopName = troopName;
        cellsGame[x, y].isPlayerTroop = playerTroop;
    }
    public void ClearTroopAtPosition(int x,int y)
    {
        cellsGame[x, y].troop = null;
        cellsGame[x, y].isPlayerTroop = false;
        cellsGame[x, y].troopName = "";
        Destroy(cellsGame[x, y].go.transform.GetChild(0));
    }

    private void SwapCellTroop(Cell cell1,Cell cell2)
    {
        cell2.troop = cell1.troop;
        cell2.troop.transform.parent = cell2.go.transform;
        cell2.troop.transform.localPosition = new Vector3(0, 0, -1);
        cell2.isPlayerTroop = cell1.isPlayerTroop;
        cell2.troopName = cell1.troopName;
        cell1.troop = null;
        cell1.isPlayerTroop = false;
        cell1.troopName = "";
        //Destroy(cell1.go.transform.GetChild(0));
    }

    private List<Cell> getMovableCells(Cell troopCell,bool isPlayerTurn = true)
    {
        List<Cell> movable = new List<Cell>();
        int movingDistance = 0;
       
        switch (troopCell.troopName)
        {
            case "Archer":
                movingDistance = Archer.MoovingDistance;
            break;

            case "Knight":
                movingDistance = Knight.MoovingDistance;
            break;

            case "Soldier":
                movingDistance = Soldier.MoovingDistance;
            break;
        }

        for(int i = 1;i <= movingDistance; i++)
        {
            /*vecin stanga */
            int X = troopCell.x - i;
            int Y = troopCell.y;
            if (isInMap(X, Y, isPlayerTurn)){
                movable.Add(cellsGame[X, Y]);
            }

            /*vecin dreapta*/
            X = troopCell.x + i;
            Y = troopCell.y;
            if (isInMap(X, Y, isPlayerTurn))
            {
                movable.Add(cellsGame[X, Y]);
            }

            /*vecin sus*/
            X = troopCell.x;
            Y = troopCell.y + i;
            if (isInMap(X, Y, isPlayerTurn))
            {
                movable.Add(cellsGame[X, Y]);
            }
            /*vecin jos*/
            X = troopCell.x;
            Y = troopCell.y - i;
            if (isInMap(X, Y, isPlayerTurn))
            {
                movable.Add(cellsGame[X, Y]);
            }

        }

        return movable;
    }

    /* return true if a given position is in matrix vbounds and walkable*/
    public bool isInMap(int x,int y,bool isPlayerTurn)
    {
        if(x >= 0 && y >= 0 && x <= 7 && y <= 3)
        {
            if(cellsGame[x,y].walkable == true){
                Debug.Log(x + "-" + y);
                if(cellsGame[x,y].troop == null)
                {
                    return true;
                }
                if (isPlayerTurn && cellsGame[x, y].isPlayerTroop == true)
                {
                    return false;
                }
                if(isPlayerTurn && cellsGame[x, y].isPlayerTroop == false)
                {
                    return false;
                }
                if(!isPlayerTurn && cellsGame[x,y].isPlayerTroop == true)
                {
                    return true;
                }
                if(!isPlayerTurn && cellsGame[x,y].isPlayerTroop == false)
                {
                    return false;
                }
            }
            return false;
        }

        return false;
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
                Debug.Log(IsSpriteSelected);
                if (IsSpriteSelected == false)
                {
                  
                    if(cell.troop == null)
                    {
                        return;
                    }
                    if(cell.isPlayerTroop == false)
                    {
                        return;
                    }
                    UnselectAllCells();
                    spriteRenderer = cell.go.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = selected;

                    cell.selected = true;
                    LastSelectedCell = cell;
                    IsSpriteSelected = true;
                    movable = getMovableCells(cell);
                    foreach (Cell m in movable)
                    {
                        SpriteRenderer mRenderer = m.go.GetComponent<SpriteRenderer>();
                        mRenderer.sprite = selected;
                    }
                } else
                {
                   
                    if(cell.selected == true)
                    {
                        UnselectAllCells();
                        return;
                    }
                    if (!findInMovable(cell))
                    {
                        return;
                    }
                    if (cell.troop == null)
                    {
                        SwapCellTroop(LastSelectedCell, cell);
                        UnselectAllCells();
                    }
                }

            }
        }
    }
    


    private bool findInMovable(Cell cell)
    {
        foreach(Cell m in movable)
        {
            if (m.x == cell.x && m.y == cell.y)
            {
                return true;
            }
        }

        return false;

    }
    private struct Troop
    {
        public string Name;
        public int HP;
        public int Attack;
        public int MoovingDistance;

        public Troop(string Name, int HP, int Attack, int MoovingDistance) {

            this.Name = Name;
            this.HP = HP;
            this.Attack = Attack;
            this.MoovingDistance = MoovingDistance;
         }
    } 
}
