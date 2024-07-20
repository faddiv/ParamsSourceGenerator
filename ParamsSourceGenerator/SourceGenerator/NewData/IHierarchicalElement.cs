namespace Foxy.Params.SourceGenerator.NewData;


internal interface IHierarchicalElement<TParent>
{
    TParent? Parent { get; }
}
