using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PathFindingMain.Services;
using Shared;
using System.Net.Http.Headers;
namespace PathFindingMain.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IPathFindService pathFindService;
        public MainGrid Grid;
        public IndexModel(ILogger<IndexModel> logger, IPathFindService pathFindService, MainGrid Grid)
        {
            this.logger = logger;
            this.pathFindService = pathFindService;
            this.Grid = Grid;
        }

        [BindProperty]
        public Cell start { get; set; }

        [BindProperty]
        public Cell end { get; set; }

        public List<Cell> locations { get; set; } = new();


        [BindProperty]
        public int UserX { get; set; }

        [BindProperty]
        public int UserY { get; set; }

        public string Status { get; set; }


        public char[,] grid = new char[0, 0];

        public void OnGet()
        {
            Grid.Grid = new char[0, 0];
        }

        public async Task OnPostGrid(int userx, int usery)
        { 
            Grid.Grid = new char[userx, usery];

            await pathFindService.sendBoard(userx,usery);  
        }


        public void ClearGrid()
        {
            for(int x = 0; x < Grid.Grid.GetLength(0); x++)
            {
                for(int i = 0; i < Grid.Grid.GetLength(1); i++)
                {
                    if(Grid.Grid[x,i] == 'S' || Grid.Grid[x, i] == 'E' ||Grid.Grid[x, i] == 'P' || Grid.Grid[x,i] == 'V')
                    {
                        Grid.Grid[x, i] = ' ';
                    }
                }
            }
        }

        public async Task OnPostBFS(Cell start, Cell end)
        {
            ClearGrid();
            var test = await pathFindService.returnPathBFS(start, end);
            if (test.Path == null)
            {
                Status = "Path Could Not Be Found";
            }
            else
            {

                foreach (var item in test.Path)
                {
                    if (item.X == start.X && item.Y == start.Y)
                    {
                        Grid.Grid[item.X, item.Y] = 'S';
                    }
                    else if (item.X == end.X && item.Y == end.Y)
                    {
                        Grid.Grid[item.X, item.Y] = 'E';
                    }
                    else if (item.visited == true && item.isPath == false)
                    {
                        Grid.Grid[item.X, item.Y] = 'V';
                    }
                    else if (item.isPath == true)
                    {
                        Grid.Grid[item.X, item.Y] = 'P';

                    }

                    Status = "Path Has Been Found";
                }
            }
        }
        public async Task OnPostAStar(Cell start, Cell end)
        {
            var path = await pathFindService.returnPath_AStar(start, end);
            foreach(var item in path.Path)
            {
                if(item.visited == true && item.isPath == false)
                {
                    Grid.Grid[item.X, item.Y] = 'V';
                }
                else if(item.isPath == true)
                {
                    Grid.Grid[item.X, item.Y] = 'P';
                }
                else
                {
                    Grid.Grid[item.X, item.Y] = ' ';
                }
            }
        }

        public async Task OnPostCreateMaze(int gridX, int gridY)
        {
            var maze = await pathFindService.createMaze(gridX, gridY);
            foreach(var item in maze)
            {
                if(item.isWall == true)
                {
                    Grid.Grid[item.X, item.Y] = 'W';

                }
                
            }
        }
    }
}