# build environment
FROM node:alpine as build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY package*.json .
RUN npm install --silent --force
COPY . .

RUN npm run build

EXPOSE 3000
CMD [ "npm", "start" ]