namespace MySkodaSharp.Enums
{
    public enum ChargeStatus
    {
        StatusNone = 0,
        StatusA = 'A',          // Fzg. angeschlossen: nein    Laden aktiv: nein    Ladestation betriebsbereit, Fahrzeug getrennt
        StatusB = 'B',          // Fzg. angeschlossen:   ja    Laden aktiv: nein    Fahrzeug verbunden, Netzspannung liegt nicht an
        StatusC = 'C',          // Fzg. angeschlossen:   ja    Laden aktiv:   ja    Fahrzeug lädt, Netzspannung liegt an
        StatusD = 'D',          // Fzg. angeschlossen:   ja    Laden aktiv:   ja    Fahrzeug lädt mit externer Belüfungsanforderung (für Blei-Säure-Batterien)
        StatusE = 'E',          // Fzg. angeschlossen:   ja    Laden aktiv: nein    Fehler Fahrzeug / Kabel (CP-Kurzschluss, 0V)
        StatusF = 'F'           // Fzg. angeschlossen:   ja    Laden aktiv: nein    Fehler EVSE oder Abstecken simulieren (CP-Wake-up, -12V)
    }
}
