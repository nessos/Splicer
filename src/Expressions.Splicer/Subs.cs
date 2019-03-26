namespace Nessos.Expressions.Splicer
{
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public class Subs : ExpressionVisitor
	{
		public Subs(Dictionary<ParameterExpression, Expression> env) 
			=> this.env = env;

		private Dictionary<ParameterExpression, Expression> env;

		protected override Expression VisitParameter(ParameterExpression node)
			=> env.ContainsKey(node) ? env[node] : base.VisitParameter(node);
	}
}
