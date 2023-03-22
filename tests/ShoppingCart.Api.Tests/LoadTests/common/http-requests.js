import {check} from 'k6';
import http from 'k6/http';

export function getCart(host, cartId){
    const getResult = http.get(`${host}/api/Cart/${cartId}`);
    check(getResult, {
        'cart received with status 200': r => r.status === 200
    });
    return getResult;
}

export function putItem(host, cartId, item) {
    const url = `${host}/api/Cart/put-item/${cartId}`;
    const body = JSON.stringify(item);
    const options = {headers: {'Content-Type': 'application/json'}}
    const putItemRes = http.put(url, body, options);
    check(putItemRes, {
        'item put with status 200': r => r.status === 200
    });
    return putItemRes;
}

export function clearCart(host, cartId) {
    const clearRes = http.put(`${host}/api/Cart/clear/${cartId}`);
    check(clearRes, {
        'cart deleted with status 200': r => r.status === 200
    });
    return clearRes;
}

export function createCart(host, cartId) {
    const createRes = http.post(`${host}/api/Cart/${cartId}?isAnonymous=false`);
    check(createRes, {
        'cart created with status 201': r => r.status === 201
    });
    return createRes;
}