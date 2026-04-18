<template>
  <InfoLayout>
    <h1>Felhasználási feltételek</h1>

    <div class="section">
      <p>
        A Fortivex Projekt egy vizsgaprojekt keretében készülő, nem kereskedelmi célú videojáték és
        kapcsolódó weboldal. A szolgáltatás használatával a felhasználó elfogadja az alábbi
        Felhasználási feltételeket.
      </p>
    </div>

    <div class="section">
      <h2>1. Általános rendelkezések</h2>
      <p>
        A Fortivex Projekt fenntartja a jogot, hogy a jelen feltételeket bármikor módosítsa. A
        módosítások a közzétételt követően azonnal hatályba lépnek.
      </p>
    </div>

    <div class="section">
      <h2>2. A szolgáltatás jellege</h2>
      <p>
        A játék és a weboldal „ahogy van” (as-is) formában érhető el. A projekt fejlesztés alatt
        állhat, funkciók változhatnak, ideiglenesen nem érhetők el vagy megszűnhetnek.
      </p>
      <p>A fejlesztők nem garantálják a szolgáltatás folyamatos, hibamentes működését.</p>
    </div>

    <div class="section">
      <h2>3. Felhasználói fiók</h2>
      <p>A játék bizonyos funkcióinak használatához felhasználói fiók létrehozása szükséges.</p>
      <p>
        A felhasználó felelőssége a fiókadatok biztonságos kezelése, valamint minden, a fiókjával
        végrehajtott tevékenység.
      </p>
      <p>
        A Fortivex Projekt fenntartja a jogot, hogy szabályszegés esetén a fiókot ideiglenesen vagy
        véglegesen felfüggessze.
      </p>
    </div>

    <div class="section">
      <h2>4. Felhasználói magatartás</h2>
      <p>Tilos:</p>
      <ul>
        <li>csalás, exploitok vagy hackek használata,</li>
        <li>a játék működésének szándékos akadályozása,</li>
        <li>más felhasználók zaklatása vagy sértése,</li>
        <li>jogellenes vagy sértő tartalom megosztása.</li>
      </ul>
      <p>A szabályok megsértése a fiók korlátozását vagy törlését vonhatja maga után.</p>
    </div>

    <div class="section">
      <h2>5. Szellemi tulajdon</h2>
      <p>
        A Fortivex Projekt keretében létrehozott minden tartalom – beleértve a játékmechanikákat,
        grafikai elemeket, logókat és szövegeket – a fejlesztők szellemi tulajdonát képezi.
      </p>
      <p>Ezek engedély nélküli másolása, terjesztése vagy módosítása nem megengedett.</p>
    </div>

    <div class="section">
      <h2>6. Felelősség korlátozása</h2>
      <p>
        A Fortivex Projekt fejlesztői nem vállalnak felelősséget adatvesztésért,
        szolgáltatáskimaradásért vagy technikai hibákból eredő károkért.
      </p>
      <p>A felhasználó a szolgáltatást saját felelősségére használja.</p>
    </div>

    <div class="section">
      <h2>7. Kapcsolat</h2>
      <p>
        Hibajelentés vagy kérdés esetén a felhasználók a támogatási oldalon keresztül vehetik fel a
        kapcsolatot a fejlesztőkkel.
      </p>
    </div>

    <div class="section note">
      <p>
        <strong>Megjegyzés:</strong> Ez a dokumentum oktatási célból készült, és nem minősül
        hivatalos jogi tanácsnak.
      </p>
    </div>
  </InfoLayout>
</template>

<script setup>
import InfoLayout from "./InfoLayout.vue";
</script>
<style scoped>
.section {
  margin-bottom: 32px;
}
.section h2 {
  color: #f5b942;
  margin-bottom: 10px;
  font-size: 28px;
}
.section p {
  line-height: 1.7;
  font-size: 18px;
  color: #f4e7c6;
}
h1 {
  color: #f5b942;
  font-size: 42px;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin: 12px 0 32px; /* <-- nagyobb alsó margó */
  text-align: center; /* <-- középre igazítás */
}
.note {
  background: rgba(245, 185, 66, 0.1);
  border-left: 4px solid #f5b942;
  padding: 12px 16px;
}
.note p {
  margin: 0;
  font-style: italic;
}
.section ul {
  margin-top: 12px;
  margin-left: 20px;
  padding: 16px 18px;
  list-style: none;
  background: rgba(245, 185, 66, 0.08);
  border-left: 3px solid rgba(245, 185, 66, 0.6);
  border-radius: 8px;
}

.section ul li {
  position: relative;
  padding-left: 20px;
  margin-bottom: 10px;
  color: #f4e7c6;
  line-height: 1.6;
  font-size: 18px;
}

.section ul li::before {
  content: "•";
  position: absolute;
  left: 0;
  top: 0;
  color: #f5b942;
  font-size: 18px;
  line-height: 1;
}
</style>
