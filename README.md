# 🏢 Sistema de Gestión de Empleados

Un sistema completo de gestión de empleados desarrollado con tecnologías modernas de Microsoft, que ofrece una solución robusta y escalable para la administración de recursos humanos.

## 🚀 Demo en Vivo

- **🌐 Aplicación Frontend**: (Ver Demo)(https://employees-g5cthjhuemhjfdc0.canadacentral-01.azurewebsites.net/){:target="_blank" rel="noopener"}
- **🔧 API Backend**: [Endpoint](https://employees-api-ajc5czd6f2h0a5ee.canadacentral-01.azurewebsites.net){:target="_blank" rel="noopener"}
- **📚 Documentación API**: [Swagger UI](https://employees-api-ajc5czd6f2h0a5ee.canadacentral-01.azurewebsites.net/swagger/index.html){:target="_blank" rel="noopener"}

## 🛠️ Stack Tecnológico

### Frontend
- **Blazor WebAssembly** con .NET 8
- **Bootstrap** para diseño responsive
- **Componentes interactivos** del lado del cliente

### Backend
- **ASP.NET Core Web API** con .NET 8
- **Entity Framework Core** para acceso a datos
- **MySQL** como base de datos (Cualquier Servicio de terceros)
- **JWT Authentication** con refresh tokens

### Infraestructura
- **Azure App Services** para despliegue

## ✨ Características Principales

### 🔐 Autenticación y Autorización
- Sistema de roles: **Administrador**, **Gestor** y **Usuario**
- Autenticación JWT con tokens de actualización
- Protección de rutas según nivel de acceso

### 👥 Gestión de Empleados
- **CRUD completo** de empleados
- Búsqueda y filtrado avanzado
- Gestión de información personal y laboral
- Historial de cambios

### 📊 Reportes e Informes
- Generación de reportes personalizados
- Exportación a múltiples formatos
- **Funcionalidad de impresión** integrada
- Dashboards con métricas clave

### 👤 Administración de Usuarios
- Gestión de usuarios y gestores (solo Administrador)
- Asignación de roles y permisos
- Control de acceso granular

## 🏗️ Arquitectura del Proyecto

```
EmployeeManagementSystem/
|
│── EmployeeManagementSystem.ClientLibrary/     # Biblioteca de clases que incluye los Helpers y Services del frontend
|── EmployeeManagementSystem.Client/            # Blazor WebAssembly (Frontend principal)
│── EmployeeManagementSystem.Server/            # ASP.NET Core Web API (Backend principal)
│── EmployeeManagementSystem.ServerLibrary/     # Biblioteca de clases que incluye el contexto de datos, clases de extensión, helpers, repositorios del backend
│── EmployeeManagementSystem.BaseLibrary/       # Biblioteca de clases que incluye los DTOs, entidades y modelos compartidos
```

## ⚙️ Configuración

### Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0){:target="_blank"}
- [MySQL Server](https://dev.mysql.com/downloads/mysql/){:target="_blank"}
- [Visual Studio 2022](https://visualstudio.microsoft.com/){:target="_blank"} o [VS Code](https://code.visualstudio.com/){:target="_blank"}

### Instalación

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/jchablepat/employee-management-system.git
   cd employee-management-system
   ```

2. **Configurar la base de datos**
   
   Actualiza la cadena de conexión en `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=EmployeeManagementDB;Uid=root;Pwd=tu-password;"
     }
   }
   ```

3. **Ejecutar migraciones**
   ```bash
   dotnet ef database update --project EmployeeManagementSystem.ServerLibrary
   ```

4. **Ejecutar el proyecto**
   ```bash
   # Terminal 1 - API Backend
   cd src/EmployeeManagementSystem.Server
   dotnet run
   
   # Terminal 2 - Frontend
   cd src/EmployeeManagementSystem.Client
   dotnet run
   ```

## 🔧 Variables de Entorno

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "tu-cadena-de-conexion-mysql"
  },
  "JwtSettings": {
    "Key": "tu-clave-secreta-jwt",
    "Issuer": "EmployeeManagement",
    "Audience": "EmployeeManagement"
  }
}
```

## 🚀 Despliegue

### Azure App Services

El proyecto está configurado para desplegarse en Azure App Services:

1. **Backend**: Configurado como Web API
2. **Frontend**: Configurado como Static Web App
