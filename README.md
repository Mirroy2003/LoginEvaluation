# LoginEvaluation

Aplicación ASP.NET Core MVC que simula un flujo de autenticación con validación de usuarios, bloqueo temporal y manejo básico de perfiles.

## Tecnologías usadas
- ASP.NET Core MVC (net10.0), Razor Views, autenticación por cookies
- Bootstrap y jQuery para la interfaz
- SQL Server como base de datos

## Funcionalidades principales
- Inicio de sesión con DNI y contraseña
- Validación de usuario activo/inactivo
- Bloqueo temporal tras 5 intentos fallidos con correo de aviso
- Pantalla de bienvenida y perfil de usuario
- Expiración de sesión por inactividad con opción para extender

## Cómo ejecutar el proyecto en local
```powershell
cd "C:\Users\user\RiderProjects\LoginEvaluation"
dotnet restore
dotnet build -v minimal
dotnet run --project LoginEvaluation
```
> Requiere SQL Server accesible según `appsettings.Development.json`. Ejecuta los scripts en `Database/` para crear y poblar la tabla `Users` si aún no existe.

## Credenciales de prueba
- Usuario activo: DNI `87654321`, contraseña según valor configurado en la semilla de base de datos (`Database/init_users.sql`).
- Usuario inactivo: DNI `12345678`, contraseña configurada en la misma semilla.
> Ajusta las contraseñas directamente en la base de datos si necesitas cambiarlas.

## Flujo general del sistema
1. Al abrir la raíz, se muestra la pantalla de bienvenida.
2. El botón “Iniciar sesión” lleva a `/Account/Login` para ingresar DNI y contraseña.
3. Si el usuario está inactivo, se rechaza el acceso; si falla 5 veces, se bloquea 15 minutos y se notifica por correo.
4. Tras autenticarse, se redirige al perfil de usuario.
5. La sesión expira por inactividad; aparece un modal con un botón para extenderla.

## Nota breve sobre el correo de notificación
Cuando una cuenta se bloquea, se envía un correo usando la configuración SMTP definida en `appsettings.Development.json` (servicio `SmtpNotificationService`).

## Desarrollador
Luis Eduardo Lagos Aguilar

