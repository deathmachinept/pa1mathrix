using UnityEngine;
using System.Collections;

public class DoorLeftOpenAnimScript : MonoBehaviour {

	// Use this for initialization

    public bool executeOpenAnim = false, executeCloseAnim = false, once = false;
    private int contar; float contarX;

    void Start()
    {
        contar = 0;
        contarX = 0;
    }

	// Update is called once per frame
	void Update () {

	    if (executeOpenAnim)
	    {
            Vector3 openY = new Vector3(0,-0.01f,0);
	        if (contar < 4)
	        {
	            this.transform.localPosition += openY;
	            contar++;
	        }
	        else
	        {
                Vector3 openX = new Vector3(-0.04f, 0, 0);
	            if (this.transform.localPosition.x > -3.03f)
	            {
	                this.transform.localPosition += openX;
	            }
	            else
	            {
	                executeOpenAnim = false;
	                contar = 0;
	            }

	        }

	    }
        if (executeCloseAnim)
	    {

            Vector3 openX = new Vector3(0.04f, 0, 0);
	        if (this.transform.localPosition.x < -2.29f)
	        {
	            this.transform.localPosition += openX;
	        }
	        else
	        {
                if (!once)
                {
                    Vector3 ajustarPorta = this.transform.localPosition;
                    ajustarPorta.x = -2.29f;
                    this.transform.localPosition = ajustarPorta;
                    once = true;
                }
                Vector3 openY = new Vector3(0, 0.01f, 0);
	            if (contar < 4)
	            {
	                this.transform.localPosition += openY;
	                contar++;
	            }
	            else
	            {
                    executeCloseAnim = false;
                    contar = 0;
	                once = false;
	            }

	        }

	    }

	}
}
