from io import BytesIO
import dill
import boto3
import numpy as np
import random
import string
import h2o
import os.path
from predictors.h2o_predictor import H2OPredictor
from predictors.pkl_predictor import PkLPredictor


class ModelProvider:
    def __init__(self, s3_url, region,
                 s3_access_key_id, s3_access_key, s3_bucket):
        self._active_models = {}
        self.s3_bucket_name = s3_bucket
        self.s3_res = boto3.resource(
            service_name="s3",
            endpoint_url=s3_url,
            aws_access_key_id=s3_access_key_id,
            aws_secret_access_key=s3_access_key,
            region_name=region
        )
        self._s3_bucket = self.s3_res.Bucket(s3_bucket)

    def get_model(self, name):
        return self._active_models[name]
    
    def load_model_from_s3(self, model_meta):
        if model_meta.FileName in self._active_models:
            print('model already loaded')
            return
        file_extension = os.path.splitext(model_meta.FileName)[1]
        with BytesIO() as data:
            self._s3_bucket.download_fileobj(model_meta.FileName, data)
            data.seek(0)
            if file_extension == '.zip':
                file = "files/{}.zip".format(self._get_random_string(16))
                with open(file, "wb") as f:
                    f.write(data.getbuffer().tobytes())
                model = h2o.import_mojo(file)
                model = H2OPredictor(model, model_meta.ParamsNames)
                self._active_models[model_meta.FileName] = model
                #TODO delete temp file
            elif file_extension == ".pkl":
                model = dill.load(data)
                model = PkLPredictor(model)
                self._active_models[model_meta.FileName] = model           
        
    
    def upload_model(self, bytes, filename):
        object = self.s3_res.Object(self.s3_bucket_name, filename)
        object.put(Body=bytes)

    def delete_model(self, filename):
        object = self.s3_res.Object(self.s3_bucket_name, filename)
        object.delete()

    def _get_random_string(self, length):
    # With combination of lower and upper case
        result_str = ''.join(random.choice(string.ascii_letters) for i in range(length))
        return result_str