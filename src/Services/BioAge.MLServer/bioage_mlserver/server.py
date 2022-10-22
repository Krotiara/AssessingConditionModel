import json
from models.active_model import ActiveModel
from flask import Flask, jsonify, request
from flask_cors import CORS, cross_origin
import numpy as np
from model_provider import ModelProvider
from models.model_meta import ModelMeta
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


model_provider = ModelProvider("https://storage.yandexcloud.net", "ru-central1", "...", "...")

app = Flask(__name__)
app.json_encoder = NpEncoder
CORS(app, origins='*')
app.config["CORS_SUPPORTS_CREDENTIALS"] = "true"


@app.route('/model/active', methods=['POST'], endpoint="activate_model")
@cross_origin()
@convert_input_to(ActiveModel)
def activate_model(active):
    current_meta = db_connection.session.query(ModelMeta).filter_by(pipeline_name=active.pipeline_name).filter_by(version=active.version).one_or_none()
    if current_meta is None:
        return "", 404
    current_meta.actives.append(active)
    model_provider.load_model(current_meta)
    db_connection.session.commit()
    return jsonify(current_meta.to_dict()), 201


@app.route('/model', methods=['PATCH'], endpoint="patch_model")
@cross_origin()
@convert_input_to(ModelMeta)
def patch_model(meta):
    current_meta = db_connection.session.query(ModelMeta).filter_by(pipeline_name=meta.pipeline_name).filter_by(version=meta.version).one_or_none()
    if current_meta is None:
        return "", 404
    current_meta.params = meta.params
    print(meta.params)
    db_connection.session.commit()
    return jsonify(current_meta.to_dict()), 200


@app.route('/model/active/<name>', methods=['DELETE'], endpoint="delete_active")
@cross_origin()
def delete_active(name):
    current_meta = db_connection.session.query(ActiveModel).filter_by(name=name).one_or_none()
    if current_meta is None:
        return "", 404
    db_connection.session.delete(current_meta)
    db_connection.session.commit()
    return jsonify(current_meta.to_dict()), 200


@app.route('/model', methods=['POST'])
@cross_origin()
@convert_input_to(ModelMeta)
def add_model_meta(meta):
    db_connection.session.add(meta)
    db_connection.session.commit()
    return jsonify(meta.to_dict()), 201


@app.route('/model/<model_name>', methods=['GET'])
@cross_origin()
def get_model_meta_by_name(model_name):
    meta = db_connection.session.query(ActiveModel).filter_by(name=model_name).one_or_none()
    if meta is not None:
        return jsonify(meta.to_dict()), 200
    else:
        return "", 404


@app.route('/model/<pipeline_name>==<version>', methods=['GET'])
@cross_origin()
def get_model_meta_by_key(pipeline_name, version):
    meta = db_connection.session.query(ModelMeta).filter_by(pipeline_name=pipeline_name).filter_by(version=version).one_or_none()
    if meta is None:
        return "", 404
    return jsonify(meta.to_dict()), 200


@app.route('/model', methods=['GET'])
@cross_origin()
def get_models_metas():
    metas = db_connection.session.query(ModelMeta).order_by(ModelMeta.score.desc())
    return jsonify([meta.to_dict() for meta in metas]), 200


@app.route('/model/<model_name>/predict', methods=['POST'])
@cross_origin()
def predict(model_name):
    array = np.array(request.json).reshape(1, -1)
    prediction = model_provider.get_model(model_name).predict(array)
    return jsonify(list(prediction))


if __name__ == '__main__':
    metas = db_connection.session.query(ModelMeta).options(joinedload(ModelMeta.actives)).all()
    model_provider.load_models(metas)
    app.run(port=8080, host='0.0.0.0')
