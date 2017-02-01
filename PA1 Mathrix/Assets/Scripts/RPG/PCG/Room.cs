using System;
using UnityEngine;
using System.Collections;


public class Room 
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
    public int wall_y1, wall_y1_TopText, wall_y2_TopText;
    public int wall_y2;
    public int[,] opening; // Doors
    public int opening_num; // Number of doors
    private System.Random RandomG;
    public int InitialWorldSeed;

    private int typeAutobuild;
    //int based = int((room_min + 1)*0.5); room_min = tamanho min do quarto eg. (9+1) *0.5 = 5 
    //int radix = int((room_max - room_min)*0.5 + 1); (18 - 9) * 0.5f +1 = 5.5f

    public Room(int w, int h, int based, int radix, int c_num, System.Random seed, int preBuild, int width, int height, Vector2 coordenada )
    {
        RandomG = seed;
        typeAutobuild = preBuild;
        //Random.InitState(InitialWorldSeed);
        int[,] novo = new int[2,2];
        pcgrid_width = w;
        pcgrid_height = h;

        if (typeAutobuild == 0)
        {
            room_width = (int)(NextFloat(RandomG, 0, radix) + based);
            room_height = (int)(NextFloat(RandomG, 0, radix) + based);
            room_x1 = (int)(NextFloat(RandomG, 0, (pcgrid_width - room_width))); //ponto incial de dsesenho X
            room_y1 = (int)(NextFloat(RandomG, 0, (pcgrid_height - room_height)));  //ponto incial de desenho Y 
            room_x2 = room_x1 + room_width - 1;  // largura ponto Final
            room_y2 = room_y1 + room_height - 1; //altura Ponto Final
            room_x = room_x1 + (int)((room_x2 - room_x1) * 0.5f); // pontocentral X
            room_y = room_y1 + (int)((room_y2 - room_y1) * 0.5f); // pontocentrla Y
            wall_x1 = room_x1 - 1;
            wall_x2 = room_x2 + 1;
            wall_y1 = room_y1 - 1;
            wall_y2 = room_y2 + 1; // mais um passo
        }
        else
        {
            room_width = width;
            room_height = height;
            room_x = (int) coordenada.x;
            room_y = (int) coordenada.y;
            room_x1 = (int) coordenada.x - width;
            room_y1 = (int)coordenada.y - height;
            room_x2 = (int)coordenada.x + width;
            room_y2 = (int) coordenada.y ;

            wall_x1 = room_x1 - 1;
            wall_x2 = room_x2 + 1;
            wall_y1 = room_y1 - 1;
            wall_y2 = room_y2 + 1;

            //wall_x1 = room_x1 - 1;
            //wall_x2 = room_x2 + 1;
            //wall_y1_TopText = room_y1 - 1;
            //wall_y1 = room_y1 - 2;
            //wall_y1 = room_y1 - 3;

            //wall_y2 = room_y2 + 1; // mais um passo

            //wall_y2 = room_y2 + 2;
            //wall_y2_TopText = room_y2 +3; // mais um passo

            Debug.Log("Aqui!");
        }




        int tempCnum = c_num + 1; //1
        if (tempCnum < 1) 
        {
            tempCnum = 1;
        }
        opening_num = (int)(NextFloat(RandomG, 1, tempCnum)); // Open up doorway

        opening = new int[opening_num, 3];

        initDoors();
    }

    private float NextFloat(System.Random random,float min, float max)
    {

        return min + ((max - min) * (float)(random.NextDouble()));
    }
 

    void initDoors()
    {
        int count = opening_num; //1
        while (count != 0)
        {

            if (typeAutobuild != 0)
            {
                opening[count - 1, 2] = 0;
            }
            else{
                opening[count - 1, 2] = (int)(NextFloat(RandomG, 0, 4)); // Door orientation
                }
            // Make sure door is not on corner or facing wall
            switch (opening[count - 1,2])
            {
                case 0: // South wall
                    int x1;
                    if (typeAutobuild ==2) // metro
                    {
                        x1 = (int)((wall_x2 - wall_x1) / 2) + wall_x1;

                    }
                    else if (typeAutobuild == 1) //cela
                    {
                        x1 = wall_x1 + 2;
                    }
                    else{
                         x1 = (int)NextFloat(RandomG, wall_x1, wall_x2);
                    }
                    if (x1 != wall_x1 && x1 != wall_x2 && wall_y1 >= 1)
                    {
                        opening[count - 1,0] = x1;
                        opening[count - 1,1] = wall_y1;
                        opening[count - 1,2] = 0;
                        count--;
                    }
                    break;
                case 1: // west wall
                    int y2 = (int)NextFloat(RandomG, wall_y1, wall_y2);
                    if (y2 != wall_y1 && y2 != wall_y2 && wall_x2 < pcgrid_width - 1)
                    {
                        opening[count - 1,0] = wall_x2;
                        opening[count - 1,1] = y2;
                        opening[count - 1,2] = 1;
                        count--;
                    }
                    break;
                case 2: // north Wall
                    int x2 = (int)((wall_x2 - wall_x1)/2) + wall_x1;//(int)NextFloat(RandomG, wall_x1, wall_x2);

                    if (x2 != wall_x1 && x2 != wall_x2 && wall_y2 < pcgrid_height - 1) // confirma que o ponto nao esta nas esquinas 
                    {
                        opening[count - 1,0] = x2;
                        opening[count - 1,1] = wall_y2;
                        opening[count - 1,2] = 2;
                        count--;
                    }
                    break;
                case 3: // east wall
                    int y1 = (int)((wall_y2 - wall_y1) / 2) + wall_y1;


                    //int y1 = (int)NextFloat(RandomG, wall_y1, wall_y2);
                    if (y1 != wall_y1 && y1 != wall_y2 && wall_x1 >= 1)
                    {
                        opening[count - 1,0] = wall_x1; // coordenada
                        opening[count - 1,1] = y1; //posicao da porta
                        opening[count - 1,2] = 3; // dir
                        count--;
                    }
                    break;
            }


        }
    }
}
