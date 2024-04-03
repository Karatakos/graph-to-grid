namespace Dungen;

public class DungenLayout {
    public DungenGraph Graph { get; private set; }
    public Room[] Rooms { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    public Vector2F Center { get; private set; }
    public AABB2F BoundingBox { get; private set; }

    public DungenLayout (Layout layout, DungenGraph graph) {
        Rooms = layout.Rooms.Values.ToArray();
        Width = layout.Width;
        Height = layout.Height;
        Center = layout.Center;
        BoundingBox = layout.BoundingBox;
    
        Graph = graph;
    }
}
