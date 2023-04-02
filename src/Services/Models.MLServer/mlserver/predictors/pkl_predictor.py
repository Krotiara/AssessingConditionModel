from predictors.predictor import Predictor

class PkLPredictor(Predictor):
    def __init__(self, model):
        self._model = model

    def predict(self, x):
        prediction = self._model.predict(x)
        return prediction