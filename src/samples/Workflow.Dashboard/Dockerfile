FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y libgdiplus && apt-get -y install msmtp && rm -rf /var/lib/apt/lists/*


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY /FirstCall.sln ./
COPY /docker-compose.dcproj ./
COPY ["src/Services/FirstCallServices/FirstCall.Host.SignalR/FirstCall.Host.SignalR.csproj", "src/Services/FirstCallServices/FirstCall.Host.SignalR/"]
COPY ["src/Services/FirstCallServices/FirstCallServices.Api/FirstCallServices.Api.csproj", "src/Services/FirstCallServices/FirstCallServices.Api/"]
COPY ["src/Services/FirstCallServices/FirstCall.Data/FirstCall.Data.csproj", "src/Services/FirstCallServices/FirstCall.Data/"]
COPY ["src/Services/FirstCallServices/FirstCall.Data.EntityFramework/FirstCall.Data.EntityFramework.csproj", "src/Services/FirstCallServices/FirstCall.Data.EntityFramework/"]
COPY ["src/Services/FirstCallServices/Workflow.Events.FirstCall/Workflow.Events.FirstCall.csproj", "src/Services/FirstCallServices/Workflow.Events.FirstCall/"]


COPY ["src/Services/Messaging/Messaging.Api/Messaging.Api.csproj", "src/Services/Messaging/Messaging.Api/"]
COPY ["src/Building Blocks/EventBus/EventBus/EventBus.csproj", "src/Building Blocks/EventBus/EventBus/"]
COPY ["src/Building Blocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "src/Building Blocks/EventBus/EventBus.RabbitMQ/"]
COPY ["src/Building Blocks/EventBus/IntegrationEventLog.EF/IntegrationEventLog.EF.csproj", "src/Building Blocks/EventBus/IntegrationEventLog.EF/"]
COPY ["src/Building Blocks/Messaging/Messaging/Messaging.csproj", "src/Building Blocks/Messaging/Messaging/"]
COPY ["src/Building Blocks/Messaging/Messaging.SendGrid/Messaging.SendGrid.csproj", "src/Building Blocks/Messaging/Messaging.SendGrid/"]
COPY ["src/Building Blocks/WebHost/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj","src/Building Blocks/WebHost/WebHostCustomization/WebHost.Customization/"]

COPY ["src/ApiGateways/ApiGw-Base/OcelotApiGw.csproj", "src/ApiGateways/ApiGw-Base/"]
COPY ["src/ApiGateways/Web.Bff.Operations/Web.Operations.HttpAggregator/Web.Operations.HttpAggregator.csproj", "src/ApiGateways/Web.Bff.Operations/Web.Operations.HttpAggregator/"]

COPY ["src/Services/Testing/Sample.Api/Sample.Api.csproj", "src/Services/Testing/Sample.Api/"]

COPY ["src/Building Blocks/Indexing/Indexing/Indexing.csproj", "src/Building Blocks/Indexing/Indexing/"]
COPY ["src/Building Blocks/Indexing/Indexing.Algolia/Indexing.Algolia.csproj", "src/Building Blocks/Indexing/Indexing.Algolia/"]


COPY ["src/Conversion/FirstCall.FromSRS/FirstCall.FromSRS.csproj", "src/Conversion/FirstCall.FromSRS/"]

COPY ["src/Web Apps/Workflow.Dashboard/Workflow.Dashboard.csproj", "src/Web Apps/Workflow.Dashboard/"]


COPY ["src/Services/PeaceOfMindServices/PeaceOfMindServices.Api/PeaceOfMindServices.Api.csproj", "src/Services/PeaceOfMindServices/PeaceOfMindServices.Api/"]
COPY ["src/Services/PeaceOfMindServices/PeaceOfMind.Common/PeaceOfMind.Common.csproj", "src/Services/PeaceOfMindServices/PeaceOfMind.Common/"]
COPY ["src/Services/PeaceOfMindServices/PeaceOfMind.Google/PeaceOfMind.Google.csproj", "src/Services/PeaceOfMindServices/PeaceOfMind.Google/"]
COPY ["src/Services/PeaceOfMindServices/Workflow.Events.Intake/Workflow.Events.Intake.csproj", "src/Services/PeaceOfMindServices/Workflow.Events.Intake/"]


COPY ["src/Services/Identity/IdentityServer.Api/IdentityServer.Api.csproj", "src/Services/Identity/IdentityServer.Api/"]

RUN dotnet restore FirstCall.sln

COPY . .


WORKDIR "src/Web Apps/Workflow.Dashboard"
RUN dotnet publish -c Release -o /app/publish



FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Workflow.Dashboard.dll"]
