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

## Why these two metrics were chosen together

These metrics were selected because they represent two different kinds of user-visible degradation:

- `catalog_search_zero_results_total` measures search usefulness
- `catalog_product_details_model_build_durations` measures product page composition cost

---
