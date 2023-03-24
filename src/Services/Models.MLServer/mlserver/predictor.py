class Predictor:
    def __init__(self, model):
        self._model = model

    def predict(self, x):
        predicted = self._model.predict(x)
        return predicted