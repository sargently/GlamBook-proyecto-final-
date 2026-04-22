# GlamBook - Arquitectura distribuida mínima

Este proyecto ahora está dividido en dos aplicaciones:

- **GlamBook.Api (servidor)**: API REST ASP.NET Core que expone operaciones de `Clienta`.
- **GlamBook.UI (cliente consola)**: aplicación de consola que consume la API por HTTP con `HttpClient`.

La base de datos sigue en SQL Server y se accede desde la API usando la DAL existente (`ClientaDAL`) y stored procedures.

## Endpoints implementados

- `GET /api/clientas` → lista clientas.
- `POST /api/clientas` → crea clienta.
- `DELETE /api/clientas/{id}` → elimina clienta.

## Configuración de cadena de conexión

En `GlamBook.Api/appsettings.json` configure:

```json
"ConnectionStrings": {
  "GlamBookDB": "Data Source=...;Initial Catalog=GlamBookDB;Integrated Security=True;TrustServerCertificate=True;"
}
```

La API **no** hardcodea la conexión; la toma desde `ConnectionStrings:GlamBookDB`.

## Ejecución

1. Configure la cadena de conexión en `GlamBook.Api/appsettings.json`.
2. Verifique que en `GlamBookDB` existan los stored procedures usados por la DAL:
   - `sp_ObtenerClientas`
   - `sp_InsertarClienta`
   - `sp_EliminarClienta`
3. Ejecute la API:

```bash
dotnet run --project GlamBook.Api/GlamBook.Api.csproj
```

4. En otra terminal, ejecute la consola cliente (usa por defecto `http://localhost:5137`):

```bash
dotnet run --project GlamBook.UI/GlamBook.UI.csproj
```

También puede indicar otra URL de API:

```bash
dotnet run --project GlamBook.UI/GlamBook.UI.csproj -- http://localhost:5137
```
