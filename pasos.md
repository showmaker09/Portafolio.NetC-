# Plan de Desarrollo: Integración de API en aplicaciones .NET MAUI

## 1. Reutilización de Modelos (`TherapyApp.Core`)

El objetivo principal es usar una sola fuente de verdad para los modelos de datos compartidos entre la API y las aplicaciones móviles.

* **Paso 1.1:** Asegúrate de que los proyectos de cliente (`TherapyApp.Patient` y `TherapyApp.Therapist`) tengan una referencia de proyecto apuntando a `TherapyApp.Core`.
* **Paso 1.2:** Mantén centralizados en `TherapyApp.Core` (por ejemplo, en una carpeta `DTOs` o `Models`) todos los objetos expuestos y consumidos por la API observados en el archivo `test.http`:
  * `ApiResponse<T>`
  * `JournalEntryRequest` y `JournalEntryResponse`
  * `SessionRequest`
  * Respuestas y Modelos de Reportes.
* **Paso 1.3:** Verifica y purga cualquier modelo duplicado en los clientes; todas las pantallas y servicios deben hacer un `using TherapyApp.Core.DTOs;` (o namespace correspondiente).

## 2. Capa de Servicios de Red (Consumo API)

En este paso crearemos la comunicación HTTP desde las apps MAUI hacia el backend.

* **Paso 2.1:** Crea una carpeta `Services` en los proyectos MAUI (o en el *Core* si buscas máxima reutilización).
* **Paso 2.2:** Define una interfaz base, por decir `IApiService`, que contenga firmas como `Task<ApiResponse<T>> GetAsync<T>(string endpoint)` y `Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)`.
* **Paso 2.3:** Implementa la interfaz usando un `HttpClient` configurado.
* **Paso 2.4:** **Gestión de Cabeceras (API Key):** 
  * Configura el `HttpClient` para inyectar por defecto el encabezado de autenticación (`DefaultRequestHeaders.Add("X-Api-Key", "tu-api-key")`).
* **Paso 2.5:** **Direccionamiento para Desarrollo:**
  * Define la URL Base de tu API dependiendo de la plataforma que corre la app. Por ejemplo, en Windows Local será `http://localhost:5109`, pero si lanzas un Emulador en Android deberá usar `http://10.0.2.2:5109` para poder salir de la red virtual del emulador hacia el host.
* **Paso 2.6:** Registra los servicios creados usando inyección de dependencias en `MauiProgram.cs` de cada proyecto cliente (p. ej. `builder.Services.AddSingleton<IApiService, ApiService>();`).

## 3. Pasos Posteriores de Desarrollo

Una vez cubierta la conexión básica, debes abordar la construcción de la UI y lógica de negocio para cerrar el ciclo:

* **3.1 Arquitectura MVVM:**
  * Añade el paquete NuGet `CommunityToolkit.Mvvm` en los clientes.
  * Crea los ViewModels por funcionalidad (p. ej., `SessionViewModel` o `JournalViewModel`).
  * Inyecta `IApiService` en el constructor de estos ViewModels para efectuar llamadas vía comandos de usuario (botones).
* **3.2 Gestión del Estado (Storage Seguro):**
  * Para no dejar en el código los `patientId`, el `therapistId`, `claves de API` o el `SESSION_ID` que se usarán habitualmente, implementa `SecureStorage` (provisto por MAUI) para guardarlos y leerlos persistentemente a nivel local.
* **3.3 Pantallas (XAML) y Navegación:**
  * Define la navegación base modificando `AppShell.xaml`.
  * Genera las pantallas visuales (`ContentPage`) para los Diarios del paciente y Dashboard del terapeuta, y enlaza cada pantalla (Binding) con su ViewModel.
* **3.4 Configuración Nativa (Opcional en Dev):**
  * Ya que la API corre temporalmente en `http`, es importante configurar el archivito XML de reglas de seguridad en Android para admitir tráfico en texto plano (`Cleartext traffic permitted`) en la red de pruebas a `10.0.2.2`, o habilitar entornos integrados con certificados HTTPS de desarrollo para Mobile.
