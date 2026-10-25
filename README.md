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

The stock Blazor template puts the `DOCTYPE` and surrounding markup in `App.razor`
[as seen here](https://github.com/seanterry/BlazorSsrHtmx/blob/b32c3a3c307eef03dd21cddcebd0df142efbcd8f/BlazorSsrHtmx/Components/App.razor).

This is problematic for a few reasons:
- `App.razor` only runs on Blazor routes. It will not run for components returned by Minimal APIs.
- `HeadOutlet` is stuck in `App.razor`, so partial responses that include a `PageTitle` component won't update the title.
- You may want layouts that use different stylesheets or scripts (e.g. a print layout).

There are multiple ways to work around these problems. In the next several steps, I will share the way I think works
best. And by "best" I mean it solves all of the above with the least amount of cognitive load.

## Step 2: Move full-page markup to layout

First, we'll deal with `App.razor` only running on Blazor routes. That's a baked-in limitation, so the easiest way
around it is to move the content elsewhere. In this case all of it. So our `App.razor` simply becomes:
```html
<Routes />
```

You can also move the content of `Routes.razor` to `App.razor` and eliminate the extra component, but I'm not going to
do that here.

We then move all the markup into our `MainLayout.razor`. The complete change can be seen in
[this commit](https://github.com/seanterry/BlazorSsrHtmx/commit/1708bf73f84696714b6a0c7315d3c8b3b0fa7eef).

Components returned by Minmal APIs can now opt-in to a full-page layout with an explicit `@layout` directive. And using
a layout other than `MainLayout` for any component is done the same way. So two out of our three problems are salved.

But we created one new one: Components that have an explicit `@layout` directive are always full-page. They cannot be
partials.

## Step 3: Add HTMX

Next you'll want to get the script for [HTMX 4](https://four.htmx.org/). You can just link to the CDN, but I prefer to
serve it as a static asset. To do so, download the script locally to `wwwroot/lib/htmx@4.0.0/htmx.min.js` and add it to
your MainLayout like I did in [this commit](https://github.com/seanterry/BlazorSsrHtmx/commit/8554cdd85511c9f4bd83309845d567a106b22ca5).

```html
<script defer src="@Assets["lib/htmx@4.0.0/htmx.min.js"]"></script>
```

