-- Create Hotels table
CREATE TABLE Hotels (
    hotel_id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    contact VARCHAR(255) NOT NULL
    -- Add additional columns as needed
);

-- Create RoomTypes table
CREATE TABLE RoomTypes (
    room_type_id INT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(255),
    price DECIMAL(10, 2)
    -- Add additional columns as needed
);

-- Create Services table
CREATE TABLE Services (
    service_id INT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(255),
    price DECIMAL(10, 2)
    -- Add additional columns as needed
);

-- Create Rooms table
CREATE TABLE Rooms (
    room_id INT PRIMARY KEY,
    hotel_id INT NOT NULL,
    room_number VARCHAR(50) NOT NULL,
    room_type_id INT,
    availability_status VARCHAR(50) NOT NULL,
    price DECIMAL(10, 2),
    -- Add additional columns as needed

    FOREIGN KEY (hotel_id) REFERENCES Hotels(hotel_id),
    FOREIGN KEY (room_type_id) REFERENCES RoomTypes(room_type_id)
);

-- Create Guests table
CREATE TABLE Guests (
    guest_id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    contact VARCHAR(255) NOT NULL,
    nationality VARCHAR(50),
    special_requirements VARCHAR(255)
    -- Add additional columns as needed
);

-- Create Bookings table
CREATE TABLE Bookings (
    booking_id INT PRIMARY KEY,
    guest_id INT,
    room_id INT,
    check_in_date DATE,
    check_out_date DATE,
    payment_info VARCHAR(255),
    -- Add additional columns as needed

    FOREIGN KEY (guest_id) REFERENCES Guests(guest_id),
    FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
);

-- Create Invoices table
CREATE TABLE Invoices (
    invoice_id INT PRIMARY KEY,
    booking_id INT,
    amount DECIMAL(10, 2),
    payment_status VARCHAR(50)
    -- Add additional columns as needed

    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

-- Create Payments table
CREATE TABLE Payments (
    payment_id INT PRIMARY KEY,
    invoice_id INT,
    payment_date DATE,
    payment_method VARCHAR(50),
    amount DECIMAL(10, 2)
    -- Add additional columns as needed

    FOREIGN KEY (invoice_id) REFERENCES Invoices(invoice_id)
);

-- Create Reservations table
CREATE TABLE Reservations (
    reservation_id INT PRIMARY KEY,
    hotel_id INT,
    room_id INT,
    guest_id INT,
    check_in_date DATE,
    check_out_date DATE,
    payment_info VARCHAR(255),
    -- Add additional columns as needed

    FOREIGN KEY (hotel_id) REFERENCES Hotels(hotel_id),
    FOREIGN KEY (room_id) REFERENCES Rooms(room_id),
    FOREIGN KEY (guest_id) REFERENCES Guests(guest_id)
);
