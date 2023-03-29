
/* create databases */
db = db.getSiblingDB("shop_db");
db.createCollection("shopping_carts");

/* delete abandoned carts with index after 14 days without activity */
db.shopping_carts.createIndex(
    { lastModifiedDate: 1 },
    { 
        partialFilterExpression: { isAnonymous: true},
        expireAfterSeconds: 300 //86400 * 14                  
    }
);