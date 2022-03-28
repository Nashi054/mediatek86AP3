-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Mar 28, 2022 at 04:01 PM
-- Server version: 5.7.31
-- PHP Version: 7.4.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: mediatek86
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `finAbonnement` ()  BEGIN
	SELECT d.titre, a.dateFinAbonnement
    	FROM document d JOIN revue r ON d.id=r.id
    	JOIN abonnement a ON r.id=a.idRevue
    	JOIN commande c ON a.id=c.id
    	WHERE DATE_SUB(a.dateFinAbonnement, INTERVAL 30 DAY) < NOW()
        ORDER BY a.dateFinAbonnement ASC ;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table abonnement
--

CREATE TABLE abonnement (
  id varchar(5) NOT NULL,
  dateFinAbonnement date DEFAULT NULL,
  idRevue varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table abonnement
--

INSERT INTO abonnement (id, dateFinAbonnement, idRevue) VALUES
('00003', '2022-03-31', '10011');

--
-- Triggers abonnement
--
DELIMITER $$
CREATE TRIGGER `insAbonnement` BEFORE INSERT ON `abonnement` FOR EACH ROW BEGIN
	IF ((NEW.idRevue < 10000) AND (NEW.idRevue >= 20000)) THEN
    	SIGNAL SQLSTATE "45000"
			SET MESSAGE_TEXT = "impossible d'insérer un commandedocument dans abonnement" ;
    END IF ;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table commande
--

CREATE TABLE commande (
  id varchar(5) NOT NULL,
  dateCommande date DEFAULT NULL,
  montant double DEFAULT NULL,
  idSuivi char(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table commande
--

INSERT INTO commande (id, dateCommande, montant, idSuivi) VALUES
('00001', '2022-03-25', 15, '3'),
('00002', '2022-03-25', 15, '2'),
('00003', '2022-03-22', 10, NULL),
('00004', '2022-03-27', 10, '2'),
('00005', '2022-03-25', 10, '2'),
('00006', '2022-03-25', 20, '3'),
('00007', '2022-03-25', 15, '1');

--
-- Triggers commande
--
DELIMITER $$
CREATE TRIGGER `commandeLivreDvdLivree` AFTER UPDATE ON `commande` FOR EACH ROW BEGIN
        DECLARE v_idLivreDvd VARCHAR(10) ;
        DECLARE dernierNumero INT(11) ;
        DECLARE v_dateAchat DATE ;
        DECLARE v_photo VARCHAR(100) ;
        DECLARE v_nbExemplaire INT(11) ;

        SELECT idLivreDvd INTO v_idLivreDvd
            FROM commandedocument
            WHERE id = NEW.id ;

        SELECT dateCommande INTO v_dateAchat
            FROM commande
            WHERE id = NEW.id ;

        SELECT image INTO v_photo
            FROM document
            WHERE id = NEW.id ;
            
        SELECT nbExemplaire INTO v_nbExemplaire
            FROM commandedocument
            WHERE id = NEW.id ;

	WHILE (v_nbExemplaire > 0) DO
    	SELECT numero INTO dernierNumero
            FROM exemplaire
            WHERE id = v_idLivreDvd
            ORDER BY numero DESC
            LIMIT 1 ;
            
        IF (NEW.idSuivi = 2) THEN
            IF (dernierNumero IS NULL) THEN
            	IF ((v_idLivreDvd >= 00000) AND (v_idLivreDvd < 10000)) THEN
                	INSERT INTO exemplaire VALUES (v_idLivreDvd, 00001, v_dateAchat, v_photo, "00001") ;
                ELSEIF (v_idLivreDvd >= 20000) THEN
                	INSERT INTO exemplaire VALUES (v_idLivreDvd, 20001, v_dateAchat, v_photo, "00001") ;
                END IF ;
            ELSE
            	SET dernierNumero = dernierNumero + 1 ;
                INSERT INTO exemplaire VALUES (v_idLivreDvd, dernierNumero, v_dateAchat, v_photo, "00001") ;
            END IF ;
        END IF ;
        SET v_nbExemplaire = v_nbExemplaire - 1 ;
       END WHILE ;
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table commandedocument
--

CREATE TABLE commandedocument (
  id varchar(5) NOT NULL,
  nbExemplaire int(11) DEFAULT NULL,
  idLivreDvd varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table commandedocument
--

INSERT INTO commandedocument (id, nbExemplaire, idLivreDvd) VALUES
('00001', 2, '00017'),
('00002', 3, '20003'),
('00004', 1, '00017'),
('00005', 2, '00007'),
('00006', 2, '20002'),
('00007', 1, '00007');

--
-- Triggers commandedocument
--
DELIMITER $$
CREATE TRIGGER `insCommandeDocument` BEFORE INSERT ON `commandedocument` FOR EACH ROW BEGIN
	IF ((NEW.idLivreDvd >= 10000) AND (NEW.idLivreDvd < 20000)) THEN
    	SIGNAL SQLSTATE "45000"
			SET MESSAGE_TEXT = "impossible d'insérer un abonnement dans commandedocument" ;
    END IF ;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table document
--

CREATE TABLE document (
  id varchar(10) NOT NULL,
  titre varchar(60) DEFAULT NULL,
  image varchar(100) DEFAULT NULL,
  idRayon varchar(5) NOT NULL,
  idPublic varchar(5) NOT NULL,
  idGenre varchar(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table document
--

INSERT INTO document (id, titre, image, idRayon, idPublic, idGenre) VALUES
('00001', 'Quand sort la recluse', '', 'LV003', '00002', '10014'),
('00002', 'Un pays à l\'aube', '', 'LV001', '00002', '10004'),
('00003', 'Et je danse aussi', '', 'LV002', '00003', '10013'),
('00004', 'L\'armée furieuse', '', 'LV003', '00002', '10014'),
('00005', 'Les anonymes', '', 'LV001', '00002', '10014'),
('00006', 'La marque jaune', '', 'BD001', '00003', '10001'),
('00007', 'Dans les coulisses du musée', '', 'LV001', '00003', '10006'),
('00008', 'Histoire du juif errant', '', 'LV002', '00002', '10006'),
('00009', 'Pars vite et reviens tard', '', 'LV003', '00002', '10014'),
('00010', 'Le vestibule des causes perdues', '', 'LV001', '00002', '10006'),
('00011', 'L\'île des oubliés', '', 'LV002', '00003', '10006'),
('00012', 'La souris bleue', '', 'LV002', '00003', '10006'),
('00013', 'Sacré Pêre Noël', '', 'JN001', '00001', '10001'),
('00014', 'Mauvaise étoile', '', 'LV003', '00003', '10014'),
('00015', 'La confrérie des téméraires', '', 'JN002', '00004', '10014'),
('00016', 'Le butin du requin', '', 'JN002', '00004', '10014'),
('00017', 'Catastrophes au Brésil', '', 'JN002', '00004', '10014'),
('00018', 'Le Routard - Maroc', '', 'DV005', '00003', '10011'),
('00019', 'Guide Vert - Iles Canaries', '', 'DV005', '00003', '10011'),
('00020', 'Guide Vert - Irlande', '', 'DV005', '00003', '10011'),
('00021', 'Les déferlantes', '', 'LV002', '00002', '10006'),
('00022', 'Une part de Ciel', '', 'LV002', '00002', '10006'),
('00023', 'Le secret du janissaire', '', 'BD001', '00002', '10001'),
('00024', 'Pavillon noir', '', 'BD001', '00002', '10001'),
('00025', 'L\'archipel du danger', '', 'BD001', '00002', '10001'),
('00026', 'La planète des singes', '', 'LV002', '00003', '10002'),
('10001', 'Arts Magazine', '', 'PR002', '00002', '10016'),
('10002', 'Alternatives Economiques', '', 'PR002', '00002', '10015'),
('10003', 'Challenges', '', 'PR002', '00002', '10015'),
('10004', 'Rock and Folk', '', 'PR002', '00002', '10016'),
('10005', 'Les Echos', '', 'PR001', '00002', '10015'),
('10006', 'Le Monde', '', 'PR001', '00002', '10018'),
('10007', 'Telerama', '', 'PR002', '00002', '10016'),
('10008', 'L\'Obs', '', 'PR002', '00002', '10018'),
('10009', 'L\'Equipe', '', 'PR001', '00002', '10017'),
('10010', 'L\'Equipe Magazine', '', 'PR002', '00002', '10017'),
('10011', 'Geo', '', 'PR002', '00003', '10016'),
('20001', 'Star Wars 5 L\'empire contre attaque', '', 'DF001', '00003', '10002'),
('20002', 'Le seigneur des anneaux : la communauté de l\'anneau', '', 'DF001', '00003', '10019'),
('20003', 'Jurassic Park', '', 'DF001', '00003', '10002'),
('20004', 'Matrix', '', 'DF001', '00003', '10002');

-- --------------------------------------------------------

--
-- Table structure for table dvd
--

CREATE TABLE dvd (
  id varchar(10) NOT NULL,
  synopsis text,
  realisateur varchar(20) DEFAULT NULL,
  duree int(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table dvd
--

INSERT INTO dvd (id, synopsis, realisateur, duree) VALUES
('20001', 'Luc est entraîné par Yoda pendant que Han et Leia tentent de se cacher dans la cité des nuages.', 'George Lucas', 124),
('20002', 'L\'anneau unique, forgé par Sauron, est porté par Fraudon qui l\'amène à Foncombe. De là, des représentants de peuples différents vont s\'unir pour aider Fraudon à amener l\'anneau à la montagne du Destin.', 'Peter Jackson', 228),
('20003', 'Un milliardaire et des généticiens créent des dinosaures à partir de clonage.', 'Steven Spielberg', 128),
('20004', 'Un informaticien réalise que le monde dans lequel il vit est une simulation gérée par des machines.', 'Les Wachowski', 136);

-- --------------------------------------------------------

--
-- Table structure for table etat
--

CREATE TABLE etat (
  id char(5) NOT NULL,
  libelle varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table etat
--

INSERT INTO etat (id, libelle) VALUES
('00001', 'neuf'),
('00002', 'usagé'),
('00003', 'détérioré'),
('00004', 'inutilisable');

-- --------------------------------------------------------

--
-- Table structure for table exemplaire
--

CREATE TABLE exemplaire (
  id varchar(10) NOT NULL,
  numero int(11) NOT NULL,
  dateAchat date DEFAULT NULL,
  photo varchar(100) NOT NULL,
  idEtat char(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table exemplaire
--

INSERT INTO exemplaire (id, numero, dateAchat, photo, idEtat) VALUES
('00007', 1, '2022-03-25', '', '00001'),
('00007', 2, '2022-03-25', '', '00001'),
('00017', 1, '2022-03-25', '', '00001'),
('00017', 2, '2022-03-25', '', '00001'),
('00017', 3, '2022-03-27', '', '00001'),
('10002', 418, '2021-12-01', '', '00001'),
('10007', 3237, '2021-11-23', '', '00001'),
('10007', 3238, '2021-11-30', '', '00001'),
('10007', 3239, '2021-12-07', '', '00001'),
('10007', 3240, '2021-12-21', '', '00001'),
('10011', 506, '2021-04-01', '', '00001'),
('10011', 507, '2021-05-03', '', '00001'),
('10011', 508, '2021-06-05', '', '00001'),
('10011', 509, '2021-07-01', '', '00001'),
('10011', 510, '2021-08-04', '', '00001'),
('10011', 511, '2021-09-01', '', '00001'),
('10011', 512, '2021-10-06', '', '00001'),
('10011', 513, '2021-11-01', '', '00001'),
('10011', 514, '2021-12-01', '', '00001'),
('10011', 515, '2022-03-23', '', '00001'),
('20002', 20001, '2022-03-25', '', '00001'),
('20002', 20002, '2022-03-25', '', '00001'),
('20003', 20001, '2022-03-25', '', '00001'),
('20003', 20002, '2022-03-25', '', '00001'),
('20003', 20003, '2022-03-25', '', '00001');

-- --------------------------------------------------------

--
-- Table structure for table genre
--

CREATE TABLE genre (
  id varchar(5) NOT NULL,
  libelle varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table genre
--

INSERT INTO genre (id, libelle) VALUES
('10000', 'Humour'),
('10001', 'Bande dessinée'),
('10002', 'Science Fiction'),
('10003', 'Biographie'),
('10004', 'Historique'),
('10006', 'Roman'),
('10007', 'Aventures'),
('10008', 'Essai'),
('10009', 'Documentaire'),
('10010', 'Technique'),
('10011', 'Voyages'),
('10012', 'Drame'),
('10013', 'Comédie'),
('10014', 'Policier'),
('10015', 'Presse Economique'),
('10016', 'Presse Culturelle'),
('10017', 'Presse sportive'),
('10018', 'Actualités'),
('10019', 'Fantazy');

-- --------------------------------------------------------

--
-- Table structure for table livre
--

CREATE TABLE livre (
  id varchar(10) NOT NULL,
  ISBN varchar(13) DEFAULT NULL,
  auteur varchar(20) DEFAULT NULL,
  collection varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table livre
--

INSERT INTO livre (id, ISBN, auteur, collection) VALUES
('00001', '1234569877896', 'Fred Vargas', 'Commissaire Adamsberg'),
('00002', '1236547896541', 'Dennis Lehanne', ''),
('00003', '6541236987410', 'Anne-Laure Bondoux', ''),
('00004', '3214569874123', 'Fred Vargas', 'Commissaire Adamsberg'),
('00005', '3214563214563', 'RJ Ellory', ''),
('00006', '3213213211232', 'Edgar P. Jacobs', 'Blake et Mortimer'),
('00007', '6541236987541', 'Kate Atkinson', ''),
('00008', '1236987456321', 'Jean d\'Ormesson', ''),
('00009', '3,21457E+12', 'Fred Vargas', 'Commissaire Adamsberg'),
('00010', '3,21457E+12', 'Manon Moreau', ''),
('00011', '3,21457E+12', 'Victoria Hislop', ''),
('00012', '3,21457E+12', 'Kate Atkinson', ''),
('00013', '3,21457E+12', 'Raymond Briggs', ''),
('00014', '3,21457E+12', 'RJ Ellory', ''),
('00015', '3,21457E+12', 'Floriane Turmeau', ''),
('00016', '3,21457E+12', 'Julian Press', ''),
('00017', '3,21457E+12', 'Philippe Masson', ''),
('00018', '3,21457E+12', '', 'Guide du Routard'),
('00019', '3,21457E+12', '', 'Guide Vert'),
('00020', '3,21457E+12', '', 'Guide Vert'),
('00021', '3,21457E+12', 'Claudie Gallay', ''),
('00022', '3,21457E+12', 'Claudie Gallay', ''),
('00023', '3,21457E+12', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00024', '3,21457E+12', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00025', '3,21457E+12', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00026', '', 'Pierre Boulle', 'Julliard');

-- --------------------------------------------------------

--
-- Table structure for table livres_dvd
--

CREATE TABLE livres_dvd (
  id varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table livres_dvd
--

INSERT INTO livres_dvd (id) VALUES
('00001'),
('00002'),
('00003'),
('00004'),
('00005'),
('00006'),
('00007'),
('00008'),
('00009'),
('00010'),
('00011'),
('00012'),
('00013'),
('00014'),
('00015'),
('00016'),
('00017'),
('00018'),
('00019'),
('00020'),
('00021'),
('00022'),
('00023'),
('00024'),
('00025'),
('00026'),
('20001'),
('20002'),
('20003'),
('20004');

-- --------------------------------------------------------

--
-- Table structure for table public
--

CREATE TABLE public (
  id varchar(5) NOT NULL,
  libelle varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table public
--

INSERT INTO public (id, libelle) VALUES
('00001', 'Jeunesse'),
('00002', 'Adultes'),
('00003', 'Tous publics'),
('00004', 'Ados');

-- --------------------------------------------------------

--
-- Table structure for table rayon
--

CREATE TABLE rayon (
  id char(5) NOT NULL,
  libelle varchar(30) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table rayon
--

INSERT INTO rayon (id, libelle) VALUES
('BD001', 'BD Adultes'),
('BL001', 'Beaux Livres'),
('DF001', 'DVD films'),
('DV001', 'Sciences'),
('DV002', 'Maison'),
('DV003', 'Santé'),
('DV004', 'Littérature classique'),
('DV005', 'Voyages'),
('JN001', 'Jeunesse BD'),
('JN002', 'Jeunesse romans'),
('LV001', 'Littérature étrangère'),
('LV002', 'Littérature française'),
('LV003', 'Policiers français étrangers'),
('PR001', 'Presse quotidienne'),
('PR002', 'Magazines');

-- --------------------------------------------------------

--
-- Table structure for table revue
--

CREATE TABLE revue (
  id varchar(10) NOT NULL,
  empruntable tinyint(1) DEFAULT NULL,
  periodicite varchar(2) DEFAULT NULL,
  delaiMiseADispo int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table revue
--

INSERT INTO revue (id, empruntable, periodicite, delaiMiseADispo) VALUES
('10001', 1, 'MS', 52),
('10002', 1, 'MS', 52),
('10003', 1, 'HB', 15),
('10004', 1, 'HB', 15),
('10005', 0, 'QT', 5),
('10006', 0, 'QT', 5),
('10007', 1, 'HB', 26),
('10008', 1, 'HB', 26),
('10009', 0, 'QT', 5),
('10010', 1, 'HB', 12),
('10011', 1, 'MS', 52);

-- --------------------------------------------------------

--
-- Table structure for table service
--

CREATE TABLE service (
  id int(11) NOT NULL,
  libelle varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table service
--

INSERT INTO service (id, libelle) VALUES
(1, 'administratif'),
(2, 'prêts'),
(3, 'culture'),
(0, 'administrateur');

-- --------------------------------------------------------

--
-- Table structure for table suivi
--

CREATE TABLE suivi (
  id char(5) COLLATE utf8_unicode_ci NOT NULL,
  libelle varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table suivi
--

INSERT INTO suivi (id, libelle) VALUES
('1', 'en cours'),
('2', 'livrée'),
('3', 'réglée'),
('4', 'relancée');

-- --------------------------------------------------------

--
-- Table structure for table utilisateur
--

CREATE TABLE utilisateur (
  id varchar(5) COLLATE utf8_unicode_ci NOT NULL,
  login varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  pwd varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  idService varchar(5) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table utilisateur
--

INSERT INTO utilisateur (id, login, pwd, idService) VALUES
('00001', 'JRoger', 'JRogerpwd', '0'),
('00002', 'EDupont', 'EDupontpwd', '1'),
('00003', 'ABoulanger', 'ABoulangerpwd', '2'),
('00004', 'FGregoire', 'FGregoirepwd', '3');

--
-- Indexes for dumped tables
--

--
-- Indexes for table abonnement
--
ALTER TABLE abonnement
  ADD PRIMARY KEY (id),
  ADD KEY idRevue (idRevue);

--
-- Indexes for table commande
--
ALTER TABLE commande
  ADD PRIMARY KEY (id),
  ADD KEY idSuivi (idSuivi) USING BTREE;

--
-- Indexes for table commandedocument
--
ALTER TABLE commandedocument
  ADD PRIMARY KEY (id),
  ADD KEY idLivreDvd (idLivreDvd);

--
-- Indexes for table document
--
ALTER TABLE document
  ADD PRIMARY KEY (id),
  ADD KEY idRayon (idRayon),
  ADD KEY idPublic (idPublic),
  ADD KEY idGenre (idGenre);

--
-- Indexes for table dvd
--
ALTER TABLE dvd
  ADD PRIMARY KEY (id);

--
-- Indexes for table etat
--
ALTER TABLE etat
  ADD PRIMARY KEY (id);

--
-- Indexes for table exemplaire
--
ALTER TABLE exemplaire
  ADD PRIMARY KEY (id,numero),
  ADD KEY idEtat (idEtat);

--
-- Indexes for table genre
--
ALTER TABLE genre
  ADD PRIMARY KEY (id);

--
-- Indexes for table livre
--
ALTER TABLE livre
  ADD PRIMARY KEY (id);

--
-- Indexes for table livres_dvd
--
ALTER TABLE livres_dvd
  ADD PRIMARY KEY (id);

--
-- Indexes for table public
--
ALTER TABLE public
  ADD PRIMARY KEY (id);

--
-- Indexes for table rayon
--
ALTER TABLE rayon
  ADD PRIMARY KEY (id);

--
-- Indexes for table revue
--
ALTER TABLE revue
  ADD PRIMARY KEY (id);

--
-- Indexes for table service
--
ALTER TABLE service
  ADD PRIMARY KEY (id);

--
-- Indexes for table suivi
--
ALTER TABLE suivi
  ADD PRIMARY KEY (id);

--
-- Indexes for table utilisateur
--
ALTER TABLE utilisateur
  ADD PRIMARY KEY (id),
  ADD KEY idService (idService);

--
-- Constraints for dumped tables
--

--
-- Constraints for table abonnement
--
ALTER TABLE abonnement
  ADD CONSTRAINT abonnement_ibfk_1 FOREIGN KEY (id) REFERENCES commande (id),
  ADD CONSTRAINT abonnement_ibfk_2 FOREIGN KEY (idRevue) REFERENCES revue (id);

--
-- Constraints for table commandedocument
--
ALTER TABLE commandedocument
  ADD CONSTRAINT commandedocument_ibfk_1 FOREIGN KEY (id) REFERENCES commande (id),
  ADD CONSTRAINT commandedocument_ibfk_2 FOREIGN KEY (idLivreDvd) REFERENCES livres_dvd (id);

--
-- Constraints for table document
--
ALTER TABLE document
  ADD CONSTRAINT document_ibfk_1 FOREIGN KEY (idRayon) REFERENCES rayon (id),
  ADD CONSTRAINT document_ibfk_2 FOREIGN KEY (idPublic) REFERENCES public (id),
  ADD CONSTRAINT document_ibfk_3 FOREIGN KEY (idGenre) REFERENCES genre (id);

--
-- Constraints for table dvd
--
ALTER TABLE dvd
  ADD CONSTRAINT dvd_ibfk_1 FOREIGN KEY (id) REFERENCES livres_dvd (id);

--
-- Constraints for table exemplaire
--
ALTER TABLE exemplaire
  ADD CONSTRAINT exemplaire_ibfk_1 FOREIGN KEY (id) REFERENCES document (id),
  ADD CONSTRAINT exemplaire_ibfk_2 FOREIGN KEY (idEtat) REFERENCES etat (id);

--
-- Constraints for table livre
--
ALTER TABLE livre
  ADD CONSTRAINT livre_ibfk_1 FOREIGN KEY (id) REFERENCES livres_dvd (id);

--
-- Constraints for table livres_dvd
--
ALTER TABLE livres_dvd
  ADD CONSTRAINT livres_dvd_ibfk_1 FOREIGN KEY (id) REFERENCES document (id);

--
-- Constraints for table revue
--
ALTER TABLE revue
  ADD CONSTRAINT revue_ibfk_1 FOREIGN KEY (id) REFERENCES document (id);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
