using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DragSprite : MonoBehaviour {

    private Collider2D novoCollider;
    private Vector2 mousePos;
    Vector3 offset;
    private Vector3 mPos;
    private Vector3 oldPosition;
    private int count = 0, contarLista, numeroSubEquacoes, numeroSubMatrizes;
    private string findArrayPosition, findIdName;
    private int arrayPos;
    List<ArrayMembros> novaLista;
    private bool cicle, executouAccao = true;
    private GameObject[] MatrizResolucao;
    private List<int> MatrizCorte = new List<int>();
    private List<int> MarcaPosicoes = new List<int>();
    private List<GameObject> ListaNormal = new List<GameObject>();
    private List<GameObject> ListaSubEquacoes;
    private List<GameObject> ListaReverse;
    private List<GameObject> NovaMatrizEquacao = new List<GameObject>();
    private List<GameObject> OldMatrizEquacao = new List<GameObject>();

    List<GameObject> recebeLista = new List<GameObject>();
    List<GameObject> tempInversa = new List<GameObject>();
    List<GameObject> ListaSubEquacao = new List<GameObject>();
    private bool mouseDrag = false;
    private bool mouseClick = false;
    private bool removeDuplaInversa = false;
    private BoxCollider2D box;
    public int ScoreSprite;
    // Update is called once per frame

    void saveOldPosition()
    {
        oldPosition = transform.position;
    }



    void OnMouseDown()
    {
        if (count == 0)
        {
            saveOldPosition();
            count++;
        }
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        novoCollider = Physics2D.OverlapPoint(mousePosition);

    }

    void OnMouseDrag()
    {
        mPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mPos) + offset;

        transform.position = objPosition;
        if (oldPosition != objPosition)
        {
            mouseDrag = true;

        }
    }

    List<GameObject> retornarListaGameObjects(List<GameObject> ListaRetornar)
    {
        Debug.Log("Contar Elementos Lista!!_" + ListaRetornar.Count);

        return new List<GameObject>(ListaRetornar);
    }


    void ReEscreverEquacao(List<GameObject> MatrizPrint)
    {
        int nElementos = MatrizPrint.Count;
        GameObject parent = GameObject.FindGameObjectWithTag("Equacoes");

        Transform parentTrans = parent.transform;
        int contarFilhos = parentTrans.childCount;
        Vector3 scale = new Vector3(20f, 20f, 20f);
        Vector3 currentPosition = new Vector3(-30f, 0f, 0f);
        bool lastInvTrans = false;
        bool lastParEsq = false;
        bool lastParDir = false;




        for (int i = 0; i < nElementos; i++)
        {
            MatrizPrint[i].transform.localScale = scale;

            if (i > 0)
            {
                if (MatrizPrint[i].tag == "Transposta" ||
      MatrizPrint[i].tag == "Inversas")
                {
                    currentPosition.y = 0.5f;

                    if (lastInvTrans)
                    {
                        if (lastParDir)
                        {
                            currentPosition += new Vector3(5f, 0f, 0f);
                            lastParDir = false;
                            //lastParEsq = false;
                        }
                        else
                        {
                            currentPosition += new Vector3(6f, 0f, 0f);

                        }

                    }
                    else
                    {
                        currentPosition += new Vector3(4f, 0f, 0f);

                    }
                    lastInvTrans = true;

                }
                else if (MatrizPrint[i].tag == "ParentesesDireito")
                {
                    if (lastInvTrans)
                    {
                        currentPosition += new Vector3(3f, 0f, 0f);
                        lastInvTrans = false;
                        lastParDir = true;
                    }
                    else
                    {
                        currentPosition += new Vector3(2f, 0f, 0f);
                        lastParEsq = true;
                    }


                }
                else if (MatrizPrint[i].tag == "ParentesesEsquerdo")
                {
                    if (lastInvTrans)
                    {
                        currentPosition += new Vector3(5f, 0f, 0f);
                        lastInvTrans = false;
                        lastParEsq = true;
                    }
                    else
                    {
                        currentPosition += new Vector3(4f, 0f, 0f);
                        lastParEsq = true;
                    }


                }
                else
                {
                    if (lastParEsq)
                    {
                        currentPosition += new Vector3(4f, 0f, 0f);
                        lastParEsq = false;
                    }
                    else if (lastInvTrans)
                    {
                        currentPosition += new Vector3(5f, 0f, 0f);
                        lastInvTrans = false;
                    }
                    else { currentPosition += new Vector3(6f, 0f, 0f); }

                }
            }

            GameObject tempMembro = (GameObject)Instantiate(MatrizPrint[i], currentPosition, Quaternion.identity);
            tempMembro.transform.name = MatrizPrint[i].name;
            tempMembro.transform.parent = parent.transform;

            //Debug.Log(tempMembro.transform.position);
        }

        if (executouAccao)
        {
            var children = new List<GameObject>();

            for (int co = 0; co < contarFilhos; co++)
            {
                children.Add(parentTrans.GetChild(co).gameObject);

            }

            children.ForEach(child => Destroy(child));
        }


    }

    string DaNomeMembroEquacao(string nomeTransformBox)
    {
        findIdName = nomeTransformBox.Substring(3, 8);
        findIdName = findIdName.TrimStart();
        findIdName = findIdName.TrimEnd();
        //Debug.Log("DaName_" + findIdName);

        return findIdName;
    }

    int DaIdMembroEquacao(string nomeTransformBox)
    {
        findArrayPosition = nomeTransformBox.Substring(0, 2);
        findArrayPosition = findArrayPosition.Trim();
        int idPosicao;
        if (int.TryParse(findArrayPosition, out idPosicao))
        {

        }
        else
        {
            idPosicao = 69;
        }
        //Debug.Log("Not number_" + idPosicao);
        return idPosicao;
    }

    List<GameObject> InverterLista(List<GameObject> TempListDesorganizada)
    {
        var reverseList = new GameObject[TempListDesorganizada.Count];
        TempListDesorganizada.CopyTo(reverseList);

        ListaSubEquacoes = new List<GameObject>(reverseList);
        ListaSubEquacoes.Reverse();

        return new List<GameObject>(ListaSubEquacoes);
    }

    private bool CheckIfInputIsInInnerEquation(int posicao)
    {
        GameObject parent = GameObject.FindGameObjectWithTag("Equacoes");


        int tamanho = parent.transform.childCount - 1;
        if (posicao < tamanho)
            return true;
        return false;
    }



    //check apenas resulta se estiver ja dentro de um parenteses

    private bool CheckIfMoreThanOneSubEquation(int posicao, int tamanhoLista, out int numeroMatrizesInteriores,
        out int numeroSubMatrizes)
    {
        int contarMatriz = 0, contarMatrizSub = 0, oldContarMatrizSub = 0;
        int contarParenteses = 0, contarElemento = 0;
        int subEquacoes = 0;
        bool abriu = false;
        int contarAbretura = 0;

        GameObject objectoInversa = new GameObject();

        GameObject parent = GameObject.FindGameObjectWithTag("Equacoes");



        for (int i = 0; i < tamanhoLista; i++)
        {
            if (parent.transform.GetChild(i).tag == "Inversas")
            {
                objectoInversa = parent.transform.GetChild(i).gameObject;
                break;
            }
        }

        List<GameObject> subEquacaoTemporaria = new List<GameObject>();
        List<GameObject> subEquacaoResolvida = new List<GameObject>();

        for (int i = posicao; i >= 0; i--)
        {
            //Debug.Log(novaLista[0].MembroEquacao[i].tag);

            if (parent.transform.GetChild(i).gameObject.tag != "ParentesesEsquerdo" &&
                parent.transform.GetChild(i).gameObject.tag != "ParentesesDireito")
            {
                if (abriu)
                {
                    subEquacaoTemporaria.Add(parent.transform.GetChild(i).gameObject);

                    if (parent.transform.GetChild(i).tag == "Matriz")
                    {
                        contarMatrizSub++;
                    }
                    contarElemento++;
                }
                else
                {

                    if (parent.transform.GetChild(i).tag == "Matriz")
                    {
                        contarMatriz++;
                    }
                    subEquacaoResolvida.Add(objectoInversa);
                    subEquacaoResolvida.Add(parent.transform.GetChild(i).gameObject);
                    contarElemento++;
                }
            }
            else if (parent.transform.GetChild(i).tag == "ParentesesEsquerdo")
            {

                if (abriu) //vai fechar Parenteses
                {
                    if (contarMatrizSub > 1)
                    {
                        subEquacaoResolvida.Add(objectoInversa);
                        foreach (GameObject objecto in subEquacaoTemporaria)
                        {
                            subEquacaoResolvida.Add(objecto);
                        }
                        subEquacoes++;
                        oldContarMatrizSub = contarMatrizSub; // conta o numero total de matrizes dentro de subsequacoes
                    }

                    contarAbretura--;

                    if (contarAbretura == 0)
                    {
                        abriu = false;
                    }
                    contarMatrizSub = 0;
                }
            }
            else if (parent.transform.GetChild(i).tag == "ParentesesDireito")
            {
                abriu = true;
                contarAbretura++;
            }

        }
        numeroMatrizesInteriores = contarMatriz;
        numeroSubMatrizes = subEquacoes;
        //Debug.Log("Matrizes Singulares Dentro de Equacao " + contarMatriz);
        //Debug.Log("Subequacoes " + subEquacoes);

        if (subEquacoes > 1)
        {
            return true;
        }
        if (subEquacoes >= 1 && contarMatriz >= 1)
        {
            return true;
        }
        else if (contarMatriz >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    List<GameObject> resolverInversaSubEquacao(int posicao, int tamanhoLista, GameObject pai)
    {
        int contarMatriz = 0, contarMatrizSub = 0, oldContarMatrizSub = 0;
        int contarParenteses = 0, contarElemento = 0;
        int subEquacoes = 0;
        bool abriu = false, esperaConfirmacaoQnaoEumParenteses = true;
        int contarAbretura = 0, oldContarAbertura = 0;
        GameObject objectoInversa = new GameObject();

        for (int i = 0; i < tamanhoLista; i++)
        {
            if (pai.transform.GetChild(i).tag == "Inversas")
            {
                objectoInversa = pai.transform.GetChild(i).gameObject;
                break;
            }
        }

        Debug.Log(7);

        List<GameObject> subEquacaoTemporaria = new List<GameObject>();
        List<GameObject> subEquacaoResolvida = new List<GameObject>();
        List<GameObject> subEquacaoFinal = new List<GameObject>();
        List<GameObject> subEquacaoTest = new List<GameObject>();


        for (int i = posicao; i > 0; i--)
        {
            //Debug.Log("Find Bug"+novaLista[0].MembroEquacao[i].tag + " i " + i + " nome " + novaLista[0].MembroEquacao[i].name);

            if (pai.transform.GetChild(i).tag != "ParentesesEsquerdo" && pai.transform.GetChild(i).tag != "ParentesesDireito")
            {

                if (abriu)
                {
                    subEquacaoTemporaria.Add(pai.transform.GetChild(i).gameObject);


                    if (pai.transform.GetChild(i).tag == "Matriz")
                    {
                        contarMatrizSub++;
                    }
                    else
                    {
                        contarElemento++;
                    }
                }
                else
                {
                    if (pai.transform.GetChild(i).tag == "Matriz")
                    {
                        contarMatriz++;
                        if (contarElemento > 1)
                        { // se já tiver mais uma inversa ou transposta 
                            subEquacaoFinal.Add(pai.transform.GetChild(i).gameObject);
                            subEquacaoTemporaria = InverterLista(subEquacaoTemporaria);
                            foreach (GameObject transInversa in subEquacaoTemporaria)
                            {
                                subEquacaoFinal.Add(transInversa);

                            }
                            subEquacaoFinal.Add(objectoInversa);
                            contarElemento = 0;
                            subEquacaoTemporaria.Clear();

                        }
                        else if (contarElemento == 1)
                        {
                            subEquacaoFinal.Add(pai.transform.GetChild(i).gameObject);
                            subEquacaoFinal.Add(subEquacaoTemporaria[0]);
                            subEquacaoFinal.Add(objectoInversa);
                            contarElemento = 0;
                            subEquacaoTemporaria.Clear();
                        }
                        else if (contarElemento == 0)
                        {
                            subEquacaoFinal.Add(pai.transform.GetChild(i).gameObject);
                            subEquacaoFinal.Add(objectoInversa);

                        }

                    }
                    else if (pai.transform.GetChild(i).tag == "Inversas" ||
                             pai.transform.GetChild(i).tag == "Transposta")
                    {
                        subEquacaoTemporaria.Add(pai.transform.GetChild(i).gameObject);
                        contarElemento++;
                        //Debug.Log("Devo entrar aqui a seguiar a posicao 4 tamanho: " + subEquacaoTemporaria.Count + " Elemento valor " + contarElemento);

                    }
                }
            }
            else if (pai.transform.GetChild(i).tag == "ParentesesEsquerdo")
            {
                if (abriu) //vai fechar Parenteses
                {
                    if (contarMatrizSub > 1)
                    {
                        subEquacaoTemporaria.Add(pai.transform.GetChild(i).gameObject); //adiciona ParEsquerdo
                        //subEquacaoResolvida.Add(objectoInversa);
                        foreach (GameObject objecto in subEquacaoTemporaria)
                        {
                            subEquacaoResolvida.Add(objecto);
                        }

                        subEquacaoResolvida = InverterLista(subEquacaoResolvida);

                        foreach (GameObject objecto in subEquacaoResolvida)
                        {
                            subEquacaoFinal.Add(objecto);
                        }
                        subEquacaoFinal.Add(objectoInversa);
                        subEquacaoTemporaria.Clear();
                        subEquacaoResolvida.Clear();
                        subEquacoes++;
                        oldContarMatrizSub = contarMatrizSub; // conta o numero total de matrizes dentro de subsequacoes
                        contarElemento = 0;

                    }

                    contarAbretura--;
                    if (contarAbretura == 0)
                    {
                        abriu = false;
                    }
                    //Debug.Log(" Abriu " + abriu + "Contar Abertura " + contarAbretura + " i " + i);

                    contarMatrizSub = 0;
                }
            }
            else if (pai.transform.GetChild(i).tag == "ParentesesDireito")
            {
                abriu = true;
                contarAbretura++;

                subEquacaoTemporaria.Add(pai.transform.GetChild(i).gameObject);
            }
            //Debug.Log("Contar Abertura " + contarAbretura + " i " + i + " name " + novaLista[0].MembroEquacao[i].name);

        }

        //foreach (GameObject debuggMatriz in subEquacaoFinal)
        //{
        //    Debug.Log(debuggMatriz.name + " tag " + debuggMatriz.tag);
        //}

        return new List<GameObject>(subEquacaoFinal);
    }

    void FixedUpdate()
    {
        if (mouseClick == true)
        {
            if (executouAccao)
            {
                NovaMatrizEquacao.Clear();
            }
            Debug.Log("Inicio valor de old matriz " + OldMatrizEquacao.Count);
            int ContarParenteses = 0;
            GameObject findTest = GameObject.FindGameObjectWithTag("Equacoes");
            //ScoreSprite = findTest.GetComponent<Spawn>().score;

            if (mouseDrag)
            {
                Debug.Log("Mouse Drag" + mouseDrag);
                int posicaoOrigem = novoCollider.transform.GetSiblingIndex(), posicaoPretendida = 0;
                GameObject tempInicio = new GameObject();
                GameObject tempFinal = new GameObject();
                bool resolverIdentidade = false;
                foreach (Transform child in findTest.transform)
                {

                    box = child.GetComponent<BoxCollider2D>();
                    if (box != null && box != novoCollider)
                    {
                        if (novoCollider.bounds.Intersects(box.bounds))
                        {
                            Debug.Log("Intersect_" + novoCollider.name + " box_" + box.name);
                            if (box.tag == "Matriz")
                            {
                                if (findTest.transform.GetChild(posicaoOrigem + 1).tag == "Inversas")
                                {
                                    if (findTest.transform.GetChild(posicaoOrigem + 2).name == box.name)
                                    {
                                        for (int i = 0; i < findTest.transform.childCount; )
                                        {

                                            if (i == posicaoOrigem)
                                            {
                                                i = i + 3;
                                            }
                                            else
                                            {
                                                NovaMatrizEquacao.Add(findTest.transform.GetChild(i).gameObject);
                                                i++;
                                            }
                                        }
                                        mouseDrag = false;
                                        mouseClick = false;
                                        OldMatrizEquacao = retornarListaGameObjects(NovaMatrizEquacao);
                                        findTest.GetComponent<Spawn>().updateScore(10);
                                        Debug.Log("Old Matriz " + OldMatrizEquacao.Count);
                                        ReEscreverEquacao(NovaMatrizEquacao);
                                        executouAccao = true;

                                    }
                                }
                                else if (findTest.transform.GetChild(posicaoOrigem + 1).name == box.name)
                                {
                                    if (findTest.transform.GetChild(posicaoOrigem + 2).tag == "Inversas")
                                    {
                                        for (int i = 0; i < findTest.transform.childCount; )
                                        {

                                            if (i == posicaoOrigem)
                                            {
                                                i = i + 3;
                                            }
                                            else
                                            {
                                                NovaMatrizEquacao.Add(findTest.transform.GetChild(i).gameObject);
                                                i++;
                                            }
                                        }
                                        mouseDrag = false;
                                        mouseClick = false;
                                        findTest.GetComponent<Spawn>().updateScore(10);
                                        OldMatrizEquacao = retornarListaGameObjects(NovaMatrizEquacao);
                                        ReEscreverEquacao(NovaMatrizEquacao);
                                        executouAccao = true;

                                        Debug.Log("Old Matriz " + OldMatrizEquacao.Count);
                                    }
                                }
                                Debug.Log("Index Box " + box.transform.GetSiblingIndex());
                                Debug.Log("novoCollider Index " + novoCollider.transform.GetSiblingIndex());
                                Debug.Log("Matriz Index " + novoCollider.transform.GetSiblingIndex());


                            }
                            mouseDrag = false;
                            mouseClick = false;
                            break;
                        }
                    }
                }
                mouseDrag = false;
                mouseClick = false;
                if (NovaMatrizEquacao.Count == 0)
                {
                    NovaMatrizEquacao = retornarListaGameObjects(OldMatrizEquacao);
                }
                OldMatrizEquacao = retornarListaGameObjects(NovaMatrizEquacao);

            }
            else
            {
                Debug.Log("1 ");
                foreach (Transform child in findTest.transform)
                {


                    box = novoCollider.GetComponent<BoxCollider2D>();


                    //if (box != null && box != novoCollider)
                    if (box != null)
                    {

                        //novaLista = box.GetComponentInParent<Spawn>().ListaEquacoes;

                        //findIdName = DaNomeMembroEquacao(box.transform.);
                        findIdName = box.tag;
                        arrayPos = transform.GetSiblingIndex();
                        //arrayPos = DaIdMembroEquacao(box.transform.name);
                        bool parSubParenteses = false;
                        bool InputDentroEquacao = CheckIfInputIsInInnerEquation(arrayPos);


                        Debug.Log("findIdName " + findIdName);
                        bool temSubEquacao = false;

                        if (findIdName == "Inversas")//(AB)^-1
                        {
                            Debug.Log("2 ");

                            int TempArrayPos;
                            Debug.Log("Entrei em inversa?");
                            string tempCheckPar = findTest.transform.GetChild(arrayPos - 1).tag;
                            //string tempCheckPar = novaLista[0].MembroEquacao[arrayPos - 1].tag;


                            int OldPositionParEsquerdoSubEquacaoFinal = 0; //recebe valor sempre que é percorrido parte das Subequações
                            int OldPositionParEsquerdoSubEquacaoInicial = arrayPos, contarDentroParenteses = 0;

                            #region Clique Na Inversa com Equacao Dentro de Parenteses

                            contarLista = findTest.transform.childCount;
                            //novaLista[0].MembroEquacao.Length;



                            // Debug.Log("tempCheckPar "+tempCheckPar +"arrayPos " + arrayPos +"arrayPos-1 "+ (arrayPos-1));

                            if (tempCheckPar == "ParentesesDireito") //(AB)^1 != AB-1
                            {
                                ContarParenteses++;
                                Debug.Log("3 ");

                                //Debug.Log("TESTES!! Novo Metodo");

                                int oldposition1 = 0, oldposition2 = 0, contar = 0, posicaoParEsq = 0, posicaoInversa = arrayPos;
                                bool encontrouInversaTransposta = false;
                                bool podeDecrementar = false;

                                parSubParenteses = CheckIfInputIsInInnerEquation(arrayPos);

                                OldPositionParEsquerdoSubEquacaoInicial = arrayPos;
                                GameObject objecto = new GameObject();
                                //Debug.Log("VALOR INICIAL "+ (arrayPos-2));                              

                                for (int i = arrayPos - 2; i >= 0; ) // -2 começa apos parenteses direito Corre até encontrar -1
                                { //pode correr if matriz, else ^-1 || ^T, Parenteses Esquerdo
                                    // Debug.Log("I devia ser 4 "+i);
                                    objecto = findTest.transform.GetChild(i).gameObject;


                                    if (i >= 0)
                                    {
                                        podeDecrementar = true;
                                    }
                                    else
                                    {
                                        podeDecrementar = false;
                                        break;
                                    }
                                    int posicaoObjecto;

                                    if (objecto.tag == "Matriz") //(A B
                                    {

                                        /*TODO Vou ter que fazer reverse a subequacao smpre que encontre um matriz com pelo menos uma inversa   */

                                        #region Tem matriz dentro de uma inversa com um parDireito A)^-1

                                        //Debug.Log("Matiz dentro de for_"+objecto.transform.tag+"_"+objecto.transform.name);

                                        if (encontrouInversaTransposta)
                                        {
                                            //Debug.Log("Inseri uma Matriz " + objecto.tag + "Contagem TempInversa_ " + tempInversa.Count);
                                            ListaNormal.Add(objecto); // adiciona a lista B
                                            bool test = false;
                                            if (tempInversa.Count > 1)
                                            {
                                                int ultimaPos = ListaNormal.Count - 1;
                                                recebeLista = InverterLista(tempInversa);

                                                for (int j = 0; j < recebeLista.Count; j++)
                                                {
                                                    ListaNormal.Add(recebeLista[j]);
                                                }
                                                //Debug.Log("Devia ter entrado aqui");
                                                test = true;
                                            }
                                            else
                                            {
                                                // Debug.Log("TempInversa maior que um = "+tempInversa[0].tag);
                                                ListaNormal.Add(tempInversa[0]);
                                            }



                                            ListaNormal.Add(findTest.transform.GetChild(arrayPos).gameObject);
                                            encontrouInversaTransposta = false;

                                            recebeLista.Clear();
                                            tempInversa.Clear();


                                            //Debug.Log("Removi tudo na TempInversa_ " + tempInversa.Count);
                                        }
                                        else
                                        {
                                            ListaNormal.Add(objecto); // adiciona a lista B
                                            ListaNormal.Add(findTest.transform.GetChild(arrayPos).gameObject);

                                        }
                                        i--;
                                        //add inversa com transform de posições erradas B^-1

                                        //if (contarDentroParenteses > 0) // mais tarde evitar (A)^-1
                                        //{
                                        //}

                                        #endregion
                                    }
                                    else if (objecto.tag == "Inversas" || objecto.tag == "Transposta") // (A B^-1 ou (A B^T ou (A B)^-1
                                    {
                                        //se tiver inversa ou transposta pode ser de uma equacao ou de um subequacao
                                        #region Tem inversa ou transposta logo a seguir de parDireito e inversa ^-1)^-1
                                        Debug.Log("4 ");

                                        //Debug.Log("ENTRO AQUI");

                                        if (podeDecrementar)
                                        {
                                            Debug.Log("5 ");

                                            GameObject tempObjectoCheckForParentesesDir =
                                                findTest.transform.GetChild(i - 1).gameObject;


                                            //Debug.Log("Check pardireito"+tempObjectoCheckForParentesesDir.tag);
                                            if (tempObjectoCheckForParentesesDir.tag == "ParentesesDireito") // )^T e )^-1 verifica se tem parentses direito a seguir a inversa ou transposta
                                            // tem que guardar uma SubEquacao ENCONTROU SUBEQUACAO
                                            {
                                                ListaNormal.Clear();
                                                Debug.Log("6 ");
                                                executouAccao = true;
                                                ListaNormal = resolverInversaSubEquacao(i, contarLista, findTest);
                                                temSubEquacao = true;

                                                foreach (GameObject test in ListaNormal)
                                                {
                                                    Debug.Log("Nome " + test.name + " tag " + test.tag);
                                                }

                                                break;
                                                //vou ter que correr que resolver a equacao toda

                                                #region Parenteses direito logo a seguir a transpota ou inversa

                                                #endregion
                                            }
                                            else // inversa ou transposta seguida de matriz  (B^-1
                                            {
                                                //em principio não é perciso marcar o -1 pois tem sempre uma matriz nesta secção do codigo logo
                                                //a seguir caso se vosse ) entrava na anterior
                                                encontrouInversaTransposta = true;
                                                tempObjectoCheckForParentesesDir = objecto;
                                                for (int j = i; j >= 0; )
                                                {

                                                    tempObjectoCheckForParentesesDir =
                                                findTest.transform.GetChild(j).gameObject;
                                                    //tempObjectoCheckForParentesesDir = novaLista[0].MembroEquacao[j];

                                                    if (tempObjectoCheckForParentesesDir.tag == "Inversas" ||
                                                        tempObjectoCheckForParentesesDir.tag == "Transposta")
                                                    {
                                                        tempInversa.Add(tempObjectoCheckForParentesesDir);
                                                        //Debug.Log(" Devia correr 2 vezes Tag " + tempObjectoCheckForParentesesDir.tag + " j " + j + " i " + i);
                                                        j--;
                                                        i--;

                                                    }
                                                    else
                                                    {
                                                        //Debug.Log(" Break Tag " + tempObjectoCheckForParentesesDir.tag + " j " + j);
                                                        break;
                                                    }

                                                }

                                            }
                                        }
                                        //break; //b

                                        #endregion
                                    }
                                    else if (objecto.tag == "ParentesesEsquerdo")
                                    {
                                        if (ContarParenteses % 2 == 0) //
                                        {
                                            parSubParenteses = true;
                                        }
                                        ContarParenteses--;

                                        if (parSubParenteses)
                                        {

                                            parSubParenteses = false;
                                        }
                                        OldPositionParEsquerdoSubEquacaoFinal = i;

                                        // alterei
                                        if (ContarParenteses <= 0)
                                        {
                                            parSubParenteses = true;
                                            break;
                                        }
                                        i--;
                                    }
                                    else if (objecto.tag == "ParentesesDireito")
                                    {
                                        ContarParenteses++;

                                        i--;
                                    }

                                }// Leu toda a matriz original


                            }
                            #endregion
                            else if (tempCheckPar == "Inversas")
                            {
                                //Debug.Log("Clicou na segunda Inversa!!");
                                removeDuplaInversa = true;

                            }
                            else
                            {
                                Debug.Log("Last Hope " + NovaMatrizEquacao.Count + " old " + OldMatrizEquacao.Count);

                                NovaMatrizEquacao = retornarListaGameObjects(OldMatrizEquacao);
                                executouAccao = false;

                                ReEscreverEquacao(NovaMatrizEquacao);

                            }

                            if (temSubEquacao)
                            {

                                foreach (GameObject add in ListaNormal)
                                {
                                    NovaMatrizEquacao.Add(add);
                                }
                                mouseClick = false;

                                ReEscreverEquacao(NovaMatrizEquacao);

                                findTest.GetComponent<Spawn>().updateScore(20);

                                temSubEquacao = false;
                            }
                            else if (removeDuplaInversa)
                            {
                                //Debug.Log("Clicou na segunda Inversa!!");
                                NovaMatrizEquacao.Clear();
                                for (int i = 0; i < contarLista; i++)
                                {
                                    if (i != arrayPos - 1)
                                    {
                                        if (i != arrayPos)
                                        {
                                            // Debug.Log(" Entrou com que I " + i + " " +(arrayPos - 1)+ " "+arrayPos);
                                            NovaMatrizEquacao.Add(findTest.transform.GetChild(i).gameObject);
                                        }
                                    }
                                }
                                // Debug.Log("Fechei Dupla Inversaa Inversa!!");
                                ReEscreverEquacao(NovaMatrizEquacao);

                                mouseClick = false;

                                removeDuplaInversa = false;
                            }
                            else
                            {

                                if (parSubParenteses)
                                {
                                    for (int i = 0; i < OldPositionParEsquerdoSubEquacaoFinal; i++)
                                    {

                                        NovaMatrizEquacao.Add(findTest.transform.GetChild(i).gameObject);
                                    }
                                }
                                foreach (GameObject novoElemento in ListaNormal)
                                {
                                    NovaMatrizEquacao.Add(novoElemento);
                                    //Debug.Log("Antes de insercao a frente "+novoElemento.name);
                                }


                                if (parSubParenteses)
                                {
                                    for (int i = OldPositionParEsquerdoSubEquacaoInicial + 1; i < contarLista; i++) // mais um para reescrever sobre a posicao inversa onde foi clickado
                                    {
                                        NovaMatrizEquacao.Add(findTest.transform.GetChild(i).gameObject);
                                        // NovaMatrizEquacao.Add(novaLista[0].MembroEquacao[i]);   
                                    }
                                }
                                parSubParenteses = false;
                                if (NovaMatrizEquacao.Count == 0)
                                {
                                    NovaMatrizEquacao = retornarListaGameObjects(OldMatrizEquacao);
                                }

                                findTest.GetComponent<Spawn>().updateScore(15);

                                OldMatrizEquacao = retornarListaGameObjects(NovaMatrizEquacao);
                                ReEscreverEquacao(NovaMatrizEquacao);
                                executouAccao = true;


                            }
                            transform.position = oldPosition;
                            mouseClick = false;
                            break;

                        }
                        else if (findIdName == "Transposta")
                        {
                            Debug.Log("Novo Collider tag " + novoCollider.transform.tag);

                            Debug.Log("Novo Collider parent tag " + novoCollider.transform.parent.tag);
                            Debug.Log("Box tag " + box.transform.tag);
                            Debug.Log("Box transform parent tag " + box.transform.parent.tag);
                            if (NovaMatrizEquacao.Count == 0)
                            {
                                NovaMatrizEquacao = retornarListaGameObjects(OldMatrizEquacao);
                            }
                            OldMatrizEquacao = retornarListaGameObjects(NovaMatrizEquacao);

                            ReEscreverEquacao(NovaMatrizEquacao);
                            transform.position = oldPosition;
                            mouseClick = false;
                            executouAccao = true;

                            break;

                        }


                        Debug.Log("Nome Child Transform Name " + child.transform.name);
                        Debug.Log("Nome NovoCollider Transform Name " + novoCollider.transform.name);
                        Debug.Log("Box  Transform Name " + box.name + " " + box.tag);


                        #region Clique Inversa


                        #endregion

                    }

                }
            }

            transform.position = oldPosition;
            mouseClick = false;
        }
    }



    void OnMouseUp()
    {
        mouseClick = true;
    }
}