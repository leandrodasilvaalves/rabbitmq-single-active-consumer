#!/bin/bash

docker compose down api-1 api-2 api-3
docker compose up -d --build