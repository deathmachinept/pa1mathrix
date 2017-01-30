using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class MapGenerator:MonoBehaviour
{
  private PCG_Basic pcgb;
  private PCG pcg;

  private Random RandomGen;
  public int InitialWorldSeed;

  public int room_type, room_min_size, room_max_size, corridor_num, corridor_weight, turning_weight;
  int pcg_type; // PCG type
  
  byte[,] grid; // Grid array
  public int grid_width; // Grid width
  public int grid_height; // Grid height
  
  bool debug = true; // Debug mode
  
  // World parameters  
    public MapGenerator()
    {
        generateWorld(1, 40, 30);
    }

    void Start()
    {
        generateWorld(1, grid_width, grid_height);
    }

    void Update()
    {
        
        renderGrid();

    }
      public void generateWorld(int p_t, int g_w, int g_h)
      {
        pcg_type = p_t; // PGC type
        grid_width = g_w; // Grid width
        grid_height = g_h; // Grid height
    
        initGrid(); // Initialize empty grid
    
        // Generate PCG
        switch (pcg_type) {
            case 0: pcg = new PCG();
                pcg.updateParam(grid_width, grid_height);
                pcg.generatePCG(grid);
                  break;
          case 1: pcgb = new PCG_Basic();
                  pcgb.updateParam(grid_width, grid_height, room_type, room_min_size, room_max_size, corridor_num, corridor_weight, turning_weight);
                  pcgb.generatePCGBasic(grid);
                  break;
        }
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

              break;
          case 2: // Wall

              break;
          case 3: // Door

              break;
          case 4: // Corridor

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
