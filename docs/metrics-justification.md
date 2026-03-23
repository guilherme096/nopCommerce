# Metrics Justification

These are the custom metrics used for the `Catalogue -> Search -> Pricing` flow.

## `catalog_search_zero_results_total`

**Type:** Counter

### What it measures

Search operations that returned no products.

### Why it matters

Search can still be fast and return `200 OK` while being useless to customers. If people keep searching and getting nothing back, something in the search path is off even if the site looks healthy at the HTTP level.

### What an operator can do with it

If this starts climbing, check:

- search provider health or fallback behavior
- recent catalogue imports or product visibility changes
- broken filters or search settings
- category, manufacturer, or vendor mappings

### Limitation

This number needs context. A count on its own can be misleading, so it should be read together with `catalog_search_total`.

## `catalog_browse_zero_results_total`

**Type:** Counter

### What it measures

Browse operations that returned no products for category, manufacturer, or vendor catalogue views.

### Why it matters

This is the browse-side version of the same problem. A listing page can load fine from a technical point of view and still fail the user because it shows an empty catalogue where there should be products.

### What an operator can do with it

If it rises, check:

- category or manufacturer assignments
- vendor/product mappings
- publication state and ACL or store-mapping changes
- filters that are excluding too much content

### Limitation

Like search zero-results, this is most useful when read as a ratio against total browse traffic.

## `catalog_search_total` and `catalog_browse_total`

**Type:** Counter

### What they measure

The total number of search and browse operations, whether they returned products or not.

### Why they matter

These are denominator metrics. Without them, the zero-result counters are hard to interpret. Ten empty searches might be normal traffic, or it might mean half of all searches are failing.

### What an operator can do with them

Use them to decide whether a spike in zero-results is a real degradation or just normal low-volume noise. They also help show whether a drop in conversions is tied to reduced traffic or to worse search and browse quality.

### Limitation

These are support metrics. On their own they do not say whether the experience is good or bad; they only make the zero-result metrics meaningful.

## `catalog_product_details_model_build_duration`

**Type:** Histogram

### What it measures

Server-side time spent building the product details page model.

### Why it matters

The product page is where search and browse traffic turns into actual product evaluation. If this gets slower, the user feels it directly. A generic request-duration metric would tell you the page is slow, but this metric narrows the problem to PDP assembly work instead of the full HTTP request.

### What an operator can do with it

If it trends up, look at the heavy parts of PDP assembly first:

- price calculation
- media and picture loading
- review and attribute preparation
- cache misses or expensive plugin work

### Limitation

This is still a coarse metric. It tells you PDP assembly is slower, not which exact sub-step is responsible.

## `catalog_browse_model_build_duration`

**Type:** Histogram

### What it measures

Time spent building catalogue browse models for category, manufacturer, and vendor views.

### Why it matters

This is the main latency signal for list-style catalogue pages. It helps separate "the site is slow" from "listing-page assembly is slow", which is more useful during an incident.

### What an operator can do with it

If it rises, check:

- product retrieval and filtering cost
- paging and sorting behavior
- expensive listing-page composition work
- whether one browse type is worse than the others

### Limitation

Like the PDP metric, this is an assembly-level metric. It is useful for narrowing the area, not for proving the exact root cause on its own.

## `catalog_product_overview_models_build_duration`

**Type:** Histogram

### What it measures

Time spent building product overview models for listing pages and search results.

### Why it matters

This is one of the best current signals for the pricing part of the flow on list pages. Product overview models are used in category pages, search results, and other catalogue surfaces, so if this metric gets worse, users will feel it across multiple entry points.

### What an operator can do with it

If it rises, compare runs where price preparation is enabled and disabled. That helps show whether the slowdown is mostly in pricing work or in general model assembly.

### Limitation

This metric is still shared across several pieces of list rendering. It is a good pointer, but not yet a full breakdown.

## Why the `prepare_price` split is on the dashboard

The dashboard compares product overview build time with `prepare_price=true` and `prepare_price=false`.

That split is useful because it helps answer a practical question: is the slowdown in general listing work, or is it caused by pricing? If the `prepare_price=true` series is much worse, pricing is the first place to look.
