namespace MySkodaSharp.Enums
{
    public enum ChargeMode
    {
        HOME_STORAGE_CHARGING,
        IMMEDIATE_DISCHARGING,
        ONLY_OWN_CURRENT,
        PREFERRED_CHARGING_TIMES,
        TIMER_CHARGING_WITH_CLIMATISATION,
        TIMER,
        MANUAL,
        OFF
    }

    public enum ChargeType
    {
        AC,
        DC,
        OFF
    }

    public enum ChargingErrorType
    {
        CARE_MODE_IS_NOT_AVAILABLE,
        AUTO_UNLOCK_IS_NOT_AVAILABLE,
        MAX_CHARGE_CURRENT_IS_NOT_AVAILABLE,
        CHARGE_LIMIT_IS_NOT_AVAILABLE,
        STATUS_OF_CHARGING_NOT_AVAILABLE,
        STATUS_OF_CONNECTION_NOT_AVAILABLE
    }
    public enum ChargingState
    {
        READY_FOR_CHARGING,
        CONNECT_CABLE,
        CONSERVING,
        CHARGING
    }

    public enum MaxChargeCurrent
    {
        MAXIMUM,
        REDUCED
    }
    public enum PlugUnlockMode
    {
        PERMANENT,
        ON,
        OFF
    }
}
