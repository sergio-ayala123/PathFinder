using Shared;
using System.Net.Http.Json;
namespace PathFindingMain.Services
{
    public interface IPathFindService
    {
        Task<APIinput> returnPathBFS(Cell start, Cell end);
        Task<APIinput> returnPath_AStar(Cell start, Cell end);
        Task sendBoard(int gridX, int gridY);
        Task<List<Cell>> createMaze(int gridX, int gridY);
    }

    public class PathFinder : IPathFindService
    {
        private readonly HttpClient httpClient;

        public PathFinder(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Cell>> createMaze(int gridX, int gridY)
        {
            var result = await httpClient.GetFromJsonAsync<List<Cell>>($"http://localhost:5066/CreateMaze?gridX={gridX}&&gridY={gridY}");

            return result;
        }

        public async Task<APIinput> returnPathBFS(Cell start, Cell end)
        {
            APIinput input = new APIinput();
            input.start = start;
            input.end = end;
            var test = await httpClient.PostAsJsonAsync($"http://localhost:5066/FindPathBFS", input);
            var path =  test.Content.ReadFromJsonAsync<APIinput>();
            return path.Result;
        }

        public async Task<APIinput> returnPath_AStar(Cell start, Cell end)
        {
            APIinput input = new APIinput();
            input.start = start;
            input.end = end;
            var test = await httpClient.PostAsJsonAsync($"http://localhost:5066/FindPath_AStar", input);
            var path = test.Content.ReadFromJsonAsync<APIinput>();
            return path.Result;
        }
        public async Task sendBoard(int gridX, int gridY)
        {
            await httpClient.GetAsync($"http://localhost:5066/ReceiveBoard?gridX={gridX}&&gridY={gridY}");

        }
    }
}
