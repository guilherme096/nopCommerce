import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
  scenarios: {
    demo_normal: {
      executor: 'constant-vus',
      vus: 4,
      duration: '5h',
      exec: 'demoNormalScenario',
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

function thinkNormal() {
  const occasionalPause = Math.random() < 0.15 ? 0.75 + Math.random() * 1.5 : 0;
  sleep(1 + Math.random() * 2 + occasionalPause);
}

function pickSearchTerm(zeroResultRate) {
  const useZeroResultTerm = Math.random() < zeroResultRate;
  const term = pick(useZeroResultTerm ? ZERO_RESULT_TERMS : SEARCH_TERMS);

  return { term, useZeroResultTerm };
}

function browseListing() {
  if (Math.random() < 0.6) {
    const category = pick(CATEGORIES);
    http.get(`${BASE_URL}/${category}`, { tags: { name: 'category_browse_demo' } });
  } else {
    const manufacturer = pick(MANUFACTURERS);
    http.get(`${BASE_URL}/${manufacturer}`, { tags: { name: 'manufacturer_browse_demo' } });
  }

  thinkNormal();
}

function visitProductDetails(preferPriceHeavy = false) {
  const productPool = preferPriceHeavy ? PRICE_HEAVY_PRODUCTS : PRODUCTS;
  const product = pick(productPool);
  const tagName = preferPriceHeavy ? 'pdp_demo_price_heavy' : 'pdp_demo';

  http.get(`${BASE_URL}/${product}`, { tags: { name: tagName } });
  thinkNormal();
}

function visitSearch(zeroResultRate, tagSuffix = 'primary') {
  const { term, useZeroResultTerm } = pickSearchTerm(zeroResultRate);
  const searchTag = useZeroResultTerm ? `search_demo_zero_${tagSuffix}` : `search_demo_${tagSuffix}`;
  const autocompleteTag = useZeroResultTerm ? `autocomplete_demo_zero_${tagSuffix}` : `autocomplete_demo_${tagSuffix}`;

  http.get(`${BASE_URL}/search?q=${encodeURIComponent(term)}`, {
    tags: { name: searchTag },
  });
  thinkNormal();

  http.get(`${BASE_URL}/catalog/searchtermautocomplete?term=${encodeURIComponent(term)}`, {
    tags: { name: autocompleteTag },
  });
  thinkNormal();
}

function maybeAddSmallVariation() {
  if (Math.random() < 0.3) {
    browseListing();
  }

  if (Math.random() < 0.2) {
    visitProductDetails(true);
  }

  if (Math.random() < 0.12) {
    visitSearch(0.1, 'secondary');
  }
}

export function demoNormalScenario() {
  http.get(BASE_URL + '/', { tags: { name: 'homepage_demo' } });
  thinkNormal();

  browseListing();
  visitProductDetails(true);
  browseListing();
  visitSearch(0, 'primary');
  visitSearch(0.12, 'follow_up');
  visitProductDetails();
  maybeAddSmallVariation();

  http.get(`${BASE_URL}/gift-cards`, { tags: { name: 'empty_category_demo' } });
  thinkNormal();
}
