#!/usr/bin/env bash

source ./build.sh

DOCKER_USERNAME="vvhistler"
DOCKER_PASSWORD="!!!!!!!!!!!!"
DOCKER_IMAGE="$DOCKER_USERNAME/webtracksystem:local"

# build docker images
cp dockerfile "./$ARTIFACTS"
docker build -t $DOCKER_IMAGE "./$ARTIFACTS" --build-arg WEB_CONTENT=$WEB_CONTENT --build-arg API_CONTENT=$API_CONTENT
docker login --username $DOCKER_USERNAME --password $DOCKER_PASSWORD
docker push $DOCKER_IMAGE
rm -r "./$ARTIFACTS"