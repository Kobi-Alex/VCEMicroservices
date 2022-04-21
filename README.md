# VCEMicroservices
VCE (Virtual computer environment) Microservices

# Email
 Required to register api key in https://sendgrid.com/ and set in appsettings.json in Applicant and Report Services.

# Docker (Staging)
At the root directory which include docker-compose.yml files, run below command:
 * docker-compose up -d

Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut).

Url: http://localhost:9000
 * /api/auth
 * /api/users
 * /api/categories
 * /api/questions
 * /api/answers
 * /api/exams
 * /api/reports

# Kubernetes (Production)
At the root directory which include docker-compose.yml files, run below command:
 * Build images: docker-compose build 
 * Push to Docker Hub: docker push {docker_hub_id}/servicename

NETWORK LOAD BALANCER (NLB) (https://kubernetes.github.io/ingress-nginx/deploy/#docker-desktop):
 * kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.1.3/deploy/static/provider/aws/deploy.yaml
 * Add acme.com (127.0.0.1 acme.com) in hosts: C:\Windows\System32\drivers\etc
 * kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"
  
At the root directory which include kubernetes files, run below command:
 * kubectl apply -f mssql-depl.yaml
 * kubectl apply -f users-depl.yaml
 * kubectl apply -f questions-depl.yaml
 * kubectl apply -f exams-depl.yaml
 * kubectl apply -f reports-depl.yaml

Url: http://acme.com:
 * /api/auth
 * /api/users
 * /api/categories
 * /api/questions
 * /api/answers
 * /api/exams
 * /api/reports 

# Tutorial
https://www.youtube.com/watch?v=DgVjEo3OGBI&t=19801s&ab_channel=LesJackson

 
