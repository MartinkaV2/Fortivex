# FortivexAPI – Backend Tesztelési Dokumentáció

Ez a modul a **Fortivex** ökoszisztéma backend szolgáltatásaihoz készült automatizált teszteseteket tartalmazza. A rendszer stabilitását és az adatok integritását három különböző tesztelési metodika együttes alkalmazása garantálja.

## 🛠 Alkalmazott Technológiai Stack

A projekt a .NET ökoszisztéma legmodernebb tesztelési eszközeit használja:

*   **xUnit**: A kontroller logika és az alapvető üzleti folyamatok izolált egységtesztelésére (`Unit Test`).
*   **MSTest**: A Microsoft hivatalos keretrendszere, amely a hitelesítési réteg (`Authentication`) és a biztonsági protokollok validálásáért felel.
*   **Integrációs Tesztek**: A `WebApplicationFactory` osztály segítségével a teljes HTTP kérés-válasz ciklus tesztelésre kerül a routingtól kezdve a JSON deszerializációig.

## 🔍 Tesztelt Funkciók (AccountsController)

A tesztelési lefedettség az `AccountsController` összes kritikus végpontjára kiterjed:

| Funkció | Leírás |
| :--- | :--- |
| **Fiókok listázása** | A regisztrált felhasználók biztonságos lekérdezésének ellenőrzése. |
| **Egyedi lekérdezés** | Fiók keresése azonosító alapján, beleértve a `NotFound` (404) hibakezelést is. |
| **Regisztráció** | Új entitások létrehozása, szerepkör-hozzárendelés és adatbázis-perzisztencia. |
| **Bejelentkezés** | Hitelesítési folyamat validálása és érvényes `JWT` token generálásának ellenőrzése. |
| **Törlés** | Felhasználói profilok végleges és biztonságos eltávolítása a rendszerből. |

## 📊 Tesztelési Statisztikák

Az utolsó futtatás eredményei alapján a backend réteg **100%-os sikerességi rátával** rendelkezik:

*   **Összes teszteset:** 7
*   **Sikeres:** 7
*   **Sikertelen:** 0
*   **Állapot:** ![Pass](https://img.shields.io/badge/Tests-7%2F7%20Passed-brightgreen)

## 🏗 Tesztkörnyezet és Izoláció

A tesztek futtatása teljesen önfenntartó, nincs szükség külső adatbázis-szerverre:

1.  **InMemory Database**: Az `Entity Framework Core` memóriabeli adatbázis-megoldását használjuk, ami extrém gyors futást tesz lehetővé.
2.  **Adatizoláció**: Minden egyes teszteset egy teljesen új, tiszta adatbázis-kontextust kap, így a tesztek nem befolyásolják egymás eredményeit.
3.  **Automatikus Seed**: A rendszer induláskor automatikusan feltölti a szükséges tesztadatokat (pl. `testplayer` profil), biztosítva a reprodukálható környezetet.

## 🚀 Futtatási Útmutató

### Visual Studio használatával:
1. Nyissa meg a `FortivexAPI.sln` projektfájlt.
2. A felső menüsorban válassza a **Test** -> **Test Explorer** menüpontot.
3. Kattintson a **Run All Tests** gombra.

### .NET CLI használatával:
Futtassa az alábbi parancsot a projekt gyökérkönyvtárában:
```bash
dotnet test

Kattintson a Run All Tests gombra.
