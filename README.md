# Veterinary Management System

Sistema fullstack para la gestión de una clínica veterinaria, desarrollado con **.NET (Backend API)** y **React (Frontend)**, enfocado en prácticas profesionales como autenticación JWT, auditoría, control por roles y visualización de datos.

---

## Tecnologías utilizadas

### Backend
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- Middleware global de errores
- Rate Limiting
- Auditoría (Audit Logs)

### Frontend
- React + Vite
- React Router
- Recharts (gráficos)
- Axios (apiClient)
- CSS modular

---

## Funcionalidades principales

### Autenticación
- Login con JWT
- Protección de rutas (PrivateRoute)
- Control de sesión en frontend

### Gestión de Clientes
- Crear, editar, eliminar
- Estado activo/inactivo

### Gestión de Mascotas
- Asociadas a clientes
- CRUD completo

### Tipos de Servicio
- Crear y administrar servicios veterinarios

### Servicios Asignados
- Asociar servicios a mascotas
- Control de:
  - Precio final
  - Duración
  - Estado de facturación:
    - Pendiente
    - Facturado
    - Pagado
    - Cancelado

### Dashboard (BI básico)
- Métricas:
  - Clientes
  - Mascotas
  - Servicios
  - Ingresos
- Filtros por fecha
- Gráficos:
  - Servicios por estado
  - Ingresos
  - Distribución del sistema
  - Ingresos por mes (line chart)

### Reportes
- Exportación a Excel:
  - Servicios asignados
  - Auditoría

### Auditoría
- Registro de:
  - Login
  - Creación de entidades
  - Cambios de facturación
- Visualización en UI

### Control por Roles (Frontend)
- Admin
- Veterinarian
- Assistant

---

## Arquitectura

```
Backend
│
├── Domain (Entidades)
├── Application (DTOs, Interfaces)
├── Infrastructure (EF Core, DbContext)
└── API (Controllers, Middleware)

Frontend
│
├── pages
├── services
├── components
├── auth
└── layouts
```

---

## Instalación

### Backend

```bash
cd VeterinarySystem.Api
dotnet restore
dotnet build
dotnet run
```

Configurar:

```json
appsettings.json
```

```json
"ConnectionStrings": {
  "DefaultConnection": "TU_CADENA_SQL"
}
```

---

### Frontend

```bash
cd frontend/veterinary-system-web
npm install
npm run dev
```

---

## Endpoints principales

```http
POST   /api/Auth/login
GET    /api/Clients
POST   /api/Clients
GET    /api/Pets
POST   /api/PetServices
GET    /api/Dashboard/summary
GET    /api/Dashboard/monthly-revenue
GET    /api/AuditLogs
```

---

## Capturas (recomendado agregar)

Agregar screenshots aquí:
- Dashboard
- Auditoría
- Servicios
- Sidebar

---

## Estado del proyecto

✔ Backend completo
✔ Frontend funcional
✔ Dashboard con gráficos
✔ Auditoría
✔ Exportación Excel
✔ Control por roles

Pendiente:
- Deploy
- Notificaciones (toast)
- UI refinements

---

## Próximos pasos

- Deploy (Azure / Railway / Vercel)
- Mejoras UX/UI
- Reportes avanzados
- Gráficos tipo Power BI

---

## Autor

Caleb Arce

---

## Valor del proyecto

Este sistema demuestra:

- Arquitectura limpia
- Buenas prácticas backend (.NET)
- Integración frontend–backend
- Seguridad (JWT + roles)
- Visualización de datos (BI básico)
