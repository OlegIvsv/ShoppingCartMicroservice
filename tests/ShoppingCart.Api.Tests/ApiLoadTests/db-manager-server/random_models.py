import uuid
from random_titles import random_title
from bson import Decimal128 as MongoDecimal
import random


class RandomCartItem:

    def __init__(self, max_price, max_quantity):
        self.title = random_title()
        self.price = MongoDecimal(str(random.uniform(0.1, max_price)))
        self.quantity = random.randint(1, max_quantity)
        self.discount = random.uniform(0, 0.7)
        self.id = uuid.uuid4()
        self.product_id = uuid.uuid4()

    def to_mongo_document(self) -> dict:
        return {
            "_id": self.id,
            "productId": self.product_id,
            "productTitle": self.title,
            "unitPrice": self.price,
            "quantity": self.quantity,
            "discount": round(self.discount, 2)
        }


class RandomCart:

    def __init__(self, max_items_number):
        self.items = [RandomCartItem(1000, 20) for i in range(1, max_items_number)]
        self.id = uuid.uuid4()

    def to_mongo_document(self) -> dict:
         return {
            "_id": self.id,
            "items": ([item.to_mongo_document() for item in self.items])
        }

