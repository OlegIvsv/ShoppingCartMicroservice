# Shoping cart microservice 🛒🛒🛒

## Table of Contents

- [❔ Main responsibilities](#main-responsibilities)
- [🧩 Approximate data format can be described as following](#approximate-data-format-can-be-described-as-following-)
- [🛠 Technologies](#technologies-and-tools)
- [🚀 Running the microservice](#running-the-microservice)
  - [🐳 Running a docker container](#running-the-microservice)
  - [💻 Running locally](#running-locally)
- [🪟 API _**v1.0**_](#api--v10)
  - [🔑 Get shopping cart by customer id](#-get-shopping-cart-by-customer-id)
  - [🔑 Create a new shopping cart](#-create-a-new-shopping-cart)
  - [🔑 Delete an existing shopping cart](#-delete-an-existing-shopping-cart)
  - [🔑 Clear shopping cart](#-clear-shopping-cart)
  - [🔑 Put item to shopping cart](#-put-item-to-shopping-cart)
  - [🔑 Update item in shopping cart](#-update-item-in-shopping-cart)
  - [🔑 Remove item from shopping cart](#-remove-item-from-shopping-cart)

## Main Responsibilities ❔

 1. Storing shopping carts and items they contain.
 
 2. Providing API to create, and delete baskets, allows us to manipulate items in a particular shopping cart and clear baskets. Also, it should let us get cart data.

 3. The main goal we are trying to achieve is to avoid losing the user's shopping cart by storing it on the server side instead of using local storage or, what is even worth, session storage. So, if the user login with another device or browser, his shopping cart will still be filled with selected items.

## Technologies and Tools

### .NET 6 and ASP.NET 6
I chose to use [.NET 6]() for this project because it is a long-term support (LTS) release with several new features and
improvements over previous versions. It also has a robust ecosystem with strong community support and tooling. While
.NET 7 was also available at the time, I decided to use .NET 6 because it was the most stable and widely adopted version
at the time.

### Docker
A tool that allows you to create, deploy, and run applications in containers. By using 
[Docker](), it becomes easier to manage and distribute applications across different environments, 
such as development, testing, and production.

### Logging
- [**Serilog**](https://www.nuget.org/packages/Serilog.AspNetCore)  —
  A logging library for .NET that provides a simple and highly configurable approach to logging, allowing developers to
  easily capture structured log data.

### API Documentation
- [**Swagger**](https://swagger.io/) — 
  An open-source toolset for building, documenting, and testing RESTful APIs that provides a user interface
  for developers to interact with and understand the API's capabilities.

### Database
- [**MongoDB**](https://www.mongodb.com/) —
  a cross-platform document-oriented NoSQL database that uses a flexible data model to store data, making it easier to
  build and scale applications.
- [**MongoDB.Driver**](https://www.nuget.org/packages/MongoDB.Driver) — 
  A package for interacting with MongoDB databases using C# code.

### Job Scheduling
- [**Quartz.NET**](https://www.nuget.org/packages/Quartz) — 
  Quartz.NET is a full-featured, open-source job scheduling library that can be integrated within 
  .NET applications, allowing developers to schedule and execute tasks based on a predefined set of criteria.

### Testing
- [**xUnit**](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing) — 
  A package for testing ASP.NET Core APIs.
- [**Moq**](https://github.com/moq/moq) —
  a popular mocking library for .NET developers that allows you to easily create mock 
  objects for testing.
- [**Mongo2Go**](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk) —
  a library for .NET developers that provides an in-memory MongoDB database instance for use 
  in testing and development environments.
- [**k6**](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk) —
  an open-source load testing tool for web applications that enables developers to test the performance and scalability
  of their applications. 


## Running the Microservice

### Running a Docker Container

1. Create `.env` file with this configuration to `/src`:

```dockerfile
#Mongo DB Initializatoin
MONGO_INIT_USER=user
MONGO_INIT_PASS=pass
# Mongo DB connection
MONGODB_CONNECTION=mongodb://user:pass@mongodb:port/
MONGODB_DB_NAME=db_name
MONGODB_COLLECTION_NAME=collection_name
# DB Cleaning options 
ABANDONMENT_PERIOD=00:05:00
CLEANUP_FREQUENCY=00:05:00
LEANUP_ENABLED=false
# Authentication & Authorization
JWT_KEY=qwertyuiopasdfghjklzxcvbnm1234567890
JWT_AUDIENCE=YourAudience
JWT_ISSUER=YourIssuer
```
2. Replace values with your configuration.
3. Run docker-compose: 

```commandline
docker compose -f ./docker-compose.yml up 
```

### Running Locally

1. Run a **_mongodb_** instanse.
2. Go to `launchSetting.json`
3. In chosen the profile repalce the `environmentVariables` section:
```json
   "environmentVariables": {
     "ASPNETCORE_ENVIRONMENT": "Production",
     "MongoDb__ConnectionString": "mongodb://adminn:passwordd@localhost:27017/",
     "MongoDb__Database": "shop_db",
     "MongoDb__ShoppingCartsCollection": "shopping_carts",
     "Jobs__CartCleanUpJob__AbandonmentPeriod": "01:00:00",
     "Jobs__CartCleanUpJob__CleanUpFrequency": "01:00:00",
     "Jobs__CartCleanUpJob__Enabled": "false",
     "Auth__Issuer": "Issuer",
     "Auth__Audience": "Audience",
     "Auth__Key": "Key"
   }
```
4. Replace values with your configuration.
5. Build and run.


## Data Format 🧩

```json
{
    "id": "f434c23b-cdc0-436f-832a-08541b3c3ae1",
    "lastModificatedDate": "2023-01-01T100:00:00.000+00:00",
    "isAnonymous": true,
    "items": [
        {
            "id": "a90da8ee-af2a-4d8e-863c-d384c82650ea",
            "productId": "df473118-8878-443d-b8a0-864e612d593a",
            "productTitle": "Mango",
            "unitPrice": 54.00,
            "discount": 0.05,
            "itemQuantity" : 7,
            "imageUrl": "https://example.com/images/example.jpg"
        }
    ]
}
```



## API 🪟 *v1.0*



####  🔑 Get shopping cart by customer id

```js
 GET api/cart/{customerId}
 ```
 Returns shoping cart.

 ***Input:*** 

- **customerId** 

 ***Output:***

 - *200* status and cart in case of success, error code and problem details otherwise

 ```json
{
    "customerId": "00000000-0000-0000-0000-000000000000",
    "lastModificatedDate": "2023-01-01T100:00:00.000+00:00",
    "isAnonymous": true,
    "items": [
        {
            "id": "00000000-0000-0000-0000-000000000000",
            "productId": "00000000-0000-0000-0000-000000000000",
            "itemQuantity": 1,
            "productTitle": "text",
            "unitPrice": 0.00,
            "discount": 0.00,
            "imageUrl": "https://example.com/images/example.jpg"
        }
    ]
}
 ```
 



 #### 🔑 Create a new shopping cart
 
 ```js
POST api/cart/{customerId}?{isAnonymous}
```
 Creates empty shopping carts for new users.

 ***Input:***

- **customerId**
- **isAnonymous** - specifies if the cart is anonymous with a bool value

 ***Output:***

- *200* status code if was created successfully, error code and problem details otherwise and just created empry cart



#### 🔑 Delete an existing shopping cart 

```js
DELETE api/cart/{customerId}
```

Deletes empty shopping carts for the user.

 ***Input:***

    - customerId

 ***Output:***

*200* status code if the cart has been deleted successfully, error code and problem details otherwise



#### 🔑 Clear shopping cart

```js 
PUT api/cart/clear/{customerId}
```
Clears shopping cart.

 ***Input:***

- **customerId**

 ***Output:***

*200* status code if the cart has been cleared successfully, error code and problem details otherwise



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
    "itemQuantity": 1,
    "productTitle": "text",
    "unitPrice": 0.00,
    "discount": 0.00,
    "imageUrl": "https://example.com/images/example.jpg"
}
 ```
 ***Output:***

*200* status code if the cart has been cleared successfully, error code and problem details otherwise



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
    "itemQuantity": 1,
    "productTitle": "text",
    "unitPrice": 0.00,
    "discount": 0.00,
    "imageUrl": "https://example.com/images/example.jpg"
}
 ```
 
 ***Output:***

*200* status code if the cart has been cleared successfully, error code and problem details otherwise



#### 🔑 Remove item from shopping cart

```js
PUT api/cart/remove-item/{customerId}?{productId}
```
Removes item from shopping cart.

 ***Input:***

- customerId
- productId

 ***Output:***

*200* status code if the cart has been cleared successfully, error code and problem details otherwise


