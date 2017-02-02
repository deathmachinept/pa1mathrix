using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    private Animator animDoor;
    private bool isdoorOpen = false;
    private bool podeCarregar = false;
	// Use this for initialization
	void Start () {
        animDoor = this.GetComponent<Animator>();
	}


    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Colision");
        podeCarregar = true;

        //if (posicaoJogador.y < posicaoPorta.y)
        //{
        //    collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
        //}
        //else
        //{
        //    collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
        //}

        //if (  GetComponent<SpriteRenderer>().sortingOrder == 3)
        //{
        //    collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
        //}
        //else
        //{
        //    collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 3;

        //}

    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        podeCarregar = false;
        Debug.Log("Falha Colision");
    }

    // Update is called once per frame
    void Update () {
       // 
        if (podeCarregar && Input.GetKeyDown(KeyCode.F))
	    {
            isdoorOpen = animDoor.GetBool("IsDoorClosed");
	        if (!isdoorOpen)
	        {
	            isdoorOpen = true;
	            this.animDoor.SetBool("IsDoorClosed", isdoorOpen);
                if(this.name == "CorridorDoor2")
                {
                    transform.GetComponent<BoxCollider2D>().enabled = false;
                }else {
                this.transform.parent.FindChild("DoorCollider").GetComponent<BoxCollider2D>().enabled = false;
                }
            }
	        else
	        {

	            isdoorOpen = false;
                this.animDoor.SetBool("IsDoorClosed", isdoorOpen);
                if (this.name == "CorridorDoor2")
                {
                    transform.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    this.transform.parent.FindChild("DoorCollider").GetComponent<BoxCollider2D>().enabled = true;
                }
            }

	    }
	}
}
