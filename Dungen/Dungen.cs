namespace Dungen;

public class DungenLayout {
    public DungenGraph Graph { get; private set; }
    public Room[] Rooms { get; private set; }

    public DungenLayout (Layout layout, DungenGraph graph) {
        Rooms = layout.Rooms.Values.ToArray();
        Graph = graph;
    }
}
