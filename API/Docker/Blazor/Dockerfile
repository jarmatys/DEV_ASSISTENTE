FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG PROJECT_PATH
ARG ASSEMBLY_NAME

WORKDIR /src
COPY ["${PROJECT_PATH}/${ASSEMBLY_NAME}.csproj", "${PROJECT_PATH}/"]
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages dotnet restore "${PROJECT_PATH}/${ASSEMBLY_NAME}.csproj"

COPY . .
WORKDIR "/src/${PROJECT_PATH}"
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages dotnet publish "${ASSEMBLY_NAME}.csproj" -c Release -o output

FROM nginx:alpine

ARG ASSEMBLY_NAME

WORKDIR /var/www/web
COPY --from=build "/src/${ASSEMBLY_NAME}/output/wwwroot" .
COPY "Docker/Blazor/nginx.conf" "/etc/nginx/nginx.conf"
EXPOSE 80