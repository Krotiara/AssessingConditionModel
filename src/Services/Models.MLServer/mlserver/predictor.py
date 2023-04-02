from abc import ABC, abstractmethod

class Predictor(ABC):
    
    @abstractmethod
    def predict(self, x):
        pass
        #predicted = self._model.predict(x)
        #return predicted