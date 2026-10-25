namespace BlazorSsrHtmx.Htmx;

public sealed class HtmxMiddleware( RequestDelegate next )
{
    public Task InvokeAsync( HttpContext http, HtmxService htmx )
    {
        htmx.Initialize( http );
        return next( http );
    }
}
