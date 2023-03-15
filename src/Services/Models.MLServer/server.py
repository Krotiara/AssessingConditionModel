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


@app.route('/models/<model_id>==<version>', methods=['GET'])
@cross_origin()
def get_model_meta_by_key(model_id, version):
    meta = db_connection.session \
        .query(ModelMeta) \
        .filter_by(storage_Id=model_id) \
        .filter_by(version=version) \
        .one_or_none()
    if meta is not None:
        return jsonify(meta.to_dict()), 200
    else:
        return "", 404


@app.route('/models/insert', methods=['POST'])
@cross_origin()
def upload_model(model_meta, file_name):
    json_data = request.get_json()
    print(json_data)


@app.route('/models/predict', methods=['POST'])
@cross_origin()
def predict(model_id, version):
    input_args = np.array(request.json).reshape(1, -1)
    meta = get_model_meta_by_key(model_id, version)
    if meta[1] == 404:
        return "", 404
    meta = ModelMeta(meta[0])
    model = model_provider.get_model_from_s3(meta)
    print(model)
