from sqlalchemy import JSON, Column, Float, String, Boolean, ARRAY, Integer
from sqlalchemy.orm import relationship
from db_connection import DeclarativeBase

class ModelMeta(DeclarativeBase):
    __tablename__ = 'Models'
    storage_Id = Column('Id', String, primary_key=True)
    version = Column('Version', String, primary_key=True)
    file_name = Column('FileName', String)
    accuracy = Column('Accuracy', Integer)
    input_count = Column('InputParamsCount', Integer)
    output_count = Column('OutputParamsCount', Integer)
    params_names = Column("ParamsNames", ARRAY(String))

    def to_dict(self):
        return dict(storage_id=self.storage_Id, version=self.version,
                    file_name=self.file_name, accuracy=self.accuracy,
                    input_count=self.input_count,output_count=self.output_count,
                    params_names=self.params_names)