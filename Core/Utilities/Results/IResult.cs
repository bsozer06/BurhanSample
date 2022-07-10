using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IResult
    {
        // Sadece read-only özelliğini içerir.
        bool Success { get; }
        string Message { get; }
    }
}
