using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

class RectangleHealing
{
    int rows=3;
    int columns=3;
    public static void Main(String[] args)
    {
        int[,] collisionMap =
        {
            {1,1,1},
            {1,0,1},
            {1,1,1}
        };
        
        
        //adding map to the graph
        Graph map = new Graph(rows*columns);
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                if (collisionMap[i, j] == 1)
                {
                    if (i > 0 && collisionMap[i - 1, j] == 1) 
                    {
                        map.addEdge(i*rows+j, (i-1)*rows +j, Direction.UP);
                    }
                    if (i < rows-1 && collisionMap[i + 1, j] == 1)
                    {
                        map.addEdge(i * rows + j, (i + 1) * rows + j, Direction.DOWN);
                    }
                    if (j > 0 && collisionMap[i, j-1] == 1)
                    {
                        map.addEdge(i * rows + j, i * rows + j-1, Direction.LEFT);
                    }
                    if (i > 0 && collisionMap[i, j+1] == 1)
                    {
                        map.addEdge(i * rows + j, i * rows + j+1, Direction.RIGHT);
                    }s
                }
            }
        }

        //DFS the graph weirdly
        bool[] visited=new bool[rows*columns];
        List<Rect> rects=new List<Rect>();
                
    }
    public static void DFS(Graph graph,int currNode, bool[] visited, List<Rect> rects, int currDir,Rect currRect)
    {
        if (graph.doesExist(currNode) && !visited[currNode])
        {
            visited[currNode]=true;
            int sameDir= graph.getAdj(currNode,currDir)
            if (sameDir != -1)){
                Rect newRect;
                int row =currNode/columns;
                int col = currNode%columns;
                if(currRect == null)
                {    
                    //assuming tile size is one
                    newRect= new Rect(col-1,col,row,row+1);
                    DFS(graph, sameDir, visited, rects, currDir, newRect);
                }
                else
                {
                    if (currDir = Direction.RIGHT)
                    {
                        newRect = new Rect(col - 1, currRect.xMax, row, row+1);
                        DFS(graph, sameDir, visited, rects, currDir, newRect);
                    }
                    else if (currDir = Direction.LEFT)
                    {
                        newRect = new Rect(currRect.xMin, col, row, row + 1);
                        DFS(graph, sameDir, visited, rects, currDir, newRect);
                    }
                    else if(currDir = Direction.UP)
                    {
                        newRect = new Rect(col - 1, col, row, currRect.yMax);
                        DFS(graph, sameDir, visited, rects, currDir, newRect);
                    }
                    else if(currDir = Direction.DOWN)
                    {
                        newRect = new Rect(col - 1, col, currRect.yMin, row + 1);
                        DFS(graph, sameDir, visited, rects, currDir, newRect);
                    }
                }
            }
        }
        visited[currNode]=true;
        for(int i = 0; i < visited.Length; i++)
        {
            if (!visited[i])
            { 
                return DFS(graph,i,visited,rects,-1,null);
            }        
        }
    }
    class Rect
    {
        public int xMin;
        public int xMax;
        public int yMin;
        public int yMax;
        public Rect(xMin, xMax, yMin, yMax)
        {
            this.xMax =xMax
            this.yMin=yMin;
            this.xMin=xMin;
            this.yMax=xMax;
        }
     }
}
