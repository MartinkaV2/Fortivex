<template>
  <InfoLayout>
    <template #title> Források és Köszönetnyilvánítás </template>

    <div class="section">
      <h2>Felhasznált technológiák</h2>
      <p>
        A Fortivex fejlesztése során a legmodernebb nyílt forráskódú technológiákat alkalmaztuk a
        zökkenőmentes játék és weboldal használat érdekében:
      </p>
      <ul>
        <li><strong>Vue 3</strong> - A felhasználói felület és komponensrendszer alapja</li>
        <li><strong>Vite</strong> - Villámgyors fordító és fejlesztői környezet</li>
        <li><strong>Vue Router</strong> - A navigáció és útvonalkezelés felelőse</li>
        <li><strong>ChatGPT.com</strong> - A szöveg alapú interakciók és válaszok generálása</li>
        <li><strong>Gemini.com</strong> - A vizuális effektek és animációk motorja</li>
        <li><strong>Claude.com</strong> - A szöveg alapú interakciók és válaszok generálása</li>
        <li><strong>Suno.com</strong> - A zene és hanghatások generálása</li>
        <li><strong>Visualgpt.com</strong> - A vizuális tartalmak generálása</li>
        <li>
          <strong>Unity</strong> - A 3D-s játékfejlesztés és valós idejű renderelés platformja
        </li>
      </ul>
    </div>

    <div class="section">
      <h2>Grafikai elemek</h2>
      <p>
        A játékban látható karakterek, tornyok és környezeti elemek az alábbi művészek és források
        munkáin alapulnak:
      </p>
      <ul>
        <li>Tower Defense Asset Pack - Kenney.nl</li>
        <li>Egyedi környezeti textúrák - OpenGameArt.org</li>
        <li>Ikonkészlet - FontAwesome & Flaticon</li>
      </ul>
    </div>

    <div class="section">
      <h2>Hangok és Zene</h2>
      <p>A magával ragadó atmoszférát a következő audio források biztosítják:</p>
      <ul>
        <li>Háttérzene: "Cybernetic Warfare" - Creative Commons 4.0</li>
        <li>Hangeffektek: Freesound.org közösségi gyűjtemény</li>
      </ul>
    </div>

    <div class="section">
      <h2>Fejlesztői csapat</h2>
      <p>
        Külön köszönet mindenkinek, aki ötletekkel, teszteléssel vagy kódolással hozzájárult a
        projekthez.
      </p>
    </div>

    <div class="section">
      <h2>Licenc</h2>
      <p>
        A szoftver az MIT Licenc alatt áll. A harmadik féltől származó eszközök (assets) saját
        licencszerződéseik hatálya alá tartoznak.
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

.section ul li strong {
  color: #f5b942;
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

.note {
  background: rgba(245, 185, 66, 0.1);
  border-left: 4px solid #f5b942;
  padding: 12px 16px;
  margin-top: 15px;
}
.note p {
  margin: 0;
  font-style: italic;
  color: #f4e7c6;
}

h1 {
  color: #f5b942;
  font-size: 42px;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin: 12px 0 32px;
  text-align: center;
}
</style>
