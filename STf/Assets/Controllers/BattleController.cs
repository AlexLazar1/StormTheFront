using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    //public Board board;
    //public Tilemap m_level;
    public GameObject troopPanel;
    public Text troopInfo;
    public GameObject Board;
    public Sprite selected;
    public Sprite unselected;
    public Sprite attacked;
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
    private bool IsPlayerTurn = true;



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

    private List<Cell> GetEnemyCells()
    {
        List<Cell> cells = new List<Cell>();
        foreach(Cell cell in cellsGame)
        {
            if(cell.troop != null && cell.isPlayerTroop == false)
            {
                cells.Add(cell);
            }
        }
        return cells;
    }
    private List<Cell> GetPlayerCells()
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell in cellsGame)
        {
            if (cell.troop != null && cell.isPlayerTroop == true && cell.numberOfTroops >= 1)
            {
                cells.Add(cell);
            }
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
            SetTroopAtPosition(0, 0, archerSprite,"Archer",hero1.ArchersNo,Archer.Attack,Archer.HP);

        }
        if (hero1.KnightsNo > 0)
        {
            SetTroopAtPosition(0, 1, knightSprite,"Knight",hero1.KnightsNo,Knight.Attack,Knight.HP);

        }
        if (hero1.SoldiersNo > 0)
        {
            SetTroopAtPosition(0, 2, soldierSprite,"Soldier",hero1.SoldiersNo,Soldier.Attack,Soldier.HP);
        }

        if (hero2.ArchersNo > 0)
        {
            SetTroopAtPosition(7, 1, archerSprite,"Archer",hero2.ArchersNo,Archer.Attack,Archer.HP,false,false);

        }
        if (hero2.KnightsNo > 0)
        {
            SetTroopAtPosition(7, 2, knightSprite,"Knight",hero2.KnightsNo,Knight.Attack,Knight.HP,false,false);

        }
        if (hero2.SoldiersNo > 0)
        {
            SetTroopAtPosition(7, 3, soldierSprite,"Soldier",hero2.SoldiersNo,Soldier.Attack,Soldier.HP,false,false);

        }

        //SetTroopAtPosition(0, 1, knight);



    }

    public void SetTroopAtPosition(int x,int y,Sprite sprite,string troopName,int troopNumber,
        int troopAttack,int troopHP,bool flipX = true,bool playerTroop = true)
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
        cellsGame[x, y].numberOfTroops = troopNumber;
        cellsGame[x, y].troopAttack = troopAttack;
        cellsGame[x, y].troopHP = troopHP;
    }
    public void ClearTroopAtPosition(int x,int y)
    {
        Destroy(cellsGame[x, y].troop);
        cellsGame[x, y].isPlayerTroop = false;
        cellsGame[x, y].troopName = "";
        cellsGame[x, y].numberOfTroops = 0;
        cellsGame[x, y].troopHP = 0;
        cellsGame[x, y].troopAttack = 0;
    }

    private void SwapCellTroop(Cell cell1,Cell cell2)
    {
        cell2.troop = cell1.troop;
        cell2.troop.transform.parent = cell2.go.transform;
        cell2.troop.transform.localPosition = new Vector3(0, 0, -1);
        cell2.isPlayerTroop = cell1.isPlayerTroop;
        cell2.troopName = cell1.troopName;
        cell2.numberOfTroops = cell1.numberOfTroops;
        cell2.troopAttack = cell1.troopAttack;
        cell2.troopHP = cell1.troopHP;
        cell1.troop = null;
        cell1.isPlayerTroop = false;
        cell1.troopName = "";
        cell1.numberOfTroops = 0;
        cell1.troopHP = 0;
        cell1.troopAttack = 0;
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
                    return true;
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
    void LoadScene()
    {
        SceneManager.LoadScene("MainSceneIG");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            if (!IsPlayerTurn)
            {
                return;
            }
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Cell cell = FindCell(hit.collider.gameObject);
                Debug.Log(IsSpriteSelected);
                troopPanel.SetActive(false);
                if (IsSpriteSelected == false)
                {
                  
                    if(cell.troop == null)
                    {
                        return;
                    }
                    switch (cell.troopName)
                    {
                        case "Archer":
                            troopPanel.SetActive(true);
                            troopInfo.text = "Archers \n" + "-" + cell.numberOfTroops + " Troops\n-" + Archer.HP + " HP \n-" + Archer.Attack + " Attack";
                            break;

                        case "Soldier":
                            troopPanel.SetActive(true);
                            troopInfo.text = "Soldiers \n" + "-" + cell.numberOfTroops + " Troops\n-" + Soldier.HP + " HP \n-" + Soldier.Attack + " Attack";
                            break;

                        case "Knight":
                            troopPanel.SetActive(true);
                            troopInfo.text = "Knights \n" + "-" + cell.numberOfTroops + " Troops\n-" + Knight.HP + " HP \n-" + Knight.Attack + " Attack";
                            break;
                    }

                    if (cell.isPlayerTroop == false)
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
                    } else
                    {
                        int remainingHP = 0;
                        remainingHP = cell.troopHP * cell.numberOfTroops - LastSelectedCell.troopAttack * LastSelectedCell.numberOfTroops;
                        cell.numberOfTroops = remainingHP / cell.troopHP;
                        Debug.Log(cell.numberOfTroops);
                        if (cell.numberOfTroops < 1)
                        {
                            ClearTroopAtPosition(cell.x, cell.y);
                            if (GetPlayerCells().Count < 1)
                            {
                                dialog.text = "You died!";
                                Invoke("LoadScene", 3.0f);
                            }
                            if (GetEnemyCells().Count < 1)
                            {
                                dialog.text = "You Won!";
                                Invoke("LoadScene", 3.0f);
                            }
                        }
                        if (LastSelectedCell.troopName != "Archer")
                        {
                            remainingHP = LastSelectedCell.numberOfTroops * LastSelectedCell.troopHP - cell.numberOfTroops * cell.troopAttack;
                            LastSelectedCell.numberOfTroops = remainingHP / LastSelectedCell.troopHP;
                            if (LastSelectedCell.numberOfTroops < 1)
                            {
                                ClearTroopAtPosition(LastSelectedCell.x, LastSelectedCell.y);
                            }
                        }
                        UnselectAllCells();

                    }
                    IsPlayerTurn = false;
                    EnemyTurn();
                }

            }
        }
    }


    public void EnemyTurn()
    {

        List<Cell> enemies = GetEnemyCells();
        if(enemies.Count == 0)
        {
            dialog.text = "You won!";
            Invoke("LoadScene", 3.0f);
            return;
        }
        bool attack = false;
        foreach(Cell cell in enemies)
        {
            List<Cell> movables = getMovableCells(cell, false);
            foreach(Cell movable in movables)
            {
                if(movable.troop != null && movable.isPlayerTroop == true)
                {
                    if(cell.troopName == "Archer")
                    {
                        //Attack the troop no matter what
                        attack = true;
                        int remainingHP = movable.troopHP * movable.numberOfTroops - cell.troopAttack * cell.numberOfTroops;
                        movable.numberOfTroops = remainingHP / movable.troopHP;
                        if(movable.numberOfTroops < 1)
                        {
                            ClearTroopAtPosition(movable.x, movable.y);
                        }
                        movable.go.GetComponent<SpriteRenderer>().sprite = attacked;
                        IsPlayerTurn = true;
                        return;
                    }
                    //fear, attack only if not die or causing some good damage
                    if(cell.troopHP * cell.numberOfTroops > movable.troopAttack * movable.numberOfTroops
                        || cell.troopAttack * cell.troopHP >= (movable.troopHP * movable.numberOfTroops) / 2)
                    {
                        int remainingHP = movable.troopHP * movable.numberOfTroops - cell.troopAttack * cell.numberOfTroops;
                        movable.numberOfTroops = remainingHP / movable.troopHP;
                        bool enemylDied = false;
                        bool playerDied = false;
                        if (movable.numberOfTroops < 1)
                        {
                            ClearTroopAtPosition(movable.x, movable.y);
                            playerDied = true;
                        }

                        remainingHP = cell.troopHP * cell.numberOfTroops - movable.troopAttack*movable.numberOfTroops;
                        cell.numberOfTroops = remainingHP / cell.troopHP;
                        if(cell.numberOfTroops < 1)
                        {
                            ClearTroopAtPosition(cell.x, cell.y);
                            enemylDied = true;
                        }
                        if(playerDied && !enemylDied)
                        {
                            SwapCellTroop(cell, movable);
                        }
                        movable.go.GetComponent<SpriteRenderer>().sprite = attacked;
                        IsPlayerTurn = true;
                        return;
                    }
                    if(GetPlayerCells().Count < 1)
                    {
                        dialog.text = "You died!";
                        Invoke("LoadScene", 3.0f);
                    }
                }
            }
        }

        int tries = 0;
        while (true)
        {
            if(tries == 5)
            {
                //maybe he is blocked?
                IsPlayerTurn = true;
                return;
            }
            tries++;
            Cell troopToMove = enemies[Random.Range(0, enemies.Count)];
            foreach (Cell movable in getMovableCells(troopToMove,false))
            {
                if(movable.x <= troopToMove.x)
                {
                    SwapCellTroop(troopToMove, movable);
                    IsPlayerTurn = true;
                    return;
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
