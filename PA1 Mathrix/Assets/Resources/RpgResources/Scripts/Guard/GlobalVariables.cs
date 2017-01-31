using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalVariables : MonoBehaviour {

    public static GlobalVariables singleton { get; private set; }

    public List<Vector2> GuardPoints;

    public void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
        }
        singleton = this;

        GuardPoints.Add(new Vector2(-13,-24));
        GuardPoints.Add(new Vector2(-3, -25));
        GuardPoints.Add(new Vector2(1, -25));
        GuardPoints.Add(new Vector2(5, -14));
        GuardPoints.Add(new Vector2(-12, -11));
        GuardPoints.Add(new Vector2(-5, 2));
        GuardPoints.Add(new Vector2(-18, -3));
        GuardPoints.Add(new Vector2(-30, -6));

        foreach (Transform t in transform)
        {
            GuardPoints.Add(t.position);
        }
    }
}
