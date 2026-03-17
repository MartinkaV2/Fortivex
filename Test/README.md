FortivexAPI Tesztelési Dokumentáció
Ez a mappa tartalmazza a FortivexAPI backend szolgáltatásaihoz készült automatizált teszteket. A projekt három különböző tesztelési megközelítést alkalmaz a maximális megbízhatóság érdekében.

Alkalmazott Tesztkeretrendszerek
xUnit: A kontroller logika és az üzleti folyamatok egységtesztelésére (Unit Test) szolgál.

MSTest: A Microsoft hivatalos tesztkeretrendszere, amely kiegészítő ellenőrzéseket végez a hitelesítési folyamatokon.

API (Integrációs) Teszt: A WebApplicationFactory segítségével a teljes HTTP kérésciklust teszteli az útvonalaktól (Routing) a JSON válaszokig.

Tesztelt Funkciók (AccountsController)
A tesztek az alábbi végpontok és funkciók helyes működését igazolják:

Fiókok listázása: Az összes regisztrált felhasználó lekérése.

Egyedi lekérdezés: Fiók keresése azonosító alapján (létező és nem létező ID esetén).

Regisztráció: Új felhasználók létrehozása, szerepkörök (User/Admin) kezelése és adatbázisba mentés.

Bejelentkezés (Login): Felhasználónév és jelszó ellenőrzése, JWT token generálás.

Törlés: Felhasználói fiókok eltávolítása a rendszerből.

Tesztelési Eredmények
Összes teszteset: 7

Sikeres: 7

Sikertelen: 0

Lefedettség: A kritikus backend üzleti logika teljes körűen lefedve.

Tesztkörnyezet és Adatbázis
A tesztek futtatásához nincs szükség külső adatbázis-kapcsolatra.

InMemory Database: A tesztek az Entity Framework Core UseInMemoryDatabase megoldását használják.

Izoláció: Minden tesztfuttatás saját, egyedi nevű memóriabeli adatbázist kap, így a tesztek nem módosítják egymás adatait.

Seed adatok: A tesztkörnyezet induláskor automatikusan feltölti az alapvető tesztadatokat (pl. testplayer).

Futtatási útmutató
Visual Studio használatával:
Nyisd meg a FortivexAPI.sln fájlt.

Navigálj a felső menüsorban a Test -> Test Explorer ablakhoz.

Kattintson a Run All Tests gombra.