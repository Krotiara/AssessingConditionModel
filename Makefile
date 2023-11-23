COMPOSE_ARGS=-f docker-compose.yml

push:
	docker compose build --pull
	docker compose ${COMPOSE_ARGS} push

up:
	docker compose ${COMPOSE_ARGS} up -d --build

upgrade:
	docker compose ${COMPOSE_ARGS} pull
	docker compose ${COMPOSE_ARGS} up -d --build

log:
	docker compose ${COMPOSE_ARGS} logs -f -n 100
