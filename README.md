
![Nuget](https://img.shields.io/nuget/v/GraphToGrid)

# Graph To Grid

Given a _single_ undirected planar graph as well as an array of shapes, procedurally generates multiple unique layouts each mapped onto a 2d cartesian grid. These layouts can be used directly in your graphics framework or game engine of choice to represent maps in a 2d game, be it a Tiled or regular map. Implementation of an algorithm defined in a paper _'Game Level Layout from Design Specification'_ by [Chongyang Ma, et al.](http://chongyangma.com/publications/gl/index.html "Game Level Layout from Design Specification"). 

![Example Graph Output](https://github.com/Karatakos/graph-to-map/assets/6386987/27497c64-c991-4d65-bebf-5db450822661)

## Usage

```dotnet add package GraphToGrid```

Create blueprints representing shapes
```
RoomBlueprint squareBlueprint = new RoomBlueprint(
    points: new List<Vector2F>(
        new Vector2F[] {
            new Vector2F(0, 0), 
            new Vector2F(0, 10),
            new Vector2F(10, 10),
            new Vector2F(10, 0)}));
```

Create definitions that can be assigned to any given graph node to control available blueprints
```
RoomDefinition regularRoom = new RoomDefinition( 
    blueprints: new List<RoomBlueprint>() {
        squareBlueprint});
```

Create a graph assigning definitions to nodes and connections between nodes
```
LayoutGraph graph = new LayoutGraph();

graph.AddRoom(0, regularRoom);
graph.AddRoom(1, regularRoom);
graph.AddRoom(2, regularRoom);

graph.Connection(0, 1);
graph.Connection(1, 2);
```

Optionaly override default configuration 
```
GraphToGridConfiguration config = new GraphToGridConfiguration();

config.DoorWidth = 10;
config.DoorToCornerMinGap = 5;
config.TargetSolutions = 1;
config.Logger = LoggerFactory.Create(builder => {
    builder.AddSimpleConsole(options => {
        options.SingleLine = true;
    }).CreateLogger("GraphToGrid");
```

Initialize a layout generator.
```
LayoutGenerator generator = new LayoutGenerator(graph, config);
generator.Initialize(); 
```

Generate and retrieve a layout snapped to an integer grid useful for generating Tiled Maps
```
if (generator.TryGenerate()) {
    var layout = generator.Vend();

    layout.SnapToGrid(); 
}
```

## Contribute

Feel free to open an issue and reference it in a Pull Request. Please ensure there is a corresponding unit test and that the demo project is updated and renders as expected. The demo project is uses the Monogame SDK for rendering.

### Continuous Deployment

A GitHub workflow will deploy a new nuget package upon Pull Request merge to main. Publish has been set to ignore if the version matches an existing packge. Version can be updated via ```GraphToGrid.csproj```.
