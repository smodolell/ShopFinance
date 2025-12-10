namespace ShopFinance.Application.Common.Constants;

public static class ApplicationConstants
{
    /// <summary>
    /// SOLICITUD DE CRÉDITO
    /// </summary>
    public const int PhaseId_CreditRequest = 1;

    /// <summary>
    /// CUESTIONARIO
    /// </summary>
    public const int PhaseId_CUESTIONARIO = 2;


    /// <summary>
    /// CONFIGURAR COTIZACIÓN
    /// </summary>
    public const int FaseId_CONFIG_COTIZADOR = 3;

    /// <summary>
    /// COTIZACIÓN
    /// </summary>
    public const int FaseId_COTIZADOR = 4;



    /// <summary>
    /// SCORING
    /// </summary>
    public const int FaseId_SCORING = 7;


    /// <summary>
    /// EXPEDIENTE
    /// </summary>
    public const int FaseId_EXPEDIENTE = 8;

    /// <summary>
    /// VALIDACIÓN DE EXPEDIENTE
    /// </summary>
    public const int FaseId_VALIDACION_EXPEDIENTE = 9;




    /// <summary>
    /// FORMALIZACIÓN
    /// </summary>
    public const int FaseId_FORMALIZACION = 11;


    /// <summary>
    /// ENTREGA DE RECURSOS
    /// </summary>
    public const int FaseId_ENTREGA_RECURSO = 12;

    

    /// <summary>
    /// EXPORTACIÓN
    /// </summary>
    public const int FaseId_EXPORTACION = 15;



    /// <summary>
    /// SOLICITANTE
    /// </summary>
    public const int SeccionSolicitudId_SOLICITANTE = 1;

    /// <summary>
    /// REPRESENTANTE LEGAL
    /// </summary>
    public const int SeccionSolicitudId_REPRESENTANTE_LEGAL = 2;

    /// <summary>
    /// PERSONA MORAL
    /// </summary>
    public const int SeccionSolicitudId_PERSONA_JURIDICA = 3;

    /// <summary>
    /// REFERENCIAS PERSONALES (no familiares)
    /// </summary>
    public const int SeccionSolicitudId_REF_PERSONALES = 4;


    /// <summary>
    /// REFERENCIAS COMERCIALES (principales proveedores)
    /// </summary>
    public const int SeccionSolicitudId_REF_COMERCIALES = 5;
    /// <summary>
    /// DATOS DE LA ACTIVIDAD ACTUAL
    /// </summary>
    public const int SeccionSolicitudId_ACTIVIDAD_ACTUAL = 6;

    /// <summary>
    /// BALANCE GENERAL
    /// </summary>
    public const int SeccionSolicitudId_BALANCE_GENERAL = 7;

    /// <summary>
    /// ESTADO DE RESULTADOS
    /// </summary>
    public const int SeccionSolicitudId_ESTADO_RESULTADOS = 8;

    /// <summary>
    /// BIENES INMUEBLES DEL SOLICITANTE
    /// </summary>
    public const int SeccionSolicitudId_BIENES_INMUEBLES_SOLICITANTE = 9;

    /// <summary>
    /// CREDITO SOLICITADO
    /// </summary>
    public const int SeccionSolicitudId_CREDITO_SOLICITADO = 10;


    /// <summary>
    /// PARA ESTAR EN POSIBILIDAD DE PARTICIPAR EN EL PROGRAMA DE FINANCIAMIENTO 
    /// DEBE MANIFESTAR SI SE ENCUENTRA O NO EN LOS SIGUIENTES SUPUESTOS
    /// </summary>
    public const int SeccionSolicitudId_SUPUESTO_FINANCIAMIENTO = 11;

    //AVAL
    public const int SeccionSolicitudId_AVAL = 12;

    //BIENES INMUEBLES DEL AVAL
    public const int SeccionSolicitudId_BIENES_INMUEBLES_AVAL = 13;


    //DESTINO DEL FINANCIAMIENTO
    public const int SeccionSolicitudId_DESTINO_FINANCIAMIENTO = 14;





    public const string PERSONALES = "PERSONALES";
    public const string COMERCIALES = "COMERCIALES";


}
