
namespace MinimalAPIRoutes.RouteConstraints;

public class CheckName : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        return values[routeKey] as string == "Gabriel";
    }
}