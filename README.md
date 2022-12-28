# Shoping cart microservice 🛒🛒🛒

## Main responsibilities ❔

 1. Storing shopping carts and items they contain.
 
 2. Providing API to create, and delete baskets, allows us to manipulate items in a particular shopping cart and clear baskets. Also, it should let us get cart data.

 3. The main goal we are trying to achieve is to avoid losing the user's shopping cart by storing it on the server side instead of using local storage or, what is even worth, session storage. So, if the user login with another device or browser, his shopping cart will still be filled with selected items.

## Approximate data format can be described as following 🧩

```json
{
    "id": "f434c23b-cdc0-436f-832a-08541b3c3ae1",
    "customerId": "7397b6ad-279c-4fea-a530-bd1c1f2d9350",
    "items": [
        {
            "productId": "beb453ac-a070-4759-9e24-55d32c86be4b",
            "productTitle": "Mango",
            "unitPrice": 54.00,
            "quantity" : 7,
        }
    ]
}
```

## API 🪟

#### GET 

```js
 api/cart/{customerId}
 ```
 Returns shoping cart.

 ***Input:*** 

- **customerId** 

 ***Output:***

 ```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "customerId": "00000000-0000-0000-0000-000000000000",
    "items": [
        {
            "productId": "00000000-0000-0000-0000-000000000000",
            "quantity": 1,
            "productTitle": "",
            "unitPrice": 0.00
        }
    ]
}
 ```
 
 #### POST 
 
 ```js
api/cart/{customerId}
```
 Creates empty shopping carts for new users.

 ***Input:***

- **customerId**

 ***Output:***

- 2xx status code if was created successfully, 4xx otherwise

#### DELETE 

```js
api/cart/{customerId}
```

Deletes empty shopping carts for the user.

 ***Input:***

    - customerIdd

 ***Output:***

2XX status code if the cart has been deleted successfully, 4XX otherwise

#### PUT

```js 
api/cart/clear/{customerId}
```
Clears shopping cart.

 ***Input:***

- **customerId**

 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise

```js
api/cart/put-item/{customerId}
```
Puts product to shoping cart.

 ***Input:***

 ```json
{
    "productId": "00000000-0000-0000-0000-000000000000",
    "productTitle": "",
    "unitPrice": 0.00,
    "quantity" : 1 
}
 ```


 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise


```js
api/cart/remove-item/{customerId}?{productId}
```
Puts product to shoping cart.

 ***Input:***

- customerId
- productId

 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise


