-- create your database and use this script to create the corresponding tables.
CREATE TABLE Users (
    id INT NOT NULL auto_increment PRIMARY KEY
    ,`name` VARCHAR(30) NOT NULL
    ,email VARCHAR(255) NOT NULL
    ,salt VARCHAR(255) NOT NULL
    ,`password` VARCHAR(300) NOT NULL
    ,created_at DATETIME NOT NULL
    ,blocked BOOL NOT NULL
    )

CREATE TABLE Roles (
    id INT NOT NULL auto_increment PRIMARY KEY
    ,`name` VARCHAR(30) NOT NULL
    ,description VARCHAR(255)
    )

CREATE TABLE Menus (
    id INT NOT NULL auto_increment PRIMARY KEY
    ,`name` VARCHAR(30) NOT NULL
    ,`path` VARCHAR(255) NOT NULL
    )

CREATE TABLE Menus_Roles (
    id INT NOT NULL auto_increment PRIMARY KEY
    ,menu_id INT
    ,role_id INT
    )

CREATE TABLE Users_Roles (
    id INT NOT NULL auto_increment PRIMARY KEY
    ,user_id INT
    ,role_id INT
    )
