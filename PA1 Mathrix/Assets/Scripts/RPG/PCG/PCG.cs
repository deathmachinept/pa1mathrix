﻿using UnityEngine;
using System.Collections;
using System;

public class PCG  {

    
    public byte[,] pcgrid; // Grid array
    public byte[,] guardarDir; // Grid array
    public byte[,] floorsBase; // 1 para chao gerados 2 para wall gerados , 3 para centro de metro, 4 centro cela
    public int pcgrid_width; // Grid width
    public int pcgrid_height; // Grid height



    public void updateParam(int g_width, int g_height)
    {
        pcgrid_width = g_width; // Get grid length
        pcgrid_height = g_height; // Get grid width
    }



    public void generatePCG(byte[,] g, byte[,] gdir, byte[,]floorsInfo)
    {
        pcgrid = g; // Copy grid
        guardarDir = gdir;
        floorsBase = floorsInfo;


    }

    public bool bounded(int x, int y)
    {
        // Check if cell is inside grid
        if (x < 0 || x >= pcgrid_width || y < 0 || y >= pcgrid_height) return false;
        return true;
    }

    public bool blocked(int x, int y, int type)
    {
        // Check if cell is occupied
        if (type == 4)
        {
            if ((bounded(x, y) && pcgrid[x, y] == type) || (bounded(x, y) && pcgrid[x, y] == 5)) return true;

        }
        else {
        if (bounded(x, y) && pcgrid[x,y] == type) return true;
        }
        return false;
    }

}
