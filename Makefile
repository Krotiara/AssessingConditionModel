COMPOSE_ARGS=-f docker-compose.yml

prepare:
	cp -f .env.test .env 2>/dev/null || true

up:
	docker compose ${COMPOSE_ARGS} up -d --build

upgrade:
	docker compose ${COMPOSE_ARGS} pull
	docker compose ${COMPOSE_ARGS} up -d --build

log:
	docker compose ${COMPOSE_ARGS} logs -f -n 100
