using Kse.Algorithms.Samples;

bool IsEqual(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 40,
    Width = 40,
    Seed = 3,
    AddTraffic = true,
    TrafficSeed = 1,
    Noise=0.25f
});


string[,] map = generator.Generate();
var start = new Point(row: 0, column: 0);
var goal = new Point(row: 38, column: 39);
map[start.Column, start.Row] = " ";
map[goal.Column, goal.Row] = " ";
List<Point> Visited = new List<Point>();

new MapPrinter().Print(map, new List<Point>(), start, goal);
List<Point> path = AStar(start, goal, Visited);
Console.WriteLine($"\n Heuristic Visited {Visited.Count}\n");
new MapPrinter().Print(map, path, start, goal);


path = Dijkstra(start, goal, Visited);
Console.WriteLine($"\n Dijkstra Visited {Visited.Count}\n");
new MapPrinter().Print(map, path, start, goal);

int Heuristic(Point current, Point goal)
{
    return Math.Abs(current.Row - goal.Row) + Math.Abs(current.Column - goal.Column);
}

List<Point> AStar(Point start, Point goal, List<Point> Visited)
{
    Dictionary<Point, Point> origins = new Dictionary<Point, Point>();
    var queue = new PriorityQueue<Point, int>();
    queue.Enqueue(start, 0);
    Point next = start;
    int priority;
    float time = 0f;

    while (!IsEqual(next, goal) && queue.Count > 0)
    {
        var is_ok = queue.TryDequeue(out next, out priority);

        foreach (Point neighbour in GetNeighbours(next.Row, next.Column, map))
        {
            Visit(neighbour);
            if (!origins.TryGetValue(neighbour, out _))
            {
                int.TryParse(map[neighbour.Column, neighbour.Row], out int num);
                origins.Add(neighbour, next);
                queue.Enqueue(neighbour, priority + num + Heuristic(neighbour, goal));
            }
        }
    }


    List<Point> path = new List<Point>();
    Point current = goal;

    while (!IsEqual(current, start))
    {
        path.Add(current);
        origins.TryGetValue(current, out current);
        int.TryParse(map[current.Column, current.Row], out int num);
        time += 1 / (60 - (num - 1) * 6f);
    }

    path.Add(start);
    path.Reverse();
    Console.WriteLine($"\nRoad time - {time}\n");
    return path;
}

void Visit(Point point)
{
    Visited.Add(point);
}

List<Point> Dijkstra(Point start, Point goal, List<Point> Visited)
{
    Visited.Clear();
    Dictionary<Point, Point> origins = new Dictionary<Point, Point>();
    var queue = new PriorityQueue<Point, int>();
    queue.Enqueue(start, 0);
    Point next = start;
    int priority;
    float time = 0f;

    while (!IsEqual(next, goal) && queue.Count > 0)
    {
        var is_ok = queue.TryDequeue(out next, out priority);
        foreach (Point neighbour in GetNeighbours(next.Row, next.Column, map))
        {
            Visit(neighbour);
            if (!origins.TryGetValue(neighbour, out _))
            {
                int.TryParse(map[neighbour.Column, neighbour.Row], out int num);
                origins.Add(neighbour, next);
                queue.Enqueue(neighbour, priority + num);
            }
        }
    }

    List<Point> path = new List<Point>();
    Point current = goal;

    while (!IsEqual(current, start))
    {
        path.Add(current);
        origins.TryGetValue(current, out current);
        int.TryParse(map[current.Column, current.Row], out int num);
        time += 1 / (60 - (num - 1) * 6f);
    }

    path.Add(start);
    path.Reverse();
    return path;
}

List<Point> GetNeighbours(int row, int column, string[,] maze)
{
    var result = new List<Point>();
    TryAddWithOffset(1, 0);
    TryAddWithOffset(-1, 0);
    TryAddWithOffset(0, 1);
    TryAddWithOffset(0, -1);
    return result;

    void TryAddWithOffset(int offsetRow, int offsetColumn)
    {
        var newX = row + offsetRow;
        var newY = column + offsetColumn;
        if (newX >= 0 && newY >= 0 && newX < maze.GetLength(0) && newY < maze.GetLength(1) && maze[newY, newX] != "█")
        {
            result.Add(new Point(newY, newX));
        }
    }
}