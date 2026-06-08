# ProyectoTicketing

Primera entrega

# Sistema de Ticketing - Primera Entrega

Este proyecto es un sistema de gestion de eventos y reserva de asientos, desarrollado con una arquitectura de capas en **.NET** para el Backend y **React** para el Frontend.

---

## Como ejecutar el proyecto

### 1. Requisitos previos
* **PostgreSQL** instalado y corriendo (Puerto 5433).
* **.NET 8 SDK**.
* **Node.js** y **npm**.
* Configurar la cadena de conexión en: API/appsettings.json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=Ticket_db;Username=postgres;Password=1234"
        }
### 2. Configuración del Backend (.NET)
Desde la terminal en la carpeta raiz del proyecto:

1. **Restaurar dependencias:**
   ```bash
   dotnet restore

### MIGRACIONES
   dotnet ef database update --project Infrastructure --startup-project API

### Ejecutar el servidor
   dotnet run --project API

### Instalar dependencias
   npm install

### Ejecutar en modo desarrollo
   cd Frontend
   npm run dev

### Uso del Sistema
   Registrarse con un email y contraseña,
   Iniciar sesión
   ,Seleccionar un sector (Platea Baja / Alta)
   ,Elegir asientos disponibles
   ,Confirmar la reserva

Tecnologías utilizadas

    Backend: .NET 8 (C#) con Entity Framework Core.

    Frontend: React + Vite (JavaScript).

    Base de Datos: PostgreSQL.

    Documentacion: Swagger / OpenAPI.

Funcionalidades - Entrega 1

   Persistencia de datos con Entity Framework.

   Data Seeding: Carga automatica de 100 asientos y un evento inicial.

   Manejo de Errores: Respuestas 404 controladas para recursos no encontrados.

   Frontend Interactivo: Visualizacion de sectores y persistencia de seleccion local.

   CORS: Configurado para comunicacion entre el puerto 5173 y 5171.

   Segunda Entrega: Sistema de Ticketing Dinámico 
   Funcionalidades Implementadas

   En esta etapa se integró la lógica completa de gestión de eventos y la persistencia de datos:
   
   Generador Automático de Eventos: Implementación de un GenerateEventHandler que permite crear de forma masiva sectores, filas y asientos con identificadores únicos.
   
   Gestión Dinámica de Espacios: Capacidad de definir el nombre del evento y el Venue (Lugar/Estadio) desde el frontend.Numeración de Asientos por Sector: Lógica corregida para que cada sector (A, B, etc.) reinicie su numeración del 1 al $N$.
   
   Panel de Administración: Interfaz para la creación rápida de eventos con validaciones de campos obligatorios.
  
   Sección de "Mis Compras": Nueva vista dinámica en la cartelera que permite al usuario consultar sus tickets reservados con scroll automático y carga desde la base de datos.
   
   Tecnologías y ArquitecturaSe profundizó en la arquitectura limpia (Clean Architecture) dividida en capas:
   
   Domain: Entidades (Event, Seat, Sector, User, Reservation).
   
   Application: Casos de uso y Commands para la generación de eventos.
   
   Infrastructure: Persistencia con Entity Framework Core y PostgreSQL.
   
   API: Controladores REST con mapeo de objetos para evitar ciclos de referencia.
   
   Base de Datos y Seed
   El sistema cuenta con un Data Seeder automático en el AppDbContext que:
   
   Crea un Usuario Administrador por defecto (admin@tickets.com).
   
   Genera un Evento de Prueba inicial ("Concierto de Rock") con 100 asientos distribuidos en dos sectores (A y B).
   
   Implementa restricciones de unicidad para evitar duplicidad de asientos en un mismo sector.
   
   Instrucciones de EjecuciónBackend: * Configurar el ConnectionString en appsettings.json.Ejecutar dotnet ef database update para aplicar las migraciones.dotnet run para iniciar la API.Frontend: * npm installnpm run dev

   ### Credenciales de Acceso (Importante)

   Para evaluar las funcionalidades de administración y creación de eventos, se ha precargado un usuario con privilegios de Admin:

   ### Usuario: admin@test.com

   ### Contraseña: 1234

   Nota: Al ingresar con estas credenciales, se habilitará el botón "Panel Admin" en la barra de navegación, desde donde se pueden generar nuevos eventos masivos con sectores y asientos dinámicos.
   
   ### Luego crear un usuario comun para realizar la compra de entradas