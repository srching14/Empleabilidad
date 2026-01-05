# Plataforma de Cursos Online - Evaluación Técnica

Backend de API REST profesional desarrollado con .NET 8 y un frontend moderno construido con Vue.js 3 para la gestión integral de cursos y lecciones en línea.

## Características Principales

- Implementación de Arquitectura Limpia (capas de Dominio, Aplicación, Infraestructura y API)
- Integración de Entity Framework Core con PostgreSQL
- Autenticación JWT y ASP.NET Identity
- Funcionalidad de Eliminación Lógica (Soft Delete) con filtros de consulta globales
- Validación de reglas de negocio para la publicación de cursos y el orden de las lecciones
- Panel de control interactivo en Vue.js 3 con Tailwind CSS v4
- Contenerización completa utilizando Docker y Docker Compose

## Estructura del Proyecto

```text
OnlineCoursePlatform/
├── backend/
│   ├── OnlineCoursePlatform.Domain/         # Entidades, Interfaces, Enums
│   ├── OnlineCoursePlatform.Application/    # Lógica de Negocio, Servicios, DTOs
│   ├── OnlineCoursePlatform.Infrastructure/ # Contexto de EF Core, Identity, Migraciones
│   └── OnlineCoursePlatform.API/            # Controladores, Configuración JWT, Program.cs
├── tests/
│   └── OnlineCoursePlatform.Tests/          # Pruebas unitarias xUnit
├── frontend/                                 # Aplicación Vue.js 3 + Vite
└── docker-compose.yml                       # Orquestación para DB, API y Frontend
```

## Inicio Rápido con Docker

Toda la plataforma está contenerizada para una ejecución sin interrupciones. Asegúrese de tener instalados Docker y Docker Compose.

1. Clone o extraiga el repositorio.
2. Navegue a la raíz del proyecto.
3. Ejecute el siguiente comando:

```bash
docker compose up --build -d
```

### Puntos de Acceso

- Panel de Control (Frontend): http://localhost:8080
- API REST (Backend): http://localhost:5000
- Documentación de la API (Swagger): http://localhost:5000/swagger
- Base de Datos (PostgreSQL): localhost:5433

### Credenciales

El sistema preconfigura un usuario administrativo predeterminado para pruebas:

- Correo electrónico: admin@courseplatform.com
- Contraseña: Admin123!

## Ejecución Manual (Modo Desarrollo)

Si prefiere ejecutar los componentes de forma independiente:

### Configuración de la Base de Datos
1. Asegúrese de que PostgreSQL esté en ejecución.
2. Actualice la cadena de conexión en `backend/OnlineCoursePlatform.API/appsettings.json`.
3. Aplique las migraciones: `dotnet ef database update`.

### Backend API
```bash
cd backend/OnlineCoursePlatform.API
dotnet run
```

### Frontend
```bash
cd frontend 
npm install
npm run dev
```

## Reglas de Negocio

1. Publicación: Un curso solo puede publicarse si contiene al menos una lección activa.
2. Orden de Lecciones: Cada lección debe tener un orden único dentro de su curso.
3. Reordenamiento Automático: Eliminar o reordenar una lección activa un intercambio o actualización de posiciones para mantener la integridad de la secuencia.
4. Persistencia de Datos: Se aplica la eliminación lógica a cursos y lecciones para evitar la pérdida permanente de datos.

## Pruebas Unitarias

El proyecto incluye pruebas xUnit que cubren la lógica de negocio crítica:

1. Validación de publicación de cursos.
2. Integridad de la eliminación lógica.
3. Unicidad del orden de las lecciones.
4. Lógica de reordenamiento e intercambio de lecciones.

Ejecute las pruebas utilizando:
```bash
dotnet test
```

## Tecnologías Utilizadas

- Backend: .NET 8, ASP.NET Core, EF Core, Npgsql
- Frontend: Vue 3, Vite, Tailwind CSS v4, Pinia, Axios
- Servidor: Nginx (para servir el frontend estático en Docker)
- Infraestructura: Docker, Docker Compose, PostgreSQL

---

Evaluación técnica desarrollada para el proceso de selección de la Plataforma de Cursos Online.
