using Kse.Algorithms.Samples;

bool IsEqual(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 10,
    Width = 15,
    Seed = 3,
    Noise= 0.1f,
});


string[,] map = generator.Generate();

var start = new Point(row: 2, column: 0);
var goal = new Point(row: 8, column: 14);

GetShortestPath(map, start, goal);



void GetShortestPath(string[,] map, Point start, Point goal)
{
    List<Point> result = BFS(start, goal);
    new MapPrinter().Print(map, result);
}

List<Point> BFS(Point start, Point goal)
{
    Dictionary<Point, Point?> origins = new Dictionary<Point, Point?>();
    var queue = new Queue<Point>();
    queue.Enqueue(start);
    origins.Add(start, null);

    while (queue.Count > 0)
    {
        var next = queue.Dequeue();
        var neighbours = GetNeighbours(next.Row, next.Column, map);
        foreach (var neighbour in neighbours)
        {
            if (!origins.TryGetValue(neighbour, out _))
            {
                origins.Add(neighbour, next);
                queue.Enqueue(neighbour);
            }
        }
    }

    List<Point> path = new List<Point>();
    Point current = goal;
    Console.WriteLine("|");
    Console.WriteLine(current);
    Console.WriteLine("|");

    while (!IsEqual(current, start)) 
    { 
        path.Add(current);
        origins.TryGetValue(current, out current);
    }
    path.Add(start);
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
        if (newX >= 0 && newY >= 0 && newX < maze.GetLength(0) && newY < maze.GetLength(1) && maze[newX, newY] != "█")
        {
            result.Add(new Point(newY, newX));
        }
    }
}