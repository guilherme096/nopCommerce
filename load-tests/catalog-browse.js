import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
  scenarios: {
    intense: {
      executor: 'constant-vus',
      vus: 60,
      duration: '5m',
      startTime: '0s',
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

const INTENSE_CONFIG = {
  browseCount: 3,
  priceHeavyPdpCount: 2,
  regularPdpCount: 1,
  searchCount: 3,
  emptyBrowseCount: 1,
  searchZeroResultRate: 0.6,
};

function pick(arr) {
  return arr[Math.floor(Math.random() * arr.length)];
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

function visitEmptyBrowse(thinkFn) {
  http.get(`${BASE_URL}/gift-cards`, { tags: { name: 'empty_category' } });
  thinkFn();
}

function runIntenseLoop() {
  const totalPdpCount = INTENSE_CONFIG.priceHeavyPdpCount + INTENSE_CONFIG.regularPdpCount;
  const roundCount = Math.max(INTENSE_CONFIG.browseCount, INTENSE_CONFIG.searchCount, totalPdpCount);

  for (let i = 0; i < roundCount; i += 1) {
    if (i < INTENSE_CONFIG.browseCount) {
      browseListing(thinkFast);
    }

    if (i < INTENSE_CONFIG.priceHeavyPdpCount) {
      visitProductDetails(thinkFast, true);
    } else if (i < totalPdpCount) {
      visitProductDetails(thinkFast);
    }

    if (i < INTENSE_CONFIG.searchCount) {
      visitSearch(thinkFast, INTENSE_CONFIG.searchZeroResultRate, `intense_${i + 1}`);
    }
  }

  for (let i = 0; i < INTENSE_CONFIG.emptyBrowseCount; i += 1) {
    visitEmptyBrowse(thinkFast);
  }
}

export function intenseScenario() {
  runIntenseLoop();
}
