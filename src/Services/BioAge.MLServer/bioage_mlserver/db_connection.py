from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.ext.declarative import declarative_base

engine = create_engine(
    "postgresql+psycopg2://admin:543873@45.11.27.34/mlserver",
    pool_pre_ping=True,
    connect_args={
        "keepalives": 1,
        "keepalives_idle": 30,
        "keepalives_interval": 10,
        "keepalives_count": 5,
    },
)

DeclarativeBase = declarative_base()
DeclarativeBase.metadata.create_all(engine)
Session = sessionmaker(bind=engine)
session = Session()
