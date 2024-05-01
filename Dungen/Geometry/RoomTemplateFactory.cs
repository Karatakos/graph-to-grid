namespace Dungen;

public class RoomTemplateFactory {
    private RoomTemplateFactory() {}

    public static Room VendRoom(RoomDefinition definition, int roomId, Room exclude = null) {
        List<Room> rooms = new List<Room>();
        foreach (RoomBlueprint blueprint in definition.Blueprints) {
            var room = new Room(blueprint, roomId);

            // BUG: Why the fuck is the class not doing this itself?
            //
            room.Position = room.GetCenter();

            if (exclude != room)
                rooms.Add(new Room(blueprint, roomId));
        }
        
        // No special selection logic for the blueprint
        //
        Random rnd = new Random();
        int index = rnd.Next(0, rooms.Count);

        return rooms[index];
    }

    /* Helper method to spit out a room for each unique blueprint in a graph
    *
    *  Important: Not designed to call multiple times efficiently (no caching)
    */
    public static Room[] VendRoomTemplates(DungenGraph graph) {
        var roomsHashmap = new HashSet<Room>();
        var rooms = new List<Room>();

        foreach (Vertex v in graph.Vertices) {
            foreach (RoomBlueprint bp in v.Definition.Blueprints) {
                var room = new Room(bp);

                if (!roomsHashmap.Contains(room)) {
                    roomsHashmap.Add(room);
                    rooms.Add(room);
                }   
            }
        }

        return rooms.ToArray();
    }
}