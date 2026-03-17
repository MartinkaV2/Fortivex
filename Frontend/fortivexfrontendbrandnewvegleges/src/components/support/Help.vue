<template>
  <InfoLayout>
    <template #title> Súgó </template>

    <div class="section">
      <h2>Hogyan kezdjem el?</h2>
      <p>
        A Fortivex egy stratégiai tower defense játék, ahol a célod megvédeni a bázisodat az
        ellenséges hullámoktól. A játék elején érdemes megismerkedni a pályával, a tornyok
        tulajdonságaival és az ellenségek típusával.
      </p>
    </div>

    <div class="section">
      <h2>Játékmenet alapjai</h2>
      <ul>
        <li>Tornyokat építhetsz a pályán kijelölt helyekre</li>
        <li>Minden torony különböző képességekkel rendelkezik</li>
        <li>Az ellenségek hullámokban érkeznek</li>
        <li>Sikeres védekezéssel jutalmakat szerezhetsz</li>
      </ul>
    </div>

    <div class="section">
      <h2>Irányítás</h2>
      <p>
        A játék egérrel irányítható. A menükben és a játéktérben
        a megfelelő ikonokra kattintva építhetsz vagy fejleszthetsz.
      </p>
    </div>

    <div class="section">
      <h2>Tippek kezdőknek</h2>
      <ul>
        <li>Ne költs el minden erőforrást az első hullámban</li>
        <li>Kombináld a különböző toronytípusokat</li>
        <li>Figyeld meg az ellenségek gyenge pontjait</li>
        <li>Fejlesztés előtt ellenőrizd a torony statisztikáit</li>
      </ul>
    </div>

    <div class="section">
      <h2>Gyakori kérdések</h2>
      <p>
        Nézd meg a <router-link to="/support/gyik">Gyakori kérdések</router-link> szekciót, ahol választ
        találhatsz a leggyakoribb kérdésekre.
      </p>
    </div>

    <div class="section">
      <h2>Technikai problémák</h2>
      <p>
        Ha a játék nem megfelelően működik, próbáld meg frissíteni az oldalt, vagy ellenőrizni az
        internetkapcsolatot. Tartós hiba esetén használd a támogatási oldalt.
      </p>
    </div>

    <div class="section">
      <h2>Felhasználási feltételek és Adatvédelem</h2>
      <p>
        Kérjük, olvasd el a
        <router-link to="/support/terms">Felhasználási feltételeket</router-link>
        és az
        <router-link to="/support/privacy">Adatvédelmi irányelveket</router-link>, hogy megismerd jogaidat
        és kötelezettségeidet.
      </p>
    </div>
  </InfoLayout>
</template>

<script setup>
import InfoLayout from "./InfoLayout.vue";
import { RouterLink } from "vue-router";
</script>

<style scoped>
section {
  margin-bottom: 32px;
  margin-top: 32px;
}
.section h2 {
  color: #f5b942;
  margin-bottom: 10px;
  font-size: 28px;
  margin-top: 20px;
}
.section p {
  line-height: 1.7;
  font-size: 18px;
  color: #f4e7c6;
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
.email {
  color: #f4e4c1;
  font-size: 1.2rem;
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

@media (max-width: 768px) {
  .submit-btn {
    width: 100%;
  }
}
</style>
