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

        GuardPoints.Add(new Vector2(20,85));
        GuardPoints.Add(new Vector2(25, 88));

        foreach (Transform t in transform)
        {
            GuardPoints.Add(t.position);
        }
    }
}
