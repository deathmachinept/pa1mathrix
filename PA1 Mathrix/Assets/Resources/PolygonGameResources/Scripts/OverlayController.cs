using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverlayController : MonoBehaviour {
    
	void Start ()
	{
        transform.parent.GetComponent<AudioSource>().Play();
	    StartCoroutine(Smooth.Fade(transform.FindChild("Background").GetComponent<Image>(), 0, 3));

        StartCoroutine(Smooth.Fade(transform.FindChild("Logo").FindChild("hanka").GetComponent<Text>(), 0, 3));
        StartCoroutine(Smooth.Fade(transform.FindChild("Logo").FindChild("os").GetComponent<Text>(), 0, 3));
        StartCoroutine(Smooth.Fade(transform.FindChild("Logo").FindChild("slogan").GetComponent<Text>(), 0, 3));
    }
	
	// Update is called once per frame
	void Update () {
	    if (transform.FindChild("Background").GetComponent<Image>().color.a==0 && transform.FindChild("Logo").FindChild("hanka").GetComponent<Text>().color.a==0 && transform.FindChild("Logo").FindChild("os").GetComponent<Text>().color.a == 0 && transform.FindChild("Logo").FindChild("slogan").GetComponent<Text>().color.a == 0)
	    {
	        gameObject.SetActive(false);
	    }
	}
}
