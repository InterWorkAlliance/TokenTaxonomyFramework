#https://docs.microsoft.com/en-us/azure/aks/ingress-static-ip
# Create a namespace for your ingress resources
#kubectl create namespace ingress-basic

# Use Helm to deploy an NGINX ingress controller
helm install stable/nginx-ingress \
    --namespace ingress-basic \
    --set controller.replicaCount=2 \
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux \
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux \
    --set controller.service.loadBalancerIP="137.135.65.212"

#!/usr/bin/env bash
cat launch-aks.yaml | linkerd inject - | kubectl apply -f -
kubectl cp artifacts ttf:/mnt/azure/artifacts