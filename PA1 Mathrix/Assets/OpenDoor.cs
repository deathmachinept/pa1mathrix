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
        Vector3 posicaoPorta = collider.transform.parent.FindChild("Cell_South_Door1_close").position;
        Vector3 posicaoJogador = collider.transform.position;
        if (posicaoJogador.y < posicaoPorta.y)
        {
            collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        else
        {
            collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        //if (  GetComponent<SpriteRenderer>().sortingOrder == 3)
        //{
        //    collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
        //}
        //else
        //{
        //    collider.transform.GetComponent<SpriteRenderer>().sortingOrder = 3;

        //}

    }

	// Update is called once per frame
	void Update () {
       // 
        if (podeCarregar && Input.GetKeyDown(KeyCode.F))
	    {
            Debug.Log("Entro!");
            isdoorOpen = animDoor.GetBool("IsDoorClosed");
	        if (!isdoorOpen)
	        {
	            isdoorOpen = true;
	            this.animDoor.SetBool("IsDoorClosed", isdoorOpen);
                this.transform.parent.FindChild("DoorCollider").GetComponent<BoxCollider2D>().enabled = false;

	        }
	        else
	        {

	            isdoorOpen = false;
                this.animDoor.SetBool("IsDoorClosed", isdoorOpen);
                this.transform.parent.FindChild("DoorCollider").GetComponent<BoxCollider2D>().enabled = true;

	        }

	    }
	}
}
