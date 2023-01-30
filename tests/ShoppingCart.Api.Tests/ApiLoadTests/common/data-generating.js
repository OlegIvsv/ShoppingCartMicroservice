import {randomIntBetween, uuidv4,} from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

export function createTestItem() {
    return {
        productId: uuidv4(),
        productTitle: "Some Product Title",
        itemQuantity: randomIntBetween(1, 10),
        unitPrice: Math.random() * (1000 - 0.1) + 0.1,
        discount: Math.random() * 0.7
    };
}

export function createUserInfo(){
    return {
        vuCustomerId: uuidv4(),
        hasCart: false,
        numberOfItemsInCart: 0,
        itemsLimitForThisUser: randomIntBetween(5, 15),
        requiresClear: false
    };
}