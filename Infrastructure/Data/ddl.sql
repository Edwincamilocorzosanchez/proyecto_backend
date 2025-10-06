DROP DATABASE IF EXISTS backend_cs;
CREATE DATABASE backend_cs;
USE backend_cs;
-- MySQL DDL statements

CREATE TABLE IF NOT EXISTS users_members
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_name VARCHAR(80) NOT NULL,
    email VARCHAR(80) NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL
);

CREATE TABLE IF NOT EXISTS roles
(
    id INT PRIMARY KEY,
    name VARCHAR(15) NOT NULL,
    description VARCHAR(255) NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL
);

CREATE TABLE IF NOT EXISTS user_member_rol
(
    user_id INT NOT NULL,
    rol_id INT NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL,
    PRIMARY KEY (user_id, rol_id),
    CONSTRAINT fk_user_member_user FOREIGN KEY (user_id) REFERENCES user(id),
    CONSTRAINT fk_user_member_rol FOREIGN KEY (rol_id) REFERENCES roles(id)
);

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
    CONSTRAINT fk_refresh_token_user FOREIGN KEY (user_id) REFERENCES user(id)
);
