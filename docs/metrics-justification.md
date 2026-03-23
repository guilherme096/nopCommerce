# Custom Metrics and Justification

## 1. `catalog_search_zero_results_total`

**Type:** Counter

### What it measures

This metric counts search operations that return zero products.

### Why this metric is important

A search endpoint can still be fast, return `200 OK`, and appear healthy from an infrastructure perspective while failing users functionally. If customers search products and get no results, the system is no longer providing value, even if no technical error is raised.

### Why it is operationally useful

A spike in zero-result searches can indicate:

- broken search-provider integration or fallback behavior
- degraded search behavior
- incorrect filtering logic
- catalogue data problems

This makes the metric important, as it can warn engineers of degrading performance before it becomes a problem.

## 2. `catalog_product_details_model_build_duration`

**Type:** Histogram

### What it measures

This metric measures the server-side time spent building the product details page model.

### Why this metric matters

The product page is one of the most important parts of the selected user flow. The process to build and render the page is not a trivial controller action: it branches out into multiple modules. A generic request-duration metric would show that the page is slow, but not whether the real problem comes from.

### Why it is operationally useful

If this metric rises, it points directly to backend work involved in product-page assembly, such as:

- price calculation
- media preparation
- review loading
- cache misses

## 3. `catalog_search_total` / `catalog_browse_total`

**Type:** Counter

### What they measure

These counters track the total number of search and browse requests, regardless of whether they returned results.

### Why these metrics exist

The zero-result counters alone only tell us how many requests failed to return products. To understand the *proportion* of failing requests, we need the total request count as a denominator. These counters enable the "Zero-Result Percentage" dashboard panel, which divides zero-result rate by total rate to show what fraction of user requests are returning empty results.

### Why they are operationally useful

A raw count of zero-result events can be misleading — 10 zero-result searches out of 20 total is a serious problem, but 10 out of 10,000 is normal. The percentage view surfaces real degradation more reliably than absolute counts.

## Why these metrics were chosen together

These metrics were selected because they represent two different kinds of user-visible degradation:

- `catalog_search_zero_results_total` measures search usefulness
- `catalog_product_details_model_build_duration` measures product page composition cost

---
