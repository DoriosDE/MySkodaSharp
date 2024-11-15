namespace MySkodaSharp.Enums
{
    public enum ActiveState
    {
        ACTIVATED,
        DEACTIVATED
    }

    public enum ChargerLockedState
    {
        LOCKED,
        UNLOCKED,
        INVALID
    }

    public enum ConnectionState
    {
        CONNECTED,
        DISCONNECTED
    }

    public enum DoorLockedState
    {
        LOCKED,
        OPENED,
        TRUNK_OPENED,
        UNLOCKED,
        UNKNOWN
    }

    public enum EnabledState
    {
        ENABLED,
        DISABLED,
        NOT_ALLOWED
    }

    public enum OnOffState
    {
        ON,
        OFF,
        INVALID
    }
    public enum OpenState
    {
        OPEN,
        CLOSED,
        UNSUPPORTED,
        UNKNOWN
    }
    public enum Side
    {
        LEFT,
        RIGHT
    }

    public enum Weekday
    {
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATURDAY,
        SUNDAY
    }
}
