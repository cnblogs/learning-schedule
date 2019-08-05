FROM nginx:latest
ARG GIT_COMMIT
ENV GIT_COMMIT=${GIT_COMMIT}
LABEL GIT_COMMIT=${GIT_COMMIT}
WORKDIR /app
COPY nginx.conf /etc/nginx/nginx.conf
RUN echo "nginx -g 'daemon off;'" > run.sh
