using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } // esto realiza una asignación de valor predeterminado a null para tipos de referencia y al valor predeterminado para tipos de valor

        public static ApiResponse<T> Ok(T data, string message = "OK") =>
            new() { Success = true, Message = message, Data = data };

        public static ApiResponse<T> Fail(string message) =>
            new() { Success = false, Message = message, Data = default };
    }
}
