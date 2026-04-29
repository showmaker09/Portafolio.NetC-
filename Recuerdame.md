## La regla es simple: 
## si es una clase de datos pura (modelo, DTO, interfaz) va en Core. 
## Si es lógica del servidor (base de datos, endpoints, repositorios) va en Api. 
## Si es pantalla o lógica de UI va en Patient o Therapist.


## Un DTO (Data Transfer Object o Objeto de Transferencia de Datos) es un patrón de diseño utilizado en programación 
## para transportar datos entre capas de una aplicación (ej. de base de datos a interfaz) 
## o entre cliente-servidor en una sola invocación.

## asíncrona: puedes iniciar una tarea que toma tiempo y, en lugar de quedarte de brazos cruzados esperando
## sigues haciendo otras cosas hasta que esa tarea termina. 