using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Expressions
{
    public class ExpressionConverter<TSourceEntity, TDestEntity> : ExpressionVisitor
    {

        private ParameterExpression _parameter;
        private IDictionary<string, string> propertiesMapper;

        public ExpressionConverter(ParameterExpression parameter, IDictionary<string, string> properties)
        {
            _parameter = parameter;
            propertiesMapper = properties;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(TSourceEntity))
            {
                return _parameter;
            }
            return base.VisitParameter(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == MemberTypes.Property && node.Expression.Type == typeof(TSourceEntity))
            {
                PropertyInfo otherMember;
                string mappedName = "";
                if (propertiesMapper != null)
                {
                    if (propertiesMapper.TryGetValue(node.Member.Name, out mappedName))
                    {
                        otherMember = typeof(TDestEntity).GetProperty(mappedName);
                    }
                    else
                    {
                        otherMember = typeof(TDestEntity).GetProperty(node.Member.Name);
                    }
                }
                else
                {
                    otherMember = typeof(TDestEntity).GetProperty(node.Member.Name);
                }

                //var memberName = node.Member.Name;
                //var otherMember = typeof(TEntity).GetProperty(memberName);
                Expression e = Visit(node.Expression);
                MemberExpression memberExpression = Expression.Property(e, otherMember);
                return memberExpression;
            }
            else
            {
                return base.VisitMember(node);
            }
        }
    }
}
