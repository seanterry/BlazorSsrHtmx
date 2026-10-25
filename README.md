# Blazor Static SSR + HTMX 4

todo: add hook

**Blazor Static Server-Side Rendering (SSR)** lets you build web pages/components with Razor Components (not to be
confused with Razor Pages) that run on the server. When a browser requests a page, the server renders the components to
plain HTML and sends it back. No WebAssembly. No persistent SignalR connection. The page is fast and lightweight,
but it lacks client interactivity.

[**HTMX**](https://four.htmx.org/) is a small JavaScript library that adds interactivity through HTML attributes. An
attribute like `hx-get` or `hx-post` tells an element to send a request to the server, and the server responds with a
fragment of HTML that HTMX swaps into the page.

Razor Components pair well with HTMX, since they can represent either a fragment or an entire page with a layout. By
adding HTMX-awareness to your layouts, your components can automatically switch between full-page and fragment
rendering. Razor Components can also be returned from Minimal APIs.

Whatever interaction cannot be handled with HTMX, can usually easily by added by vanilla Javascript or commonly
available libraries.

## Step 1: Blazor Static SSR project

To start, we'll create a basic Blazor server project with no interactivity. This is easily done with the command:
```
dotnet new blazor -n BlazorSsrHtmx -int None
```
I did that in [this commit](https://github.com/seanterry/BlazorSsrHtmx/commit/93c9aa8f4a6ed7fa438a6f5df739d4842dd36ed5).

Note: I was using the .NET 11 SDK RC2. I am aware of one major difference since .NET 10 that I will point out later in
the relevant section.

