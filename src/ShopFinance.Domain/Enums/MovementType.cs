namespace ShopFinance.Domain.Enums;

public enum MovementType // Para diferenciar entre los tipos de movimientos
{
    // Débitos (lo que el cliente debe)
    ScheduledInstallment = 1, // La cuota programada completa (reemplaza a la tabla de amortización)
    AdministrationFee = 2,    // Gasto de administración (puede ser periódico o único)
    LateFee = 3,              // Cargo por mora
    Disbursement = 4,         // El desembolso inicial del préstamo (monto principal)

    // Créditos (lo que reduce la deuda, solo para registros internos)
    // También puedes usar la tabla Payment para esto
    Correction = 99
}