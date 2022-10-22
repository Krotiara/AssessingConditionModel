from sqlalchemy import JSON, Column, Float, String, Boolean
from sqlalchemy.orm import relationship
from db_connection import DeclarativeBase

class ModelMeta(DeclarativeBase):
    __tablename__ = 'model_meta_new'

    pipeline_name = Column('pipeline_name', String, primary_key=True)
    version = Column('version', String, primary_key=True)
    additionals = Column('additionals', JSON)
    score = Column('score', Float)
    params = Column('params', JSON)
    actives = relationship("ActiveModel", backref="model_meta_new")

    def to_dict(self):
        return dict(pipeline_name=self.pipeline_name, version=self.version, additionals=self.additionals, score=self.score, params=self.params, actives=[active.name for active in self.actives])
