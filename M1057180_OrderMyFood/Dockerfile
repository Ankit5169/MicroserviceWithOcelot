#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["M1057180_OrderMyFood/M1057180_OrderMyFood.csproj", "M1057180_OrderMyFood/"]
RUN dotnet restore "M1057180_OrderMyFood/M1057180_OrderMyFood.csproj"
COPY . .
WORKDIR "/src/M1057180_OrderMyFood"
RUN dotnet build "M1057180_OrderMyFood.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "M1057180_OrderMyFood.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "M1057180_OrderMyFood.dll"]