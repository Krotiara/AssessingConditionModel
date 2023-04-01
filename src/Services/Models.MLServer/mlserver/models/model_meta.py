from sqlalchemy import JSON, Column, Float, String, Boolean, ARRAY, Integer
from sqlalchemy.orm import relationship
from db_connection import DeclarativeBase

class ModelMeta(DeclarativeBase):
    def __init__(self, d=None):
            if d is not None:
                for key, value in d.items():
                    setattr(self, key, value)

    __tablename__ = 'Models'
    StorageId = Column('Id', String, primary_key=True)
    Version = Column('Version', String, primary_key=True)
    FileName = Column('FileName', String)
    Accuracy = Column('Accuracy', Integer)
    InputCount = Column('InputParamsCount', Integer)
    OutputCount = Column('OutputParamsCount', Integer)
    ParamsNames = Column("ParamsNames", ARRAY(String))

    def to_dict(self):
        return dict(StorageId=self.StorageId, Version=self.Version,
                    FileName=self.FileName, Accuracy=self.Accuracy,
                    InputCount=self.InputCount,OutputCount=self.OutputCount,
                    ParamsNames=self.ParamsNames)