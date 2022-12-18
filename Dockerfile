FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
EXPOSE 8080

# copy .csproj and restore as distinct layers
COPY "DevReviews.sln" "DevReviews.sln"
COPY "DevReviews.API/DevReviews.API.csproj" "DevReviews.API/DevReviews.API.csproj"

RUN dotnet restore "DevReviews.sln"

# copy everything else build

COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

# build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "DevReviews.API.dll" ]