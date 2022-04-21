# VCEMicroservices
VCE (Virtual computer environment) Microservices

# Docker
At the root directory which include docker-compose.yml files, run below command:
 * docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut).

# Kubernets
At the root directory which include kubernetes files, run below command:
Execute commands:
 * Build images:
  - docker build -t <your docker hub id>/servicename .
 * Push to Docker Hub:
  - docker push <your docker hub id>/servicename
 * kubectl apply -f *-depl.yaml
