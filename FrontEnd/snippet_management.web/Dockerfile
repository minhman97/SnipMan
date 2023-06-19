# build environment
FROM node:alpine as build
WORKDIR /app

COPY package*.json .
RUN npm install --force
COPY . .

RUN npm run build
# test
EXPOSE 3000
ENTRYPOINT [ "npm", "start" ]
