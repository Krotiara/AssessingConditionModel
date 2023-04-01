FROM python:3.9

WORKDIR /app
EXPOSE 5000

WORKDIR /src/Services/Models.MLServer
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

RUN pip install --upgrade pip
RUN pip install poetry
RUN pip install -U apispec
RUN pip install apispec-webframeworks
RUN pip install -U apispec[marshmallow]
ENV PATH="${PATH}:/root/.poetry/bin"
COPY /src/Services/Models.MLServer .
RUN pip install -r requirements.txt
RUN poetry config virtualenvs.create false \
  && poetry install --no-interaction --no-ansi

CMD [ "poetry", "run", "python", "mlserver/server.py" ]