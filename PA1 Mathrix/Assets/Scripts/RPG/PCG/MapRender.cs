using UnityEngine;
using System.Collections;

public class MapRender : MonoBehaviour
{
    private MapGenerator world;
    
    void Start()
    {
        world = new MapGenerator();
    }

    void renderGrid()
    {
        // Render grid
        for (int j = 0; j < world.grid_height; j++)
        {
            for (int i = 0; i < world.grid_width; i++)
            {
                renderGridCell(i, j);
            }
        }
    } 
	// Use this for initialization
	void generate () {
        //world.generateWorld(g_layout + 1, g_width, g_height);

	}

    void renderGridCell(int x, int y)
    {
        // Render grid cell content
        //switch (world.grid[x][y])
        //{
        //    case 0: // Empty

        //        break;
        //    case 1: // Floor

        //        break;
        //    case 2: // Wall

        //        break;
        //    case 3: // Door

        //        break;
        //    case 4: // Corridor

        //        break;
        //    case 5: // BSP border

        //        break;
        //}
    }
}
