using Kse.Algorithms.Samples;

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

var my_result = GetShortestPath(map, start, goal);



List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    List<Point> result = new List<Point>();
    result.Add(start);

    result.Add(goal);
    new MapPrinter().Print(map, result);
    return result;
}

void BFS(Point point)
{
    var visited = new List<Point>();
    var queue = new Queue<Point>();
    Visit(point);
    queue.Enqueue(point);
    while (queue.Count > 0)
    {
        var next = queue.Dequeue();
        var neighbours = GetNeighbours(next.Row, next.Column, map);
        foreach (var neighbour in neighbours)
        {
            if (!visited.Contains(neighbour))
            {
                Visit(neighbour);
                queue.Enqueue(neighbour);
            }
        }
    }

    void Visit(Point point)
    {
        map[point.Row, point.Column] = "1";
        visited.Add(point);
    }
}
