import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
  scenarios: {
    baseline: {
      executor: 'constant-vus',
      vus: 5,
      duration: '2m',
      startTime: '0s',
      exec: 'baselineScenario',
    },
    intense: {
      executor: 'constant-vus',
      vus: 20,
      duration: '2m',
      startTime: '2m',
      exec: 'intenseScenario',
    },
  },
  thresholds: {
    'http_req_duration{expected_response:true}': ['p(95)<3000'],
    http_req_failed: ['rate<0.05'],
  },
};

const BASE_URL = 'http://localhost:80';

const CATEGORIES = [
  'electronics',
  'computers',
  'desktops',
  'notebooks',
  'cell-phones',
  'camera-photo',
  'clothing',
  'shoes-2',
  'accessories',
];

const PRODUCTS = [
  'build-your-own-computer',
  'apple-macbook-pro',
  'htc-smartphone',
  'nokia-lumia-1020',
  'apple-iphone-16-128gb',
  'samsung-galaxy-s24-256gb',
  'lenovo-ideacentre',
];

const PRICE_HEAVY_PRODUCTS = [
  'build-your-own-computer',
  'apple-macbook-pro',
  'htc-smartphone',
  'nokia-lumia-1020',
];

const MANUFACTURERS = ['apple', 'hp', 'nike'];

const SEARCH_TERMS = ['phone', 'laptop', 'computer', 'book', 'shirt', 'camera'];
const ZERO_RESULT_TERMS = ['xyznoexist', 'qqqqq', 'asdfnotfound', 'noresulthere'];

function pick(arr) {
  return arr[Math.floor(Math.random() * arr.length)];
}

function think() {
  sleep(1 + Math.random() * 2);
}

function thinkFast() {
  sleep(0.5 + Math.random());
}

function pickSearchTerm(zeroResultRate) {
  const useZeroResultTerm = Math.random() < zeroResultRate;
  const term = pick(useZeroResultTerm ? ZERO_RESULT_TERMS : SEARCH_TERMS);

  return { term, useZeroResultTerm };
}

function browseListing(thinkFn) {
  if (Math.random() < 0.5) {
    const category = pick(CATEGORIES);
    http.get(`${BASE_URL}/${category}`, { tags: { name: 'category_browse' } });
  } else {
    const manufacturer = pick(MANUFACTURERS);
    http.get(`${BASE_URL}/${manufacturer}`, { tags: { name: 'manufacturer_browse' } });
  }

  thinkFn();
}

function visitProductDetails(thinkFn, preferPriceHeavy = false) {
  const productPool = preferPriceHeavy ? PRICE_HEAVY_PRODUCTS : PRODUCTS;
  const product = pick(productPool);
  const tagName = preferPriceHeavy ? 'pdp_price_heavy' : 'pdp';

  http.get(`${BASE_URL}/${product}`, { tags: { name: tagName } });
  thinkFn();
}

function visitSearch(thinkFn, zeroResultRate, tagSuffix = 'primary') {
  const { term, useZeroResultTerm } = pickSearchTerm(zeroResultRate);
  const searchTag = useZeroResultTerm ? `search_zero_${tagSuffix}` : `search_${tagSuffix}`;
  const autocompleteTag = useZeroResultTerm ? `autocomplete_zero_${tagSuffix}` : `autocomplete_${tagSuffix}`;

  http.get(`${BASE_URL}/search?q=${encodeURIComponent(term)}`, {
    tags: { name: searchTag },
  });
  thinkFn();

  http.get(`${BASE_URL}/catalog/searchtermautocomplete?term=${encodeURIComponent(term)}`, {
    tags: { name: autocompleteTag },
  });
  thinkFn();
}

function fullJourney({
  thinkFn,
  followUpZeroResultRate,
  extraBrowseCount = 0,
  extraSearchCount = 0,
  extraPriceHeavyPdpCount = 0,
}) {
  // 1. Homepage
  http.get(BASE_URL + '/', { tags: { name: 'homepage' } });
  thinkFn();

  // 2-4. Normal browse/search/PDP path with price rendering work
  browseListing(thinkFn);
  visitProductDetails(thinkFn, true);
  browseListing(thinkFn);
  visitSearch(thinkFn, 0, 'primary');

  // 5. Follow-up search sometimes misses to model normal zero-result behavior
  visitSearch(thinkFn, followUpZeroResultRate, 'follow_up');

  // 6. Another PDP hit to keep product-details and pricing active
  visitProductDetails(thinkFn);

  // 7+. Intense mode can lean harder on listing and price-heavy product traffic
  for (let i = 0; i < extraBrowseCount; i += 1) {
    browseListing(thinkFn);
  }

  for (let i = 0; i < extraSearchCount; i += 1) {
    visitSearch(thinkFn, followUpZeroResultRate, `extra_${i + 1}`);
  }

  for (let i = 0; i < extraPriceHeavyPdpCount; i += 1) {
    visitProductDetails(thinkFn, true);
  }

  // Final empty browse for a naturally sparse catalog surface
  http.get(`${BASE_URL}/gift-cards`, { tags: { name: 'empty_category' } });
  thinkFn();
}

export function baselineScenario() {
  fullJourney({
    thinkFn: think,
    followUpZeroResultRate: 0.2,
  });
}

export function intenseScenario() {
  fullJourney({
    thinkFn: thinkFast,
    followUpZeroResultRate: 0.35,
    extraBrowseCount: 1,
    extraSearchCount: 1,
    extraPriceHeavyPdpCount: 1,
  });
}
