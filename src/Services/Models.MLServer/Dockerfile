FROM python:3.8-slim

ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

RUN pip install -U pip \
    && apt-get update \
    && apt install -y curl netcat \
    && curl -sSL https://raw.githubusercontent.com/python-poetry/poetry/master/get-poetry.py | python -
ENV PATH="${PATH}:/root/.poetry/bin"

COPY . .
RUN poetry config virtualenvs.create false \
  && poetry install --no-interaction --no-ansi

CMD [ "poetry", "run", "python", "Models.MLServer/server.py" ]