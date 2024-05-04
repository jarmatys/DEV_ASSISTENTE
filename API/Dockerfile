FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG PROJECT_PATH
ARG ASSEMBLY_NAME

WORKDIR /src
COPY ["${PROJECT_PATH}/${ASSEMBLY_NAME}.csproj", "${PROJECT_PATH}/"]
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages dotnet restore "${PROJECT_PATH}/${ASSEMBLY_NAME}.csproj"

COPY . .
WORKDIR "/src/${PROJECT_PATH}"
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages dotnet publish "${ASSEMBLY_NAME}.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

ARG ASSEMBLY_NAME

ENV RUN_ASSEMBLY_FILENAME="${ASSEMBLY_NAME}.dll"

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT dotnet ${RUN_ASSEMBLY_FILENAME}