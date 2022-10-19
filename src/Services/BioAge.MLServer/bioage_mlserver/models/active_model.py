from sqlalchemy import Column, String, ForeignKey, ForeignKeyConstraint
from sqlalchemy.orm import relationship
from db_connection import DeclarativeBase
from models.model_meta import ModelMeta


class ActiveModel(DeclarativeBase):
    __tablename__ = "active_model"

    name = Column("name", String, primary_key=True)
    pipeline_name = Column("pipeline_name", String)
    version = Column("version", String)
    __table_args__ = (
        ForeignKeyConstraint(
            [pipeline_name, version],
            [ModelMeta.pipeline_name, ModelMeta.version],
        ),
        {},
    )
    model = relationship("ModelMeta", backref="active_model")

    def to_dict(self):
        return dict(name=self.name, pipeline_name=self.model.pipeline_name, version=self.model.version, score=self.model.score, params=self.model.params)
