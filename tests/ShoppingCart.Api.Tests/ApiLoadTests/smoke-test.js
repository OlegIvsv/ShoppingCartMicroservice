/**
/ Testing the system in smoke mode.During a long period of time API receives usual 
/   request mostly to put item in cart.
/ Plot:
/   From the very beginning the DB is filled with 4000 carts (have a look at SETUP). 
/   If VU does not hava a cart, he creates it. Else if VU has a cart, then he puts 
/   an item to cart every 5 seconds. In other case user gets his cart and we suppose 
/   that the cart is to be cleared because an order was made or user changes his mind.
/ 
/ !!! If you change the test, don't forget to update this description too !!!
*/
import http from 'k6/http';
import {sleep, check} from 'k6';
import {Trend} from 'k6/metrics';
import {randomIntBetween, uuidv4,} from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

export const options = {
    stages: [
        {duration: '3m', target: 400},
        {duration: '50m', target: 400},
        {duration: '3m', target: 0}
    ]
};
export const setup = () => {
    const numberOfCartsInDb = 4000;
    const dbManagerHost = 'http://localhost:5000';
    http.get(`${dbManagerHost}/refill-db?number=${numberOfCartsInDb}`);
}
const customTrends = {
    createCartTrend: new Trend('x_create_cart_req_duration', true),
    putItemTrend: new Trend('x_put_item_req_duration', true),
    getCartTrend: new Trend('x_get_item_req_duration', true),
    clearCartTrend: new Trend('x_clear_cart_req_duration', true)
}
const vuInfo = {
    vuCustomerId: uuidv4(),
    hasCart: false,
    numberOfItemsInCart: 0,
    itemsLimitForThisUser: randomIntBetween(5, 15)
}

export default function() {
    
    const host = 'https://localhost:7015';
    
    if (!vuInfo.hasCart) {
        // Create cart and check status code
        const createRes = http.post(`${host}/api/Cart/${vuInfo.vuCustomerId}`);
        check(createRes, {
            'cart created with status 200': r => r.status === 201
        });
        customTrends.createCartTrend.add(createRes.timings.duration);
        
        vuInfo.hasCart = true;
    }
    else if (vuInfo.numberOfItemsInCart < vuInfo.itemsLimitForThisUser) {
        // Put random item to cart and check status code
        const url = `${host}/api/Cart/put-item/${vuInfo.vuCustomerId}`;
        const body = JSON.stringify(createTestItem());
        const options = {headers: {'Content-Type': 'application/json'}}
        const putItemRes = http.put(url, body, options);
        check(putItemRes, {
            'item put with status 200': r => r.status === 200
        });
        customTrends.putItemTrend.add(putItemRes.timings.duration);

        ++vuInfo.numberOfItemsInCart;
    }
    else {
        // Get cart and check status code
        const getRes = http.get(`${host}/api/Cart/${vuInfo.vuCustomerId}`);
        check(getRes, {
            'cart received with status 200': r => r.status === 200
        });
        customTrends.getCartTrend.add(getRes.timings.duration);

        // Clear cart and check status code
        const clearRes = http.put(`${host}/api/Cart/clear/${vuInfo.vuCustomerId}`);
        check(clearRes, {
            'cart deleted with status 200': r => r.status === 200
        });
        customTrends.clearCartTrend.add(clearRes.timings.duration);
        
        vuInfo.numberOfItemsInCart = 0;
    }
    
    sleep(5);
};

function createTestItem() {
    return {
        productId: uuidv4(),
        productTitle: "Some Product Title",
        itemQuantity: randomIntBetween(1, 10),
        unitPrice: Math.random() * (1000 - 0.1) + 0.1,
        discount: Math.random() * 0.7
    };
}
