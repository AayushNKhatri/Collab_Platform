using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Collab_Platform.DomainLayer.Models
{
    public class CustomResult<TData, TError>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<TError>? Errors { get; set; }
        public TData? Data { get; set; }

        public static CustomResult<TData, TError> Ok(TData? data=default,  string? messege = null) 
            => new CustomResult<TData, TError>
            {   
                Success = true,
                Data = data,
                Message = messege 
            };
        public static CustomResult<TData, TError> Fail(IEnumerable<TError> errors, string? message = null) 
            => new CustomResult<TData, TError> 
            { 
                Success = false, 
                Errors = errors,
                Message = message
            };
    }
}
