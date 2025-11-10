-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Nov 10. 09:29
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `fortivexdb`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `accounts`
--

CREATE TABLE `accounts` (
  `Id` int(11) NOT NULL,
  `Username` varchar(255) NOT NULL,
  `PasswordHash` longtext NOT NULL,
  `Email` longtext DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastLogin` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `accounts`
--

INSERT INTO `accounts` (`Id`, `Username`, `PasswordHash`, `Email`, `CreatedAt`, `LastLogin`) VALUES
(1, 'testplayer', 'test123', 'test@fortivex.com', '2025-10-27 20:06:44.567009', '2025-11-06 20:06:44.567009'),
(2, 'adminuser', 'admin123', 'admin@fortivex.com', '2025-10-17 20:06:44.567010', '2025-11-06 20:06:44.567010'),
(3, 'player2', 'pass123', 'player2@fortivex.com', '2025-11-01 20:06:44.567011', '2025-11-06 20:06:44.567011'),
(4, 'xyz123', 'teszt123', 'mittudomen@gmail.com', '2025-11-10 08:50:50.828631', NULL),
(5, 'gretathunberg', 'wokeism123', 'mergesvagyokamuanyagdildokra@gmail.com', '2025-11-10 08:54:06.290966', NULL),
(6, 'zsooombi', 'camilllesupport', 'dnd@gmail.com', '2025-11-10 09:10:14.556164', NULL);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Accounts_Username` (`Username`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `accounts`
--
ALTER TABLE `accounts`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
