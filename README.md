# Shoping cart microservice 🛒🛒🛒

## Main responsibilities ❔

 1. Storing shopping carts and items they contain.
 
 2. Providing API to create, and delete baskets, allows us to manipulate items in a particular shopping cart and clear baskets. Also, it should let us get cart data.

 3. The main goal we are trying to achieve is to avoid losing the user's shopping cart by storing it on the server side instead of using local storage or, what is even worth, session storage. So, if the user login with another device or browser, his shopping cart will still be filled with selected items.

## Approximate data format can be described as following 🧩

```json
{
    "id": "f434c23b-cdc0-436f-832a-08541b3c3ae1",
    "items": [
        {
            "id": "a90da8ee-af2a-4d8e-863c-d384c82650ea",
            "productId": "df473118-8878-443d-b8a0-864e612d593a",
            "productTitle": "Mango",
            "unitPrice": 54.00,
            "discount": 0.05,
            "quantity" : 7,
        }
    ]
}
```



## API 🪟




####  🔑 Get shopping cart by customer id

```js
 GET api/cart/{customerId}
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
             "id": "00000000-0000-0000-0000-000000000000",
            "productId": "00000000-0000-0000-0000-000000000000",
            "quantity": 1,
            "productTitle": "text",
            "unitPrice": 0.00,
            "discount": 0.00
        }
    ]
}
 ```
 



 #### 🔑 Create a new shopping cart
 
 ```js
POST api/cart/{customerId}
```
 Creates empty shopping carts for new users.

 ***Input:***

- **customerId**

 ***Output:***

- 2xx status code if was created successfully, 4xx otherwise




#### 🔑 Delete an existing shopping cart 

```js
DELETE api/cart/{customerId}
```

Deletes empty shopping carts for the user.

 ***Input:***

    - customerIdd

 ***Output:***

2XX status code if the cart has been deleted successfully, 4XX otherwise



#### 🔑 Clear shopping cart

```js 
PUT api/cart/clear/{customerId}
```
Clears shopping cart.

 ***Input:***

- **customerId**

 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise



#### 🔑 Put item to shopping cart

```js
PUT api/cart/put-item/{customerId}
```
Puts product to shoping cart. If there is an item for this product in the cart, quantity will be added up. Otherwise, the product will just be added to the cart.

 ***Input:***

 ```json
 {
    "id": "00000000-0000-0000-0000-000000000000",
    "productId": "00000000-0000-0000-0000-000000000000",
    "quantity": 1,
    "productTitle": "text",
    "unitPrice": 0.00,
    "discount": 0.00
}
 ```
 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise



#### 🔑 Update item in shopping cart

```js
PUT api/cart/update-item/{customerId}
```
Updates product quantity or adds the product to the customer's shopping cart if the cart doesn't contain it.

 ***Input:***

 ```json
 {
    "id": "00000000-0000-0000-0000-000000000000",
    "productId": "00000000-0000-0000-0000-000000000000",
    "quantity": 1,
    "productTitle": "text",
    "unitPrice": 0.00,
    "discount": 0.00
}
 ```
 
 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise



#### 🔑 Remove item from shopping cart

```js
api/cart/remove-item/{customerId}?{productId}
```
Removes item from shopping cart.

 ***Input:***

- customerId
- productId

 ***Output:***

2XX status code if the cart has been cleared successfully, 4XX otherwise


