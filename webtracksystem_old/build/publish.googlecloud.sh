#!/usr/bin/env bash

source ./build.sh

DOCKER_IMAGE="gcr.io/gsmtrackersystem/webtracksystem:local"

cp dockerfile "./$ARTIFACTS"
docker build -t $DOCKER_IMAGE "./$ARTIFACTS" --build-arg WEB_CONTENT=$WEB_CONTENT --build-arg API_CONTENT=$API_CONTENT
gcloud docker -- push $DOCKER_IMAGE
rm -r $ARTIFACTS

#gcloud container clusters create webtrackersystem-cluster --num-nodes 1 --zone europe-west1-b
#gcloud container clusters get-credentials webtrackersystem-cluster --zone europe-west1-b --project gsmtrackersystem
#kubectl run webtrackersystem --image=gcr.io/gsmtrackersystem/webtrackersystem:v0 --port=5000
#kubectl expose deployment webtrackersystem --type="LoadBalancer"