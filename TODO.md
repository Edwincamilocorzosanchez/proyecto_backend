## Entidades estandar
<!-- mark as md -->
### Domain
Cliente: propietario de uno o varios vehículos, con datos de contacto (nombre, teléfono, correo).

Vehículo: asociado a un cliente; registra datos como marca, modelo, año, número de serie (VIN) y kilometraje.

OrdenServicio: representa una solicitud de trabajo que incluye referencia al vehículo, tipo de servicio (mantenimiento preventivo, reparación, diagnóstico), mecánico asignado, fecha de ingreso y fecha estimada de entrega.
Repuesto: piezas o insumos necesarios para ejecutar una orden; contiene código, descripción, cantidad en stock y precio unitario.

DetalleOrden: relación entre OrdenServicio y Repuesto, indicando cantidades y costo de cada pieza utilizada.

Usuario: persona que utiliza el sistema (con roles como “Admin”, “Mecánico” o “Recepcionista”); almacena credenciales (correo y contraseña hasheada) y rol asignado.

Factura: documento generado al cerrar una orden, con resumen de servicios, repuestos utilizados, mano de obra y monto total.

Mecanico 

TipoServicio

Pago 

HistorialInventario

Proveedor 

Cita

## App
ClienteDto, VehiculoDto, OrdenServicioDto, RepuestoDto, DetalleOrdenDto, FacturaDto y UsuarioDto
### servicios 
RegistrarClienteConVehiculo: crea un cliente y simultáneamente uno o varios vehículos asociados.

CrearOrdenServicio: genera una nueva orden, asigna mecánico, reserva repuestos si están disponibles y calcula fecha estimada de entrega basado en la complejidad.

ActualizarOrdenConTrabajoRealizado: permite al mecánico registrar el avance, actualizar el estado de la orden y descontar repuestos del inventario.

GenerarFactura: al cerrar la orden, calcula mano de obra más repuestos y crea la factura correspondiente.

### Usar automaper

# Validaciones 
 validar que un vehículo no esté agendado en dos órdenes simultáneas, calcular fechas estimadas según tipo de servicio y reglas de inventario que impiden usar repuestos fuera de stock.

| 🧑‍💻 Persona                                            | Módulo / Área                  | Tablas principales                                                                                                                             | Flujo completo                                                                                        | Dependencias                                          |
| -------------------------------------------------------- | ------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------- |
| **Desarrollador A – Gestión de Usuarios y Citas**        | **Usuarios, Clientes y Citas** | `clientes`, `vehiculos`, `citas`, `estados_cita`                                                                                               | Registro y gestión de clientes → registro de vehículos → agendamiento de citas.                       | Se apoya en `users_members` y `roles` (solo lectura). |
| **Desarrollador B – Operaciones y Servicios del Taller** | **Órdenes y Servicios**        | `tipo_servicio`, `orden_servicio`, `detalle_orden`, `mecanicos`, `estados_orden`                                                               | Asignación de mecánicos, gestión de órdenes, servicios realizados, estado del trabajo, piezas usadas. | Se apoya en vehículos (de A) y repuestos (de C).      |
| **Desarrollador C – Inventario, Facturación y Pagos**    | **Inventario y Finanzas**      | `repuestos`, `historial_inventario`, `proveedores`, `facturas`, `pagos`, `metodos_pago`, `tipos_movimiento`, `estados_pago`, `administradores` | Gestión de stock, entradas/salidas, proveedores, facturación y pagos.                                 | Se apoya en órdenes (de B) para generar facturas.     |



## Listado de verbos HTTP

### Usuarios
AuthController, UsersController, RolesController
| Verbo    | Endpoint                               | Descripción                        |
| :------- | :------------------------------------- | :--------------------------------- |
| `POST`   | `/api/auth/register`                   | Registrar un nuevo usuario         |
| `POST`   | `/api/auth/login`                      | Iniciar sesión y obtener token JWT |
| `POST`   | `/api/auth/refresh`                    | Renovar token JWT                  |
| `POST`   | `/api/auth/logout`                     | Revocar token de sesión            |
| `GET`    | `/api/auth/profile`                    | Obtener perfil autenticado         |
| `PATCH`  | `/api/auth/change-password`            | Cambiar contraseña                 |
| `GET`    | `/api/users`                           | Listar usuarios (admin only)       |
| `GET`    | `/api/users/{id}`                      | Detalle de usuario                 |
| `PUT`    | `/api/users/{id}`                      | Actualizar usuario                 |
| `DELETE` | `/api/users/{id}`                      | Eliminar usuario                   |
| `GET`    | `/api/roles`                           | Listar roles disponibles           |
| `POST`   | `/api/users/{id}/assign-role`          | Asignar rol a usuario              |
| `DELETE` | `/api/users/{id}/revoke-role/{roleId}` | Revocar rol de usuario             |


### Clientes/Mecanicos/Administradores/Proveedores
ClientesController, MecanicosController, AdministradoresController, ProveedoresController
| Verbo    | Endpoint                           | Descripción                |
| :------- | :--------------------------------- | :------------------------- |
| `GET`    | `/api/clientes`                    | Listar clientes            |
| `GET`    | `/api/clientes/{id}`               | Detalle de cliente         |
| `POST`   | `/api/clientes`                    | Registrar cliente          |
| `PUT`    | `/api/clientes/{id}`               | Editar cliente             |
| `PATCH`  | `/api/clientes/{id}/toggle-active` | Activar/Desactivar cliente |
| `DELETE` | `/api/clientes/{id}`               | Eliminar cliente           |
| `GET`    | `/api/mecanicos`                   | Listar mecánicos           |
| `GET`    | `/api/mecanicos/{id}`              | Detalle de mecánico        |
| `POST`   | `/api/mecanicos`                   | Crear mecánico             |
| `PUT`    | `/api/mecanicos/{id}`              | Actualizar mecánico        |
| `PATCH`  | `/api/mecanicos/{id}/especialidad` | Cambiar especialidad       |
| `GET`    | `/api/proveedores`                 | Listar proveedores         |
| `POST`   | `/api/proveedores`                 | Crear proveedor            |
| `PUT`    | `/api/proveedores/{id}`            | Editar proveedor           |
| `DELETE` | `/api/proveedores/{id}`            | Eliminar proveedor         |
| `GET`    | `/api/administradores`             | Listar administradores     |
| `POST`   | `/api/administradores`             | Crear administrador        |
| `PATCH`  | `/api/administradores/{id}/nivel`  | Cambiar nivel de acceso    |

### Vehiculos y Citas 
VehiculosController, CitasController
| Verbo    | Endpoint                       | Descripción             |
| :------- | :----------------------------- | :---------------------- |
| `GET`    | `/api/vehiculos`               | Listar vehículos        |
| `GET`    | `/api/vehiculos/{id}`          | Detalle de vehículo     |
| `POST`   | `/api/vehiculos`               | Registrar vehículo      |
| `PUT`    | `/api/vehiculos/{id}`          | Editar vehículo         |
| `GET`    | `/api/clientes/{id}/vehiculos` | Vehículos de un cliente |
| `DELETE` | `/api/vehiculos/{id}`          | Eliminar vehículo       |
| `GET`    | `/api/citas`                   | Listar citas            |
| `GET`    | `/api/citas/{id}`              | Detalle de cita         |
| `POST`   | `/api/citas`                   | Crear cita              |
| `PUT`    | `/api/citas/{id}`              | Editar cita             |
| `PATCH`  | `/api/citas/{id}/estado`       | Cambiar estado          |
| `GET`    | `/api/citas/cliente/{id}`      | Citas de un cliente     |
| `GET`    | `/api/citas/vehiculo/{id}`     | Citas de un vehículo    |
| `DELETE` | `/api/citas/{id}`              | Cancelar cita           |

### Servicios y Ordenes
TiposServicioController, OrdenServicioController, DetalleOrdenController
| Verbo    | Endpoint                                  | Descripción                       |
| :------- | :---------------------------------------- | :-------------------------------- |
| `GET`    | `/api/tipos-servicio`                     | Listar tipos de servicio          |
| `POST`   | `/api/tipos-servicio`                     | Crear tipo de servicio            |
| `PUT`    | `/api/tipos-servicio/{id}`                | Editar tipo de servicio           |
| `DELETE` | `/api/tipos-servicio/{id}`                | Eliminar tipo de servicio         |
| `GET`    | `/api/ordenes`                            | Listar órdenes de servicio        |
| `GET`    | `/api/ordenes/{id}`                       | Detalle de orden                  |
| `POST`   | `/api/ordenes`                            | Crear orden de servicio           |
| `PUT`    | `/api/ordenes/{id}`                       | Actualizar orden                  |
| `PATCH`  | `/api/ordenes/{id}/estado`                | Cambiar estado de orden           |
| `GET`    | `/api/ordenes/mecanico/{id}`              | Órdenes asignadas a un mecánico   |
| `GET`    | `/api/ordenes/vehiculo/{id}`              | Órdenes de un vehículo            |
| `GET`    | `/api/ordenes/{id}/detalles`              | Detalles (repuestos) de una orden |
| `POST`   | `/api/ordenes/{id}/detalles`              | Agregar repuesto a orden          |
| `DELETE` | `/api/ordenes/{id}/detalles/{repuestoId}` | Eliminar repuesto de orden        |

### Facturacion y Pagos
FacturasController, PagosController
| Verbo   | Endpoint                         | Descripción               |
| :------ | :------------------------------- | :------------------------ |
| `GET`   | `/api/facturas`                  | Listar facturas           |
| `GET`   | `/api/facturas/{id}`             | Detalle de factura        |
| `POST`  | `/api/facturas`                  | Generar factura           |
| `GET`   | `/api/facturas/orden/{ordenId}`  | Obtener factura por orden |
| `GET`   | `/api/pagos`                     | Listar pagos              |
| `POST`  | `/api/pagos`                     | Registrar pago            |
| `GET`   | `/api/pagos/{id}`                | Detalle de pago           |
| `PATCH` | `/api/pagos/{id}/estado`         | Cambiar estado de pago    |
| `GET`   | `/api/pagos/factura/{facturaId}` | Pagos por factura         |

### Inventario y Repuestos
RepuestosController, HistorialInventarioController
| Verbo    | Endpoint                         | Descripción                           |
| :------- | :------------------------------- | :------------------------------------ |
| `GET`    | `/api/repuestos`                 | Listar repuestos                      |
| `GET`    | `/api/repuestos/{id}`            | Detalle de repuesto                   |
| `POST`   | `/api/repuestos`                 | Crear repuesto                        |
| `PUT`    | `/api/repuestos/{id}`            | Editar repuesto                       |
| `PATCH`  | `/api/repuestos/{id}/stock`      | Actualizar stock                      |
| `DELETE` | `/api/repuestos/{id}`            | Eliminar repuesto                     |
| `GET`    | `/api/historial-inventario`      | Listar movimientos de inventario      |
| `POST`   | `/api/historial-inventario`      | Registrar movimiento (entrada/salida) |
| `GET`    | `/api/historial-inventario/{id}` | Detalle de movimiento                 |

### Tablas auxiliares
EstadosController, TiposController
| Verbo | Endpoint                | Descripción                |
| :---- | :---------------------- | :------------------------- |
| `GET` | `/api/estados/cita`     | Listar estados de cita     |
| `GET` | `/api/estados/orden`    | Listar estados de orden    |
| `GET` | `/api/estados/pago`     | Listar estados de pago     |
| `GET` | `/api/tipos/movimiento` | Listar tipos de movimiento |
| `GET` | `/api/metodos-pago`     | Listar métodos de pago     |

## Reportes y estadísticas
ReportesController
| Verbo | Endpoint                   | Descripción                          |
| :---- | :------------------------- | :----------------------------------- |
| `GET` | `/api/reportes/ventas`     | Reporte de ventas por periodo        |
| `GET` | `/api/reportes/servicios`  | Reporte de servicios más solicitados |
| `GET` | `/api/reportes/mecanicos`  | Rendimiento por mecánico             |
| `GET` | `/api/reportes/inventario` | Repuestos con bajo stock             |
| `GET` | `/api/reportes/clientes`   | Clientes más frecuentes              |

















### Cosas que quizas se pueden hacer mejor
- En la base de datos agregar la tabla inventario y anidarla con repuestos, historial_inventario...

- puedo mejorar los mensajes de los Validatos de la parte de Application

- Quitarles las columnas Nombres a todas las personas que se relacionen con UserMember

- agregar la columnas que me faltan de los DTOs que tienen entidades foraneas y que pueden tener consultas grandes

- En UserMember hay UserName y en las otras entidades hay Nombre, correo y telefono. ¿Por qué no se usa el mismo? Tengo que corregirlo en todos los DTOs, entidades, Profiles, Services, Controller y configuraciones de EF.

- Si el UserMember tiene un id Guid entonces todos los demas entidades que heredan el ID principal de UserMember deben ser Guid tambien. :(
- tengo que revisar si debo de crear un Service por cada entidad o si puedo dejar los servicios de las entidades que he colocado 

- En cada controlador se debe de colocar el CORS y el RateLimiter que va a aplicar

- mirar si la interfaz del servicio debe de tener el CancellationToken, quizas debo de refactorizar todos los servicios porque no implementan todos los metodos de los repositorios



# Preguntas
- hacer la refactorizacion de los controladores
- hacer la refactorizacion de los servicios
- hacer que los controladores sean todos plurales o singulares
- hacer los RateLimiter y CORS en el nivel de la Api
- que debo de hacer para que el DBSeaderFuncione 









## TODO por ahora 

- hacer lo de los CORS y RateLimiter