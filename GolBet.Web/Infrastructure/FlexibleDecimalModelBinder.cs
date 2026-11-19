using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GolBet.Web.Infrastructure;

public class FlexibleDecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var type = context.Metadata.ModelType;
        return type == typeof(decimal) || type == typeof(decimal?)
            ? new FlexibleDecimalModelBinder() : null;
    }
}

public class FlexibleDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var name = bindingContext.ModelName;
        var result = bindingContext.ValueProvider.GetValue(name);
        if (result == ValueProviderResult.None) return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(name, result);
        var raw = result.FirstValue;

        if (string.IsNullOrWhiteSpace(raw))
        {
            if (bindingContext.ModelType == typeof(decimal?))
                bindingContext.Result = ModelBindingResult.Success(null);
            else
                bindingContext.ModelState.TryAddModelError(name, "El valor es obligatorio");
            return Task.CompletedTask;
        }

        var normalized = raw.Trim().Replace(',', '.');
        if (decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            bindingContext.Result = ModelBindingResult.Success(value);
        else
            bindingContext.ModelState.TryAddModelError(name, "Ingrese un número válido");

        return Task.CompletedTask;
    }
}