# build environment
FROM node:alpine as build
WORKDIR /app

COPY package*.json .
RUN npm install --silent --force
COPY . .

RUN npm run build -- --profile

EXPOSE 3000
ENTRYPOINT [ "npm", "start" ]