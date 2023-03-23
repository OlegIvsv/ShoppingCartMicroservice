/**
 / Discovering the system in case of sudden load increase.
 / Plan:
 /   From the very beginning the DB is filled with 5000 carts (have a look at SETUP).
 /   If VU does not hava a cart, he creates it. Else if VU has a cart, then he puts
 /   an item to cart. In other case user gets his cart. Next, we suppose the cart must 
 /   be cleared. Rate increases to 800 requests per second in 30s. Requests made during
 /   the spikes are tracked with separate trends to catch the difference.
 / Params:
 /  HOST - the tested api;
 /  DB_MANAGER_HOST - db manager host api for filling test db;
 */
import http from 'k6/http';
import {sleep, check} from 'k6';
import {Trend} from 'k6/metrics';
import {getCurrentStageIndex} from 'https://jslib.k6.io/k6-utils/1.3.0/index.js';
import {clearCart, createCart, getCart, putItem} from './common/http-requests.js';
import {createTestItem, createUserInfo} from './common/data-generating.js';
import {readCommandLineParam} from "./common/command-line.js";

const customTrends = {
    /* During spikes */
    createCartSpikeTrend: new Trend('x_create_cart_spike_req_duration', true),
    putItemSpikeTrend: new Trend('x_put_item_spike_req_duration', true),
    getCartSpikeTrend: new Trend('x_get_item_spike_req_duration', true),
    clearCartSpikeTrend: new Trend('x_clear_cart_spike_req_duration', true),
    /* In general */
    createCartTrend: new Trend('x_create_cart_req_duration', true),
    putItemTrend: new Trend('x_put_item_req_duration', true),
    getCartTrend: new Trend('x_get_item_req_duration', true),
    clearCartTrend: new Trend('x_clear_cart_req_duration', true),
};
export const options = {
    scenarios: {
        spike: {
            executor: "ramping-arrival-rate",
            preAllocatedVUs: 1000,
            stages: [
                {duration: '30s', target: 50},
                {duration: '30s', target: 50},
                {duration: '30s', target: 800}, 
                {duration: '5m', target: 800},  // spike 
                {duration: '30s', target: 50},  
                {duration: '30s', target: 50}, 
                {duration: '30s', target: 800}, 
                {duration: '5m', target: 800},  // spike
                {duration: '30s', target: 50},
                {duration: '30s', target: 50}
            ],
            timeUnit: '1s'
        }
    }
};
export const setup = () => {
    const numberOfCartsInDb = 5000;
    const dbManagerHost =  readCommandLineParam("DB_MANAGER_HOST");
    http.get(`${dbManagerHost}/refill-db?number=${numberOfCartsInDb}`);
};
const vuInfo = createUserInfo();

export default function() {
    
    const host =  readCommandLineParam("HOST");
    const isSpike = [3, 7].indexOf(getCurrentStageIndex()) >= 0;
    
    if (!vuInfo.hasCart) {
        /* Create cart and check status code */
        const createRes = createCart(host, vuInfo.vuCustomerId);
        customTrends.createCartTrend.add(createRes.timings.duration);
        if(isSpike)
            customTrends.createCartSpikeTrend.add(createRes.timings.duration);
        
        vuInfo.hasCart = true;
    }
    else if (vuInfo.numberOfItemsInCart < vuInfo.itemsLimitForThisUser) {
        /* Put random item to cart and check status code */
        const item = createTestItem();
        const putItemRes = putItem(host, vuInfo.vuCustomerId, item);
        customTrends.putItemTrend.add(putItemRes.timings.duration);
        if(isSpike)
            customTrends.putItemSpikeTrend.add(putItemRes.timings.duration);

        ++vuInfo.numberOfItemsInCart;
    }
    else if(vuInfo.requiresClear)
    {
        /* Clear cart and check status code */
        const clearRes = clearCart(host, vuInfo.vuCustomerId);
        customTrends.clearCartTrend.add(clearRes.timings.duration);
        if(isSpike)
            customTrends.clearCartSpikeTrend.add(clearRes.timings.duration);

        vuInfo.numberOfItemsInCart = 0;
        vuInfo.requiresClear = false;
    } 
    else {
        /* Get cart and check status code */
        const getRes = getCart(host, vuInfo.vuCustomerId);
        customTrends.getCartTrend.add(getRes.timings.duration);
        if(isSpike)
            customTrends.getCartSpikeTrend.add(getRes.timings.duration);
        
        vuInfo.requiresClear = true;
    }
    sleep(1);
};
