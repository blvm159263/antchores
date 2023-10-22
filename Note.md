**Build AuthService**
`docker build -t [username-docker]/authservice -f AuthService.API/Dockerfile .`

**Push AuthService**
`docker push [username-docker]/authservice`

**Build ProductService**
`docker build -t [username-docker]/productservice -f ProductService.API/Dockerfile .`

**Push ProductService**
`docker push [username-docker]/productservice`

**Run Kubernate**
`kubectl apply -f rabbitmq-depl.yaml`
`kubectl apply -f local-pvc.yaml`

**Generate Key**
`kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"`

**Run MSSQL**
`kubectl apply -f mssql-plat-depl.yaml`
`kubectl apply -f auths-depl.yaml`
`kubectl apply -f products-depl.yaml`

**Nginx Image**
`kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml`

**Apply Ingress Nginx Config**
`kubectl apply -f ingress-srv.yaml`

**Check container status**
`kubectl get pods`

**Check deployment status**
`kubectl get deployments`

**Check service status**
`kubectl get services`

**Restart Deployment**
`kubectl rollout restart deployment [deployment-name]`

**Check logs**
`kubectl logs [pod-name]`

**Delete deployment**
`kubectl delete deployment [deployment-name]`