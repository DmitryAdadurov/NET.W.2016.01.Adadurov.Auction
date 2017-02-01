using ORM.EF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Mappers
{
    class ReplaceVisitor<TDalEntity, TEntity> : ExpressionVisitor
    {

        private ParameterExpression _parameter;
        private IDictionary<string, string> propertiesMapper;

        public ReplaceVisitor(ParameterExpression parameter, IDictionary<string, string> properties)
        {
            _parameter = parameter;
            propertiesMapper = properties;
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(TDalEntity))
            {
                return _parameter;
            }
            return base.VisitParameter(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo otherMember;
                string mappedName;
                if (propertiesMapper.TryGetValue(node.Member.Name, out mappedName))
                {
                    otherMember = typeof(TEntity).GetProperty(mappedName);
                }
                else
                {
                    otherMember = typeof(TEntity).GetProperty(node.Member.Name);
                }
                //var memberName = node.Member.Name;
                //var otherMember = typeof(TEntity).GetProperty(memberName);
                MemberExpression memberExpression = Expression.Property(Visit(node.Expression), otherMember);
                return memberExpression;
            }
            else
            {
                return base.VisitMember(node);
            }
        }
    }

}
