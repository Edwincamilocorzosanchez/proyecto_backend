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