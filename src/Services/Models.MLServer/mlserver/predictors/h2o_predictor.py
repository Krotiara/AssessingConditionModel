from mlserver.predictor import Predictor

class H2OPredictor(Predictor):
    def __init__(self, model):
        self._model = model
   
    def predict(self, x):
        pass