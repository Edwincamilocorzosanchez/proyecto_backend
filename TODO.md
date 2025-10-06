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
