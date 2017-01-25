using UnityEngine;
using System.Collections;

public class RangeFloat : MonoBehaviour {

	// Use this for initialization
    private float minI;
    private float maxI;
    private float weightF;


    public RangeFloat(float min, float max, float weight)
    {

        minI = min;
        maxI = max;
        weightF = weight;
    }

}
