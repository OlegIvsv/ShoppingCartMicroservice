from pymongo import MongoClient
from random_models import RandomCart
from flask import Flask, request, Response

CONNECTION_STRING = "mongodb://adminn:passwordd@localhost:27017/"
DB_NAME = "test_shop_db"
COLLECTION_NAME = "test_collection_1"

app = Flask(__name__)

def get_collection():
    client = MongoClient(CONNECTION_STRING, uuidRepresentation='csharpLegacy')
    db = client[DB_NAME]
    return db[COLLECTION_NAME]


@app.route('/refill-db', methods=['GET'])
def refill_db():
    collection = get_collection()
    collection.drop()
    number_of_documents = request.args.get("number", type=int)
    carts = (RandomCart(10) for i in range(0, number_of_documents))
    collection.insert_many([cart.to_mongo_document() for cart in carts])

    return Response(status=200)


if __name__ == '__main__':
    app.run(port=5000)