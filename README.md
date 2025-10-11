# Proyecto de Aplicación Web ASP.NET Core


## Ejecucion del proyecto 

### 1. clone el repositorio
```
git clone https://github.com/servorx/backend_cs
cd backend_cs
code .
```

### 2. Ejecutar docker-compose
```
cd docker
docker compose up -d
```
Despues de este paso se debe de abrir pgadmin con el localhost:8080 y crear la base de datos backend_cs

### 3. Crear migraciones 
```
cd .. 
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api -o Data/Migrations
dotnet ef database update --project Infrastructure --startup-project Api   --connection "Host=localhost;Port=5433;Database=backend_cs;Username=postgres;Password=postgres"
```

### 3. Ejecutar el proyecto con swagger
```
dotnet  watch run --project Api --startup-project Api
```

## Configuracion del proyecto 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "PostgresDocker": "Host=db;Port=5432;Database=backend_cs;Username=postgres;Password=postgres",
    "PostgresLocal": "Host=localhost;Port=5433;Database=backend_cs;Username=postgres;Password=postgres"
  },
  "JWT": {
    "Key": "njMCY^UbEskeAFL6eDzHuqY!s^x6Qrwe",
    "Issuer": "MyStoreApi",
    "Audience": "MyStoreApiUser",
    "DurationInMinutes":  1
  }
}
```

el Key es la clave de encriptación de JWT, el Issuer es el nombre del emisor del JWT y el Audience es el nombre del receptor del JWT.

El Issuer es quien emite el token, en este caso la ejecucion del backend 
El Audience es quien recibe el token, en este caso el frontend 
La duración del token es de 1 minuto, esto es para que el token no expire y se vuelva a emitir.