

Table creation and data insertion queries:

CREATE DATABASE JobsDb;

CREATE TABLE Departments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL
);


CREATE TABLE Locations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    City NVARCHAR(100),
    State NVARCHAR(100),
    Country NVARCHAR(100),
    Zip INT
);
CREATE TABLE Jobs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(20) NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(max),
    PostedDate DATETIME NOT NULL,
    ClosingDate DATETIME NOT NULL,
    LocationId INT NOT NULL,
    DepartmentId INT NOT NULL,
    FOREIGN KEY (LocationId) REFERENCES Locations(Id),
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);


CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    USERROLE NVARCHAR(50) DEFAULT 'User',
    CREATEDATE DATETIME NOT NULL DEFAULT GETDATE()
);


INSERT INTO Departments (Title) VALUES
('Software Development'),
('Project Management'),
('Human Resources'),
('Marketing'),
('Finance');

INSERT INTO Locations (Title, City, State, Country, Zip) VALUES
('US Head Office', 'Baltimore', 'MD', 'United States', 21202),
('India Office', 'Panaji', 'Goa', 'India', 403001),
('UK Office', 'London', 'Greater London', 'United Kingdom', 12345),
('Canada Branch', 'Toronto', 'Ontario', 'Canada', 54321);

INSERT INTO Jobs (Code, Title, Description, PostedDate, ClosingDate, LocationId, DepartmentId) VALUES
('JOB-001', 'Software Developer', 'Responsible for writing and maintaining code.', GETDATE(), DATEADD(DAY, 30, GETDATE()), 2, 1),
('JOB-002', 'Project Manager', 'Oversees project planning and delivery.', GETDATE(), DATEADD(DAY, 20, GETDATE()), 1, 2),
('JOB-003', 'HR Executive', 'Handles employee relations and recruitment.', GETDATE(), DATEADD(DAY, 15, GETDATE()), 3, 3),
('JOB-004', 'Marketing Specialist', 'Creates marketing strategies.', GETDATE(), DATEADD(DAY, 25, GETDATE()), 4, 4);

-- ============================

User creation -

INSERT INTO Users
    (Username,PasswordHash, UserRole, CreateDate)
VALUES
    ('admin123',
     'AQAAAAIAAYagAAAAEFotfEwvQeg1Ms0IgZgLXe5x2X2NftLIj0Ab1GZtbUWfMkFtpjYoAQ2bzHYwwPTbHA==',
     'Admin',
     GETDATE()),
	    ('user',
     'AQAAAAIAAYagAAAAEFEFl2jrsgFRl5P1N8UK1sqWIgM+Aysn8Jy3xkChKHfDrpLP0hbP8lIHyN61j7MCXw==',
     'User',
     GETDATE());

credentials: [admin123,admin] ,[user,user]


swagger ui url:https://localhost:7194/swagger/index.html


please feel free to contact at 8668387623/prateeksatardekar@gmail.com for any clarifications