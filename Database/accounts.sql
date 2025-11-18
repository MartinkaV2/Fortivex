-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Nov 17. 09:27
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
(1, 'testplayer', 'test123', 'test@fortivex.com', '2025-11-04 10:13:33.000000', '2025-11-14 10:13:33.000000'),
(2, 'adminuser', 'admin123', 'admin@fortivex.com', '2025-10-25 10:13:33.000000', '2025-11-14 10:13:33.000000'),
(4, 'GretaThunberg', 'HOWDAREYOU', 'mergesvagyokamuanyagdildora@gmail.com', '2025-11-14 11:14:30.560912', NULL),
(5, 'patrik', 'bigpatrik1', 'groszpatrik@gmail.com', '2025-11-14 12:29:26.310180', NULL),
(6, 'MartinkaaMedvecz', 'ADSIONGOESBRR', 'ElgazolomaRedaitegySionRrel@gmail.com', '2025-11-17 08:17:07.201320', NULL),
(7, 'hetvegeazmi', 'projektezzunk69', 'projektmunka@yahoo.mail', '2025-11-17 08:27:24.786343', NULL),
(8, 'KovacsBenceazUltimateKelcsy', 'projektezniminek', 'Nemvagyokfogykos@yahoo.mail.com', '2025-11-17 08:29:49.237979', NULL);

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
