FROM python:3.9

WORKDIR /src/Services/Models.MLServer
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

RUN pip install poetry
ENV PATH="${PATH}:/root/.poetry/bin"
COPY /src/Services/Models.MLServer .
RUN pip install -r requirements.txt
RUN poetry config virtualenvs.create false \
  && poetry install --no-interaction --no-ansi

CMD [ "poetry", "run", "python", "mlserver/server.py" ]