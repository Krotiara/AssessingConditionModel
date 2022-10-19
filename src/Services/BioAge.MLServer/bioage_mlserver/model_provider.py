from io import BytesIO
import dill
import boto3
import numpy as np


class ModelProvider:
    def __init__(self, s3_url, region, s3_access_key_id, s3_access_key):
        self._active_models = {}
        s3_res = boto3.resource(
            service_name="s3",
            endpoint_url=s3_url,
            aws_access_key_id=s3_access_key_id,
            aws_secret_access_key=s3_access_key,
            region_name=region,
        )
        self._s3_bucket = s3_res.Bucket("bioage")  # type: ignore

    def get_model(self, name):
        return self._active_models[name]

    def load_model(self, model_meta):
        if len(model_meta.actives) == 0:
            return
        model = self._get_model_from_s3(model_meta)
        for active in model_meta.actives:
            self._active_models[active.name] = model

    def load_models(self, model_metas):
        for meta in model_metas:
            self.load_model(meta)

    def _get_model_from_s3(self, model_meta):
        model_key = self._get_key(model_meta, "model")
        print(model_key)
        model = None
        with BytesIO() as data:
            self._s3_bucket.download_fileobj(model_key, data)
            data.seek(0)
            model = dill.load(data)
        additionals = []
        for additional_name in model_meta.additionals:
            key = self._get_key(model_meta, additional_name)
            with BytesIO() as data:
                self._s3_bucket.download_fileobj(key, data)
                data.seek(0)
                handler = dill.load(data)
                additionals.append(handler)
        
        return Predictor(model, additionals)

    def _get_key(self, model_meta, name):
        return f"{model_meta.pipeline_name}={model_meta.version}={name}"


class Predictor:
    def __init__(self, model, additionals):
        self._model = model
        self._additionals = additionals
    
    def predict(self, x):
        for additional in self._additionals:
            print(x)
            x = additional.transform(x)
        print(x)
        predicted = self._model.predict(x)
        return predicted
