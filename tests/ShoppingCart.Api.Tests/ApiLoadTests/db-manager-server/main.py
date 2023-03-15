# This script runs a single endpoint which refills the database with test data
# PORT: 5000
# COMMAND LINE PARAMETERS:
# --connection - a connection string
# --db - a name of database where data should be inserted
# --collection - a name of collection where data should be inserted
import sys
from pymongo import MongoClient
from random_models import RandomCart
from flask import Flask, request, Response
import argparse

# 1. Read the params from command line #
parser = argparse.ArgumentParser()
parser.add_argument("--connection", type=str, help="Connection string")
parser.add_argument("--db", type=str, help="Database name")
parser.add_argument("--collection", type=str, help="Collection name")
args = parser.parse_args()

CONNECTION_STRING = args.connection
DB_NAME = args.db
COLLECTION_NAME = args.collection

# 2. Check if all the necessary parameters were specified #
if None in [CONNECTION_STRING, DB_NAME, COLLECTION_NAME]:
    print("Error: Some parameters were not specified!", file=sys.stderr)
    exit()

# 3. Run the endpoint #
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
