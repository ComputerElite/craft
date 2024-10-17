# Include dotnet6 sdk

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
RUN apt-get update && \
    apt-get install -y curl && \
    curl -sL https://deb.nodesource.com/setup_21.x | bash - && \
    apt-get install -y nodejs && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
# Run the build frontend bash script in frontend
COPY . .
WORKDIR /app/craft
RUN rm -rf bin/ obj/
WORKDIR /app/craft
RUN dotnet build
RUN mkdir -p bin/Debug/net6.0/
RUN mkdir -p bin/Release/net6.0/
RUN dotnet tool install --global --version 6.0.33 dotnet-ef
RUN cp /app/docker_config.json /app/craft/bin/Debug/net6.0/config.json
RUN cp /app/docker_config.json /app/craft/bin/Release/net6.0/config.json
RUN cp /app/docker_config.json /app/craft/config.json
WORKDIR /app
CMD ["bash", "migrate_db_and_start.sh"]