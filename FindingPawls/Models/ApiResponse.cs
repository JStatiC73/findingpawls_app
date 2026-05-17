namespace FindingPawls.Models;

// Respuesta generica envuelta del API.
// Todos los endpoints del API retornan este formato estandar.
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}

// Respuesta de error del API cuando la operacion falla.
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }
}
