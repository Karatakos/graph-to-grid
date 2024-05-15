namespace GraphToGrid;

public enum DoorConstraintType {
    Placeholder,
    Fixed
}

public enum DefaultDoorAccess {
    Accessible,
    Inaccessible
}

public class Door {
    public int ConnectingRoomNumber { get; private set; }

    public DefaultDoorAccess DefaultAccess { get; private set; }

    public Line Marker { get; private set; }

    public Door(
        Line marker, 
        int connectingRoom,
        DefaultDoorAccess defaultAccess = DefaultDoorAccess.Accessible) {
            Marker = marker;
            ConnectingRoomNumber = connectingRoom;
            DefaultAccess = defaultAccess;
    }

    public Door(Door copy) {
        Marker = copy.Marker;
        ConnectingRoomNumber = copy.ConnectingRoomNumber;
        DefaultAccess = copy.DefaultAccess;
    }

    public void Translate(Vector2F v) {
        Marker = new Line(
            Marker.Start + v,
            Marker.End + v);
    }

    public void Scale(float s) {
        Marker = new Line(
            Marker.Start * s,
            Marker.End * s);
    }
}

public class DoorContraint {
    public DoorConstraintType Type { get; private set; }

    public bool HasRestrictedDoor { get => (AllowedPositions != null && AllowedPositions.Count > 0); }

    public List<ValueTuple<Vector2F, Vector2F>> AllowedPositions { get; private set; }

    public DoorContraint(DoorConstraintType type = DoorConstraintType.Placeholder) : 
        this(new List<ValueTuple<Vector2F, Vector2F>>(), type) {}

    public DoorContraint(
        List<ValueTuple<Vector2F, Vector2F>> allowedPositions, 
        DoorConstraintType type = DoorConstraintType.Placeholder) {
        AllowedPositions = allowedPositions;
        Type = type;
    }

    public DoorContraint(DoorContraint copy) {
        AllowedPositions = new List<ValueTuple<Vector2F, Vector2F>>();
        foreach (ValueTuple<Vector2F, Vector2F> t in copy.AllowedPositions) 
            AllowedPositions.Add(t);

        Type = copy.Type;
    }

    public DoorContraint AddConstraintLine(Vector2F start, Vector2F end) {
        AllowedPositions.Add(ValueTuple.Create(start, end));

        return this;
    }
}