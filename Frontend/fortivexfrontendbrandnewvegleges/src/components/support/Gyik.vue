<template>
  <InfoLayout>
    <h1>GYAKORI KÉRDÉSEK</h1>

    <div class="section" v-for="(item, i) in faq" :key="i">
      <h2>{{ item.q }}</h2>
      <p>{{ item.a }}</p>
    </div>
  </InfoLayout>
</template>

<script setup>
import InfoLayout from "./InfoLayout.vue";

const faq = [
  {
    q: "Ingyenes a játék?",
    a: "Igen, a Fortivex teljes mértékben ingyenesen játszható.",
  },
  {
    q: "Lesz multiplayer mód?",
    a: "Nem tervezünk multiplayer módot, de alternatív játékmódok várhatóak a jövőben.",
  },
  {
    q: "Milyen tornyok érhetők el?",
    a: "Különböző típusú tornyok érhetők el, mindegyik egyedi képességekkel.",
  },
  {
    q: "Hogyan lehet fejleszteni a tornyokat?",
    a: "A tornyokat aranyért fejlesztheted a játék során.",
  },
  {
    q: "Milyen platformokon érhető el a játék?",
    a: "A játék letölthető Windows és mobil eszközökre.",
  },
  {
    q: "Hogyan lehet kapcsolatba lépni a fejlesztőkkel?",
    a: "A kapcsolatfelvételi űrlapon keresztül tudsz üzenetet küldeni nekünk (submit a ticket).",
  },
];
</script>

<style scoped>
.section {
  margin-bottom: 20px;
}
.section h2 {
  color: #f5b942;
  margin-bottom: 10px;
  font-size: 26px;
}
.section p {
  line-height: 1.7;
  font-size: 20px;
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
</style>
