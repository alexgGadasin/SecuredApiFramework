using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Domain.Api
{
    public class SkipApiExceptionFilter : Attribute, IFilterMetadata { }
}
