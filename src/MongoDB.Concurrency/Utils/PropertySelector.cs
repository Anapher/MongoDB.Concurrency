using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MongoDB.Concurrency.Utils
{
    internal class PropertySelector<T, TValue> where T : class
    {
        private readonly Func<T, TValue> _getValue;

        public PropertySelector(Expression<Func<T, TValue>> selector)
        {
            Selector = selector;
            _getValue = selector.Compile();
        }

        public Expression<Func<T, TValue>> Selector { get; }

        public TValue GetValue(T entity)
        {
            return _getValue(entity);
        }

        public void SetValue(T entity, TValue newValue)
        {
            var prop = (PropertyInfo) ((MemberExpression) Selector.Body).Member;
            prop.SetValue(entity, newValue, null);
        }
    }
}
