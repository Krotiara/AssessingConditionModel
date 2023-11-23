COMPOSE_ARGS=-f docker-compose.yml

up:
	docker compose ${COMPOSE_ARGS} up -d --build

upgrade:
	docker compose ${COMPOSE_ARGS} pull
	docker compose ${COMPOSE_ARGS} up -d --build

log:
	docker compose ${COMPOSE_ARGS} logs -f -n 100
