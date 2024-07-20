namespace Foxy.Params.SourceGenerator.NewData;

internal static class ElementExtensions
{
    public static TElement As<TElement>(this IElement? element)
        where TElement : IElement
    {
        if(element is null)
        {
            throw new System.InvalidCastException($"Can't cast null element to {typeof(TElement)}");
        }

        if(element is TElement casted)
        {
            return casted;
        }

        throw new System.InvalidCastException($"Can't cast {element} to {typeof(TElement)}");
    }
}
