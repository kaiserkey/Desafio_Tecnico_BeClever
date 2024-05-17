--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE "BeClever";
--
-- Name: BeClever; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "BeClever" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Spanish_Argentina.1252';


ALTER DATABASE "BeClever" OWNER TO postgres;

\connect "BeClever"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: BeClever; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA "BeClever";


ALTER SCHEMA "BeClever" OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Business; Type: TABLE; Schema: BeClever; Owner: postgres
--

CREATE TABLE "BeClever"."Business" (
    "idBusiness" integer NOT NULL,
    location_name character varying(100) NOT NULL
);


ALTER TABLE "BeClever"."Business" OWNER TO postgres;

--
-- Name: Business_idBusiness_seq; Type: SEQUENCE; Schema: BeClever; Owner: postgres
--

CREATE SEQUENCE "BeClever"."Business_idBusiness_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "BeClever"."Business_idBusiness_seq" OWNER TO postgres;

--
-- Name: Business_idBusiness_seq; Type: SEQUENCE OWNED BY; Schema: BeClever; Owner: postgres
--

ALTER SEQUENCE "BeClever"."Business_idBusiness_seq" OWNED BY "BeClever"."Business"."idBusiness";


--
-- Name: Departments; Type: TABLE; Schema: BeClever; Owner: postgres
--

CREATE TABLE "BeClever"."Departments" (
    "idDepartment" integer NOT NULL,
    department_name character varying(100) NOT NULL
);


ALTER TABLE "BeClever"."Departments" OWNER TO postgres;

--
-- Name: Departments_idDepartment_seq; Type: SEQUENCE; Schema: BeClever; Owner: postgres
--

CREATE SEQUENCE "BeClever"."Departments_idDepartment_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "BeClever"."Departments_idDepartment_seq" OWNER TO postgres;

--
-- Name: Departments_idDepartment_seq; Type: SEQUENCE OWNED BY; Schema: BeClever; Owner: postgres
--

ALTER SEQUENCE "BeClever"."Departments_idDepartment_seq" OWNED BY "BeClever"."Departments"."idDepartment";


--
-- Name: Employees; Type: TABLE; Schema: BeClever; Owner: postgres
--

CREATE TABLE "BeClever"."Employees" (
    "idEmployee" integer NOT NULL,
    first_name character varying(50) NOT NULL,
    last_name character varying(50) NOT NULL,
    gender character varying(1) NOT NULL,
    "idDepartments" integer NOT NULL
);


ALTER TABLE "BeClever"."Employees" OWNER TO postgres;

--
-- Name: Employees_idEmployee_seq; Type: SEQUENCE; Schema: BeClever; Owner: postgres
--

CREATE SEQUENCE "BeClever"."Employees_idEmployee_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "BeClever"."Employees_idEmployee_seq" OWNER TO postgres;

--
-- Name: Employees_idEmployee_seq; Type: SEQUENCE OWNED BY; Schema: BeClever; Owner: postgres
--

ALTER SEQUENCE "BeClever"."Employees_idEmployee_seq" OWNED BY "BeClever"."Employees"."idEmployee";


--
-- Name: Registers; Type: TABLE; Schema: BeClever; Owner: postgres
--

CREATE TABLE "BeClever"."Registers" (
    "idRegister" integer NOT NULL,
    "idEmployee" integer NOT NULL,
    date_time timestamp without time zone NOT NULL,
    register_type character varying(20) NOT NULL,
    "idBusiness" integer NOT NULL
);


ALTER TABLE "BeClever"."Registers" OWNER TO postgres;

--
-- Name: Registers_idRegister_seq; Type: SEQUENCE; Schema: BeClever; Owner: postgres
--

CREATE SEQUENCE "BeClever"."Registers_idRegister_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "BeClever"."Registers_idRegister_seq" OWNER TO postgres;

--
-- Name: Registers_idRegister_seq; Type: SEQUENCE OWNED BY; Schema: BeClever; Owner: postgres
--

ALTER SEQUENCE "BeClever"."Registers_idRegister_seq" OWNED BY "BeClever"."Registers"."idRegister";


--
-- Name: Business idBusiness; Type: DEFAULT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Business" ALTER COLUMN "idBusiness" SET DEFAULT nextval('"BeClever"."Business_idBusiness_seq"'::regclass);


--
-- Name: Departments idDepartment; Type: DEFAULT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Departments" ALTER COLUMN "idDepartment" SET DEFAULT nextval('"BeClever"."Departments_idDepartment_seq"'::regclass);


--
-- Name: Employees idEmployee; Type: DEFAULT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Employees" ALTER COLUMN "idEmployee" SET DEFAULT nextval('"BeClever"."Employees_idEmployee_seq"'::regclass);


--
-- Name: Registers idRegister; Type: DEFAULT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Registers" ALTER COLUMN "idRegister" SET DEFAULT nextval('"BeClever"."Registers_idRegister_seq"'::regclass);


--
-- Name: Business Business_pkey; Type: CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Business"
    ADD CONSTRAINT "Business_pkey" PRIMARY KEY ("idBusiness");


--
-- Name: Departments Departments_pkey; Type: CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Departments"
    ADD CONSTRAINT "Departments_pkey" PRIMARY KEY ("idDepartment");


--
-- Name: Employees Employees_pkey; Type: CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Employees"
    ADD CONSTRAINT "Employees_pkey" PRIMARY KEY ("idEmployee");


--
-- Name: Registers Registers_pkey; Type: CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Registers"
    ADD CONSTRAINT "Registers_pkey" PRIMARY KEY ("idRegister");


--
-- Name: Registers FK_BUSINESS; Type: FK CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Registers"
    ADD CONSTRAINT "FK_BUSINESS" FOREIGN KEY ("idBusiness") REFERENCES "BeClever"."Business"("idBusiness") NOT VALID;


--
-- Name: Employees FK_DEPARTMENT; Type: FK CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Employees"
    ADD CONSTRAINT "FK_DEPARTMENT" FOREIGN KEY ("idDepartments") REFERENCES "BeClever"."Departments"("idDepartment") NOT VALID;


--
-- Name: Registers FK_EMPLOYEE; Type: FK CONSTRAINT; Schema: BeClever; Owner: postgres
--

ALTER TABLE ONLY "BeClever"."Registers"
    ADD CONSTRAINT "FK_EMPLOYEE" FOREIGN KEY ("idEmployee") REFERENCES "BeClever"."Employees"("idEmployee") NOT VALID;


--
-- PostgreSQL database dump complete
--

