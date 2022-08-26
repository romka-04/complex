# build docker images
docker build -t romka04/romka04-complex-worker:latest -t romka04/romka04-complex-worker:$SHA -f './src/Workers/Romka04.Complex.Worker/Dockerfile' .
docker build -t romka04/romka04-complex-webapi:latest -t romka04/romka04-complex-webapi:$SHA -f './src/Web/Romka04.Complex.WebApi/Dockerfile' .

# take those images and push them to docker hub (both tags)
docker push romka04/romka04-complex-worker:latest
docker push romka04/romka04-complex-webapi:latest
docker push romka04/romka04-complex-worker:$SHA
docker push romka04/romka04-complex-webapi:$SHA

# apply all kubernates configuration files
kubectl apply -f ./k8s/

# update to last version of docker images
kubectl set image deployment/client-deployment client=romka04/romka04-complex-webapi:$SHA
kubectl set image deployment/worker-deployment client=romka04/romka04-complex-worker:$SHA
