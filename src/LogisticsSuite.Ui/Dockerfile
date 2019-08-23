FROM node:10.16.3-alpine as build
WORKDIR /app
COPY . .
RUN yarn install && yarn build

FROM nginx:1.15.5
RUN echo 'server {\n\
  listen 80;\n\
  location / {\n\
    root /usr/share/nginx/html;\n\
    index index.html index.htm;\n\
    try_files $uri $uri/ /index.html =404;\n\
  }\n\
  location /logisticssuite.backend/ws {\n\
    proxy_pass http://logisticssuite-backend/ws;\n\
    proxy_http_version 1.1;\n\
    proxy_set_header Upgrade $http_upgrade;\n\
    proxy_set_header Connection "upgrade";\n\
  }\n\
  location /logisticssuite.backend/ {\n\
    proxy_pass http://logisticssuite-backend/;\n\
  }\n\
}\n' > /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist/ /usr/share/nginx/html
ENTRYPOINT ["nginx", "-g", "daemon off;"]
