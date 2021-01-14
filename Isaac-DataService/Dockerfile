# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source

COPY . ./
# copy everything else and build app
RUN dotnet publish -c release -o /service -r linux-musl-x64 --self-contained true

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /service
COPY --from=build /service ./

# See: https://github.com/dotnet/announcements/issues/20
# Uncomment to enable globalization APIs (or delete)
#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
#RUN apk add --no-cache icu-libs
#ENV LC_ALL en_US.UTF-8
#ENV LANG en_US.UTF-8

EXPOSE 80

ENTRYPOINT ["dotnet", "Isaac-DataService.dll"]