using UnityEngine;
using System.Collections;


public class Agent : MonoBehaviour {


    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public GameObject agent;
    public AnimationCurve randomCurve;
    public int mapWidth;
    public int mapHeight;

    public Sprite prisonCellTop0, prisonCellTop1, prisonCellTop2;
    public Sprite prisonCellMiddle0, prisonCellMiddle1, prisonCellMiddle2;
    public Sprite prisonCellBottom0, prisonCellBottom1, prisonCellBottom2;

    public Sprite prisonCellWallTop0, prisonCellWallTop1, prisonCellWallTop2;
    public Sprite prisonCellWallMiddle0, prisonCellWallMiddle1;
    public Sprite prisonCellWallBottom0, prisonCellWallBottom1, prisonCellWallBottom2;

    public Sprite corridorTopSprite0, corridorTopSprite1, corridorTopSprite2;
    public Sprite corridorLeftSprite0, corridorLeftSprite1, corridorLeftSprite2;
    public Sprite corridorBottomSprite0, corridorBottomSprite1, corridorBottomSprite2;

    public int cellSizeX, cellSizeY;
    //public RangeInt[] rangeArray; // array de RangeInt
    public int starPosX, starPosY;

    private Direction LastDirection;
    private bool chosenStartPos;
    //public RangeInt rangeTest = new RangeInt(5,4,5f);

    public float timeLeft = 5;


    private int[,] RoomMap;

    private Random seed;

    void Start()
    {
        Random.InitState(31);
        chosenStartPos = false;
        RoomMap = new int[mapWidth,mapHeight];

        starPosX = (int)mapWidth/2;
        starPosY = (int)mapHeight/2;
        //range.Min = 3; range1.Min = 9; range2.Min = 15;
        //range.Max = 8; range1.Max = 14; range2.Min = 18;
        //range.Weight = 60f; range1.Weight = 40f; range2.Weight = 20f;

    }


    //1-Determine the number of rooms to be created based on the room layout selected, and updates various user-configured variables in updateParam()
    //2-Calls initRooms() to create a room of random width and height at a random location on the grid. A random number of openings (doors) are placed on the room's walls.
    //3-Check if the room is outside of the grid or blocked by another room or corridor through blockRoom(). If blocked, delete the room and repeat step 2.
    //4-After all rooms are initialized, call initCorridors() to connect the openings in each room. In basicAStar(), A* pathfinding is used to look for the optimal path from one door to another. Weights are used so that the corridors are more likely to be straight and join each other.
    //5-If there are any openings left unconnected, randomly tunnel corridors out from the opening by calling tunnelRandom(), if the corridor reaches a room wall, add a door there.

    bool choosePos = true;
	// Update is called once per frame
	void Update ()
	{

        if (timeLeft > 0) {

            if (!choosePos)
            {
                Direction RandomDir = ChooseDirection(25, 25, 25, 25);

                Debug.Log(RandomDir);
                
                chosenStartPos = true;
            }


        }
        if (timeLeft > 0)
            timeLeft -= Time.deltaTime;

	}

    public Direction ChooseDirection(float westWeight,float eastWeight,float northWeight, float southWeight)
    {
        Direction dir = Direction.East;
        int RandomDir = IntWeightRange(new RangeInt(0, 2, westWeight), new RangeInt(3, 5, eastWeight), new RangeInt(6, 8, northWeight), new RangeInt(9, 11, southWeight));


        if (RandomDir <= 2)
        {
            LastDirection = Direction.West;
        }
        else if (RandomDir >= 3 && RandomDir <= 5)
        {
            LastDirection = Direction.East;
        }
        else if (RandomDir >= 6 && RandomDir <= 8)
        {
            LastDirection = Direction.North;
        }
        else if (RandomDir >= 9 && RandomDir <= 11)
        {
            LastDirection = Direction.South;
        }

        return dir;
    }

    public void Corridor(bool smallTrue)
    {

        if (smallTrue)
        {
            
        }

    }

    public int IntWeightRange(params RangeInt[] ranges)
         {
             Debug.Log("Ranges Length " + ranges.Length);
             if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
             if (ranges.Length == 1) return Random.Range(ranges[0].maxI, ranges[0].minI);

             float total = 0f;
             for (int i = 0; i < ranges.Length; i++) total += ranges[i].weightF;

             float r = Random.value;
             float s = 0f;

             int cnt = ranges.Length - 1;
             for (int i = 0; i < cnt; i++)
             {
                 s += ranges[i].weightF / total;
                 if (s >= r)
                 {
                     return Random.Range(ranges[i].maxI, ranges[i].minI);
                 }
             }

             return Random.Range(ranges[cnt].maxI, ranges[cnt].minI);
         }


    //     public  int FloatWeightRange(params FloatRange[] ranges)
    //     {
    //         if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
    //         if (ranges.Length == 1) return Random.Range(ranges[0].Max, ranges[0].Min);

    //         float total = 0f;
    //         for (int i = 0; i < ranges.Length; i++) total += ranges[i].Weight;

    //         float r = Random.value;
    //         float s = 0f;

    //         int cnt = ranges.Length - 1;
    //         for (int i = 0; i < cnt; i++)
    //         {
    //             s += ranges[i].Weight / total;
    //             if (s >= r)
    //             {
    //                 return Random.Range(ranges[i].Max, ranges[i].Min);
    //             }
    //         }

    //         return Random.Range(ranges[cnt].Max, ranges[cnt].Min);
    //     }


}


