#!/bin/bash

repository_name="$1"

if [ -z "$repository_name" ]; then
  echo "Usage: $0 <repository_name>"
  exit 1
fi

# Build and push the authservice Docker image
docker build -t "$repository_name/authservice" -f AuthService.API/Dockerfile .
docker push "$repository_name/authservice"

# Build and push the productservice Docker image
docker build -t "$repository_name/productservice" -f ProductService.API/Dockerfile .
docker push "$repository_name/productservice"

cd K8S/

# Apply the auths-depl.yaml configuration
kubectl rollout restart deployment auths-depl

# Apply the products-depl.yaml configuration
kubectl rollout restart deployment products-depl