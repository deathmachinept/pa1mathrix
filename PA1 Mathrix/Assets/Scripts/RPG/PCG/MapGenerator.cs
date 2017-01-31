using System;
using UnityEngine;
using System.Collections;


public class MapGenerator : MonoBehaviour
{
  private PCG_Basic pcgb;
  private PCG pcg;

  private System.Random RandomGen;
  public int InitialWorldSeed;

  private GameObject TilesHolder;

    public GameObject floor;
    public GameObject corridor;
    public GameObject wall;
    public int numeroMaximoQuartos;
  public int room_type, room_min_size, room_max_size, corridor_num, corridor_weight, turning_weight;
  int pcg_type; // PCG type
  
  byte[,] grid; // Grid array
  public int grid_width; // Grid width
  public int grid_height; // Grid height

    private bool once = true;
  bool debug = true; // Debug mode
  
  // World parameters  
    public MapGenerator()
    {

    }



    void Start()
    {
        RandomGen = new System.Random();
        TilesHolder = new GameObject(); 
        TilesHolder.transform.position = Vector3.zero;
        TilesHolder.name = "Tiles Holder";

        generateWorld( grid_width, grid_height);
    }

    void Update()
    {
        if (once)
        {
            renderGrid();
            once = false;
        }

    }
      public void generateWorld( int g_w, int g_h)
      {
        // PGC type
        grid_width = g_w; // Grid width
        grid_height = g_h; // Grid height
    
        initGrid(); // Initialize empty grid
    
        //// Generate PCG
        //switch (pcg_type) {
        //    case 0: pcg = new PCG();
        //        pcg.updateParam(grid_width, grid_height);
        //        pcg.generatePCG(grid);
        //          break;
          //case 1: 
          pcgb = new PCG_Basic();
          pcgb.updateParam(grid_width, grid_height, room_type, room_min_size, room_max_size, corridor_num, corridor_weight, turning_weight, numeroMaximoQuartos);
                  pcgb.generatePCGBasic(grid);
                  //break;
        //}
      }


  void renderGrid()
  {
      // Render grid
      for (int j = 0; j < grid_height; j++)
      {
          for (int i = 0; i < grid_width; i++)
          {
              renderGridCell(i, j);
          }
      }
 
  } 

  void renderGridCell(int x, int y)
  {
      // Render grid cell content
      switch (grid[x,y])
      {
          case 0: // Empty

              break;
          case 1: // Floor
              GameObject Floor = Instantiate(Resources.Load("Floor")) as GameObject;
              Floor.transform.SetParent(TilesHolder.transform);
              Floor.name = "Floor Tile";
              Floor.transform.position = new Vector3(x, y, 0);
              break;
          case 2: // Wall
              GameObject Wall = Instantiate(Resources.Load("Wall")) as GameObject;
              Wall.transform.SetParent(TilesHolder.transform);
              Wall.name = "Wall Tile";
              Wall.transform.position = new Vector3(x, y, 0);
              break;
          case 3: // Door
              GameObject Door = Instantiate(Resources.Load("Door")) as GameObject;
              Door.transform.SetParent(TilesHolder.transform);
              Door.name = "Door Tile";
              Door.transform.position = new Vector3(x, y, 0);
              break;
          case 4: // Corridor
              GameObject Corridor = Instantiate(Resources.Load("Empty")) as GameObject;
              Corridor.transform.SetParent(TilesHolder.transform);
              Corridor.name = "Door Tile";
              Corridor.transform.position = new Vector3(x, y, 0);
              break;
          case 5: // Corridor
              GameObject corridorOrange = Instantiate(Resources.Load("Extracorridor")) as GameObject;
              corridorOrange.transform.SetParent(TilesHolder.transform);
              corridorOrange.name = "Door Tile";
              corridorOrange.transform.position = new Vector3(x, y, 0);
              break;
      }
  }


  public void initGrid()
  {
    grid = new byte[grid_width,grid_height]; 
    
    for (int j = 0; j < grid_height; j++) {
      for (int i = 0; i < grid_width; i++) {
        grid[i,j] = 0; // Initialize all cell as empty
      }
    }
  }

}
