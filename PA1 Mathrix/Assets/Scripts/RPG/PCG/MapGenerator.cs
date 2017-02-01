﻿using System;
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
    public byte[,] guardarDir; // portas 0south 1west 2north 3east paredes 4south 5 east 6 north 7 west
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
                  pcgb.generatePCGBasic(grid, guardarDir);
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
        // portas 0south 1west 2north 3east paredes 4south 5 east 6 north 7 west
        // Render grid cell content
        switch (grid[x, y])
        {
            case 1: // Corridor
                GameObject Floor = Instantiate(Resources.Load("CorridorTile")) as GameObject;
                Floor.transform.SetParent(TilesHolder.transform);
                Floor.name = "Chao";
                Floor.transform.position = new Vector3(x, y, 0);
                break;
            case 4: // Corridor
                GameObject Corridor = Instantiate(Resources.Load("Corridor2x2Test")) as GameObject;
                Corridor.transform.SetParent(TilesHolder.transform);
                Corridor.name = "Door Tile";
                Corridor.transform.position = new Vector3(x, y, 0);
                break;
        }
        switch (guardarDir[x,y])
      {

            case 0: // South Door
                GameObject SouthDoor = Instantiate(Resources.Load("CorridorDoor")) as GameObject;
                SouthDoor.transform.SetParent(TilesHolder.transform);
                SouthDoor.name = "South Door";
                SouthDoor.transform.position = new Vector3(x, y, 0);
                break;
            case 1: // West Door
                GameObject WestDoor = Instantiate(Resources.Load("WestWall")) as GameObject;
                WestDoor.transform.SetParent(TilesHolder.transform);
                WestDoor.name = "West Door";
                WestDoor.transform.position = new Vector3(x, y, 0);
              break;
          case 2: // North door
              GameObject Wall = Instantiate(Resources.Load("CorridorDoor")) as GameObject;
              Wall.transform.SetParent(TilesHolder.transform);
              Wall.name = "North door";
              Wall.transform.position = new Vector3(x, y, 0);
              break;
          case 3: // East Door
              GameObject Door = Instantiate(Resources.Load("CorridorPanel0")) as GameObject;
              Door.transform.SetParent(TilesHolder.transform);
              Door.name = "East Door";
              Door.transform.position = new Vector3(x, y, 0);
              break;
          case 4: // SouthWall
              GameObject SouthWall = Instantiate(Resources.Load("CorridorPanel0")) as GameObject;
                SouthWall.transform.SetParent(TilesHolder.transform);
                SouthWall.name = "South Wall";
                SouthWall.transform.position = new Vector3(x, y, 0);
              break;
          case 5: // Eastwall
              GameObject EastWall = Instantiate(Resources.Load("EastWall")) as GameObject;
                EastWall.transform.SetParent(TilesHolder.transform);
                EastWall.name = "East Wall";
                EastWall.transform.position = new Vector3(x, y, 0);
              break;
            case 6: // NorthWall
                GameObject northWall = Instantiate(Resources.Load("CorridorPanel0")) as GameObject;
                northWall.transform.SetParent(TilesHolder.transform);
                northWall.name = "North Wall";
                northWall.transform.position = new Vector3(x, y, 0);
                break;
            case 7: // WestWall
                GameObject WestWall = Instantiate(Resources.Load("WestWall")) as GameObject;
                WestWall.transform.SetParent(TilesHolder.transform);
                WestWall.name = "West Wall";
                WestWall.transform.position = new Vector3(x, y, 0);
                break;
            case 8: // Empty

                break;
        }
  }


  public void initGrid()
  {
    grid = new byte[grid_width,grid_height];
    guardarDir = new byte[grid_width, grid_height];

        for (int j = 0; j < grid_height; j++) {
      for (int i = 0; i < grid_width; i++) {
        grid[i,j] = 0; // Initialize all cell as empty
          guardarDir[i, j] = 8;
      }
    }
  }

}
