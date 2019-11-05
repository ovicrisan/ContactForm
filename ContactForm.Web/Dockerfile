FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ContactForm.Web.csproj", "."]
RUN dotnet restore "ContactForm.Web.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ContactForm.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactForm.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ContactSettings__EmailSettings__Enabled=false
ENV ContactSettings__PostSettings__Enabled=false
ENV ContactSettings__RecaptchaSettings__Enabled=false
ENTRYPOINT ["dotnet", "ContactForm.Web.dll"]
