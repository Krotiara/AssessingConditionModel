class BiomarkerMeta:
    def __init__(self, id, name, unit, min, max):
        self.id = id
        self.name = name
        self.unit = unit
        self.min = min
        self.max = max

    def to_dict(self):
        return dict(id=self.id, name=self.name, unit=self.unit, min=self.min, max=self.max)
