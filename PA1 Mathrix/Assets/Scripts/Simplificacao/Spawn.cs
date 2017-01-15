using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour
{

    public class equacao { 
        public GameObject[] MembroEquacao;
    }
    public equacao[] CollecaoEquacaos;

    public GameObject[] ResolucaoEquacao;

    public List<ArrayMembros> ListaEquacoes;

    private List<GameObject> listaFilhosTemp = new List<GameObject>();
    private List<GameObject> listaFilhosScript;

    private GameObject DuplicateSprite;

    private Vector3 scale = new Vector3(1f,1f,1f);
    public Text scoreText;
    private List<String> listaTips;
    private float timerForNextTip;
    private bool lastParDir;
    public Text tip;
    public int posicao;

    public int score;

	void Start ()
	{
	    posicao = 0;
	    lastParDir = false;
        timerForNextTip = 7f;
        listaTips = new List<string>();
        ListaEquacoes = new List<ArrayMembros>();
        ListaEquacoes.Add(GameObject.Find("saberEquacao1").GetComponent<ArrayMembros>());

        Vector3 currentPosition = new Vector3(-30f, 0f, 0f);
	    int contar;
        contar = ListaEquacoes[0].MembroEquacao.Length;
	    //contar = CollecaoEquacaos[0].MembroEquacao.Length;

        ResolucaoEquacao = new GameObject[contar];
        bool lastInvTrans = false;
        bool lastParEsq = false;
        addTips();

        for (int i = 0; i < contar; i++)
        {
            ListaEquacoes[0].MembroEquacao[i].transform.localScale = scale;

            if (i > 0)
            {
                if (ListaEquacoes[0].MembroEquacao[i].tag == "Transposta" ||
      ListaEquacoes[0].MembroEquacao[i].tag == "Inversas")
                {
                    currentPosition.y = 4f;

                    if (lastInvTrans)
                    {
                        if (lastParDir)
                        {
                            currentPosition += new Vector3(5f, 0f, 0f);
                            lastParDir = false;
                        }
                        else
                        {
                            currentPosition += new Vector3(6f, 0f, 0f);

                        }

                    }
                    else
                    {
                        currentPosition += new Vector3(8f, 0f, 0f);

                    }
                    lastInvTrans = true;

                }
                else if (ListaEquacoes[0].MembroEquacao[i].tag == "ParentesesDireito")
                {
                    if (lastInvTrans)
                    {
                        currentPosition.y = 0f;

                        currentPosition += new Vector3(8f, 0f, 0f);
                        lastInvTrans = false;
                        lastParDir = true;
                    }
                    else
                    {
                        currentPosition += new Vector3(8f, 0f, 0f);
                        lastParDir = true;
                    }

                }
                else if (ListaEquacoes[0].MembroEquacao[i].tag == "ParentesesEsquerdo")
                {
                    if (lastInvTrans)

                    {
                        currentPosition.y = 0f;

                        currentPosition += new Vector3(8f, 0f, 0f);
                        lastInvTrans = false;
                        lastParEsq = true;
                    }
                    else
                    {
                        currentPosition += new Vector3(8f, 0f, 0f);
                        lastParEsq = true;
                    }
                    //Debug.Log("ParEsquerdo " + currentPosition);

                } 
                else
                {
                    currentPosition.y = 0f;
                    if (lastParEsq)
                    {
                        currentPosition += new Vector3(8f, 0f, 0f);
                        lastParEsq = false;
                    }
                    else if (lastInvTrans)
                    {
                        currentPosition += new Vector3(5f, 0f, 0f);
                        lastInvTrans = false;
                    }
                    else { currentPosition += new Vector3(12f, 0f, 0f); }

                }
            }

            GameObject tempMembro = (GameObject)Instantiate(ListaEquacoes[0].MembroEquacao[i], currentPosition, Quaternion.identity);
            tempMembro.transform.name =  ListaEquacoes[0].MembroEquacao[i].name + i;
            tempMembro.transform.parent = gameObject.transform;

        }
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        tip.text = "";
	}

    public void updateScore(int addScore)
    {
        score += addScore;
    }

    void addTips()
    {

        listaTips.Add("Ajuda - Para unir uma Matriz do mesmo tipo com a sua inversa, arraste uma dessa matriz para cima da outra.");
        listaTips.Add("Ajuda - Quando encontrar uma matriz duplamente invertida click no -1 para a simplificar.");
        listaTips.Add("Ajuda - Para aplicar uma inversa a um conjunto de várias matrizes basta clicar no -1.");
        listaTips.Add("");


    }

    void Update()
    {
        timerForNextTip -= Time.deltaTime;
        if (timerForNextTip < 0)
        {
            if (posicao >= 2)
            {
                posicao = 0;
            }
            else
            {
                posicao++;
            }
            tip.text = listaTips[posicao];
            timerForNextTip = 7f;
        }
        scoreText.text = "Score: " + score.ToString();
    }
}