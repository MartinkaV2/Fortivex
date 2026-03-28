Deployment
# Fortivex – Deployment Útmutató (Backend & Adatbázis)

Ez a dokumentáció a **Fortivex** ökoszisztéma backend rétegének és adatbázisának élesítési folyamatát részletezi a `MonsterASP.net` környezetbe történő telepítés során.

## 🏗 Előfeltételek és Környezet
A backend élesítéséhez olyan szolgáltatót választottunk, amely natív támogatást biztosít a .NET futtatókörnyezethez.
*   **Host:** MonsterASP.net
*   **Domain:** `fortivex.runasp.net`
*   **Adatbázis:** MySQL / MS SQL Server (MonsterASP által biztosított)
*   **SSL:** Let's Encrypt (Aktív: 2026.02.18-ig)

## 🚀 Élesítési Folyamat Lépésről Lépésre

### 1. Adatbázis Konfiguráció
Mielőtt a kódot publikálnánk, az adatbázis-környezetet elő kell készíteni:
*   Hozzon létre egy új adatbázist a MonsterASP vezérlőpultján.
*   **Fontos:** Az `appsettings.json` fájlban a `ConnectionStrings` részt át kell írni. A `Server=localhost` beállítást cserélje le a szolgáltató által megadott adatbázis-szerver domainjére és a hozzá tartozó hitelesítési adatokra.

### 2. Publikálási Profil Beállítása
A Visual Studio segítségével történő automatizált feltöltéshez (WebDeploy):
*   A MonsterASP felületén engedélyezze a **WebDeploy access** funkciót.
*   Töltse le a `.publishsettings` fájlt (Publish Profile).
*   Visual Studio-ban: Jobb klikk a projekten -> **Publish** -> **Import Profile** -> Jelölje ki a letöltött fájlt.

### 3. Biztonság és SSL
A biztonságos adatforgalom érdekében a szolgáltatói oldalon az alábbiakat állítottuk be:
*   **HTTPS:** Kötelező HTTPS átirányítás engedélyezése.
*   **Let's Encrypt:** SSL tanúsítvány generálása és aktiválása (lejárati idő ellenőrzése kritikus).

### 4. Publikálás (Deployment)
*   Ellenőrizze, hogy a `deploy.json` (vagy konfigurációs fájlok) a szerver URL-jére mutatnak, nem a fejlesztői `localhost`-ra.
*   Kattintson a **Publish** gombra a Visual Studio-ban. A sikeres feltöltés után az API elérhetővé válik a megadott subdomaint alatt.

## 🛠 Ismert hibák és megoldások (Troubleshooting)

### HTTP 500 – Belső szerverhiba
Amennyiben az API JSON lekérésekor (GET kérés) 500-as hiba lép fel, ellenőrizze az alábbiakat:
*   **CORS (Cross-Origin Resource Sharing):** Ellenőrizze a `Program.cs` fájlban, hogy a frontend domainje (pl. GitHub Pages vagy a weboldal címe) engedélyezve van-e. Ennek hiányában a böngésző blokkolni fogja a kéréseket.
*   **Adatbázis kapcsolat:** Ellenőrizze a jelszó és a szerver elérhetőségét.

### JSON Adatszerzés
Sikeres deployment után az adatok JSON formátumban érkeznek az API-tól. Ha a böngészőben közvetlenül megnyitja a végpontot (pl. `fortivex.runasp.net/api/accounts`), a válasznak láthatónak kell lennie.

---
*Utolsó frissítés: 2026. január 28. - Szalai & Csapat*
