using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;

        transform.Translate(new Vector3(x, y, 0));

        if (GameObject.Find("TerminalA")!=null && GameObject.Find("TerminalB")!=null)
        {
            if (transform.position.x<GameObject.Find("TerminalA").transform.position.x+16 && transform.position.y < GameObject.Find("TerminalA").transform.position.y + 16 && transform.position.x > GameObject.Find("TerminalA").transform.position.x && transform.position.y < GameObject.Find("TerminalA").transform.position.y)
            {
                Debug.Log("Dentro do A");
            }

            if (transform.position.x < GameObject.Find("TerminalB").transform.position.x + 16 && transform.position.y < GameObject.Find("TerminalB").transform.position.y + 16 && transform.position.x > GameObject.Find("TerminalB").transform.position.x && transform.position.y < GameObject.Find("TerminalB").transform.position.y)
            {
                Debug.Log("Dentro do B");
            }
        }
    }
}
