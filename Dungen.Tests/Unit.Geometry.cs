namespace Dungen.Generator.Tests;

using System.Collections.Generic;
using NUnit.Framework;

using Dungen;

[TestFixture]
public class Layouts
{
    private DungenGraph _graph;
    private RoomBlueprint _smallSquareRoomBlueprint;
    private RoomBlueprint _squareRoomBlueprint;
    private RoomDefinition _regularRoomDefinition;

    [SetUp]
    public void Setup()
    {
        float width = 20/2;
        float height = 20/2;

        _squareRoomBlueprint = new RoomBlueprint(
            points: new List<Vector2F>(
                new Vector2F[] {
                    new Vector2F(width, height), 
                    new Vector2F(width, -height),
                    new Vector2F(-width, -height),
                    new Vector2F(-width, height)}));

        width = 10/2;
        height = 10/2;
        
        // Rectangular normal room 
        //
        _smallSquareRoomBlueprint = new RoomBlueprint(
            points: new List<Vector2F>(
                new Vector2F[] {
                    new Vector2F(-25, -25), 
                    new Vector2F(-25, -15),
                    new Vector2F(-15, -15),
                    new Vector2F(-15, -25)}));

        _regularRoomDefinition = new RoomDefinition( 
            blueprints: new List<RoomBlueprint>() {
                _smallSquareRoomBlueprint},
            type: RoomType.Normal);

        _graph = new DungenGraph();

        _graph.AddRoom(0, _regularRoomDefinition);
        _graph.AddRoom(1, _regularRoomDefinition);

        _graph.AddConnection(0, 1);
    }

    [Test]
    public void RoomDistance()
    {
        Room room1 = new Room(_squareRoomBlueprint, RoomType.Normal);
        Room room2 = new Room(_squareRoomBlueprint, RoomType.Normal);
        room2.Translate(new Vector2F(11, 11));

        Assert.That(
            Room.ComputeRoomDistance(room1, room2), 
            Is.EqualTo(9.0F));
    }

    [Test]
    public void RoomCollisionArea()
    {
        Room room1 = new Room(_squareRoomBlueprint, RoomType.Normal);
        Room room2 = new Room(_squareRoomBlueprint, RoomType.Normal);

        room2.Translate(new Vector2F(2, 2));

        Assert.That(
            Room.ComputeRoomCollisionArea(room1, room2),
            Is.EqualTo(324F));

        room2.Translate(-room2.GetCenter());
        room2.Translate(new Vector2F(10,10));

        Assert.That(
            Room.ComputeRoomCollisionArea(room1, room2),
            Is.EqualTo(99.9999924F));
    }

    [Test]
    public void RoomWallContactArea()
    {
        Room room1 = new Room(_squareRoomBlueprint, RoomType.Normal);
        Room room2 = new Room(_squareRoomBlueprint, RoomType.Normal);

        room2.Translate(new Vector2F(10, 5));

        Assert.That(
            Room.ComputeRoomContactArea(room1, room2),
            Is.EqualTo(0F));
    }

    [Test]
    public void LayoutHasCorrectDimenions()
    {
        var vertices = _graph.Vertices.ToArray();

        Layout layout = new Layout(new Layout(1), _graph);

        Room r1 = new Room(_smallSquareRoomBlueprint, RoomType.Normal, 0);
        Room r2 = new Room(_smallSquareRoomBlueprint, RoomType.Normal, 1);

        layout.Rooms.Add(vertices[0], r1);
        layout.Update(vertices[0], r1);

        layout.Rooms.Add(vertices[1], r2);
        layout.Update(vertices[1], r2);

        Assert.That(layout.Width, Is.EqualTo(20));
        Assert.That(layout.Height, Is.EqualTo(20));
    }
}