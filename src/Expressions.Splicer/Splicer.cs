namespace Nessos.Expressions.Splicer
{
	using System.Linq;
	using System.Linq.Expressions;

	public class Splicer : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			var (name, method, args) = node;
			if (name == "Invoke" && method is LambdaExpression lambda)
			{
				var env = lambda.Parameters.Zip(args, (param, arg) => (param, arg: base.Visit(arg)))
					.ToDictionary(t => t.param, t => t.arg);
				return base.Visit(new Subs(env).Visit(lambda.Body));
			}

			return base.VisitMethodCall(node);
		}
	}
}
