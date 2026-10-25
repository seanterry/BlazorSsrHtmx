namespace BlazorSsrHtmx.Htmx;

public sealed class HtmxService
{
    /// <summary>
    /// True when the current HTTP request was made by HTMX.
    /// </summary>
    public bool IsHtmx { get; private set; }

    /// <summary>
    /// True when the HTMX request was boosted.
    /// </summary>
    public bool IsBoosted { get; private set; }

    /// <summary>
    /// True when the HTMX request is pure (non-boosted).
    /// </summary>
    /// <remarks>
    /// This is useful for components that may need to render different partial results based on boosting.
    /// E.g., if you use a boosted link to a search component you may want to render the search controls on both full
    /// page load and boosted requests, but only the search results on a pure HTMX request.
    /// </remarks>
    public bool IsHtmxNonBoosted { get; private set; }

    public void Initialize( HttpContext http )
    {
        // https://four.htmx.org/reference/headers/HX-Request
        // per documentation, the value of this header is always "true", so we only need to check for its presence.
        if ( !http.Request.Headers.ContainsKey( HtmxRequestHeaders.HxRequest ) )
            return;

        IsHtmx = true;

        // https://four.htmx.org/reference/headers/HX-Boosted
        // this header can be present with a value other than true, so we need to check its value.
        // its value should be ignored on non-htmx requests.
        IsBoosted = IsHtmx && http.Request.Headers[HtmxRequestHeaders.HxBoosted]
            .Contains( "true", StringComparer.OrdinalIgnoreCase );

        IsHtmxNonBoosted = IsHtmx && !IsBoosted;
    }
}
