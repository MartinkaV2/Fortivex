<template>
  <section id="maps" class="maps-section">
    <div class="container">
      <h2 class="section-title">Pályák</h2>
      <p class="section-subtitle">
        Fedezd fel a világot, és készülj fel a kihívásokra minden pályán, már őszi és téli kiadásban
        is!
      </p>

      <div v-if="loading" class="loading">
        <LoadingSpinner text="Pályák betöltése..." />
      </div>

      <div v-else class="maps-grid">
        <div v-for="map in maps" :key="map.id" class="map-card">
          <!-- KÉP + NYILAK -->
          <div class="map-icon-wrapper">
            <img :src="map.images[map.currentImageIndex]" alt="map image" class="map-icon" />

            <div class="difficulty-indicator" :class="`difficulty-${map.difficulty}`"></div>
          </div>

          <!-- Név és nehézség -->
          <div class="map-title-section">
            <h3 class="map-name">{{ map.name }}</h3>
            <span class="map-difficulty" :class="`difficulty-${map.difficulty}`">
              {{ map.difficulty }}
            </span>
          </div>

          <p class="map-description">{{ map.description }}</p>

          <!-- Statok -->
          <div class="map-stats-section">
            <h4 class="stats-title">Pálya jellemzők</h4>

            <div class="stat-bar-item">
              <div class="stat-bar-header">
                <span class="stat-label">🌲 Terület mérete:</span>
                <span class="stat-value">{{ map.size }}%</span>
              </div>
              <div class="stat-bar">
                <div class="stat-fill size" :style="{ width: `${map.size}%` }"></div>
              </div>
            </div>

            <div class="stat-bar-item">
              <div class="stat-bar-header">
                <span class="stat-label">⚠️ Veszélyesség:</span>
                <span class="stat-value">{{ map.threat }}%</span>
              </div>
              <div class="stat-bar">
                <div class="stat-fill threat" :style="{ width: `${map.threat}%` }"></div>
              </div>
            </div>
          </div>

          <!-- Jutalmak -->
          <div class="map-rewards">
            <div class="reward-item">
              <span class="reward-icon">💰</span>
              <span class="reward-value">{{ map.goldReward }} arany</span>
            </div>
            <div class="reward-item">
              <span class="reward-icon">✨</span>
              <span class="reward-value">{{ map.xpReward }} XP</span>
            </div>
          </div>
        </div>
      </div>

      <div v-if="!loading && maps.length === 0" class="empty-state">
        <p>Még nincsenek elérhető pálya adatok</p>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from "vue";
import LoadingSpinner from "@/components/common/LoadingSpinner.vue";

// Képek importálása
import WildForestAutumnImg from "@/assets/images/maps/winter_map (Egyéni) (1).png";

import DirtyRoadImg from "@/assets/images/maps/nyar (Egyéni).png";

import QuietShoreAutumnImg from "@/assets/images/maps/osz (Egyéni).png";

// Térképek
const maps = ref([
  {
    id: 1,
    name: "Elvarázsolt Erdő",
    description: "Sűrű, misztikus erdő tele titkos ösvényekkel és rejtett veszélyekkel.",
    images: [WildForestAutumnImg],
    currentImageIndex: 0,
    difficulty: "easy",
    size: 40,
    threat: 30,
    goldReward: 20,
    xpReward: 10,
  },
  {
    id: 2,
    name: "Sötét Kanyon",
    description: "Keskeny szurdok, ahol kevés hely van a védekezésre. Tökéletes kihívás.",
    images: [DirtyRoadImg],
    currentImageIndex: 0,
    difficulty: "medium",
    size: 60,
    threat: 50,
    goldReward: 40,
    xpReward: 25,
  },
  {
    id: 3,
    name: "Csendes Part",
    description: "Az álló vizek mélyek, és a part menti területek veszélyesek lehetnek. Vigyázz!",
    images: [QuietShoreAutumnImg],
    currentImageIndex: 0,
    difficulty: "hard",
    size: 90,
    threat: 80,
    goldReward: 80,
    xpReward: 50,
  },
]);

const loading = ref(false);

const loadMaps = async () => {
  loading.value = true;
  try {
    // Backend lekérés ide, ha van
  } catch (error) {
    console.error("Error loading maps:", error);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  loadMaps();
});
</script>

<style scoped>
.maps-section {
  padding: 40px 0;
}

.maps-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 24px;
}

.map-card {
  background: var(--card-bg);
  padding: 20px;
  border-radius: 12px;
  box-shadow: var(--shadow);
  transition: 0.2s ease;
}

.map-card:hover {
  transform: translateY(-4px);
}

/* Kép + nyilak */
.map-icon-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  position: relative;
  margin-bottom: 1rem;
}

.map-icon {
  width: 150px;
  height: 150px;
  object-fit: cover;
  border-radius: 12px;
}

/* Név és nehézség */
.map-title-section {
  text-align: center;
  margin-bottom: 1rem;
}

.map-name {
  font-weight: bold;
  font-size: 1.7rem;
  margin: 0;
}

.map-difficulty {
  display: inline-block;
  font-size: 1.1rem;
  text-transform: capitalize;
  margin-top: 4px;
  padding: 2px 6px;
  border-radius: 6px;
}

.map-difficulty.difficulty-easy {
  background: #e8f5e9;
  color: #2e7d32;
}
.map-difficulty.difficulty-medium {
  background: #fff3e0;
  color: #ef6c00;
}
.map-difficulty.difficulty-hard {
  background: #ffebee;
  color: #c62828;
}

.map-description {
  margin-bottom: 1rem;
  text-align: center;
  font-size: 0.95rem;
}

/* Statok */
.stat-bar {
  background: #ddd;
  height: 8px;
  border-radius: 8px;
  overflow: hidden;
  margin-top: 4px;
}

.stat-fill {
  height: 100%;
  transition: width 0.3s;
}

.stat-fill.size {
  background: #4caf50;
}

.stat-fill.threat {
  background: #f44336;
}

/* Jutalmak */
.map-rewards {
  display: flex;
  justify-content: space-between;
  margin-top: 16px;
  padding-top: 12px;
  border-top: 1px solid #eee;
}

.reward-item {
  display: flex;
  align-items: center;
  gap: 6px;
}

/* Egyéb */
h2 {
  text-align: center;
  font-size: 2rem;
  margin-bottom: 0.5rem;
}
.section-subtitle {
  text-align: center;
  font-size: 1.1rem;
  color: #ccc;
  margin-bottom: 2rem;
}

.empty-state {
  text-align: center;
  padding: 4rem;
  color: #f4e4c1;
  font-size: 1.1rem;
}
</style>
