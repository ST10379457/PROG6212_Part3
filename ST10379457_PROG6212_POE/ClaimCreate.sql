CREATE TABLE Claims (
	ClaimID INT NOT NULL PRIMARY KEY IDENTITY,
	UserID INT,
	ModuleName VARCHAR(100),
	ModuleCode VARCHAR(50),
	ClassGroup INT,
	Hours INT,
	ClaimAmount REAL,
	Status VARCHAR(50),
	DateOfSubmission DATE
);

INSERT INTO Claims (UserID, ModuleName, ModuleCode, ClassGroup, Hours, ClaimAmount, Status, DateOfSubmission)
VALUES
(1, 'Programming 2B', 'PROG6212', 1, 30, 15000, 'pending', '2024-10-18'),
(1, 'Programming 2B', 'PROG6212', 2, 32, 16000, 'pending', '2024-10-18');

