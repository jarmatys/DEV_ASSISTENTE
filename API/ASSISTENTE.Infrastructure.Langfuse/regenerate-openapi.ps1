#kiota generate -l CSharp -n 'ASSISTENTE.Infrastructure.Langfuse.Client' -c LangfuseClient -d ./openapi_fixed.yml -o ./Client

docker run --rm -v .:/local openapitools/openapi-generator-cli generate -i /local/openapi.yml -g csharp -o /local/ClientTest