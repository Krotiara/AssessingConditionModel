from sqlalchemy import JSON, Column, Float, String, Boolean, ARRAY
from sqlalchemy.orm import relationship
from db_connection import DeclarativeBase

class ModelMeta(DeclarativeBase):
    __tablename__ = 'Models'
    storage_Id = Column('StorageId', String, primary_key=True)
    version = Column('Version', String, primary_key=True)
    file_name = Column('Name', String)
    accuracy = Column('Accuracy', Float)
    input_count = Column('InputParamsCount', int)
    output_count = Column('OutputParamsCount', int)
    params_names = Column("ParamsNames", ARRAY(String))

    def to_dict(self):
        return dict(storage_id=self.storage_Id, version=self.version,
                    file_name=self.file_name, accuracy=self.accuracy,
                    input_count=self.input_count,output_count=self.output_count,
                    params_names=self.params_names)