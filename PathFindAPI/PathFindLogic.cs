
using Shared;

namespace PathFindAPI
{
    public class PathFindLogic
    {


        public List<Cell> Path { get; set; } = new();
        public Cell[,] Board { get; set; }

        public void setBoard (int gridX, int gridY)
        {
            
            Board = new Cell[gridX, gridY];
            for(int x = 0; x < gridX; x++)
            {
                for(int i = 0; i < gridY; i++)
                {
                    Board[x, i] = new Cell { X = x, Y = i };
                    Board[x, i].visited = false;
                }
            }
            

        }
        public List<Cell> createMaze(int gridX, int gridY)
        {
            List<Cell> locations = new List<Cell>();
            Board = new Cell[gridX, gridY];

            for (int x = 0; x < gridX; x++)
            {
                for (int i = 0; i < gridY; i++)
                {
                    Board[x, i] = new Cell { X = x, Y = i };
                    Board[x, i].isWall = true;
                    Board[x, i].visited = false;

                }
            }
            Random rnd = new Random();

            int[] rowdir = new int[2] {-1, 0};
            int[] coldir = new int[2] {0, -1};
            for(int x = 0; x < gridX; x++)
            {
                for(int i = 0; i < gridY; i++)
                {
                var chance = rnd.Next(1, 3);
                if (chance == 1)
                {
                    if (x + rowdir[0] < 0) ;
                    else Board[x+rowdir[0], i].isWall = false;
                }
                else if(chance == 2)
                {
                    if (i + coldir[1] < 0) ;
                    else Board[x,i+coldir[1]].isWall = false;
                }
                    locations.Add(Board[x, i]);
                }
            }

           

            return locations;
        }
        public APIinput FindPath_AStar(APIinput input)
        { 
            PriorityQueue<Cell, double> openSet = new PriorityQueue<Cell, double>();
            List<Cell> closedSet = new List<Cell>();
            Dictionary<Cell,Cell> path = new Dictionary<Cell, Cell>();
            List<Cell> allVisited = new List<Cell> ();
            var start = input.start;
            var end = input.end;
            Cell minPath = new Cell();
            input.end.visited = true;
            input.start.visited = true;


            openSet.Enqueue(start, start.fValue);
            double pathGValue = 0;


            while(openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                closedSet.Add(current);
                if(current.X == end.X && current.Y == end.Y)
                {
                    break;
                }
                var currNeighbors = checkNeighborsAStar(current);
                
                foreach(var item in currNeighbors)
                {

                    if (closedSet.Contains(item)) continue;

                    if((item.X == end.X && item.Y == end.Y) || (item.X == start.X && item.Y == start.Y))
                    {
                        path.Add(item, current);
                        openSet.Enqueue(item, item.fValue);
                        continue;
                    }
                    var neighbor = gValue(heuristic(item, end),start);
                    neighbor.fValue = neighbor.gValue + neighbor.hValue;
                    var tentativeG = current.gValue;
                    
                    path.Add(neighbor, current);
                    

                    
                    openSet.Enqueue(neighbor, neighbor.fValue);
                    allVisited.Add(neighbor);
                    
                }
                

            }

            input.Path = new List<Cell>();
            

            foreach(var item in closedSet)
            {
                item.visited = true;
                input.Path.Add(item);
            }



            Cell value = path[path.LastOrDefault().Key];
            value.isPath = true;
            input.Path.Add(value);
            while (value != start)
            {
                value = path[value];
                value.isPath = true;
                input.Path.Add(value);
            }

            foreach(var item in allVisited)
            {
                input.Path.Add(item);
            }
            return input;
        }

        public List<Cell> checkNeighborsAStar(Cell current)
        {
            List<Cell> neighbors = new List<Cell>();

            int[] rowdir = new int[8] { -1, 1, 0, 0,-1,-1,1,1 };
            int[] coldir = new int[8] { 0, 0, 1, -1,-1,1,-1,1 };
            int curRow;
            int curCol;
            int rows = Board.GetLength(0);
            int cols = Board.GetLength(1);

            for(int x = 0; x < 8; x++)
            {
                curRow = current.X + rowdir[x];
                curCol = current.Y + coldir[x];
                if (curRow < 0 || curCol < 0) continue;
                if (curRow >= rows || curCol >= cols) continue ;
                
                else
                {
                    if (Board[curRow, curCol].isWall == true) continue ;
                    if (Board[curRow, curCol].visited == true) continue ;
                    else
                    {
                        Board[curRow, curCol].visited = true;
                        neighbors.Add(Board[curRow, curCol]);

                    }
                }
            }
            return neighbors;
        }

        public Cell heuristic(Cell current, Cell end)
        {
            int curRow = current.X;
            int curCol = current.Y;
           if(current.hValue > 0)
           {

           }
           else
           {
                if(current.X != end.X && current.Y != end.Y)
                {
                    var height = (Math.Abs(end.X - current.X));
                    var length = (Math.Abs(end.Y - current.Y));

                    current.hValue = Math.Sqrt((height*height) + (length*length)) ;
                }
                else if(current.X == end.X)
                {
                    current.hValue = Math.Abs(end.Y - current.Y);
                }
                else if(current.Y == end.Y)
                {
                    current.hValue = Math.Abs(end.X-current.X);
                }
           }
            Board[current.X, current.Y] = current;
            return current;
        }

        public Cell gValue(Cell current, Cell start)
        {
            int curRow = current.X;
            int curCol = current.Y;
             if (current.gValue > 0)
            {

            }
            else
            {
                if (current.X != start.X && current.Y != start.Y)
                {
                    var height = (Math.Abs(start.X - current.X));
                    var length = (Math.Abs(start.Y - current.Y));

                    current.gValue = Math.Sqrt((height * height) + (length * length));
                }
                else if (current.X == start.X)
                {
                    current.gValue = Math.Abs(start.Y - current.Y);
                }
                else if (current.Y == start.Y)
                {
                    current.gValue = Math.Abs(start.X - current.X);
                }

            }
            
            Board[current.X, current.Y] = current;
            return current;
        }


        public APIinput FindPathBFS(APIinput input)
        {

            foreach(var item in Board)
            {
                item.visited = false;
                item.isPath = false;
            }
            Queue<Cell> frontier = new Queue<Cell>();
            List<Cell> visited = new List<Cell>();
            List<Cell> path = new List<Cell>();
            Dictionary<Cell, Cell> shortestPath = new Dictionary<Cell, Cell>();

            int currX;
            int currY;
            int rows = Board.GetLength(0);
            int columns = Board.GetLength(1);

            int[] rowdir = new int[4] { -1, 1, 0, 0 };
            int[] coldir = new int[4] { 0, 0, 1, -1 };

            var start = Board[input.start.X, input.start.Y];
            var end = Board[input.end.X, input.end.Y];

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
                    else if (Board[currX, currY].visited == true) ;
                    else if (Board[currX, currY].isWall == true) ;
                    else
                    {
                        shortestPath.Add(Board[currX, currY], current);
                        frontier.Enqueue(Board[currX, currY]);
                        Board[currX, currY].visited = true;
                        visited.Add(Board[currX, currY]);
                    }
                }
            
            }

            path.Add(end);
            bool test = shortestPath.ContainsKey(end);

            if (!shortestPath.ContainsKey(end))
            {
                return new APIinput();
            }

            Cell value = shortestPath[end];
            value.isPath = true;
            path.Add(value);
            while (value != start)
            {
                value = shortestPath[value];
                value.isPath = true;
                path.Add(value);
            }
            input.Path = path;

            foreach(var cell in visited)
            {
                input.Path.Add(cell);
            }
            
            return input;
            
        }


    }
}
