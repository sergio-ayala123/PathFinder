using System;


namespace Shared
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool visited { get; set; }
        public bool isWall { get; set; }
        public bool isPath { get; set; }


        // A STAR ONLY
        public double gValue { get; set; } //DISTANCE FROM STARTING NODE
        public double hValue { get; set; } // HEURISTIC, DISTANCE FROM END NODE
        public double fValue { get; set; }// GVALUE + HVALUE
    }
    public class APIinput
    {
        public Cell start { get; set; }
        public Cell end { get; set; }
        public List<Cell> Path { get; set; }
    }


    public class Graph
    {
        private int vertices;
        private List<Cell>[] edges;
        public Graph(int v)
        {
            vertices = v;
            edges = new List<Cell>[v];

            for (int i = 0; i < v; i++)
            {
                edges[i] = new List<Cell>();
            }
        }

        public void addEdge(int c, Cell v)
        {
            edges[c].Add(v);
        }


        public void DFS(Cell v)
        {
            bool[] visited = new bool[vertices];
            Stack<Cell> stack = new Stack<Cell>();
            v.visited = true;
            stack.Push(v);

            while (stack.Count != 0)
            {
                v = stack.Pop();
                Console.WriteLine("next-> " + v.X);
                foreach (var i in edges[v.X])
                {
                    if (i.visited != true)
                    {
                        i.visited = true;
                        stack.Push(i);
                    }
                }
            }
        }

        public List<Cell> BFS(Cell start, Cell end, Cell[,] board)
        {
            Queue<Cell> frontier = new Queue<Cell>();
            List<Cell> visited = new List<Cell>();
            List<Cell> path = new List<Cell>();
            Dictionary<Cell, Cell> shortestPath = new Dictionary<Cell, Cell>();

            int currX;
            int currY;
            int rows = board.GetLength(0);
            int columns = board.GetLength(1);

            int[] rowdir = new int[4] { -1, 1, 0, 0 };
            int[] coldir = new int[4] { 0, 0, 1, -1 };

            frontier.Enqueue(start);

            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();
                if (current == end) break;
                for (int x = 0; x < 4; x++)
                {
                    currX = current.X + rowdir[x];
                    currY = current.Y + coldir[x];
                    if (currX < 0 || currY < 0) ;
                    else if (currX >= rows || currY >= columns) ;
                    else if (board[currX, currY].visited == true) ;
                    else if (board[currX, currY].isWall == true) ;
                    else
                    {
                        shortestPath.Add(board[currX, currY], current);
                        frontier.Enqueue(board[currX, currY]);
                        board[currX, currY].visited = true;
                        visited.Add(board[currX, currY]);
                    }
                }
            }
                path.Add(start);
                if (!shortestPath.ContainsKey(end))
                {
                    return new List<Cell>();
                }
                Cell value = shortestPath[end];
                path.Add(value);
                while (value != start)
                {
                    value = shortestPath[value];
                    path.Add(value);
                }
                return path;
        }



        class Program
        {
            static void Main(string[] args)
            {
                Graph g = new Graph(10);
                Cell[,] board = new Cell[3, 3];
                for (int x = 0; x < 3; x++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        board[x, i] = new Cell { X = x, Y = i };
                    }
                }

                board[0, 1].isWall = true;
                board[1, 1].isWall = true;
                //board[2, 0].isWall = true;
                List<Cell> path = g.BFS(board[0, 0], board[0, 2], board);



            }

        }
    }
}






