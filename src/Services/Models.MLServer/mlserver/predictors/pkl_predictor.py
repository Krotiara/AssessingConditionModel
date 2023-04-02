from mlserver.predictor import Predictor

class PkLPredictor(Predictor):
    def __init__(self, model):
        self._model = model

    def predict(self, x):
        pass