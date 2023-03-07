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

void BFS(Point start)
{
    var visited = new List<Point>();
    var queue = new Queue<Point>();
    queue.Enqueue(start);
    Visit(start);
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
        if (newX >= 0 && newY >= 0 && newX < maze.GetLength(0) && newY < maze.GetLength(1) && maze[newX, newY] != "#")
        {
            result.Add(new Point(newY, newX));
        }
    }
}