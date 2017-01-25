using UnityEngine;
using System.Collections;

public class RangeInt 
{
    public int minI;
    public int maxI;
    public float weightF;


    public RangeInt(int min, int max, float weight)
    {

        minI = min;
        maxI = max;
        weightF = weight;

    }
}
