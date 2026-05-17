# 🐾 PLAN DE TRABAJO COMPLETO - FINDING PAWLS WEB API
## Aplicación Móvil para Mascotas Extraviadas y en Adopción - Guatemala

**Proyecto de Tesis**  
**Tecnologías:** .NET Core 8 Web API + .NET MAUI + MySQL  
**Autor:** [Tu Nombre]  
**Fecha de inicio:** [Fecha]  
**Última actualización:** 2024-12-XX

---

## 📊 ESTADO ACTUAL DEL PROYECTO

### ✅ COMPLETADO HASTA AHORA

- [x] Diseño de base de datos MySQL (11 tablas)
- [x] Modelos de Entity Framework Core generados
- [x] DbContext configurado (`FindingPawsContext.cs`)
- [x] Tabla `refreshtoken` creada y migrada exitosamente
- [x] Todos los paquetes NuGet instalados (JWT, BCrypt, FluentValidation, Serilog, Azure CV, etc.)
- [x] User Secrets configurado con clave JWT segura
- [x] Estructura de carpetas del proyecto creada
- [x] DTOs de autenticación creados:
  - [x] `LoginRequestDto.cs`
  - [x] `RegisterRequestDto.cs`
  - [x] `AuthResponseDto.cs`
  - [x] `RefreshTokenRequestDto.cs`
- [x] DTOs comunes creados:
  - [x] `ApiResponse.cs`
  - [x] `ErrorResponse.cs`
- [x] Helpers creados:
  - [x] `JwtSettings.cs`
- [x] Interfaces de servicios creadas:
  - [x] `IPasswordHasher.cs`
  - [x] `IJwtService.cs`
  - [x] `IAuthService.cs`
- [x] Implementaciones de servicios:
  - [x] `PasswordHasher.cs` (BCrypt)
  - [ ] `JwtService.cs` (EN PROGRESO)

---

## 🎯 PLAN DE TRABAJO RESTANTE

---

# FASE 1: FUNDAMENTOS Y SEGURIDAD (CONTINUACIÓN)

**Prioridad:** 🔴 ALTA  
**Tiempo estimado:** 1-2 semanas  
**Estado:** 🔄 EN PROGRESO (80% completado)

---

## 1.1 Servicios de Autenticación (JWT)

### ✅ Archivos Completados
- [x] `IPasswordHasher.cs` - Interface para hashing
- [x] `PasswordHasher.cs` - Implementación BCrypt
- [x] `IJwtService.cs` - Interface para JWT
- [x] `IAuthService.cs` - Interface de autenticación

### 📝 Archivos Pendientes

#### A. Implementaciones de Servicios
- [ ] `JwtService.cs` - **SIGUIENTE TAREA**
  - [ ] Método `GenerateAccessToken(Usuario)`
  - [ ] Método `GenerateRefreshToken()`
  - [ ] Método `GetPrincipalFromExpiredToken(string)`
  - [ ] Método `ValidateToken(string)`
  
- [ ] `AuthService.cs` - Servicio principal de autenticación
  - [ ] Método `RegisterAsync()` - Registro de usuarios
  - [ ] Método `LoginAsync()` - Login con email/contraseña
  - [ ] Método `RefreshTokenAsync()` - Renovar access token
  - [ ] Método `LogoutAsync()` - Cerrar sesión
  - [ ] Método `RevokeAllUserTokensAsync()` - Revocar todos los tokens
  - [ ] Método `GetActiveRefreshTokensAsync()` - Listar tokens activos

---

## 1.2 Validadores (FluentValidation)

- [ ] `LoginRequestValidator.cs`
  - [ ] Validar formato de email
  - [ ] Validar que contraseña no esté vacía
  
- [ ] `RegisterRequestValidator.cs`
  - [ ] Validar nombre (2-100 caracteres)
  - [ ] Validar email único y formato válido
  - [ ] Validar contraseña fuerte (min 8 chars, mayúscula, minúscula, número, especial)
  - [ ] Validar confirmación de contraseña
  - [ ] Validar teléfono (opcional, formato válido)
  - [ ] Validar dirección (opcional, max 255 chars)

---

## 1.3 Middleware de Seguridad

- [ ] `ExceptionHandlingMiddleware.cs`
  - [ ] Capturar todas las excepciones no manejadas
  - [ ] Retornar respuestas JSON estandarizadas
  - [ ] Logging de errores con Serilog
  - [ ] No exponer detalles internos en producción
  
- [ ] `RequestLoggingMiddleware.cs` (OPCIONAL)
  - [ ] Loggear cada request (método, URL, IP)
  - [ ] Loggear tiempo de respuesta
  - [ ] Excluir endpoints sensibles del logging completo

---

## 1.4 Extensiones y Helpers

- [ ] `ServiceCollectionExtensions.cs`
  - [ ] Método `AddJwtAuthentication()` - Configurar JWT
  - [ ] Método `AddApplicationServices()` - Registrar servicios
  - [ ] Método `AddFluentValidation()` - Registrar validadores
  - [ ] Método `AddCustomCors()` - Configurar CORS
  - [ ] Método `AddRateLimiting()` - Configurar rate limiting

- [ ] `SecurityHelper.cs`
  - [ ] Método para obtener IP del usuario
  - [ ] Método para obtener User Agent
  - [ ] Método para validar origen de request

---

## 1.5 Configuración de Program.cs

- [ ] Configurar Serilog (logging estructurado)
- [ ] Registrar servicios con Dependency Injection
- [ ] Configurar JWT Authentication
- [ ] Configurar CORS
- [ ] Configurar Swagger con autenticación JWT
- [ ] Configurar Rate Limiting
- [ ] Registrar Middleware en orden correcto
- [ ] Configurar HTTPS obligatorio
- [ ] Configurar manejo de errores global

---

## 1.6 Controller de Autenticación

- [ ] `AuthController.cs`
  - [ ] `POST /api/auth/register` - Registro de usuarios
  - [ ] `POST /api/auth/login` - Login tradicional
  - [ ] `POST /api/auth/refresh-token` - Refrescar token
  - [ ] `POST /api/auth/logout` - Cerrar sesión
  - [ ] `POST /api/auth/revoke-all-tokens` - Revocar todos los tokens del usuario
  - [ ] `GET /api/auth/active-sessions` - Ver sesiones activas
  - [ ] Atributos de autorización correctos
  - [ ] Documentación Swagger completa
  - [ ] Manejo de errores adecuado

---

## 1.7 Testing y Validación de Seguridad

- [ ] Probar registro de usuario en Swagger
- [ ] Probar login con credenciales correctas
- [ ] Probar login con credenciales incorrectas
- [ ] Probar refresh token
- [ ] Probar logout
- [ ] Verificar que tokens expirados no funcionen
- [ ] Verificar que tokens revocados no funcionen
- [ ] Probar rate limiting
- [ ] Verificar CORS
- [ ] Revisar logs de Serilog

---

# FASE 2: MÓDULOS CORE

**Prioridad:** 🔴 ALTA  
**Tiempo estimado:** 2-3 semanas  
**Estado:** ⏳ PENDIENTE

---

## 2.1 Módulo de Usuario

### DTOs
- [ ] `UsuarioResponseDto.cs` - Datos públicos del usuario
- [ ] `UpdateUsuarioDto.cs` - Actualizar perfil
- [ ] `ChangePasswordDto.cs` - Cambiar contraseña
- [ ] `UsuarioEstadisticasDto.cs` - Estadísticas del usuario

### Servicios
- [ ] `IUsuarioService.cs` - Interface
- [ ] `UsuarioService.cs` - Implementación
  - [ ] `GetPerfilAsync(Guid usuarioId)` - Obtener perfil
  - [ ] `UpdatePerfilAsync(Guid usuarioId, UpdateUsuarioDto)` - Actualizar
  - [ ] `ChangePasswordAsync(Guid usuarioId, ChangePasswordDto)` - Cambiar contraseña
  - [ ] `GetEstadisticasAsync(Guid usuarioId)` - Estadísticas
  - [ ] `DeleteCuentaAsync(Guid usuarioId)` - Eliminar cuenta

### Controller
- [ ] `UsuariosController.cs`
  - [ ] `GET /api/usuarios/perfil` - Perfil del usuario autenticado
  - [ ] `PUT /api/usuarios/perfil` - Actualizar perfil
  - [ ] `PUT /api/usuarios/cambiar-contrasena` - Cambiar contraseña
  - [ ] `GET /api/usuarios/estadisticas` - Estadísticas
  - [ ] `DELETE /api/usuarios/cuenta` - Eliminar cuenta

### Validadores
- [ ] `UpdateUsuarioValidator.cs`
- [ ] `ChangePasswordValidator.cs`

---

## 2.2 Módulo de Mascotas

### DTOs
- [ ] `MascotaDto.cs` - Datos completos de mascota
- [ ] `CreateMascotaDto.cs` - Crear mascota
- [ ] `UpdateMascotaDto.cs` - Actualizar mascota
- [ ] `MascotaListDto.cs` - Lista resumida

### Servicios
- [ ] `IMascotaService.cs` - Interface
- [ ] `MascotaService.cs` - Implementación
  - [ ] `GetMascotasUsuarioAsync(Guid usuarioId)` - Listar mascotas
  - [ ] `GetMascotaByIdAsync(Guid mascotaId)` - Detalle
  - [ ] `CreateMascotaAsync(CreateMascotaDto)` - Crear
  - [ ] `UpdateMascotaAsync(Guid mascotaId, UpdateMascotaDto)` - Actualizar
  - [ ] `DeleteMascotaAsync(Guid mascotaId)` - Eliminar

### Servicio de Archivos
- [ ] `IFileStorageService.cs` - Interface
- [ ] `LocalFileStorageService.cs` - Almacenamiento local
  - [ ] `SaveImageAsync(IFormFile file, string folder)` - Guardar imagen
  - [ ] `DeleteImageAsync(string fileName)` - Eliminar imagen
  - [ ] `ValidateImageAsync(IFormFile file)` - Validar tipo/tamaño
  - [ ] Configurar carpetas (wwwroot/uploads/mascotas/)

### Controller
- [ ] `MascotasController.cs`
  - [ ] `GET /api/mascotas` - Listar mascotas del usuario
  - [ ] `GET /api/mascotas/{id}` - Detalle de mascota
  - [ ] `POST /api/mascotas` - Crear mascota (con foto)
  - [ ] `PUT /api/mascotas/{id}` - Actualizar mascota
  - [ ] `DELETE /api/mascotas/{id}` - Eliminar mascota
  - [ ] `POST /api/mascotas/{id}/fotos` - Agregar fotos
  - [ ] `DELETE /api/mascotas/{id}/fotos/{fotoId}` - Eliminar foto

### Validadores
- [ ] `CreateMascotaValidator.cs`
- [ ] `UpdateMascotaValidator.cs`

---

## 2.3 Módulo de Catálogos

### DTOs
- [ ] `EspecieDto.cs`
- [ ] `RazaDto.cs`
- [ ] `SexoDto.cs`

### Servicios
- [ ] `ICatalogoService.cs` - Interface
- [ ] `CatalogoService.cs` - Implementación
  - [ ] `GetEspeciesAsync()` - Listar especies
  - [ ] `GetRazasByEspecieAsync(int especieId)` - Razas por especie
  - [ ] `GetSexosAsync()` - Listar sexos

### Controller
- [ ] `CatalogosController.cs`
  - [ ] `GET /api/catalogos/especies` - Listar especies
  - [ ] `GET /api/catalogos/razas` - Listar razas (filtrable)
  - [ ] `GET /api/catalogos/sexos` - Listar sexos

### Seed Data
- [ ] Crear migration para datos iniciales
  - [ ] Especies: Perro, Gato, Otro
  - [ ] Razas principales por especie (mínimo 20)
  - [ ] Sexos: Macho, Hembra, Desconocido

---

# FASE 3: MÓDULOS DE ADOPCIÓN

**Prioridad:** 🟠 MEDIA-ALTA  
**Tiempo estimado:** 2-3 semanas  
**Estado:** ⏳ PENDIENTE

---

## 3.1 Módulo de Adopciones

### DTOs
- [ ] `AdopcionDto.cs` - Datos completos
- [ ] `CreateAdopcionDto.cs` - Publicar en adopción
- [ ] `UpdateAdopcionDto.cs` - Actualizar publicación
- [ ] `AdopcionBusquedaDto.cs` - Filtros de búsqueda
- [ ] `AdopcionListDto.cs` - Lista resumida
- [ ] `SolicitudAdopcionDto.cs` - Solicitud de adopción

### Servicios
- [ ] `IAdopcionService.cs` - Interface
- [ ] `AdopcionService.cs` - Implementación
  - [ ] `BuscarAdopcionesAsync(AdopcionBusquedaDto)` - Buscar con filtros
  - [ ] `GetAdopcionByIdAsync(Guid adopcionId)` - Detalle
  - [ ] `PublicarAdopcionAsync(CreateAdopcionDto)` - Publicar
  - [ ] `UpdateAdopcionAsync(Guid, UpdateAdopcionDto)` - Actualizar
  - [ ] `SolicitarAdopcionAsync(Guid adopcionId)` - Solicitar
  - [ ] `AprobarSolicitudAsync(Guid adopcionId)` - Aprobar
  - [ ] `RechazarSolicitudAsync(Guid adopcionId, string razon)` - Rechazar
  - [ ] `CompletarAdopcionAsync(Guid adopcionId)` - Marcar como completada
  - [ ] `CancelarAdopcionAsync(Guid adopcionId)` - Cancelar

### Servicio de Geolocalización
- [ ] `IGeolocationService.cs` - Interface
- [ ] `GeolocationService.cs` - Implementación
  - [ ] `CalculateDistance(lat1, lon1, lat2, lon2)` - Distancia en km
  - [ ] `GetNearbyAdopciones(lat, lon, radio)` - Adopciones cercanas
  - [ ] Implementar fórmula de Haversine

### Controller
- [ ] `AdopcionesController.cs`
  - [ ] `GET /api/adopciones/buscar` - Buscar (con filtros de geolocalización)
  - [ ] `GET /api/adopciones/{id}` - Detalle
  - [ ] `POST /api/adopciones` - Publicar
  - [ ] `PUT /api/adopciones/{id}` - Actualizar
  - [ ] `DELETE /api/adopciones/{id}` - Cancelar
  - [ ] `POST /api/adopciones/{id}/solicitar` - Solicitar
  - [ ] `PUT /api/adopciones/{id}/aprobar` - Aprobar (solo propietario)
  - [ ] `PUT /api/adopciones/{id}/rechazar` - Rechazar
  - [ ] `PUT /api/adopciones/{id}/completar` - Completar

### Validadores
- [ ] `CreateAdopcionValidator.cs`
- [ ] `UpdateAdopcionValidator.cs`
- [ ] `AdopcionBusquedaValidator.cs`

---

## 3.2 Módulo de Reportes de Animales Callejeros

### Modelo (si no existe)
- [ ] Crear tabla `reportecallejero` si es necesaria
- [ ] Migration correspondiente

### DTOs
- [ ] `ReporteCallejeroDto.cs`
- [ ] `CreateReporteCallejeroDto.cs`
- [ ] `UpdateReporteCallejeroDto.cs`
- [ ] `ReporteCallejeroListDto.cs`

### Servicios
- [ ] `IReporteCallejeroService.cs` - Interface
- [ ] `ReporteCallejeroService.cs` - Implementación
  - [ ] `GetReportesCercanosAsync(lat, lon, radio)` - Cercanos
  - [ ] `GetReporteByIdAsync(Guid id)` - Detalle
  - [ ] `CreateReporteAsync(CreateReporteCallejeroDto)` - Crear
  - [ ] `UpdateReporteAsync(Guid, UpdateReporteCallejeroDto)` - Actualizar
  - [ ] `ResolverReporteAsync(Guid, string resolucion)` - Resolver

### Controller
- [ ] `ReportesCallejerosController.cs`
  - [ ] `GET /api/reportes-callejeros` - Listar cercanos
  - [ ] `GET /api/reportes-callejeros/{id}` - Detalle
  - [ ] `POST /api/reportes-callejeros` - Reportar
  - [ ] `PUT /api/reportes-callejeros/{id}` - Actualizar
  - [ ] `PUT /api/reportes-callejeros/{id}/resolver` - Marcar como resuelto

### Validadores
- [ ] `CreateReporteCallejeroValidator.cs`

---

# FASE 4: MÓDULOS DE EXTRAVÍO

**Prioridad:** 🟠 MEDIA-ALTA  
**Tiempo estimado:** 2-3 semanas  
**Estado:** ⏳ PENDIENTE

---

## 4.1 Módulo de Reportes de Extravío

### DTOs
- [ ] `ReporteExtravioDto.cs`
- [ ] `CreateReporteExtravioDto.cs`
- [ ] `UpdateReporteExtravioDto.cs`
- [ ] `ReporteExtravioListDto.cs`

### Servicios
- [ ] `IReporteExtravioService.cs` - Interface
- [ ] `ReporteExtravioService.cs` - Implementación
  - [ ] `GetReportesActivosAsync(lat, lon, radio)` - Cercanos
  - [ ] `GetReporteByIdAsync(Guid id)` - Detalle
  - [ ] `CreateReporteAsync(CreateReporteExtravioDto)` - Crear
  - [ ] `UpdateReporteAsync(Guid, UpdateReporteExtravioDto)` - Actualizar
  - [ ] `CerrarReporteAsync(Guid, string resolucion)` - Cerrar (encontrada)
  - [ ] `CancelarReporteAsync(Guid)` - Cancelar

### Controller
- [ ] `ExtraviosController.cs`
  - [ ] `GET /api/extravios` - Listar activos cercanos
  - [ ] `GET /api/extravios/{id}` - Detalle
  - [ ] `POST /api/extravios` - Reportar extravío
  - [ ] `PUT /api/extravios/{id}` - Actualizar
  - [ ] `PUT /api/extravios/{id}/cerrar` - Cerrar (encontrada)
  - [ ] `DELETE /api/extravios/{id}` - Cancelar

### Validadores
- [ ] `CreateReporteExtravioValidator.cs`
- [ ] `UpdateReporteExtravioValidator.cs`

---

## 4.2 Módulo de Avistamientos

### DTOs
- [ ] `AvistamientoDto.cs`
- [ ] `CreateAvistamientoDto.cs`
- [ ] `AvistamientoListDto.cs`
- [ ] `ComparacionFotosDto.cs` - Resultado de comparación IA

### Servicios de Comparación de Imágenes (Azure Computer Vision)
- [ ] `IImageComparisonService.cs` - Interface
- [ ] `AzureImageComparisonService.cs` - Implementación
  - [ ] Configurar Azure Computer Vision API
  - [ ] `CompareImagesAsync(imagen1, imagen2)` - Comparar similitud
  - [ ] `GetSimilarityScoreAsync(imagen1, imagen2)` - Porcentaje de similitud
  - [ ] Manejo de errores de API
  - [ ] Caching de resultados

### Servicios
- [ ] `IAvistamientoService.cs` - Interface
- [ ] `AvistamientoService.cs` - Implementación
  - [ ] `GetAvistamientosByReporteAsync(Guid reporteId)` - Por reporte
  - [ ] `GetAvistamientoByIdAsync(Guid id)` - Detalle
  - [ ] `CreateAvistamientoAsync(CreateAvistamientoDto)` - Crear
  - [ ] `CompararConReporteAsync(Guid avistamientoId)` - Comparar con IA

### Controller
- [ ] `AvistamientosController.cs`
  - [ ] `GET /api/extravios/{reporteId}/avistamientos` - Listar
  - [ ] `POST /api/extravios/{reporteId}/avistamientos` - Reportar
  - [ ] `GET /api/avistamientos/{id}` - Detalle
  - [ ] `POST /api/avistamientos/{id}/comparar` - Comparar fotos con IA

### Configuración Azure
- [ ] Crear recurso de Computer Vision en Azure
- [ ] Configurar claves en User Secrets
- [ ] Configurar límites de uso (tier gratuito)

### Validadores
- [ ] `CreateAvistamientoValidator.cs`

---

# FASE 5: MÓDULOS DE SOPORTE

**Prioridad:** 🟡 MEDIA
**Tiempo estimado:** 1-2 semanas 
**Estado:** ⏳ PENDIENTE

---

## 5.1 Módulo de Calificaciones/Insignias

### DTOs
- [ ] `CalificacionDto.cs`
- [ ] `CreateCalificacionDto.cs`
- [ ] `InsigniaDto.cs`

### Servicios
- [ ] `ICalificacionService.cs` - Interface
- [ ] `CalificacionService.cs` - Implementación
  - [ ] `OtorgarCalificacionAsync(CreateCalificacionDto)` - Otorgar
  - [ ] `GetCalificacionesUsuarioAsync(Guid usuarioId)` - Por usuario
  - [ ] `GetInsigniasUsuarioAsync(Guid usuarioId)` - Insignias
  - [ ] `CalcularPuntajeUsuarioAsync(Guid usuarioId)` - Calcular puntaje total

### Controller
- [ ] `CalificacionesController.cs`
  - [ ] `POST /api/calificaciones` - Otorgar calificación
  - [ ] `GET /api/calificaciones/usuario/{id}` - Calificaciones
  - [ ] `GET /api/insignias/tipos` - Tipos disponibles

### Validadores
- [ ] `CreateCalificacionValidator.cs`

---

## 5.2 Módulo de Notificaciones

### DTOs
- [ ] `NotificacionDto.cs`
- [ ] `NotificacionPreferenciasDto.cs`

### Servicios
- [ ] `INotificacionService.cs` - Interface
- [ ] `NotificacionService.cs` - Implementación
  - [ ] `GetNotificacionesUsuarioAsync(Guid usuarioId)` - Listar
  - [ ] `MarcarComoLeidaAsync(Guid notificacionId)` - Marcar leída
  - [ ] `MarcarTodasComoLeidasAsync(Guid usuarioId)` - Todas leídas
  - [ ] `DeleteNotificacionAsync(Guid id)` - Eliminar
  - [ ] `EnviarNotificacionAsync(Guid usuarioId, string tipo, string contenido)` - Enviar
  - [ ] `GetPreferenciasAsync(Guid usuarioId)` - Preferencias
  - [ ] `UpdatePreferenciasAsync(Guid usuarioId, NotificacionPreferenciasDto)` - Actualizar

### Servicio de Push Notifications (Firebase - Opcional)
- [ ] `IPushNotificationService.cs` - Interface
- [ ] `FirebasePushNotificationService.cs` - Implementación
  - [ ] Configurar Firebase Cloud Messaging
  - [ ] `SendPushAsync(deviceToken, title, body)` - Enviar push
  - [ ] `SendToMultipleAsync(tokens[], title, body)` - Múltiples dispositivos

### Controller
- [ ] `NotificacionesController.cs`
  - [ ] `GET /api/notificaciones` - Listar
  - [ ] `PUT /api/notificaciones/{id}/leer` - Marcar leída
  - [ ] `PUT /api/notificaciones/leer-todas` - Todas leídas
  - [ ] `DELETE /api/notificaciones/{id}` - Eliminar
  - [ ] `GET /api/notificaciones/preferencias` - Preferencias
  - [ ] `POST /api/notificaciones/preferencias` - Actualizar

### Validadores
- [ ] `NotificacionPreferenciasValidator.cs`

---

# FASE 6: FUNCIONALIDADES AVANZADAS

**Prioridad:** 🟢 MEDIA-BAJA  
**Tiempo estimado:** 1-2 semanas  
**Estado:** ⏳ PENDIENTE

---

## 6.1 Servicio de Navegación (Waze/Google Maps)

### DTOs
- [ ] `RutaNavegacionDto.cs`

### Servicios
- [ ] `INavegacionService.cs` - Interface
- [ ] `NavegacionService.cs` - Implementación
  - [ ] `GetWazeUrlAsync(lat, lon)` - URL de Waze
  - [ ] `GetGoogleMapsUrlAsync(lat, lon)` - URL de Google Maps

### Controller
- [ ] `NavegacionController.cs`
  - [ ] `GET /api/navegacion/ruta` - Generar URL de navegación

---

## 6.2 Dashboard/Feed Personalizado

### DTOs
- [ ] `FeedItemDto.cs` - Item del feed
- [ ] `FeedRequestDto.cs` - Parámetros de consulta

### Servicios
- [ ] `IFeedService.cs` - Interface
- [ ] `FeedService.cs` - Implementación
  - [ ] `GetFeedUsuarioAsync(Guid usuarioId, FeedRequestDto)` - Feed personalizado
  - [ ] Combinar: adopciones cercanas, extravíos, reportes callejeros
  - [ ] Paginación
  - [ ] Filtros por tipo, distancia

### Controller
- [ ] `FeedController.cs`
  - [ ] `GET /api/feed` - Feed personalizado del usuario

---

# FASE 7: ADMINISTRACIÓN (FUTURO)

**Prioridad:** 🔵 BAJA (Post-Tesis)  
**Tiempo estimado:** 2-3 semanas  
**Estado:** 📅 PLANIFICADO

---

## 7.1 Roles y Permisos

- [ ] Crear tabla `roles`
- [ ] Crear tabla `permisos`
- [ ] Crear tabla `usuario_roles`
- [ ] Implementar autorización basada en roles
- [ ] Tipos de usuario:
  - [ ] Usuario Regular
  - [ ] Albergue/Asociación
  - [ ] Administrador

---

## 7.2 Panel de Administración

- [ ] Endpoints de gestión de usuarios
- [ ] Endpoints de moderación de contenido
- [ ] Estadísticas globales
- [ ] Reportes de actividad

---

# FASE 8: TESTING Y CALIDAD

**Prioridad:** 🟠 ALTA  
**Tiempo estimado:** 2 semanas  
**Estado:** ⏳ PENDIENTE

---

## 8.1 Pruebas Unitarias

- [ ] Tests de servicios de autenticación
- [ ] Tests de servicios de mascotas
- [ ] Tests de servicios de adopciones
- [ ] Tests de servicios de extravíos
- [ ] Tests de validadores
- [ ] Cobertura mínima: 70%

---

## 8.2 Pruebas de Integración

- [ ] Tests de endpoints de autenticación
- [ ] Tests de endpoints de CRUD
- [ ] Tests de geolocalización
- [ ] Tests de comparación de imágenes

---

## 8.3 Pruebas de Seguridad

- [ ] Verificar rate limiting
- [ ] Verificar JWT expiration
- [ ] Verificar CORS
- [ ] Verificar validación de inputs
- [ ] Verificar SQL Injection prevention
- [ ] Verificar XSS prevention
- [ ] Penetration testing básico

---

# FASE 9: DOCUMENTACIÓN

**Prioridad:** 🟠 ALTA (Para Tesis)  
**Tiempo estimado:** 1 semana  
**Estado:** ⏳ PENDIENTE

---

## 9.1 Documentación Técnica

- [ ] Swagger/OpenAPI completo con ejemplos
- [ ] README.md del proyecto
- [ ] Guía de instalación
- [ ] Guía de configuración
- [ ] Arquitectura del sistema (diagramas)
- [ ] Modelo de base de datos (diagrama ER)
- [ ] Flujos de autenticación (diagramas de secuencia)

---

## 9.2 Documentación de Tesis

- [ ] Marco teórico
- [ ] Justificación de tecnologías
- [ ] Diseño de la solución
- [ ] Implementación
- [ ] Pruebas realizadas
- [ ] Resultados
- [ ] Conclusiones
- [ ] Recomendaciones

---

# FASE 10: DEPLOYMENT

**Prioridad:** 🟡 MEDIA  
**Tiempo estimado:** 1 semana  
**Estado:** ⏳ PENDIENTE

---

## 10.1 Preparación para Producción

- [ ] Configurar variables de entorno
- [ ] Configurar logging en producción
- [ ] Optimizar queries de base de datos
- [ ] Configurar caché (Redis opcional)
- [ ] Configurar CDN para imágenes (opcional)

---

## 10.2 Deployment a Azure/AWS

- [ ] Crear App Service / EC2
- [ ] Configurar base de datos MySQL en la nube
- [ ] Configurar Azure Key Vault / AWS Secrets Manager
- [ ] Configurar CI/CD con GitHub Actions
- [ ] Configurar monitoreo (Application Insights / CloudWatch)
- [ ] Configurar backups automáticos

---

# 🔐 SEGURIDAD - CHECKLIST COMPLETO

## Base de Datos
- [x] Conexión SSL/TLS
- [x] Usuario con permisos limitados
- [x] Backups automáticos configurados
- [x] GUIDs en lugar de IDs incrementales
- [ ] Auditoría de cambios críticos
- [ ] Cifrado de datos sensibles en BD

## Web API
- [x] JWT con refresh tokens
- [x] Contraseñas hasheadas con BCrypt
- [x] User Secrets para claves
- [x] HTTPS obligatorio
- [ ] Rate Limiting implementado
- [ ] CORS configurado correctamente
- [ ] Input validation en todos los endpoints
- [ ] SQL Injection prevention (EF Core)
- [ ] XSS prevention
- [ ] CSRF protection
- [ ] Logging de eventos de seguridad
- [ ] Manejo seguro de archivos

## Aplicación Móvil (.NET MAUI)
- [ ] Secure Storage para tokens
- [ ] Certificate Pinning (opcional avanzado)
- [ ] Ofuscación de código
- [ ] Validación de certificados SSL
- [ ] No almacenar contraseñas localmente
- [ ] Timeout de sesión

---

# 📊 MÉTRICAS DE PROGRESO

## Estado General
- **Total de Tareas:** ~200+
- **Completadas:** ~40 (20%)
- **En Progreso:** ~10 (5%)
- **Pendientes:** ~150 (75%)

## Por Fase
- **Fase 1 (Seguridad):** 80% completado
- **Fase 2 (Core):** 0% completado
- **Fase 3 (Adopciones):** 0% completado
- **Fase 4 (Extravíos):** 0% completado
- **Fase 5 (Soporte):** 0% completado
- **Fase 6 (Avanzadas):** 0% completado

---

# 📅 CRONOGRAMA ESTIMADO

| Fase | Duración | Fecha Inicio | Fecha Fin |
|------|----------|--------------|-----------|
| Fase 1 | 1-2 semanas | [Actual] | [+2 semanas] |
| Fase 2 | 2-3 semanas | [+2 semanas] | [+5 semanas] |
| Fase 3 | 2-3 semanas | [+5 semanas] | [+8 semanas] |
| Fase 4 | 2-3 semanas | [+8 semanas] | [+11 semanas] |
| Fase 5 | 1-2 semanas | [+11 semanas] | [+13 semanas] |
| Fase 6 | 1-2 semanas | [+13 semanas] | [+15 semanas] |
| Fase 8 | 2 semanas | [+15 semanas] | [+17 semanas] |
| Fase 9 | 1 semana | [+17 semanas] | [+18 semanas] |
| Fase 10 | 1 semana | [+18 semanas] | [+19 semanas] |

**Total estimado:** 4-5 meses

---

# 🎯 PRÓXIMOS PASOS INMEDIATOS

## Esta Semana
1. [ ] Completar `JwtService.cs`
2. [ ] Crear `AuthService.cs`
3. [ ] Crear validadores de autenticación
4. [ ] Crear middleware de manejo de errores
5. [ ] Configurar `Program.cs`
6. [ ] Crear `AuthController.cs`
7. [ ] Probar autenticación end-to-end en Swagger

## Siguiente Semana
1. [ ] Módulo de Usuarios completo
2. [ ] Módulo de Mascotas completo
3. [ ] Servicio de almacenamiento de archivos
4. [ ] Catálogos básicos

---

# 📚 RECURSOS Y REFERENCIAS

## Documentación Oficial
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Security](https://learn.microsoft.com/en-us/aspnet/core/security/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)
- [Azure Computer Vision](https://learn.microsoft.com/en-us/azure/cognitive-services/computer-vision/)

## Herramientas
- [Swagger/OpenAPI](https://swagger.io/)
- [Postman](https://www.postman.com/) - Testing de API
- [Serilog](https://serilog.net/) - Logging
- [FluentValidation](https://fluentvalidation.net/)

---

# 📝 NOTAS IMPORTANTES

## Decisiones de Diseño
1. **RefreshToken sin FK auto-referenciada:** Se decidió no usar foreign key para `ReplacedByToken` para evitar complejidad en migrations MySQL.
2. **Campos booleanos nullable:** `IsUsed?` e `IsRevoked?` permiten distinguir entre valores no asignados y `false`.
3. **User Secrets para JWT:** Clave JWT almacenada en User Secrets para seguridad en desarrollo.
4. **BCrypt Work Factor 12:** Balance entre seguridad y rendimiento (OWASP recomendado).

## Pendientes de Decisión
- [ ] ¿Usar Redis para caché?
- [ ] ¿Implementar GraphQL además de REST?
- [ ] ¿Usar Azure Blob Storage o almacenamiento local para imágenes?
- [ ] ¿Implementar WebSockets para notificaciones en tiempo real?

---

**Última actualización:** [Fecha actual]  
**Versión del documento:** 1.0  
**Autor:** [Tu nombre]

---

✨ **¡Éxito en tu proyecto de tesis!** 🐾