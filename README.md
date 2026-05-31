# Sistema de pedidos

Proyecto de Tecnologias Web: plataforma de pedidos para un negocio con cliente, administrador y sucursales.

## Tecnologias

- Frontend: Angular con componentes, rutas, formularios y servicios.
- Backend: ASP.NET Core MVC API en la carpeta `API`.
- Base de datos: MySQL.
- Validaciones: FluentValidation.
- Mapeo: AutoMapper con profiles.
- Persistencia: Entity Framework Core y repositorio generico.
- Autenticacion: login local con JWT y Google OAuth para clientes.
- Gestor de paquetes frontend: pnpm.

## Funcionalidades

- Login por tipo de cuenta: cliente o negocio.
- Registro manual de clientes.
- Inicio de sesion con Google para clientes.
- Panel de administrador.
- CRUD de productos con imagen y precio.
- Eliminacion logica de productos usados en pedidos.
- Gestion de sucursales.
- Creacion de pedidos para recoger en tienda.
- Cambio de estado de pedidos.
- Dashboard con productos activos, pedidos, pendientes y ventas.
- Persistencia en MySQL.

## Estructura

- `client/`: aplicacion Angular.
- `API/`: API principal en ASP.NET Core.
- `img/`: imagenes del proyecto original.
- Archivos HTML/JS originales: version base reciclada del proyecto.

## Cuenta inicial

Administrador:

```txt
Correo: admin@sistemaventas.com
Contrasena: 123456
```

## Base de datos

La API usa MySQL con esta cadena en `API/appsettings.json`:

```json
"Default": "Server=localhost;Port=3306;Database=proyecto_pery;User=root;Password=root;"
```

La base se crea al ejecutar la API. Los productos se conservan en la base y los pedidos, usuarios y sucursales se crean desde la aplicacion.

## Ejecucion

API:

```bash
dotnet run --project API/API.csproj --urls http://localhost:5032
```

Frontend:

```bash
cd client
corepack pnpm install
corepack pnpm start
```

Abrir:

```txt
http://localhost:4200
```

## Pruebas

Comandos usados para verificar:

```bash
dotnet build API/API.csproj
corepack pnpm build
```

## Rubrica cubierta

- Desarrollo tecnico: Angular, asincronia con fetch, API propia y Google OAuth.
- Funcionalidad: login, CRUD, dashboard, pedidos y roles.
- Seguridad: validaciones, hash de contrasenas, JWT y limpieza de entradas.
- Arquitectura: separacion entre frontend y API, servicios, DTOs, entidades, validators, profiles y repositorio generico.
- Presentacion: flujo de cliente, administrador, productos, pedidos y dashboard.
