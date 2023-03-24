FROM python:3.8-slim

ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

RUN pip install -U pip \
    && apt-get update \
    && apt install -y curl netcat \
    && curl -sSL  https://install.python-poetry.org | python -

ENV PATH="${PATH}:/root/.local/bin"

COPY . .
RUN pip install -r requirements.txt
RUN poetry config virtualenvs.create false \
  && poetry install --no-interaction --no-ansi

CMD [ "poetry", "run", "python", "mlserver/server.py" ]