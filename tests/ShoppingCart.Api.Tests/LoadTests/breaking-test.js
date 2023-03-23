/**
 / Testing the system under extreme load. During a small period of time API receives
 /   requests increasing levels of load.
 / Plan:
 /   From the very beginning the DB is filled with 5000 carts (have a look at SETUP).
 /   If VU does not hava a cart, he creates it. Else if VU has a cart, then he puts
 /   an item to cart. In other case user gets his cart. Next, we suppose
 /   the cart must be cleared. Amount of requests increases from 1 to 1400 per second.
 / Params:
 /  HOST - the tested api;
 /  DB_MANAGER_HOST - db manager host api for filling test db;
 */
import http from 'k6/http';
import {sleep} from 'k6';
import {Trend} from 'k6/metrics';
import {createTestItem, createUserInfo} from './common/data-generating.js';
import {clearCart, createCart, getCart, putItem} from './common/http-requests.js';
import { readCommandLineParam } from './common/command-line.js';

const customTrends = {
    createCartTrend: new Trend('x_create_cart_req_duration', true),
    putItemTrend: new Trend('x_put_item_req_duration', true),
    getCartTrend: new Trend('x_get_item_req_duration', true),
    clearCartTrend: new Trend('x_clear_cart_req_duration', true),
}
export const options = {
    scenarios: {
        breaking: {
            executor: "ramping-arrival-rate",
            preAllocatedVUs: 1000,
            stages: [
                {duration: '1m', target: 100},
                {duration: '1m', target: 100},
                {duration: '1m', target: 200},
                {duration: '1m', target: 200},
                {duration: '1m', target: 300},
                {duration: '1m', target: 300},
                {duration: '1m', target: 400},
                {duration: '1m', target: 400},
                {duration: '1m', target: 500},
                {duration: '1m', target: 500},
                {duration: '1m', target: 600},
                {duration: '1m', target: 600},
                {duration: '2m', target: 800}, 
                {duration: '2m', target: 800},
                {duration: '2m', target: 1000},
                {duration: '2m', target: 1000},
                {duration: '2m', target: 1200}, 
                {duration: '2m', target: 1200},
                {duration: '3m', target: 1400},
                {duration: '3m', target: 1400},
                {duration: '5m', target: 0}
            ],
            timeUnit: '1s',
        }
    },
    thresholds: {
        x_put_item_req_duration: [{threshold: 'p(95) < 250', abortOnFail: true}],
        x_get_item_req_duration: [{threshold: 'p(95) < 250', abortOnFail: true}],
        x_clear_cart_req_duration: [{threshold: 'p(95) < 250', abortOnFail: true}],
        x_create_cart_req_duration: [{threshold: 'p(95) < 250', abortOnFail: true}],
        http_req_failed: ['rate < 0.01'],
        checks: ['rate > 0.99']
    }
};
export const setup = () => {
    const numberOfCartsInDb = 5000;
    const dbManagerHost = readCommandLineParam("DB_MANAGER_HOST");
    http.get(`${dbManagerHost}/refill-db?number=${numberOfCartsInDb}`);
};
const vuInfo = createUserInfo();

export default function() {
    
    const host = readCommandLineParam("HOST");
    
    if (!vuInfo.hasCart) {
        /* Create cart and check status code */
        const createRes = createCart(host, vuInfo.vuCustomerId);
        customTrends.createCartTrend.add(createRes.timings.duration);
        vuInfo.hasCart = true;
    }
    else if (vuInfo.numberOfItemsInCart < vuInfo.itemsLimitForThisUser) {
        /* Put random item to cart and check status code */
        const item = createTestItem();
        const putItemRes = putItem(host, vuInfo.vuCustomerId, item);
        customTrends.putItemTrend.add(putItemRes.timings.duration);
        ++vuInfo.numberOfItemsInCart;
    }
    else if(vuInfo.requiresClear)
    {
        /* Clear cart and check status code */
        const clearRes = clearCart(host, vuInfo.vuCustomerId);
        customTrends.clearCartTrend.add(clearRes.timings.duration);
        vuInfo.numberOfItemsInCart = 0;
        vuInfo.requiresClear = false;
    }
    else {
        /* Get cart and check status code */
        const getRes = getCart(host, vuInfo.vuCustomerId);
        customTrends.getCartTrend.add(getRes.timings.duration);
        vuInfo.requiresClear = true;
    }
    sleep(0.5);
};
