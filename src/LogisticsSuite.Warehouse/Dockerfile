FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
COPY . .

FROM build AS publish
WORKDIR /app
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT [ "dotnet", "LogisticsSuite.Replenishment.dll" ]
