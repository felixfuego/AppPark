namespace Park.Comun.Enums
{
    public enum VisitStatus
    {
        Pending,    // Pendiente de Check-in
        InProgress, // En progreso (Check-in realizado)
        Completed,  // Completada (Check-out realizado)
        Cancelled   // Cancelada
    }
}