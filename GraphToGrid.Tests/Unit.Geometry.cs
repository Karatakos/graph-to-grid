namespace GraphToGrid.Tests;

using System.Collections.Generic;
using NUnit.Framework;

using GraphToGrid;

[TestFixture]
public class Layouts
{
    private LayoutGraph _graph;
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

        _graph = new LayoutGraph();

        _graph.AddRoom(0, _regularRoomDefinition);
        _graph.AddRoom(1, _regularRoomDefinition);

        _graph.AddConnection(0, 1);
    }

    [Test]
    public void RoomDistance()
    {
        Room room1 = new Room(_squareRoomBlueprint);
        Room room2 = new Room(_squareRoomBlueprint);
        room2.Translate(new Vector2F(11, 11));

        Assert.That(
            Room.ComputeRoomDistance(room1, room2), 
            Is.EqualTo(9.0F));
    }

    [Test]
    public void RoomCollisionArea()
    {
        Room room1 = new Room(_squareRoomBlueprint);
        Room room2 = new Room(_squareRoomBlueprint);

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
        Room room1 = new Room(_squareRoomBlueprint);
        Room room2 = new Room(_squareRoomBlueprint);

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

        Room r1 = new Room(_smallSquareRoomBlueprint, 0);
        Room r2 = new Room(_squareRoomBlueprint, 1);

        layout.Rooms.Add(vertices[0], r1);
        layout.Update(vertices[0], r1);

        layout.Rooms.Add(vertices[1], r2);
        layout.Update(vertices[1], r2);

        Assert.That(layout.Width, Is.EqualTo(35));
        Assert.That(layout.Height, Is.EqualTo(35));
        
        Assert.That(layout.BoundingBox.Min.x, Is.EqualTo(-25));
        Assert.That(layout.BoundingBox.Min.y, Is.EqualTo(-25));
        Assert.That(layout.BoundingBox.Max.x, Is.EqualTo(10));
        Assert.That(layout.BoundingBox.Max.y, Is.EqualTo(10));

        Assert.That(layout.Center, Is.EqualTo(new Vector2F((float)-7.5, (float)-7.5)));
    }

    [Test]
    public void ScaleRoom()
    {
        Room r1 = new Room(_smallSquareRoomBlueprint, 0);

        var aabbBefore = r1.GetBoundingBox();

        Assert.That(aabbBefore.Max.x - aabbBefore.Min.x, Is.EqualTo(10));
        Assert.That(aabbBefore.Max.y - aabbBefore.Min.y, Is.EqualTo(10));

        r1.Scale(32);

        var aabbAfter = r1.GetBoundingBox();

        Assert.That(aabbAfter.Max.x - aabbAfter.Min.x, Is.EqualTo(320));
        Assert.That(aabbAfter.Max.y - aabbAfter.Min.y, Is.EqualTo(320));
    }
}