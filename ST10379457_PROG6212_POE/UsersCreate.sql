CREATE TABLE Users (
	UserID INT NOT NULL PRIMARY KEY IDENTITY,
	FullName VARCHAR(200),
	Password VARCHAR(200),
	Role VARCHAR(50),
	PhoneNumber VARCHAR(15),
	Email VARCHAR(300)
);


INSERT INTO Users (FullName, Password, Role, PhoneNumber, Email)
VALUES
('John Doe', 'admin123', 'IC', '0123456789', 'johndoe@gmail.com'),
('John May', 'admin123', 'PC', '9876543210', 'johnmay@gmail.com'),
('James May', 'admin123', 'AM', '5691023698', 'jamesmay@gmail.com');