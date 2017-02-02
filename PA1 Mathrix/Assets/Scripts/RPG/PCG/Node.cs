using UnityEngine;
using System.Collections;

public class Node {

    private int x;
    private int y;
    private int f; //total score
    private int g; //current node score
    private int h;  // herustic score
    private int dir;
    public Node parent;

    public Node(int xpos, int ypos, int gscore, int hscore, int direction)
    {
        x = xpos;
        y = ypos;
        g = gscore;
        h = hscore;
        f = g + h;
        dir = direction;
    }

    public int getX
    {
        get
        {
            return this.x;
        }
        set
        {
            this.x = value;
        }
    }
    public int getY
    {
        get
        {
            return this.y;
        }
        set
        {
            this.y = value;
        }
    }
    public int getF
    {
        get
        {
            return this.f;
        }
        set
        {
            this.f = value;
        }
    }
    public int getG
    {
        get
        {
            return this.g;
        }
        set
        {
            this.g = value;
        }
    }
    public int getH
    {
        get
        {
            return this.h;
        }
        set
        {
            this.h = value;
        }
    }
    public int getDir
    {
        get
        {
            return this.dir;
        }
        set
        {
            this.dir = value;
        }
    }

}
