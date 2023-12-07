
![Nuget](https://img.shields.io/nuget/v/Dungen)

# Dungen

Generate multiple two dimensional dungeon layouts based on an undirected input graph. Implementation of a novel algorithm defined in a paper by [Chongyang Ma, et al.](http://chongyangma.com/publications/gl/index.html "Game Level Layout from Design Specification"). 

## The Algorithm

The core algorithm takes advantage of a concept borrowed from robotics called Configuration Spaces, as well as a concept called Simulated Anealing.

At a high level:
 
1. Precomputes a set of lines for each of our shapes 
(rooms) relative to each other shape (room) defined in our graph. The lines dicate where a shape can move relative to another shape fixed in place in a way that the two shapes touch but do not intercept. 
2. Extracts faces from the undirected input graph so each face can be solved for individually. These are known as chains.
3. For each node in a given chain, based on the node's room blueprint, randomly picks a shape and places it in 2D cartesian space based on the previous node in the chain by using the two shapes' Configuration Space. We do this via DFS for all chains to naivly put together a baseline layout.
4. Simulated Annealing is leveraged to generate hundreds of new layouts from our baseline layout, each layout is validated against our set constraints (touch but do not intercept). Valid layouts are made available to the calling code immediately.

## Usage

```dotnet add package Dungen```

Create one or more blueprints that define a rooms shape.
```
RoomBlueprint normalRoomBlueprint = new RoomBlueprint(
    points: new List<Vector2F>(
        new Vector2F[] {
            new Vector2F(x, y), 
            new Vector2F(x, -y),
            new Vector2F(-x, -y),
            new Vector2F(-x, y)}));
```

Create a room definition. A room definition can contain one or many blueprints.
```
RoomDefinition normalRoom = new RoomDefinition( 
    blueprints: new List<RoomBlueprint>() {
        normalRoomBlueprint},
    type: RoomType.Normal);
```

Create a new (undirected) input graph representing the layout.
```
DungenGraph graph = new DungenGraph();
graph.AddRoom(0, normal);
graph.AddRoom(1, normal);
graph.AddRoom(2, normal);
```

Define input options.
```
DungenGeneratorProps props = new DungenGeneratorProps();
props.DoorWidth = 10;
props.DoorToCornerMinGap = 5;
props.Graph = graph;
props.TargetSolutions = 1;
```

Initialize a new dungeon generator.
```
DungenGenerator generator = new DungenGenerator(props);
generator.Initialize(); 
```

Attempt to generate a dungeon!
```
if (generator.TryGenerate())
    var dungeon = generator.Vend();
```

### Logging

Pass the generator an ```ILoggerFactory``` via ```DungenGeneratorProps``` before initializing. For example:

```
DungenGeneratorProps props = new DungenGeneratorProps();
props.LoggerFactory = LoggerFactory.Create(builder => {
    builder.AddSimpleConsole(options => {
        options.SingleLine = true;
    });
});
```

## Contribute

Please open an issue and reference it in a Pull Request. 

Ensure there is a corresponding unit test and that the test render it updated and runs (where applicable). Using Monogame SDK to render a test layout.

### Package Deployment

A GH workflow will deploy a new nuget package upon PR merge to main. Publish has been set to ignore if the version matches an existing packge. Version can be updated via ```Dungen.csproj```.