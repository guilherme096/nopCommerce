import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
  vus: 10,
  duration: '2m',
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

const MANUFACTURERS = ['apple', 'hp', 'nike'];

const SEARCH_TERMS = ['phone', 'laptop', 'computer', 'book', 'shirt', 'camera'];
const ZERO_RESULT_TERMS = ['xyznoexist', 'qqqqq', 'asdfnotfound', 'noresulthere'];

function pick(arr) {
  return arr[Math.floor(Math.random() * arr.length)];
}

function think() {
  sleep(1 + Math.random() * 2);
}

export default function () {
  // 1. Homepage
  http.get(BASE_URL + '/', { tags: { name: 'homepage' } });
  think();

  // 2. Category browse
  const category = pick(CATEGORIES);
  http.get(`${BASE_URL}/${category}`, { tags: { name: 'category_browse' } });
  think();

  // 3. Product detail page
  const product = pick(PRODUCTS);
  http.get(`${BASE_URL}/${product}`, { tags: { name: 'pdp' } });
  think();

  // 4. Manufacturer browse
  const manufacturer = pick(MANUFACTURERS);
  http.get(`${BASE_URL}/${manufacturer}`, { tags: { name: 'manufacturer_browse' } });
  think();

  // 5. Search (50/50 mix of real and zero-result terms)
  const searchPool = Math.random() < 0.5 ? SEARCH_TERMS : ZERO_RESULT_TERMS;
  const term = pick(searchPool);
  http.get(`${BASE_URL}/search?q=${term}`, { tags: { name: 'search' } });
  think();

  // 6. Search autocomplete
  const autoTerm = pick(SEARCH_TERMS);
  http.get(`${BASE_URL}/catalog/searchtermautocomplete?term=${autoTerm}`, {
    tags: { name: 'autocomplete' },
  });
  think();

  // 7. Empty category (gift cards — few/no physical products)
  http.get(`${BASE_URL}/gift-cards`, { tags: { name: 'empty_category' } });
  think();
}
