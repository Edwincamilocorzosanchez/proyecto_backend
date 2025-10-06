DROP DATABASE IF EXISTS backend_cs;
CREATE DATABASE backend_cs;
USE backend_cs;
-- MySQL DDL statements

-- =========================================================
-- USUARIOS Y ROLES
-- =========================================================
-- esta es la parte del JWT, tener en cuenta que no se puede tocar, debe de ser este mismo moddelo 
CREATE TABLE IF NOT EXISTS users_members
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_name VARCHAR(80) NOT NULL,
    email VARCHAR(80) NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL
);

-- los roles pueden ser cliente. admin y mecanico
CREATE TABLE IF NOT EXISTS roles
(
    id INT PRIMARY KEY,
    name VARCHAR(15) NOT NULL,
    description VARCHAR(255) NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL
);

-- intermedia por relacion mucho a mucho con el usuario y el rol
CREATE TABLE IF NOT EXISTS user_member_rol
(
    user_id INT NOT NULL,
    rol_id INT NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL,
    PRIMARY KEY (user_id, rol_id),
    CONSTRAINT fk_user_member_user FOREIGN KEY (user_id) REFERENCES users_members(id),
    CONSTRAINT fk_user_member_rol FOREIGN KEY (rol_id) REFERENCES roles(id)
);

-- tabla de tokens
CREATE TABLE IF NOT EXISTS refresh_token
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    token text NOT NULL,
    expires_at timestamp NOT NULL,
    is_revoked boolean NOT NULL,
    revoked_at timestamp NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL,
    CONSTRAINT fk_refresh_token_user FOREIGN KEY (user_id) REFERENCES users_members(id)
);

-- tabla de vehiculos
CREATE TABLE vehiculos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    cliente_id INT NOT NULL,
    marca VARCHAR(100) NOT NULL,
    modelo VARCHAR(100) NOT NULL,
    anio INT NOT NULL,
    vin VARCHAR(50) NOT NULL,
    kilometraje INT NOT NULL,
    CONSTRAINT fk_vehiculos_cliente FOREIGN KEY (cliente_id) REFERENCES clientes(id)
);

CREATE TABLE clientes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    telefono VARCHAR(20),
    direccion VARCHAR(255),
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL,
    CONSTRAINT fk_cliente_user FOREIGN KEY (user_id) REFERENCES users_members(id)
);

-- esta es la tabla de ordenes de servicio
CREATE TABLE orden_servicio (
    id INT PRIMARY KEY AUTO_INCREMENT,
    vehiculo_id INT NOT NULL,
    tipo_servicio VARCHAR(50) NOT NULL,
    mecanico VARCHAR(255) NOT NULL,
    fecha_ingreso TIMESTAMP NOT NULL,
    fecha_entrega_estimada TIMESTAMP NOT NULL,
    CONSTRAINT fk_orden_servicio_vehiculo FOREIGN KEY (vehiculo_id) REFERENCES vehiculos(id)
);

CREATE TABLE repuestos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    -- esto tiene que ser unico 
    codigo VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(255) NOT NULL,
    cantidad_stock INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
);

CREATE TABLE detalle_orden (
    orden_servicio_id INT NOT NULL,
    repuesto_id INT NOT NULL,
    cantidad INT NOT NULL,
    costo DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (orden_servicio_id, RepuestoId),
    CONSTRAINT fk_detalle_orden_orden_servicio FOREIGN KEY (orden_servicio_id) REFERENCES OrdenServicio(id),
    CONSTRAINT fk_detalle_orden_repuesto FOREIGN KEY (repuesto_id) REFERENCES Repuesto(id)
);

CREATE TABLE facturas (
    id INT PRIMARY KEY AUTO_INCREMENT,
    orden_servicio_id INT NOT NULL,
    monto_repuestos DECIMAL(10,2) NOT NULL,
    mano_obra DECIMAL(10,2) NOT NULL,
    total DECIMAL(10,2) NOT NULL,
    fecha_generacion DATETIME NOT NULL,
    CONSTRAINT fk_factura_orden_servicio FOREIGN KEY (orden_servicio_id) REFERENCES OrdenServicio(id)
);