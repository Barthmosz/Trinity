using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Trinity.API.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            List<string> result = new();

            foreach (ModelStateEntry item in modelState.Values)
            {
                result.AddRange(item.Errors.Select(error => error.ErrorMessage));
            }

            return result;
        }
    }
}
