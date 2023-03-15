using Kse.Algorithms.Samples;

bool IsEqual(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}

//void PrintMapWithPath(string[,] map, List<Point> path, Point start, Point goal)
//{
//    foreach (Point p in path)
//    {
//        if (IsEqual(p, start))
//        {
//            map[p.Column, p.Row] = "A";
//        }
//        else if (IsEqual(p, goal))
//        {
//            map[p.Column, p.Row] = "B";
//        }
//        else
//        {
//            map[p.Column, p.Row] = ".";
//        }
//    }

//    new MapPrinter().Print(map);
//}

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 10,
    Width = 15,
    Seed = 2,
    AddTraffic = true,
    TrafficSeed = 1
});


string[,] map = generator.Generate();
var start = new Point(row: 0, column: 0);
var goal = new Point(row: 6, column: 9);

new MapPrinter().Print(map, new List<Point>(), start, start);
List<Point> path = BFS(start, goal);
new MapPrinter().Print(map, path, start, goal);

int Heuristic(Point current, Point goal)
{
    return Math.Abs(current.Row - goal.Row) + (current.Column - goal.Column);
}

List<Point> BFS(Point start, Point goal)
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
            if (!origins.TryGetValue(neighbour, out _))
            {
                int num;
                int.TryParse(map[neighbour.Column, neighbour.Row], out num);
                origins.Add(neighbour, next);
                queue.Enqueue(neighbour, priority + num + Heuristic(neighbour, goal));
                time += 1 / (60 - (num - 1) * 6f);
            }
        }
    }

    List<Point> path = new List<Point>();
    Point current = goal;

    while (!IsEqual(current, start))
    {
        path.Add(current);
        origins.TryGetValue(current, out current);
    }

    path.Add(start);
    path.Reverse();
    Console.WriteLine($"\nRoad time - {time}\n");
    return path;
}

//List<Point> BFS(Point start, Point goal)
//{
//    Dictionary<Point, Point> origins = new Dictionary<Point, Point>();
//    var queue = new PriorityQueue<Point, int>();
//    queue.Enqueue(start, 0);
//    Point next = start;
//    int priority;

//    while (!IsEqual(next, goal) && queue.Count > 0)
//    {
//        var is_ok = queue.TryDequeue(out next, out priority);
//        foreach (Point neighbour in GetNeighbours(next.Row, next.Column, map))
//        {
//            if (!origins.TryGetValue(neighbour, out _))
//            {
//                origins.Add(neighbour, next);
//                queue.Enqueue(neighbour, priority + 1);
//            }
//        }
//    }

//    List<Point> path = new List<Point>();
//    Point current = goal;

//    while (!IsEqual(current, start))
//    {
//        path.Add(current);
//        origins.TryGetValue(current, out current);
//    }

//    path.Add(start);
//    path.Reverse();
//    return path;
//}

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