import json
from flask import Flask, jsonify, request
from flask_cors import CORS, cross_origin
import numpy as np
from model_provider import ModelProvider
from Models.model_meta import ModelMeta
import db_connection
from sqlalchemy.orm import joinedload


class NpEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, np.integer):
            return int(obj)
        if isinstance(obj, np.floating):
            return float(obj)
        if isinstance(obj, np.ndarray):
            return obj.tolist()
        return super(NpEncoder, self).default(obj)


def convert_input_to(class_):
    def wrap(f):
        def decorator(*args):
            obj = class_(**request.get_json())
            return f(obj)
        return decorator
    return wrap


model_provider = ModelProvider("https://storage.yandexcloud.net",
                               "ru-central1",
                               "YCAJEWxO5xrA69YYAGDJjT1jV",
                               "YCNFkrMvC2Hp4F4bDF7nYLRuv6uRIFy4lZUVtY54",
                               "modelsstorage")

app = Flask(__name__)
app.json_encoder = NpEncoder
CORS(app, origins='*')
app.config["CORS_SUPPORTS_CREDENTIALS"] = "true"

