using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{

    public int pcgrid_width;
    public int pcgrid_height;
    public int room_x;
    public int room_y;
    public int room_width;
    public int room_height;
    public int room_x1;
    public int room_x2;
    public int room_y1;
    public int room_y2;
    public int wall_x1;
    public int wall_x2;
    public int wall_y1;
    public int wall_y2;
    public int[,] opening; // Doors
    public int opening_num; // Number of doors
    private Random RandomGen;
    public int InitialWorldSeed;

    //int based = int((room_min + 1)*0.5); room_min = tamanho min do quarto eg. (9+1) *0.5 = 5 
    //int radix = int((room_max - room_min)*0.5 + 1); (18 - 9) * 0.5f +1 = 5.5f

    public Room(int w, int h, int based, int radix, int c_num)
    {

        InitialWorldSeed = Random.Range(1, 1000);

        Random.InitState(InitialWorldSeed);


        pcgrid_width = w;
        pcgrid_height = h;
        room_width = (int)(Random.Range(0f,radix) + based);
        room_height =(int) (Random.Range(0f,radix) + based);
        room_x1 = (int)(Random.Range(0f, (pcgrid_width - room_width)));
        room_y1 = (int)(Random.Range(0f,(pcgrid_height - room_height)));
        room_x2 = room_x1 + room_width - 1;
        room_y2 = room_y1 + room_height - 1;
        room_x = room_x1 + (int)((room_x2 - room_x1)*0.5f);
        room_y = room_y1 + (int)((room_y2 - room_y1)*0.5f);
        wall_x1 = room_x1 - 1;
        wall_x2 = room_x2 + 1;
        wall_y1 = room_y1 - 1;
        wall_y2 = room_y2 + 1;
        opening_num = (int) (Random.Range(1f, c_num + 1)); // Open up doorway
        opening = new int[opening_num, 3];
        initDoors();
    }

    void initDoors()
    {
        int count = opening_num;
        while (count != 0)
        {
            opening[count - 1,2] = Random.Range(0, 4); // Door orientation
            // Make sure door is not on corner or facing wall
            switch (opening[count - 1,2])
            {
                case 0: // North wall
                    int x1 = Random.Range(wall_x1, wall_x2);
                    if (x1 != wall_x1 && x1 != wall_x2 && wall_y1 >= 1)
                    {
                        opening[count - 1,0] = x1;
                        opening[count - 1,1] = wall_y1;
                        opening[count - 1,2] = 0;
                        count--;
                    }
                    break;
                case 1: // East wall
                    int y2 = Random.Range(wall_y1, wall_y2);
                    if (y2 != wall_y1 && y2 != wall_y2 && wall_x2 < pcgrid_width - 1)
                    {
                        opening[count - 1,0] = wall_x2;
                        opening[count - 1,1] = y2;
                        opening[count - 1,2] = 1;
                        count--;
                    }
                    break;
                case 2: // South wall
                    int x2 = Random.Range(wall_x1, wall_x2);
                    if (x2 != wall_x1 && x2 != wall_x2 && wall_y2 < pcgrid_height - 1)
                    {
                        opening[count - 1,0] = x2;
                        opening[count - 1,1] = wall_y2;
                        opening[count - 1,2] = 2;
                        count--;
                    }
                    break;
                case 3: // West wall
                    int y1 = Random.Range(wall_y1, wall_y2);
                    if (y1 != wall_y1 && y1 != wall_y2 && wall_x1 >= 1)
                    {
                        opening[count - 1,0] = wall_x1;
                        opening[count - 1,1] = y1;
                        opening[count - 1,2] = 3;
                        count--;
                    }
                    break;
            }


        }
    }
}
