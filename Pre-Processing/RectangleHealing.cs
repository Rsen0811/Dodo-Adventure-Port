using System;
using System.Collections.Generic;
using System.Text;
class RectangleHealing
{
    public static void Main(String[] args)
    {
        int[,] collisionMap =
        {
            {1,1,1},
            {1,0,1},
            {1,1,1}
        };
        int rows=3;
        int columns=3;
        
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
                    }
                }
            }
        }

    }
}
