USE master;
GO
CREATE DATABASE Proyecto;
GO
USE Proyecto;
GO
CREATE TABLE Usuarios(
	Id_Usuario int identity(1,1),
	Nombre NVARCHAR(100) NOT NULL,
	Email NVARCHAR(150) NOT NULL,
	Contrasena NVARCHAR(255) NOT NULL,
	Rol NVARCHAR(20) NOT NULL
		CHECK(Rol IN('Asistente', 'Organizador')),
	FechaRegistro DATETIME NOT NULL
		DEFAULT GETDATE(),
	CONSTRAINT Pk_Usuarios PRIMARY KEY(Id_Usuario),
	CONSTRAINT UQ_Email UNIQUE(Email)
);
GO
CREATE TABLE Eventos(
	Id_Evento int identity(1,1),
	Nombre NVARCHAR(150) NOT NULL,
	Descripcion NVARCHAR(500) NOT NULL,
	FechaInicio DATETIME NOT NULL,
	FechaFin DATETIME NOT NULL,
	Lugar NVARCHAR(200) NOT NULL,
	Precio DECIMAL(10,2) NOT NULL
	DEFAULT 0,
	CupoTotal int NOT NULL,
	Estado NVARCHAR(20) NOT NULL
	CHECK(Estado IN('Borrador', 'Publicado')),
	Id_Organizador INT NOT NULL,
	CONSTRAINT Fk_Eventos_Usuarios
		FOREIGN KEY(Id_Organizador) REFERENCES Usuarios(Id_Usuario),
	CONSTRAINT Pk_Eventos PRIMARY KEY(Id_Evento)
);
GO
CREATE TABLE Ponentes(
	Id_Ponente int identity(1,1),
	Nombre NVARCHAR(100) NOT NULL,
	Bio NVARCHAR(500),
	Foto NVARCHAR(300),
	Email NVARCHAR(150),
	Especialidad NVARCHAR(100),
	CONSTRAINT Pk_Ponentes PRIMARY KEY(Id_Ponente)
);
GO
CREATE TABLE Actividades(
	Id_Actividad INT IDENTITY(1,1),
	Id_Evento INT NOT NULL,
	Id_Ponente INT NOT NULL,
	Titulo NVARCHAR(150) NOT NULL,
	Descripcion NVARCHAR(500),
	HoraInicio DATETIME NOT NULL,
	HoraFin DATETIME NOT NULL,
	Cupo INT NOT NULL,
	Tipo NVARCHAR(50) NOT NULL
		CHECK(Tipo IN('Charla', 'Taller')),
	CONSTRAINT Pk_Actividades PRIMARY KEY(Id_Actividad),
	CONSTRAINT Fk_Actividades_Eventos
		FOREIGN KEY(Id_Evento) REFERENCES Eventos(Id_Evento),
	CONSTRAINT Fk_Actividades_Ponentes
		FOREIGN KEY(Id_Ponente) REFERENCES Ponentes(Id_Ponente)
);
GO
CREATE TABLE Inscripciones(
	Id_Inscripcion INT IDENTITY(1,1),
	Id_Usuario INT NOT NULL,
	Id_Evento INT NOT NULL,
	FechaInscripcion DATETIME DEFAULT GETDATE(),
	Estado NVARCHAR(30) NOT NULL
		CHECK(Estado IN('Pendiente', 'Confirmada', 'Cancelada')),
	CONSTRAINT Pk_Inscripciones PRIMARY KEY(Id_Inscripcion),
	CONSTRAINT Fk_Inscripciones_Usuario
		FOREIGN KEY(Id_Usuario) REFERENCES Usuarios(Id_Usuario),
	CONSTRAINT Fk_Inscripciones_Eventos
		FOREIGN KEY(Id_Evento) REFERENCES Eventos(Id_Evento)
);
GO
CREATE TABLE Pagos(
	Id_Pago INT IDENTITY(1,1),
	Id_Inscripcion INT NOT NULL,
	Monto DECIMAL(10,2) NOT NULL,
	FechaPago DATETIME NOT NULL DEFAULT GETDATE(),
	MetodoPago NVARCHAR(50) NOT NULL,
	Estado NVARCHAR(20) NOT NULL
		CHECK(Estado IN('Pendiente','Confirmado', 'Rechazado')),
	TransaccionId NVARCHAR(100),
	CONSTRAINT Pk_Pagos PRIMARY KEY(Id_Pago),
	CONSTRAINT Fk_Pagos_Inscripciones
		FOREIGN KEY(Id_Inscripcion) REFERENCES Inscripciones(Id_Inscripcion)
);
GO
CREATE TABLE Inscripciones_Actividades(
	Id INT IDENTITY(1,1),
	Id_Inscripcion INT NOT NULL,
	Id_Actividad INT NOT NULL,
	CONSTRAINT Pk_IA PRIMARY KEY(Id),
	CONSTRAINT Fk_IA_Inscripciones
		FOREIGN KEY(Id_Inscripcion) REFERENCES Inscripciones(Id_Inscripcion),
	CONSTRAINT FK_IA_Actividad
		FOREIGN KEY(Id_Actividad) REFERENCES Actividades(Id_Actividad)
);
GO
CREATE TABLE Asistencias(
	Id_Asistencia INT IDENTITY(1,1),
	Id_Inscripcion INT NOT NULL,
	Id_Actividad INT NOT NULL,
	FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	MetodoRegistro NVARCHAR(50) NOT NULL
		CHECK(MetodoRegistro IN('Manual' , 'QR')),
	CONSTRAINT Pk_Asistencias PRIMARY KEY(Id_Asistencia),
	CONSTRAINT Fk_Asistencias_Inscripciones
		FOREIGN KEY(Id_Inscripcion) REFERENCES Inscripciones(Id_Inscripcion),
	CONSTRAINT Fk_Asistencias_Actividad
		FOREIGN KEY(Id_Actividad) REFERENCES Actividades(Id_Actividad)
);
GO
CREATE TABLE Materiales(
	Id_Material INT IDENTITY(1,1),
	Id_Evento INT NOT NULL,
	Nombre NVARCHAR(150) NOT NULL,
	Tipo NVARCHAR(20) NOT NULL
		CHECK(Tipo IN('PDF', 'PPT', 'DOC')),
	Url NVARCHAR(300) NOT NULL,
	FechaSubida DATETIME NOT NULL DEFAULT GETDATE(),
	CONSTRAINT Pk_Materiales PRIMARY KEY(Id_Material),
	CONSTRAINT Fk_Materiales_Eventos
		FOREIGN KEY(Id_Evento) REFERENCES Eventos(Id_Evento)
);
GO
CREATE TABLE Certificados(
	Id_Certificado INT IDENTITY (1,1),
	Id_Inscripcion INT NOT NULL,
	FechaGeneracion DATETIME NOT NULL DEFAULT GETDATE(),
	UrlPDF NVARCHAR(300),
	CodigoValidacion NVARCHAR(100) NOT NULL,
	CONSTRAINT UQ_Codigo UNIQUE(CodigoValidacion),
	CONSTRAINT Pk_Certificados PRIMARY KEY(Id_Certificado),
	CONSTRAINT Fk_Certificados_Inscripciones
		FOREIGN KEY(Id_Inscripcion) REFERENCES Inscripciones(Id_Inscripcion)
);
GO