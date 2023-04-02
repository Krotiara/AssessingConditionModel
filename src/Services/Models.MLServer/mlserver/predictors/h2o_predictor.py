import h2o
from predictors.predictor import Predictor

class H2OPredictor(Predictor):
    def __init__(self, model, params_names):
        self._model = model
        self._params_names = params_names
   
    def predict(self, x):
        input_data =  h2o.H2OFrame(x)
        input_data.col_names = self._params_names
        with open('files/test.txt',"w") as f:
            f.write(str(input_data))
        res = self._model.predict(input_data)
        res = h2o.as_list(res)
        return res