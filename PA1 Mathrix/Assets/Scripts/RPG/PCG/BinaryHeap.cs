using UnityEngine;
using System.Collections;


public class BinaryHeap {

	// Use this for initialization
    public ArrayList h;

    //BinaryHeap()
    public BinaryHeap()
    {
        h = new ArrayList();
    }

    public void insert(Node n)
    {
        h.Add(n);
        moveUp(h.Count - 1);
    }

    public Node remove(int pos)
    {
        Node n = (Node)h[pos];
        //Guarda elemento antes de ser eliminado
        Node last = (Node) h[h.Count - 1];

        h.RemoveAt(h.Count - 1); 

        if (pos == h.Count) return last;
        if (h.Count != 0 || h!=null) // nao for vazio
        {
            h[pos] = last;
            heapify(pos);
        }
        return n;
    }
    public void heapify(int pos)
    {
        while (pos < h.Count / 2)
        {
            int child = 2 * pos + 1; // divide a arvore e cria novos ramos 
            Node n = (Node)h[pos];
            Node nc = (Node)h[child];
            Node nc2;
            if (child < h.Count - 1)
            {
                nc2 = (Node)h[child + 1];
                if (nc.getF > nc2.getF) child++;
                nc = (Node)h[child];
            }
            if (n.getF <= nc.getF) break;
            Node tmp = (Node)h[pos];
            h[pos] = (Node)h[child];
            h[child] = tmp;
            pos = child;
        }
    }

    public void moveUp(int pos)
    {
        while (pos > 0)
        {
            int parent = (pos - 1) / 2;
            Node n = (Node)h[pos];
            Node np = (Node)h[parent];

            if (n.getF >= np.getF) break;
            Node tmp = (Node)h[pos];
            h[pos] = (Node)h[parent];
            h[parent] = tmp;
            pos = parent;
        }
    }

    public int find(int x, int y)
    {
        for (int i = 0; i < h.Count; i++)
        {
            Node n = (Node)h[i];
            if (n.getX == x && n.getY == y) return i;
        }
        return -1;
    }

    public int getSize()
    {
        return h.Count;
    }
}
