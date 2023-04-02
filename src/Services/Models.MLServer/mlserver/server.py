import json
from flask import Flask, jsonify, request
from flask_cors import CORS, cross_origin
import numpy as np
from models.upload_model import UploadModel
from model_provider import ModelProvider
from models.model_meta import ModelMeta
from models.model_key import ModelPredictRequest
import db_connection
import json
from base64 import b64decode
import h2o


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
app.config.update({
    'APISPEC_SWAGGER_URL': '/swagger/',  # URI to access API Doc JSON
    'APISPEC_SWAGGER_UI_URL': '/swagger-ui/'  # URI to access UI of API Doc
})

@app.route('/models/<model_id>==<version>', methods=['GET'])
@cross_origin()
def get_model_meta_by_key(model_id, version):
    meta = db_connection.session \
        .query(ModelMeta) \
        .filter_by(StorageId=model_id) \
        .filter_by(Version=version) \
        .one_or_none()
    if meta is not None:
        return jsonify(meta.to_dict()), 200
    else:
        return "", 404


@app.route('/models/insert', methods=['POST'], endpoint='upload_model')
@cross_origin()
@convert_input_to(UploadModel)
def upload_model(upload_model):
    meta = ModelMeta(upload_model.Meta)
    data = b64decode(upload_model.DataBytes) #base 64
    model_provider.upload_model(data, meta.FileName)
    db_connection.session.add(meta)
    db_connection.session.commit()
    return "", 200


@app.route('/models/predict', methods=['POST'], endpoint='predict')
@cross_origin()
@convert_input_to(ModelPredictRequest)
def predict(request):
    #only for h2o for now
    responce = get_model_meta_by_key(request.Id, request.Version)
    if responce.status == 404:
        return "", 404
    meta = ModelMeta(responce.get_json())
    model_provider.load_model_from_s3(meta)
    model = model_provider.get_model(meta.FileName)
    input_data = np.array(request.Input).reshape(1, -1)
    prediction = model.predict(input_data)
    return jsonify(list(prediction))


@app.route('/')
def index():
    return 'App Works!'

if __name__ == '__main__':
    metas = db_connection.session.query(ModelMeta).all()
    app.run(port=5000, host='0.0.0.0', debug=True)
