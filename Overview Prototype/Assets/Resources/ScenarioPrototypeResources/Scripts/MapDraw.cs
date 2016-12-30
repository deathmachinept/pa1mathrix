using UnityEngine;
using System.Collections;

public class MapDraw : MonoBehaviour
{
    private GameObject TilesHolder;
    private int tileSize;

	void Start ()
    {
        if (GameObject.Find("TilesHolder") == null)
        {
            TilesHolder = new GameObject();
            TilesHolder.transform.position = Vector3.zero;
            TilesHolder.transform.SetParent(GameObject.Find("MainSceneObjectsHolder").transform);
            TilesHolder.name = "Tiles Holder";
            tileSize = 10;
            PlaceTiles();
        }
    }

    void PlaceTiles()
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                GameObject tile = Instantiate(Resources.Load("ScenarioPrototypeResources/Prefabs/Floor")) as GameObject;
                tile.transform.SetParent(TilesHolder.transform);
                tile.name = "Floor Tile " + j +" "+ i;
                tile.transform.position=new Vector3(i*tileSize,j*tileSize,0);
            }
        }
        GameObject TerminalA = Instantiate(Resources.Load("ScenarioPrototypeResources/Prefabs/Terminal")) as GameObject;
        TerminalA.transform.SetParent(TilesHolder.transform);
        TerminalA.name = "Terminal A";
        TerminalA.transform.position=new Vector3(-10*tileSize,0,0);
        GameObject TerminalB = Instantiate(Resources.Load("ScenarioPrototypeResources/Prefabs/Terminal")) as GameObject;
        TerminalB.transform.SetParent(TilesHolder.transform);
        TerminalB.name = "Terminal B";
        TerminalB.transform.position = new Vector3((10 * tileSize)-10, 0,0);
    }
}
