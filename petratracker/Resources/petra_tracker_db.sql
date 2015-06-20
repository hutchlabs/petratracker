-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: petra_tracker_db
-- ------------------------------------------------------
-- Server version	5.6.21

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `petra_tracker_db`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `petra_tracker_db` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `petra_tracker_db`;

CREATE TABLE IF NOT EXISTS `notifications` (
  `id` int(11) NOT NULL,
  `to` varchar(255) NOT NULL,
  `notification_type` enum('savings_booster','subscription_approval') NOT NULL,
  `job_type` enum('subscription','transfer','redemption','schedule') NOT NULL,
  `job_id` int(11) NOT NULL,
  `status` enum('active','expired') NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



--
-- Table structure for table `payment_jobs`
--

DROP TABLE IF EXISTS `payment_jobs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `payment_jobs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Payment_ID` varchar(30) NOT NULL DEFAULT '',
  `Trans_Ref` varchar(50) NOT NULL DEFAULT '',
  `Trans_Details` text NOT NULL,
  `Statement_Amt` double NOT NULL DEFAULT '0',
  `Contribution_Date` date DEFAULT NULL,
  `Statement_Value_Date` date DEFAULT NULL,
  `Subscribtion_Amt` double DEFAULT NULL,
  `Subscribtion_Value_Date` date DEFAULT NULL,
  `Comments` text,
  `Company_Code` varchar(50) DEFAULT NULL,
  `Savings_Booster` varchar(1) DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL,
  `Created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `Owner` varchar(30) DEFAULT NULL,
  `Updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Updated_by` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_jobs`
--

LOCK TABLES `payment_jobs` WRITE;
/*!40000 ALTER TABLE `payment_jobs` DISABLE KEYS */;
/*!40000 ALTER TABLE `payment_jobs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_departments`
--

DROP TABLE IF EXISTS `tbl_departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbl_departments` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `modified_by` varchar(255) DEFAULT NULL,
  `created_at` varchar(255) DEFAULT NULL,
  `updated_at` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_departments`
--

LOCK TABLES `tbl_departments` WRITE;
/*!40000 ALTER TABLE `tbl_departments` DISABLE KEYS */;
INSERT INTO `tbl_departments` VALUES (1,'I.T Department','I.T Department',NULL,NULL,NULL),(2,'Accounts Department','Accounts',NULL,'2015-05-21 13:55:16',NULL),(3,'Operations Department','Operations',NULL,'2015-05-22 09:21:10',NULL);
/*!40000 ALTER TABLE `tbl_departments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_payments`
--

DROP TABLE IF EXISTS `tbl_payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbl_payments` (
  `Payment_ID` int(11) NOT NULL AUTO_INCREMENT,
  `Job_Type` varchar(30) DEFAULT NULL,
  `Owner` varchar(30) DEFAULT NULL,
  `Created_at` timestamp NULL DEFAULT NULL,
  `Modified_by` varchar(255) DEFAULT NULL,
  `Updated_at` timestamp NULL DEFAULT NULL,
  `Status` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Payment_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_payments`
--

LOCK TABLES `tbl_payments` WRITE;
/*!40000 ALTER TABLE `tbl_payments` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_roles`
--

DROP TABLE IF EXISTS `tbl_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbl_roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Modified_By` varchar(255) DEFAULT NULL,
  `Created_At` varchar(255) DEFAULT NULL,
  `Updated_At` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_roles`
--

LOCK TABLES `tbl_roles` WRITE;
/*!40000 ALTER TABLE `tbl_roles` DISABLE KEYS */;
INSERT INTO `tbl_roles` VALUES (1,'Administrator','Admin',NULL,NULL,NULL),(2,'Super User','',NULL,NULL,NULL);
/*!40000 ALTER TABLE `tbl_roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_users`
--

DROP TABLE IF EXISTS `tbl_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbl_users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
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
  `updated_at` varchar(255) NOT NULL DEFAULT 'Active',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_users`
--

LOCK TABLES `tbl_users` WRITE;
/*!40000 ALTER TABLE `tbl_users` DISABLE KEYS */;
INSERT INTO `tbl_users` VALUES (1,'2','1','john@abc.com','ÊßI¥ä‰:%á7è¨R','john','doe','','Active',NULL,NULL,'Active'),(2,'1','1','jdoe@gmail.com','§¬lÌ‹\n0Šôä[ó¤±','David','Doe','','Active',NULL,NULL,'Active'),(3,'1','1','niicoark27@gmail.com','™›öè« KþÝYöÙ9}Ç','Nicholas','Arkaah','','Active',NULL,NULL,'Active'),(5,'2','1','niicoark@yahoo.com','GÍW«œÝñ…?—9ŸÚm','John','Nketai','','Active',NULL,NULL,'Active'),(6, '2', '1', 'dhutchful@gmail.com', 'œô0¥q™2½ìU%©P', 'David', 'Hutchful', '', 'Active', NULL, NULL, 'Active');
/*!40000 ALTER TABLE `tbl_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary table structure for view `view_users`
--

DROP TABLE IF EXISTS `view_users`;
/*!50001 DROP VIEW IF EXISTS `view_users`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `view_users` (
  `ID` tinyint NOT NULL,
  `Name of User` tinyint NOT NULL,
  `User Role` tinyint NOT NULL,
  `Status` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Current Database: `bmfdb`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `bmfdb` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `bmfdb`;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accounts` (
  `AcctNo` varchar(30) NOT NULL,
  `AcctBal` double NOT NULL,
  `Principal` double NOT NULL,
  `iniInterest` double NOT NULL,
  `CurrentPrincipal` double NOT NULL,
  `Divisor` int(11) NOT NULL,
  `UPay` double NOT NULL,
  `Duration` varchar(30) NOT NULL,
  `LastTransDate` date NOT NULL,
  `AcctType` text NOT NULL,
  `OpCode` varchar(10) NOT NULL,
  `PackCode` varchar(10) NOT NULL,
  `DateOfAcct` date NOT NULL,
  PRIMARY KEY (`AcctNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES ('12201100,February2011',70,70,0,70,0,0,'','0000-00-00','Orinary','01','98','2011-02-04'),('122018910001',20,20,0,20,0,0,'','0000-00-00','Gow','01','89','2011-01-16'),('122019910001',20,20,0,20,0,0,'','0000-00-00','Joakes','01','99','2011-01-16'),('12202101,January2011',20,20,0,20,0,0,'','0000-00-00','Orinary','02','98','2011-01-16'),('122028910001',20,20,0,20,0,0,'','0000-00-00','Gow','02','89','2011-01-16'),('122029910001',20,20,0,20,0,0,'','0000-00-00','Joakes','02','99','2011-01-16');
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accountsdump`
--

DROP TABLE IF EXISTS `accountsdump`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accountsdump` (
  `AcctNo` varchar(30) NOT NULL,
  `AcctBal` double NOT NULL,
  `Principal` double NOT NULL,
  `iniInterest` double NOT NULL,
  `CurrentPrincipal` double NOT NULL,
  `Divisor` int(11) NOT NULL,
  `UPay` double NOT NULL,
  `Duration` varchar(30) NOT NULL,
  `LastTransDate` date NOT NULL,
  `AcctType` text NOT NULL,
  `OpCode` varchar(10) NOT NULL,
  `PackCode` varchar(10) NOT NULL,
  `DateOfAcct` date NOT NULL,
  PRIMARY KEY (`AcctNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accountsdump`
--

LOCK TABLES `accountsdump` WRITE;
/*!40000 ALTER TABLE `accountsdump` DISABLE KEYS */;
INSERT INTO `accountsdump` VALUES ('12201101,January2011',2,2,0,2,0,0,'','0000-00-00','Orinary','01','98','2011-01-16');
/*!40000 ALTER TABLE `accountsdump` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `acctinttrans`
--

DROP TABLE IF EXISTS `acctinttrans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `acctinttrans` (
  `AcctNo` varchar(20) NOT NULL,
  `PrincipalBF` double NOT NULL,
  `InterestRate` double NOT NULL,
  `Interest` double NOT NULL,
  `Type` varchar(15) NOT NULL,
  `OfficerID` varchar(30) NOT NULL,
  `DateOfTrans` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `acctinttrans`
--

LOCK TABLES `acctinttrans` WRITE;
/*!40000 ALTER TABLE `acctinttrans` DISABLE KEYS */;
INSERT INTO `acctinttrans` VALUES ('122018910001',0,0,0,'N/A','','2011-01-16'),('122028910001',0,0,0,'N/A','','2011-01-16'),('122029910001',0,0,0,'N/A','','2011-01-16'),('122019910001',0,0,0,'N/A','','2011-01-16'),('12201101,January2011',0,0,0,'N/A','','2011-01-16'),('12202101,January2011',0,0,0,'N/A','','2011-01-16'),('12201100,February201',0,0,0,'N/A','','2011-02-04');
/*!40000 ALTER TABLE `acctinttrans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accttrans`
--

DROP TABLE IF EXISTS `accttrans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accttrans` (
  `AcctNo` varchar(30) NOT NULL,
  `Transaction` varchar(15) NOT NULL,
  `Amount` double NOT NULL,
  `DeptMode` varchar(15) DEFAULT 'None',
  `Bankers` varchar(100) DEFAULT 'None',
  `ChequeNo` varchar(50) DEFAULT 'None',
  `OfficerID` varchar(30) NOT NULL,
  `DateOfTrans` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accttrans`
--

LOCK TABLES `accttrans` WRITE;
/*!40000 ALTER TABLE `accttrans` DISABLE KEYS */;
INSERT INTO `accttrans` VALUES ('122018910001','Deposite',20,'Cash','','','100923','2011-01-16'),('122028910001','Deposite',20,'Cash','','','100923','2011-01-16'),('122019910001','Deposite',20,'Cash','','','100923','2011-01-16'),('122029910001','Deposite',20,'Cash','','','100923','2011-01-16'),('12201101,January2011','Deposite',20,'Cash','','','100923','2011-01-16'),('12202101,January2011','Deposite',20,'Cash','','','100923','2011-01-16'),('12201101,January2011','Withdrawal',18,'','','','100923','2011-01-16'),('12201100,February2011','Deposite',70,'','','','100923','2011-02-04');
/*!40000 ALTER TABLE `accttrans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accttype`
--

DROP TABLE IF EXISTS `accttype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accttype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accttype`
--

LOCK TABLES `accttype` WRITE;
/*!40000 ALTER TABLE `accttype` DISABLE KEYS */;
/*!40000 ALTER TABLE `accttype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `autogen`
--

DROP TABLE IF EXISTS `autogen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `autogen` (
  `Linker` varchar(5) NOT NULL,
  `OfficeCode` varchar(3) NOT NULL,
  `CompanyName` varchar(50) NOT NULL,
  `Address` varchar(50) NOT NULL,
  `Phone` varchar(20) NOT NULL,
  `Fax` varchar(20) NOT NULL,
  `WebSite` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Logo` longblob NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `autogen`
--

LOCK TABLES `autogen` WRITE;
/*!40000 ALTER TABLE `autogen` DISABLE KEYS */;
INSERT INTO `autogen` VALUES ('','122','Unregistered Version','N/A','N/A','N/A','www.datapalgh.com','info@datapalgh.com','Online','');
/*!40000 ALTER TABLE `autogen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bankers`
--

DROP TABLE IF EXISTS `bankers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bankers` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Bankers` varchar(100) NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bankers`
--

LOCK TABLES `bankers` WRITE;
/*!40000 ALTER TABLE `bankers` DISABLE KEYS */;
/*!40000 ALTER TABLE `bankers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clients` (
  `ClientID` varchar(30) NOT NULL,
  `FirstName` varchar(30) NOT NULL,
  `LastName` varchar(30) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `DOB` varchar(10) NOT NULL,
  `Phone` varchar(30) NOT NULL,
  `Address` text NOT NULL,
  `Email` varchar(70) NOT NULL,
  `Occupation` varchar(50) NOT NULL,
  `Location` text NOT NULL,
  `MaritalStatus` varchar(10) NOT NULL,
  `Picture` varchar(100) NOT NULL,
  `BioSignature` varchar(100) NOT NULL,
  `NOKName` varchar(50) NOT NULL,
  `NOKPhone` varchar(30) NOT NULL,
  `NOKAddress` text NOT NULL,
  `NOKEmail` varchar(70) NOT NULL,
  `Relationship` varchar(50) NOT NULL,
  `Age` varchar(10) NOT NULL,
  `OfficerID` varchar(30) NOT NULL,
  `DateOfReg` varchar(10) NOT NULL,
  `AcctNo` varchar(30) NOT NULL,
  `Linker` varchar(5) NOT NULL,
  PRIMARY KEY (`AcctNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES ('12201100,February2011','htut','hooo','','2011-02-04','','','','','','','','','','','','','','','','2005','12201100,February2011',''),('122018910001','John','Homes','','2011-01-16','','','','','','','','','','','','','','','','1994','122018910001',''),('122019910001','Jakson','Samson','','2011-01-16','','','','','','','','','','','','','','','','1994','122019910001',''),('12202101,January2011','Homer','Jokkk','','2011-01-16','','','','','','','','','','','','','','','','1994','12202101,January2011',''),('122028910001','Kate','Komi','','2011-01-16','','','','','','','','','','','','','','','','1994','122028910001',''),('122029910001','Kumson','Kofi','','2011-01-16','','','','','','','','','','','','','','','','1994','122029910001','');
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clientsdump`
--

DROP TABLE IF EXISTS `clientsdump`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clientsdump` (
  `ClientID` varchar(30) NOT NULL,
  `FirstName` varchar(30) NOT NULL,
  `LastName` varchar(30) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `DOB` varchar(10) NOT NULL,
  `Phone` varchar(30) NOT NULL,
  `Address` text NOT NULL,
  `Email` varchar(70) NOT NULL,
  `Occupation` varchar(50) NOT NULL,
  `Location` text NOT NULL,
  `MaritalStatus` varchar(10) NOT NULL,
  `Picture` varchar(100) NOT NULL,
  `BioSignature` varchar(100) NOT NULL,
  `NOKName` varchar(50) NOT NULL,
  `NOKPhone` varchar(30) NOT NULL,
  `NOKAddress` text NOT NULL,
  `NOKEmail` varchar(70) NOT NULL,
  `Relationship` varchar(50) NOT NULL,
  `Age` varchar(10) NOT NULL,
  `OfficerID` varchar(30) NOT NULL,
  `DateOfReg` varchar(10) NOT NULL,
  `AcctNo` varchar(30) NOT NULL,
  `Linker` varchar(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientsdump`
--

LOCK TABLES `clientsdump` WRITE;
/*!40000 ALTER TABLE `clientsdump` DISABLE KEYS */;
INSERT INTO `clientsdump` VALUES ('12201101,January2011','Jokers','Hiosio','','2011-01-16','','','','','','','','','','','','','','','','1994','12201101,January2011','');
/*!40000 ALTER TABLE `clientsdump` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `codegen`
--

DROP TABLE IF EXISTS `codegen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `codegen` (
  `OfficeCode` varchar(5) NOT NULL,
  `OpCode` varchar(5) NOT NULL,
  `PackCode` varchar(5) NOT NULL,
  `IniSerial` int(11) NOT NULL,
  `IniDate` date NOT NULL,
  PRIMARY KEY (`OfficeCode`,`OpCode`,`PackCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `codegen`
--

LOCK TABLES `codegen` WRITE;
/*!40000 ALTER TABLE `codegen` DISABLE KEYS */;
INSERT INTO `codegen` VALUES ('122','01','89',10002,'2011-01-16'),('122','01','98',101,'2011-02-04'),('122','01','99',10002,'2011-01-16'),('122','02','89',10002,'2011-01-16'),('122','02','98',100,'2011-01-16'),('122','02','99',10002,'2011-01-16');
/*!40000 ALTER TABLE `codegen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `departments`
--

DROP TABLE IF EXISTS `departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `departments` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Head` varchar(100) NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departments`
--

LOCK TABLES `departments` WRITE;
/*!40000 ALTER TABLE `departments` DISABLE KEYS */;
/*!40000 ALTER TABLE `departments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `investments`
--

DROP TABLE IF EXISTS `investments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `investments` (
  `Code` varchar(10) NOT NULL,
  `PackName` varchar(50) NOT NULL,
  `PackType` varchar(20) NOT NULL,
  `Savings` int(1) NOT NULL,
  `Withdrawals` int(1) NOT NULL,
  `MininumBalance` double NOT NULL,
  `Interest` double NOT NULL,
  `AppMode` varchar(20) NOT NULL,
  `Computation` varchar(20) NOT NULL,
  `Description` text NOT NULL,
  `Date` date NOT NULL,
  `Serial` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `investments`
--

LOCK TABLES `investments` WRITE;
/*!40000 ALTER TABLE `investments` DISABLE KEYS */;
INSERT INTO `investments` VALUES ('89','Gow','Savings',1,1,0,0,'','Simple Interest','','2011-01-16',10003),('99','Joakes','Savings',1,1,0,0,'','Simple Interest','','2011-01-16',10003),('98','Orinary','Ordinary Susu',1,1,0,0,'','Simple Interest','','2011-01-16',10004);
/*!40000 ALTER TABLE `investments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `logs` (
  `ID` int(11) NOT NULL,
  `LogDate` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logs`
--

LOCK TABLES `logs` WRITE;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
INSERT INTO `logs` VALUES (1,'201102');
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modes`
--

DROP TABLE IF EXISTS `modes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `modes` (
  `ModeID` int(11) NOT NULL AUTO_INCREMENT,
  `ModeName` varchar(30) NOT NULL,
  `Number` int(11) NOT NULL,
  PRIMARY KEY (`ModeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modes`
--

LOCK TABLES `modes` WRITE;
/*!40000 ALTER TABLE `modes` DISABLE KEYS */;
/*!40000 ALTER TABLE `modes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `oac`
--

DROP TABLE IF EXISTS `oac`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `oac` (
  `Code` varchar(4) NOT NULL,
  `Location` text NOT NULL,
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oac`
--

LOCK TABLES `oac` WRITE;
/*!40000 ALTER TABLE `oac` DISABLE KEYS */;
INSERT INTO `oac` VALUES ('01','Nungua'),('02','Teshie');
/*!40000 ALTER TABLE `oac` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `staff`
--

DROP TABLE IF EXISTS `staff`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `staff` (
  `StaffID` varchar(30) NOT NULL,
  `FirstName` varchar(30) NOT NULL,
  `LastName` varchar(30) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `DOB` varchar(10) NOT NULL,
  `Phone` varchar(30) NOT NULL,
  `Address` text NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Location` text NOT NULL,
  `Department` varchar(50) NOT NULL,
  `Position` varchar(50) NOT NULL,
  `DateReg.` varchar(10) NOT NULL,
  PRIMARY KEY (`StaffID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `staff`
--

LOCK TABLES `staff` WRITE;
/*!40000 ALTER TABLE `staff` DISABLE KEYS */;
INSERT INTO `staff` VALUES ('10007','Kofi','Jackson','Male','2010-03-15','9908309','P. O. Box 12\r\nOsu - Accra\r\nGhana.','joe@yahoo.com','Jokers Inn','Insurance','Lober','2010-03-26'),('10011','Korma','Daniel','Male','1986-03-12','2332179902','Box 23\r\nOsu-Accra\r\nGhana.','niicoark@yahoo.com','Kaneshie','Investment','Clerk','2010-07-11'),('10012','Marfo','Hanna','Female','2010-07-16','201','','','','Micro Finance','Nothing','2010-07-16'),('10013','Rachel','Adams','Female','2010-07-16','','','','','Investment','Head','2010-07-16');
/*!40000 ALTER TABLE `staff` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `StaffID` varchar(30) NOT NULL,
  `FirstName` varchar(30) NOT NULL,
  `LastName` varchar(30) NOT NULL,
  `Username` varchar(30) NOT NULL,
  `Password` varchar(30) NOT NULL,
  `UserType` varchar(20) NOT NULL,
  `UserPriv` varchar(60) NOT NULL,
  `DateOfAcct.` varchar(10) NOT NULL,
  PRIMARY KEY (`StaffID`,`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES ('100923','Benard','Smith','Admin','','Administrator','11112','2010-01-01'),('110107236','Hanna','same','Admin','Admin','Cashier','00000','2011-01-07');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Current Database: `petra_tracker_db`
--

USE `petra_tracker_db`;

--
-- Final view structure for view `view_users`
--

/*!50001 DROP TABLE IF EXISTS `view_users`*/;
/*!50001 DROP VIEW IF EXISTS `view_users`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_users` AS select `tbl_users`.`Id` AS `ID`,concat(`tbl_users`.`first_name`,' ',`tbl_users`.`last_name`) AS `Name of User`,(select `tbl_roles`.`Name` from `tbl_roles` where (`tbl_users`.`role_id` = `tbl_roles`.`Id`)) AS `User Role`,`tbl_users`.`active` AS `Status` from `tbl_users` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Current Database: `bmfdb`
--

USE `bmfdb`;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-06-18 21:22:36
