using System;
using System.Collections.Generic;
using System.Text;
class Graph
{
    //cols of the adjList represent directions
    //0 is left
    //1 is right
    //2 is down
    //3 is left
    int[,] adjList;
    public Graph(int V) {
        this.adjList = new int[V, 4];
        for(int i = 0; i < V; i++)
        {
            adjList[i, 0] = -1;
            adjList[i, 1] = -1;
            adjList[i, 2] = -1;
            adjList[i, 3] = -1;
        }
    }
    public int getAdj(int v,int dir)
    {
        return adjList[v, dir];
    }
}

