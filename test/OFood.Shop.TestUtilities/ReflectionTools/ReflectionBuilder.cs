using System.Linq.Expressions;
using System.Reflection;

namespace OFood.Shop.TestUtilities.ReflectionTools;

public abstract class ReflectionBuilder<TReflection, TBuilder>
    where TReflection : class
    where TBuilder : ReflectionBuilder<TReflection, TBuilder>
{
    private readonly TBuilder _builderInstance;

    private readonly IDictionary<string, PropertyWrapper> propertiesInfo = new Dictionary<string, PropertyWrapper>();

    public ReflectionBuilder()
    {
        _builderInstance = (TBuilder)this;
    }

    public TBuilder With<TValue>(Expression<Func<TReflection, TValue>> exp, TValue value)
    {
        if (exp.Body is not MemberExpression body)
        {
            throw new InvalidOperationException("Improperly formatted expression");
        }

        var propertyName = body.Member.Name;
        var property = GetType().GetRuntimeField(propertyName);

        if (property != null)
        {
            property.SetValue(_builderInstance, value);
        }
        else
        {
            GetType().GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)?
                .SetValue(_builderInstance, value);
        }

        return _builderInstance;
    }

    public abstract TReflection Build();

    private class PropertyWrapper
    {
        internal PropertyWrapper(PropertyInfo property, object value)
        {
            Property = property;
            Value = value;
        }

        public PropertyInfo Property { get; }
        public object Value { get; set; }
    }
}