INSERT INTO "BeClever"."Business" (location_name) VALUES ('Argentina');
INSERT INTO "BeClever"."Business" (location_name) VALUES ('Brasil');
INSERT INTO "BeClever"."Business" (location_name) VALUES ('España');

INSERT INTO "BeClever"."Departments" (department_name) VALUES ('Ventas');
INSERT INTO "BeClever"."Departments" (department_name) VALUES ('Recursos Humanos');
INSERT INTO "BeClever"."Departments" (department_name) VALUES ('Finanzas');
INSERT INTO "BeClever"."Departments" (department_name) VALUES ('Marketing');
INSERT INTO "BeClever"."Departments" (department_name) VALUES ('Producción');

INSERT INTO "BeClever"."Employees" (first_name, last_name, gender, "idDepartments") VALUES ('Juan', 'Perez', 'M', 1);
INSERT INTO "BeClever"."Employees" (first_name, last_name, gender, "idDepartments") VALUES ('María', 'González', 'F', 2);
INSERT INTO "BeClever"."Employees" (first_name, last_name, gender, "idDepartments") VALUES ('Carlos', 'Martínez', 'M', 1);
INSERT INTO "BeClever"."Employees" (first_name, last_name, gender, "idDepartments") VALUES ('Laura', 'López', 'F', 4);
INSERT INTO "BeClever"."Employees" (first_name, last_name, gender, "idDepartments") VALUES ('Pedro', 'Sánchez', 'M', 5);

-- Ingresos y egresos para el primer empleado
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (1, '2024-05-12 08:00:00', 'ingreso', 1);  -- Ingreso en la sucursal Argentina
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (1, '2024-05-12 17:00:00', 'egreso', 1);   -- Egreso en la sucursal Argentina
-- Ingresos y egresos para el segundo empleado
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (2, '2024-05-12 09:00:00', 'ingreso', 1);  -- Ingreso en la sucursal Argentina
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (2, '2024-05-12 18:00:00', 'egreso', 1);   -- Egreso en la sucursal Argentina
-- Ingresos y egresos para el tercer empleado
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (3, '2024-05-12 10:00:00', 'ingreso', 1);  -- Ingreso en la sucursal Argentina
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (3, '2024-05-12 19:00:00', 'egreso', 1);   -- Egreso en la sucursal Argentina
-- Ingresos y egresos para el cuarto empleado
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (4, '2024-05-12 11:00:00', 'ingreso', 1);  -- Ingreso en la sucursal Argentina
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (4, '2024-05-12 20:00:00', 'egreso', 1);   -- Egreso en la sucursal Argentina
-- Ingresos y egresos para el quinto empleado
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (5, '2024-05-12 12:00:00', 'ingreso', 1);  -- Ingreso en la sucursal Argentina
INSERT INTO "BeClever"."Registers" ("idEmployee", date_time, register_type, "idBusiness") VALUES (5, '2024-05-12 21:00:00', 'egreso', 1);   -- Egreso en la sucursal Argentina

