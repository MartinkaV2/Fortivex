<template>
  <InfoLayout>
    <h1>Adatvédelmi tájékoztató</h1>

    <div class="section">
      <p>
        A Fortivex projekt számára kiemelten fontos a felhasználók személyes adatainak védelme.
        Jelen adatvédelmi tájékoztató bemutatja, hogy milyen adatokat gyűjtünk, hogyan használjuk
        fel azokat, és milyen jogok illetik meg a felhasználókat.
      </p>
    </div>

    <div class="section">
      <h2>1. Gyűjtött adatok</h2>
      <p>A Fortivex használata során az alábbi adatokat kérhetjük vagy kezelhetjük:</p>
      <ul>
        <li>felhasználónév</li>
        <li>email cím</li>
        <li>felhasználói fiókhoz kapcsolódó technikai adatok</li>
        <li>támogatási űrlapon megadott üzenetek</li>
      </ul>
    </div>

    <div class="section">
      <h2>2. Az adatok felhasználása</h2>
      <p>A megadott adatokat kizárólag az alábbi célokra használjuk:</p>
      <ul>
        <li>felhasználói fiók létrehozása és kezelése</li>
        <li>a játék működésének biztosítása</li>
        <li>kapcsolattartás a felhasználóval</li>
        <li>hibajelentések és támogatási kérések kezelése</li>
      </ul>
    </div>

    <div class="section">
      <h2>3. Adattárolás</h2>
      <p>
        A személyes adatokat kizárólag a projekt működéséhez szükséges ideig tároljuk. Az adatokat
        nem adjuk tovább harmadik félnek, kivéve ha azt jogszabály írja elő.
      </p>
    </div>

    <div class="section">
      <h2>4. Harmadik felek</h2>
      <p>
        A Fortivex projekt bizonyos funkciói külső szolgáltatásokat vehetnek igénybe (például email
        küldés). Ezek a szolgáltatások kizárólag a szükséges adatokat kezelik, saját adatvédelmi
        irányelveik szerint.
      </p>
    </div>

    <div class="section">
      <h2>5. Felhasználói jogok</h2>
      <p>A felhasználó jogosult:</p>
      <ul>
        <li>tájékoztatást kérni a kezelt adatokról</li>
        <li>adatainak módosítását vagy törlését kérni</li>
        <li>hozzájárulását bármikor visszavonni</li>
      </ul>
    </div>

    <div class="section">
      <h2>6. Kapcsolat</h2>
      <p>
        Adatvédelmi kérdések esetén kapcsolatba léphetsz velünk a támogatási oldalon vagy az alábbi
        email címen:
      </p>
      <p><strong>fortivexgames@gmail.com</strong></p>
    </div>
  </InfoLayout>
</template>

<script setup>
import InfoLayout from "./InfoLayout.vue";
</script>
<style scoped>
.section {
  margin-bottom: 20px;
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
