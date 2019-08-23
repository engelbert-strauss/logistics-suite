FROM microsoft/dotnet:2.2-sdk AS restore
ARG PROJECT
WORKDIR /app
COPY ./${PROJECT}/${PROJECT}.csproj ./${PROJECT}/
COPY ./LogisticsSuite.Infrastructure/LogisticsSuite.Infrastructure.csproj ./LogisticsSuite.Infrastructure/
RUN dotnet new sln --name=${PROJECT} && dotnet sln ${PROJECT}.sln add ./${PROJECT}/${PROJECT}.csproj && dotnet sln ${PROJECT}.sln add ./LogisticsSuite.Infrastructure/LogisticsSuite.Infrastructure.csproj
RUN dotnet restore

FROM restore AS publish
ARG PROJECT
COPY ./${PROJECT}/. ./${PROJECT}/
COPY ./LogisticsSuite.Infrastructure/. ./LogisticsSuite.Infrastructure/
WORKDIR /app/${PROJECT}/
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
ARG PROJECT
WORKDIR /app
COPY --from=publish /app/${PROJECT}/out .
RUN echo "#!/bin/sh" > /bootstrap.sh && echo "dotnet ${PROJECT}.dll" >> /bootstrap.sh && chmod +x /bootstrap.sh
ENTRYPOINT /bootstrap.sh
