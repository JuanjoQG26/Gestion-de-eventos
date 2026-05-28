GestionEventos
Plataforma de Gestión de Eventos Académicos

Descripción General
Sistema web para la gestión integral de eventos académicos que automatiza inscripciones, pagos en línea, control de asistencia, distribución de materiales digitales y generación de certificados. Cuenta con dos roles: Organizador y Asistente.

Integrantes

Jefferson del Rio Aristizabal
Ferney Esteban Henao
Juan José Quiroz González

Tecnologías Utilizadas
Capa  Tecnología
Frontend  Blazor WebAssembly (.NET 10)
Backend  ASP.NET Core Web API (.NET 10)
Base de Datos  SQL Server LocalDB
ORM  Entity Framework Core
Pagos  PayPal Sandbox API v2
IDE  Visual Studio 2022/2026

Funcionalidades Implementadas

Registro e inicio de sesión con validación de roles
Dashboard personalizado por rol
CRUD de eventos, ponentes y actividades (Organizador)
Exploración e inscripción a eventos (Asistente)
Pago en línea con PayPal Sandbox
Control de asistencia
Gestión de materiales digitales
Generación y descarga de certificados
Reportes de asistencia, pagos e inscripciones


Requisitos de Instalación

.NET SDK 10 o superior
Visual Studio 2022 o superior
SQL Server LocalDB
Conexión a internet (para PayPal Sandbox)

Pasos para Ejecutar el Proyecto
1. Clonar el repositorio
   git clone https://github.com/JuanjoQG26/GestionEventos.git
cd GestionEventos

2. Crear la base de datos
Abrir SQL Server Management Studio o Azure Data Studio y ejecutar el script BD/script_gestion.sql para crear la base de datos Gestion con todas sus tablas.

3. Configurar el Backend
Abrir GestionEventosBack/appsettings.json y verificar:
{
  "ConnectionStrings": {
    "LocalConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Gestion;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "PayPal": {
    "ClientId": "TU_CLIENT_ID",
    "ClientSecret": "TU_CLIENT_SECRET",
    "Mode": "sandbox"
  }
}

4. Ejecutar el Backend
En Visual Studio establecer GestionEventosBack como proyecto de inicio y ejecutar con F5. La API quedará disponible en:
http://localhost:5259
http://localhost:5259/swagger (documentación)

5. Ejecutar el Frontend
En Visual Studio establecer GestionEventosFront como proyecto de inicio y ejecutar con F5. La app quedará disponible en:
https://localhost:7009

6. Crear usuarios de prueba
Usar Swagger en http://localhost:5259/swagger y hacer POST api/usuarios/registro con:
{
  "nombre": "Luis Organizador",
  "email": "luis@gmail.com",
  "contrasena": "123456",
  "rol": "Organizador"
}
{
  "nombre": "Juan Asistente",
  "email": "juan@gmail.com",
  "contrasena": "123456",
  "rol": "Asistente"
}

Credenciales PayPal Sandbox
Para probar los pagos usar esta tarjeta de prueba en la página de PayPal:

Número: 4032036264953859
Vencimiento: 01/2030
CSC: 722


Enlace al Despliegue
Actualmente el proyecto corre en entorno local. No hay despliegue en producción para esta versión del MVP.

Buenas Prácticas de Versionamiento
Ramas principales:

main — versión estable del proyecto

GestionEventos/
├── GestionEventosBack/        ← API REST (ASP.NET Core)
│   ├── Controllers/           ← Endpoints de la API
│   ├── Data/                  ← DataContext (Entity Framework)
│   └── Pagos/                 ← Servicio de PayPal
├── GestionEventosFront/       ← Frontend (Blazor WebAssembly)
│   ├── Pages/                 ← Páginas de la aplicación
│   ├── Layout/                ← Menús laterales reutilizables
│   └── Service/               ← IRepository y Repository
├── GestionEventosModelo/      ← Entidades del dominio
│   └── Entidades/             ← Clases C# que mapean las tablas
└── BD/
    └── script_gestion.sql     ← Script de creación de la BD
