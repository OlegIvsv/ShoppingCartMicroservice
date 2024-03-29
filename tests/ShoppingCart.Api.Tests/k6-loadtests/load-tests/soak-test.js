﻿/**
 / Discovering the system during a long period of time.
 / Plan:
 /   From the very beginning the DB is filled with 5000 carts. If VU does not hava a
 /   cart, he creates it. Else if VU has a cart, then he puts an item to cart every 5
 /   seconds. In other case user gets his cart and we suppose cart must be cleared.
 /   The test takes 4 hours and keeps the system under under low load
 / Params:
 /  HOST - the tested api;
 /  DB_MANAGER_HOST - db manager host api for filling test db;
 */
import http from 'k6/http';
import {sleep} from 'k6';
import {Trend} from 'k6/metrics';
import {createTestItem, createUserInfo} from './common/data-generating.js';
import {clearCart, createCart, getCart, putItem} from "./common/http-requests.js";
import {readCommandLineParam} from "./common/command-line";

const customTrends = {
    createCartTrend: new Trend('x_create_cart_req_duration', true),
    putItemTrend: new Trend('x_put_item_req_duration', true),
    getCartTrend: new Trend('x_get_item_req_duration', true),
    clearCartTrend: new Trend('x_clear_cart_req_duration', true)
}
export const options = {
    stages: [
        {duration: '2m', target: 150},
        {duration: '3.65m', target: 150},
        {duration: '2m', target: 0}
    ]
};
export const setup = () => {
    const numberOfCartsInDb = 5000;
    const dbManagerHost =  readCommandLineParam("DB_MANAGER_HOST");
    http.get(`${dbManagerHost}/refill-db?number=${numberOfCartsInDb}`);
};
const vuInfo = createUserInfo();

export default function() {
    
    const host =  readCommandLineParam("HOST");

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
    else if (vuInfo.requiresClear) {
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
    sleep(5);
};
