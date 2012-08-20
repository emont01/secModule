-- create your database and use this script to create the corresponding tables.
create table Users(
    id int not null auto_increment primary key,
    name varchar(30) not null,
    email varchar(255) not null,
    salt varchar(255) not null,
    `password` varchar(300) not null,
    created_at DATETIME not null,
    blocked BOOL not null
)

create table Roles(
    id int not null auto_increment primary key,
    name varchar(30) not null,
    description varchar(255)
)

create table Menus(
    id int not null auto_increment primary key,
    name varchar(30) not null,
    path varchar(255) not null
)

create table Menus_Roles(
    id int not null auto_increment primary key,
    menu_id int,
    role_id int
)

create table Users_Roles(
    id int not null auto_increment primary key,
    user_id int,
    role_id int
)