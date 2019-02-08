FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SendMail/SendMail.csproj", "SendMail/"]
RUN dotnet restore "SendMail/SendMail.csproj"
COPY . .
WORKDIR "/src/SendMail"
RUN dotnet build "SendMail.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SendMail.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SendMail.dll"]