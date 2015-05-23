-- phpMyAdmin SQL Dump
-- version 4.2.11
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: May 23, 2015 at 11:17 AM
-- Server version: 5.6.21
-- PHP Version: 5.6.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `petra_tracker_db`
--
CREATE DATABASE IF NOT EXISTS `petra_tracker_db` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `petra_tracker_db`;

DELIMITER $$
--
-- Procedures
--
DROP PROCEDURE IF EXISTS `change_password`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `change_password`(
IN in_user_id varchar(30),
IN in_new_password varchar(30)
)
BEGIN

update tbl_sys_users set
`Password` = AES_ENCRYPT(in_new_password,'p@ss2Petra')
where User_ID = in_user_id;

END$$

DROP PROCEDURE IF EXISTS `create_user`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `create_user`(
IN in_first_name varchar(30),
IN in_last_name varchar(30),
IN in_department varchar(50),
IN in_email varchar(50),
IN in_password varchar(30),
IN in_user_role varchar(50)
)
BEGIN


declare ini_user_id varchar(5);
declare ini_dept_id varchar(11);
declare ini_role_id varchar(11);

set ini_dept_id = (select id from tbl_departments where name = in_department);
set ini_role_id = (select id from tbl_roles where name = in_user_role);


insert into tbl_users
(
`first_Name`,
`last_Name`,
`department_id`,
`email`,
`password`,
`role_id`,
active
)
values
(
in_first_name,
in_last_name,
ini_dept_id,
in_email,
AES_ENCRYPT(in_password,'p@ss2Petra'),
ini_role_id,
'Active'
);



END$$

DROP PROCEDURE IF EXISTS `update_user`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_user`(
IN in_user_id varchar(10),
IN in_first_name varchar(30),
IN in_last_name varchar(30),
IN in_department varchar(50),
IN in_email varchar(50),
IN in_password varchar(30),
IN in_user_type varchar(50)
)
BEGIN



update tbl_sys_users set
`First_Name` = in_first_name,
`Last_Name` = in_last_name,
`Department` = in_department,
`Email` = in_email,
`User_Type` = in_user_type
 where `User_Id` = in_user_id;
 
 select 'OK';
 
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_departments`
--

DROP TABLE IF EXISTS `tbl_departments`;
CREATE TABLE IF NOT EXISTS `tbl_departments` (
`Id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `modified_by` varchar(255) DEFAULT NULL,
  `created_at` varchar(255) DEFAULT NULL,
  `updated_at` varchar(255) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_departments`
--

INSERT INTO `tbl_departments` (`Id`, `name`, `description`, `modified_by`, `created_at`, `updated_at`) VALUES
(1, 'I.T Department', 'I.T Department', NULL, NULL, NULL),
(2, 'Accounts Department', 'Accounts', NULL, '2015-05-21 13:55:16', NULL),
(3, 'Operations Department', 'Operations', NULL, '2015-05-22 09:21:10', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_roles`
--

DROP TABLE IF EXISTS `tbl_roles`;
CREATE TABLE IF NOT EXISTS `tbl_roles` (
`Id` int(11) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Modified_By` varchar(255) DEFAULT NULL,
  `Created_At` varchar(255) DEFAULT NULL,
  `Updated_At` varchar(255) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_roles`
--

INSERT INTO `tbl_roles` (`Id`, `Name`, `Description`, `Modified_By`, `Created_At`, `Updated_At`) VALUES
(1, 'Administrator', 'Admin', NULL, NULL, NULL),
(2, 'Super User', '', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

DROP TABLE IF EXISTS `tbl_users`;
CREATE TABLE IF NOT EXISTS `tbl_users` (
`Id` int(11) NOT NULL,
  `role_id` varchar(255) DEFAULT NULL,
  `department_id` varchar(100) NOT NULL DEFAULT '',
  `email` varchar(255) NOT NULL DEFAULT '',
  `password` varchar(50) NOT NULL DEFAULT '',
  `first_name` varchar(30) NOT NULL DEFAULT '',
  `last_name` varchar(30) NOT NULL DEFAULT '',
  `first_login` varchar(255) DEFAULT '',
  `active` varchar(255) DEFAULT NULL,
  `modified_by` varchar(225) DEFAULT NULL,
  `created_at` varchar(50) DEFAULT NULL,
  `updated_at` varchar(255) NOT NULL DEFAULT 'Active'
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`Id`, `role_id`, `department_id`, `email`, `password`, `first_name`, `last_name`, `first_login`, `active`, `modified_by`, `created_at`, `updated_at`) VALUES
(1, '2', '1', 'john@abc.com', 'ÊßI¥ä‰:%á7è¨R', 'john', 'doe', '', 'Active', NULL, NULL, 'Active'),
(2, '1', '1', 'jdoe@gmail.com', '§¬lÌ‹\n0Šôä[ó¤±', 'David', 'Doe', '', 'Active', NULL, NULL, 'Active'),
(3, '1', '1', 'niicoark27@gmail.com', '™›öè« KþÝYöÙ9}Ç', 'Nicholas', 'Arkaah', '', 'Active', NULL, NULL, 'Active');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_departments`
--
ALTER TABLE `tbl_departments`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `tbl_roles`
--
ALTER TABLE `tbl_roles`
 ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
 ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `Email` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_departments`
--
ALTER TABLE `tbl_departments`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `tbl_roles`
--
ALTER TABLE `tbl_roles`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
