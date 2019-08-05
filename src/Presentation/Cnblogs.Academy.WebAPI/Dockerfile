FROM microsoft/dotnet:2.2-aspnetcore-runtime
ARG GIT_COMMIT
ENV GIT_COMMIT=${GIT_COMMIT}
LABEL GIT_COMMIT=${GIT_COMMIT}
WORKDIR /app
COPY ./bin/publish .
HEALTHCHECK --interval=5s --timeout=20s \
    CMD curl -fs -o /dev/null localhost/alive || exit 1
RUN echo "dotnet Cnblogs.Academy.WebAPI.dll" > run.sh
