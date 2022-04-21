# VCEMicroservices
VCE (Virtual computer environment) Microservices

# Docker
At the root directory which include docker-compose.yml files, run below command:
 * docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut).

# Kubernetes
Execute commands:
 * Build images: docker-compose -f docker-compose.yml -f docker-compose.override.yml build -d
 * Push to Docker Hub: docker dockerhubid/servicename

At the root directory which include kubernetes files, run below command:

Execute commands:
 NETWORK LOAD BALANCER (NLB) (https://kubernetes.github.io/ingress-nginx/deploy/#docker-desktop)
 * kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.1.3/deploy/static/provider/aws/deploy.yaml
 * kubectl apply -f mssql-depl.yaml
 * kubectl apply -f users-depl.yaml
 * kubectl apply -f questions-depl.yaml
 * kubectl apply -f exams-depl.yaml
 * kubectl apply -f reports-depl.yaml


 
