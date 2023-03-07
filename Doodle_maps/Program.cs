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