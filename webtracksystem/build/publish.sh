
cd docker
docker build -t gcr.io/gsmtrackersystem/webtrackersystem:v0 .
gcloud docker -- push gcr.io/gsmtrackersystem/webtrackersystem:v0

#gcloud container clusters create webtrackersystem-cluster --num-nodes 1 --zone europe-west1-b
#gcloud container clusters get-credentials webtrackersystem-cluster --zone europe-west1-b --project gsmtrackersystem
#kubectl run webtrackersystem --image=gcr.io/gsmtrackersystem/webtrackersystem:v0 --port=5000
#kubectl expose deployment webtrackersystem --type="LoadBalancer"