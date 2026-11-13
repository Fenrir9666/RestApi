# RestApi · Restaurante con mesero virtual (ITQ)

## Stack
- C# 14, .NET 10, ASP.NET Core Minimal API
- SQL Server con ADO.NET (Microsoft.Data.SqlClient)
- Pruebas manuales con archivos .http

## Arquitectura (no romper)
- Entidades/ → clases simples, sin lógica
- Datos/     → repositorios, único lugar con SQL
- Negocio/   → servicios: validaciones y reglas
- Web/       → endpoints con MapGroup, sin SQL
- Los servicios dependen de interfaces (IPlatoRepositorio)

## Convenciones
- Métodos async terminan en Async
- Borrado lógico: Disponible = 0, nunca DELETE en Platos
- HTTP: 201 crear, 204 eliminar, 400 inválido,
        404 no existe, 409 conflicto
- Cadena de conexión desde .env (ConnectionStrings__Restaurante)

## Prohibido
- Concatenar valores en SQL
- Subir .env, bin/ u obj/ a GitHub