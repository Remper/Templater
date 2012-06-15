CREATE DATABASE  IF NOT EXISTS `kurs` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `kurs`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: localhost    Database: kurs
-- ------------------------------------------------------
-- Server version	5.5.22

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
-- Table structure for table `templates`
--

DROP TABLE IF EXISTS `templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `templates` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `owner` int(11) NOT NULL,
  `website` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ownerid` (`owner`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `templates`
--

LOCK TABLES `templates` WRITE;
/*!40000 ALTER TABLE `templates` DISABLE KEYS */;
INSERT INTO `templates` VALUES (2,1,'http://habrahabr.ru','Habr'),(3,2,'http://www.yandex.ru/','Yandex'),(4,3,'http://expample.org','Test4'),(5,1,'http://ria.ru/','Ria');
/*!40000 ALTER TABLE `templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL DEFAULT 'zero',
  `workgroup` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'remper@me.com','testtest',1),(2,'mail@remper.ru','123123',2),(3,'test@test.ru','123123',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tasks`
--

DROP TABLE IF EXISTS `tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tasks` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `templateid` int(11) NOT NULL,
  `timestamp` varchar(45) NOT NULL,
  `depth` int(11) NOT NULL DEFAULT '0',
  `status` varchar(45) NOT NULL DEFAULT 'open',
  `results` int(11) NOT NULL DEFAULT '0',
  `progress` int(11) NOT NULL DEFAULT '0',
  `process` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tasks`
--

LOCK TABLES `tasks` WRITE;
/*!40000 ALTER TABLE `tasks` DISABLE KEYS */;
INSERT INTO `tasks` VALUES (1,5,'04.04.2012',1,'closed',18,100,5168),(2,3,'05.04.2012',2,'open',0,0,0),(3,2,'11.04.2012',0,'open',0,0,0),(4,3,'11.04.2012',0,'open',0,0,0);
/*!40000 ALTER TABLE `tasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workgroups`
--

DROP TABLE IF EXISTS `workgroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `workgroups` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(45) DEFAULT 'Default Workgroup',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workgroups`
--

LOCK TABLES `workgroups` WRITE;
/*!40000 ALTER TABLE `workgroups` DISABLE KEYS */;
INSERT INTO `workgroups` VALUES (1,'Default Team'),(2,'Team 17');
/*!40000 ALTER TABLE `workgroups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `results`
--

DROP TABLE IF EXISTS `results`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `results` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `taskid` int(11) NOT NULL,
  `status` varchar(45) NOT NULL DEFAULT 'new',
  `result` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=163 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `results`
--

LOCK TABLES `results` WRITE;
/*!40000 ALTER TABLE `results` DISABLE KEYS */;
INSERT INTO `results` VALUES (145,1,'new','{\ntime: \"19:28\",\ntitle: \"Число людей, страдающих слабоумием, к 2030 г возрастет вдвое - ВОЗ\"\n}'),(146,1,'new','{\ntime: \"19:19\",\ntitle: \"Эксперты ООН прибудут в Сирию для подготовки работы наблюдателей\"\n}'),(147,1,'new','{\ntime: \"19:16\",\ntitle: \"Правоохранители заявляют о давлении на суд по делу вора в законе\"\n}'),(148,1,'new','{\ntime: \"19:11\",\ntitle: \"Жизнь астраханских эсеров вне опасности, заявил Левичев\"\n}'),(149,1,'new','{\ntime: \"19:05\",\ntitle: \"Весна вышла из берегов: тысячи людей лишились жилья\"\n}'),(150,1,'new','{\ntime: \"19:05\",\ntitle: \"Электоральный рейтинг Путина с февраля вырос до 54%\"\n}'),(151,1,'new','{\ntime: \"19:00\",\ntitle: \"Число подтопленных домов в Саратовской области возросло до 184\"\n}'),(152,1,'new','{\ntime: \"18:55\",\ntitle: \"МВД РФ получило часть материалов дела Дерипаски и Махмудова\"\n}'),(153,1,'new','{\ntime: \"18:54\",\ntitle: \"РФ обнулит экспортные пошлины для новых шельфовых проектов\"\n}'),(154,1,'new','{\ntime: \"18:44\",\ntitle: \"Медведев внес кандидатуру Ковтун на должность главы Мурманской области\"\n}'),(155,1,'new','{\ntime: \"18:41\",\ntitle: \"Пострадавшие в Хибинах застрахованы в \"Альянсе\" на $30 тыс каждый\"\n}'),(156,1,'new','{\ntime: \"18:34\",\ntitle: \"Одиночные пикеты в поддержку голодающего эсера Шеина прошли в Воронеже\"\n}'),(157,1,'new','{\ntime: \"18:33\",\ntitle: \"Госдума не может обратиться в международный суд по делу Бута - депутат\"\n}'),(158,1,'new','{\ntime: \"18:21\",\ntitle: \"Британская писательница Джоан Роулинг раскрыла детали нового романа\"\n}'),(159,1,'new','{\ntime: \"18:14\",\ntitle: \"Самые дорогие автопарки в кабмине - у Хлопонина, Шувалова и Трутнева\"\n}'),(160,1,'new','{\ntime: \"18:02\",\ntitle: \"\"Гладиаторы\" протестовали против запрета их работы, \"захватив\" Колизей\"\n}'),(161,1,'new','{\ntime: \"17:57\",\ntitle: \"Глава МИД РФ посетил первый открывшийся в США российский визовый центр\"\n}'),(162,1,'new','{\ntime: \"17:56\",\ntitle: \"МВД Сирии обещает амнистию боевикам, не причастным к убийствам\"\n}');
/*!40000 ALTER TABLE `results` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-06-15 20:35:59
