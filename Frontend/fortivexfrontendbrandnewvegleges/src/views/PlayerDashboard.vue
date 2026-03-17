<template>
  <div class="player-dashboard">
    <Navbar />

    <div class="dashboard-container">
      <div class="welcome-section">
        <h1 class="welcome-title">Üdvözöllek, {{ user?.username }}!</h1>
        <p class="welcome-subtitle">Itt követheted nyomon fejlődésed és statisztikáidat</p>
      </div>

      <div v-if="loading" class="loading">
        <div class="spinner"></div>
        <p>Betöltés...</p>
      </div>

      <div v-else-if="error" class="error-box">
        <p>{{ error }}</p>
        <button @click="loadPlayerData" class="retry-button">Újrapróbálás</button>
      </div>

      <div v-else class="stats-grid">
        <!-- Összesített statisztikák -->
        <div class="stat-card highlight">
          <div class="stat-icon-wrapper level">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="48"
              height="48"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M6 9H4.5a2.5 2.5 0 0 1 0-5H6"></path>
              <path d="M18 9h1.5a2.5 2.5 0 0 0 0-5H18"></path>
              <path d="M4 22h16"></path>
              <path d="M10 14.66V17c0 .55-.47.98-.97 1.21C7.85 18.75 7 20.24 7 22"></path>
              <path d="M14 14.66V17c0 .55.47.98.97 1.21C16.15 18.75 17 20.24 17 22"></path>
              <path d="M18 2H6v7a6 6 0 0 0 12 0V2Z"></path>
            </svg>
          </div>
          <div class="stat-content">
            <h3>Szint</h3>
            <p class="stat-value">{{ animatedLevel }}</p>
            <div class="progress-bar">
              <div class="progress-fill" :style="{ width: `${xpProgress}%` }"></div>
            </div>
            <p class="stat-detail">
              {{ stats?.currentXp || 0 }} / {{ stats?.nextLevelXp || 100 }} XP
            </p>
          </div>
        </div>

        <div class="stat-card">
          <div class="stat-icon-wrapper wins">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="48"
              height="48"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path
                d="M11.562 3.266a.5.5 0 0 1 .876 0L15.39 8.87a1 1 0 0 0 .781.586l5.257.808a.5.5 0 0 1 .28.849l-3.804 3.881a1 1 0 0 0-.283.906l.898 5.516a.5.5 0 0 1-.73.525l-4.701-2.532a1 1 0 0 0-.966 0l-4.7 2.532a.5.5 0 0 1-.731-.525l.898-5.516a1 1 0 0 0-.283-.906L3.502 11.113a.5.5 0 0 1 .28-.849l5.257-.808a1 1 0 0 0 .781-.586l2.542-5.604Z"
              ></path>
            </svg>
          </div>
          <div class="stat-content">
            <h3>Győzelmek</h3>
            <p class="stat-value">{{ animatedWins }}</p>
            <p class="stat-detail">Összes menet: {{ animatedTotalGames }}</p>
          </div>
        </div>

        <div class="stat-card">
          <div class="stat-icon-wrapper gold">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="48"
              height="48"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <line x1="12" x2="12" y1="2" y2="22"></line>
              <path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"></path>
            </svg>
          </div>
          <div class="stat-content">
            <h3>Összegyűjtött Arany</h3>
            <p class="stat-value">{{ formatNumber(animatedTotalGold) }}</p>
            <p class="stat-detail">Jelenlegi: {{ formatNumber(animatedCurrentGold) }}</p>
          </div>
        </div>

        <div class="stat-card">
          <div class="stat-icon-wrapper enemies">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="48"
              height="48"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <circle cx="12" cy="8" r="5"></circle>
              <path d="M20 21a8 8 0 0 0-16 0"></path>
              <path d="m9 6 6 6"></path>
              <path d="m15 6-6 6"></path>
            </svg>
          </div>
          <div class="stat-content">
            <h3>Legyőzött Ellenségek</h3>
            <p class="stat-value">{{ formatNumber(animatedEnemiesKilled) }}</p>
            <p class="stat-detail">Rekord hullám: {{ animatedMaxWave }}</p>
          </div>
        </div>

        <!-- Pálya haladás -->
        <div class="maps-progress-card">
          <h2 class="card-title">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="32"
              height="32"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
              class="title-icon"
            >
              <polygon points="3 11 22 2 13 21 11 13 3 11"></polygon>
            </svg>
            Pálya Haladás
          </h2>
          <div class="maps-list">
            <div v-for="map in progress?.maps || []" :key="map.id" class="map-progress-item">
              <div class="map-image-container">
                <img :src="getMapImage(map.id)" :alt="map.name" class="map-image" />
                <div v-if="map.completed" class="completion-overlay">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="64"
                    height="64"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="3"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  >
                    <polyline points="20 6 9 17 4 12"></polyline>
                  </svg>
                </div>
              </div>
              <div class="map-info">
                <div class="map-header">
                  <h4>{{ map.name }}</h4>
                  <span
                    class="completion-badge"
                    :class="map.completed ? 'completed' : 'incomplete'"
                  >
                    <svg
                      v-if="map.completed"
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      stroke-width="2"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    >
                      <polyline points="20 6 9 17 4 12"></polyline>
                    </svg>
                    {{ map.completed ? "Teljesítve" : "Folyamatban" }}
                  </span>
                </div>
                <div class="map-stats">
                  <span>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      stroke-width="2"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    >
                      <circle cx="12" cy="12" r="10"></circle>
                      <polyline points="12 6 12 12 16 14"></polyline>
                    </svg>
                    {{ formatTime(map.bestTime) }}
                  </span>
                  <span>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      stroke-width="2"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    >
                      <polygon
                        points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"
                      ></polygon>
                    </svg>
                    {{ map.stars }}/3
                  </span>
                </div>
                <div class="progress-bar">
                  <div class="progress-fill" :style="{ width: `${map.completionPercent}%` }"></div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Teljesítmények -->
        <div class="achievements-card">
          <h2 class="card-title">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="32"
              height="32"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
              class="title-icon"
            >
              <path d="M6 9H4.5a2.5 2.5 0 0 1 0-5H6"></path>
              <path d="M18 9h1.5a2.5 2.5 0 0 0 0-5H18"></path>
              <path d="M4 22h16"></path>
              <path d="M10 14.66V17c0 .55-.47.98-.97 1.21C7.85 18.75 7 20.24 7 22"></path>
              <path d="M14 14.66V17c0 .55.47.98.97 1.21C16.15 18.75 17 20.24 17 22"></path>
              <path d="M18 2H6v7a6 6 0 0 0 12 0V2Z"></path>
            </svg>
            Teljesítmények
          </h2>
          <div class="achievements-grid">
            <div
              v-for="achievement in progress?.achievements || []"
              :key="achievement.id"
              class="achievement-item"
              :class="{ unlocked: achievement.unlocked }"
            >
              <div class="achievement-icon">{{ achievement.icon }}</div>
              <div class="achievement-info">
                <h4>{{ achievement.name }}</h4>
                <p>{{ achievement.description }}</p>
              </div>
              <div v-if="achievement.unlocked" class="achievement-badge">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="3"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <polyline points="20 6 9 17 4 12"></polyline>
                </svg>
              </div>
              <div v-else class="achievement-lock">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="20"
                  height="20"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <rect width="18" height="11" x="3" y="11" rx="2" ry="2"></rect>
                  <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                </svg>
              </div>
            </div>
          </div>
        </div>

        <!-- Játék történet -->
        <div class="history-card">
          <h2 class="card-title">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="32"
              height="32"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
              class="title-icon"
            >
              <path d="M3 12a9 9 0 1 0 9-9 9.75 9.75 0 0 0-6.74 2.74L3 8"></path>
              <path d="M3 3v5h5"></path>
              <path d="M12 7v5l4 2"></path>
            </svg>
            Utolsó Játékok
          </h2>
          <div class="history-list">
            <div v-for="game in stats?.recentGames || []" :key="game.id" class="history-item">
              <div class="game-result" :class="game.won ? 'win' : 'loss'">
                <svg
                  v-if="game.won"
                  xmlns="http://www.w3.org/2000/svg"
                  width="20"
                  height="20"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M6 9H4.5a2.5 2.5 0 0 1 0-5H6"></path>
                  <path d="M18 9h1.5a2.5 2.5 0 0 0 0-5H18"></path>
                  <path d="M4 22h16"></path>
                  <path d="M10 14.66V17c0 .55-.47.98-.97 1.21C7.85 18.75 7 20.24 7 22"></path>
                  <path d="M14 14.66V17c0 .55.47.98.97 1.21C16.15 18.75 17 20.24 17 22"></path>
                  <path d="M18 2H6v7a6 6 0 0 0 12 0V2Z"></path>
                </svg>
                <svg
                  v-else
                  xmlns="http://www.w3.org/2000/svg"
                  width="20"
                  height="20"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <circle cx="12" cy="8" r="5"></circle>
                  <path d="M20 21a8 8 0 0 0-16 0"></path>
                  <path d="m9 6 6 6"></path>
                  <path d="m15 6-6 6"></path>
                </svg>
                {{ game.won ? "Győzelem" : "Vereség" }}
              </div>
              <div class="game-details">
                <p>
                  <strong>{{ game.mapName }}</strong>
                </p>
                <p>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="14"
                    height="14"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  >
                    <path
                      d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"
                    ></path>
                    <polyline points="7.5 4.21 12 6.81 16.5 4.21"></polyline>
                    <polyline points="7.5 19.79 7.5 14.6 3 12"></polyline>
                    <polyline points="21 12 16.5 14.6 16.5 19.79"></polyline>
                    <polyline points="3.27 6.96 12 12.01 20.73 6.96"></polyline>
                    <line x1="12" x2="12" y1="22.08" y2="12"></line>
                  </svg>
                  Hullám: {{ game.waveReached }} |
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="14"
                    height="14"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  >
                    <circle cx="12" cy="12" r="10"></circle>
                    <polyline points="12 6 12 12 16 14"></polyline>
                  </svg>
                  {{ formatTime(game.duration) }}
                </p>
                <p class="game-date">{{ formatDate(game.playedAt) }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from "vue";
import { useAuthStore } from "@/stores/auth";
import { usePlayerStore } from "@/stores/player";
import Navbar from "@/components/common/Navbar.vue";
import api from "@/services/api"; // ✅ Ez létezik

// Map képek importálása
import SummerImg from "@/assets/images/maps/nyar (Egyéni).png";
import AutumnImg from "@/assets/images/maps/osz (Egyéni).png";
import WinterImg from "@/assets/images/maps/winter_map (Egyéni) (1).png";

const authStore = useAuthStore();
const playerStore = usePlayerStore();

const user = computed(() => authStore.user);

// ✅ Valódi adatok a store-ból (mock adatok eltávolítva)
const stats = computed(() => playerStore.stats);
const progress = computed(() => playerStore.progress);
const loading = computed(() => playerStore.loading);
const error = computed(() => playerStore.error);

// Animált statisztikák
const animatedLevel = ref(0);
const animatedWins = ref(0);
const animatedTotalGames = ref(0);
const animatedTotalGold = ref(0);
const animatedCurrentGold = ref(0);
const animatedEnemiesKilled = ref(0);
const animatedMaxWave = ref(0);

const animateValue = (current, target, duration = 1000) => {
  const start = current.value;
  const diff = target - start;
  const startTime = Date.now();

  const step = () => {
    const elapsed = Date.now() - startTime;
    const progress = Math.min(elapsed / duration, 1);
    const easeOut = 1 - Math.pow(1 - progress, 3);

    current.value = Math.floor(start + diff * easeOut);

    if (progress < 1) {
      requestAnimationFrame(step);
    } else {
      current.value = target;
    }
  };

  requestAnimationFrame(step);
};

watch(
  () => stats.value,
  (newStats) => {
    if (newStats) {
      animateValue(animatedLevel, newStats.level || 1, 800);
      animateValue(animatedWins, newStats.wins || 0, 1000);
      animateValue(animatedTotalGames, newStats.totalGames || 0, 1000);
      animateValue(animatedTotalGold, newStats.totalGold || 0, 1200);
      animateValue(animatedCurrentGold, newStats.currentGold || 0, 1200);
      animateValue(animatedEnemiesKilled, newStats.enemiesKilled || 0, 1500);
      animateValue(animatedMaxWave, newStats.maxWaveReached || 0, 800);
    }
  },
  { immediate: true },
);

const xpProgress = computed(() => {
  if (!stats.value) return 0;
  const current = stats.value.currentXp || 0;
  const needed = stats.value.nextLevelXp || 100;
  return Math.min((current / needed) * 100, 100);
});

// Map képek ID alapján
const getMapImage = (mapId) => {
  const mapImages = {
    1: SummerImg,
    2: WinterImg,
    3: AutumnImg,
  };

  return mapImages[mapId] || null;
};

const formatNumber = (num) => {
  if (!num) return "0";
  return num.toLocaleString("hu-HU");
};

const formatTime = (seconds) => {
  if (!seconds) return "0:00";
  const mins = Math.floor(seconds / 60);
  const secs = seconds % 60;
  return `${mins}:${secs.toString().padStart(2, "0")}`;
};

const formatDate = (dateString) => {
  if (!dateString) return "";
  const date = new Date(dateString);
  return date.toLocaleDateString("hu-HU", {
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const loadPlayerData = async () => {
  const accountId = user.value?.id;

  if (!accountId) {
    console.error("❌ Nincs bejelentkezett felhasználó!");
    return;
  }

  try {
    await Promise.all([playerStore.fetchStats(accountId), playerStore.fetchProgress(accountId)]);
  } catch (error) {
    console.error("❌ Error:", error);
  }
};

onMounted(async () => {
  if (user.value?.id) {
    loadPlayerData();
  } else {
    await authStore.fetchCurrentUser();
    loadPlayerData();
  }
});
</script>

<style scoped lang="scss">
.player-dashboard {
  min-height: 100vh;
  background: linear-gradient(135deg, #1a0f0a 0%, #2c1810 100%);
}

.dashboard-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 2rem;
}

.welcome-section {
  text-align: center;
  margin-bottom: 3rem;
  padding-top: 2rem;
}

.welcome-title {
  font-size: 3rem;
  color: #ffd700;
  font-family: "Cinzel", serif;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
  margin-bottom: 0.5rem;
}

.welcome-subtitle {
  color: #f4e4c1;
  font-size: 1.2rem;
}

.loading {
  text-align: center;
  padding: 4rem;

  .spinner {
    width: 60px;
    height: 60px;
    border: 4px solid rgba(255, 215, 0, 0.3);
    border-top-color: #ffd700;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin: 0 auto 1rem;
  }

  p {
    color: #f4e4c1;
    font-size: 1.2rem;
  }
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.error-box {
  background: rgba(220, 20, 60, 0.2);
  border: 2px solid #dc143c;
  color: #ff6b6b;
  padding: 2rem;
  border-radius: 12px;
  text-align: center;

  .retry-button {
    margin-top: 1rem;
    padding: 0.8rem 1.5rem;
    background: #8b4513;
    color: #fff;
    border: 2px solid #ffd700;
    border-radius: 8px;
    cursor: pointer;
    font-weight: 600;
    transition: all 0.3s ease;

    &:hover {
      background: #a0522d;
      transform: translateY(-2px);
    }
  }
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.stat-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1.5rem;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    border-color: #ffd700;
    box-shadow: 0 10px 30px rgba(255, 215, 0, 0.3);
  }

  &.highlight {
    border-color: #ffd700;
    background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
  }

  .stat-icon-wrapper {
    width: 70px;
    height: 70px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 12px;
    flex-shrink: 0;

    svg {
      filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
    }

    &.level {
      background: linear-gradient(135deg, #ffd700, #ffed4e);
      color: #2c1810;
    }

    &.wins {
      background: linear-gradient(135deg, #32cd32, #90ee90);
      color: #1a0f0a;
    }

    &.gold {
      background: linear-gradient(135deg, #ff8c00, #ffa500);
      color: #1a0f0a;
    }

    &.enemies {
      background: linear-gradient(135deg, #dc143c, #ff6b6b);
      color: #fff;
    }
  }

  .stat-content {
    flex: 1;

    h3 {
      color: #f4e4c1;
      font-size: 0.9rem;
      margin-bottom: 0.5rem;
      text-transform: uppercase;
      letter-spacing: 1px;
    }

    .stat-value {
      color: #ffd700;
      font-size: 2rem;
      font-weight: 700;
      margin-bottom: 0.5rem;
      font-family: "Courier New", monospace;
    }

    .stat-detail {
      color: rgba(244, 228, 193, 0.7);
      font-size: 0.85rem;
    }
  }
}

.progress-bar {
  width: 100%;
  height: 8px;
  background: #1a0f0a;
  border-radius: 4px;
  overflow: hidden;
  margin: 0.5rem 0;

  .progress-fill {
    height: 100%;
    background: linear-gradient(90deg, #ffd700, #ffed4e);
    transition: width 0.5s ease;
    box-shadow: 0 0 10px rgba(255, 215, 0, 0.5);
  }
}

.maps-progress-card,
.achievements-card,
.history-card {
  grid-column: 1 / -1;
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 2rem;
}

.card-title {
  color: #ffd700;
  font-size: 1.8rem;
  margin-bottom: 1.5rem;
  font-family: "Cinzel", serif;
  display: flex;
  align-items: center;
  gap: 0.8rem;

  .title-icon {
    filter: drop-shadow(0 2px 4px rgba(255, 215, 0, 0.5));
  }
}

.maps-list {
  display: grid;
  gap: 1.5rem;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
}

.map-progress-item {
  background: #1a0f0a;
  border-radius: 12px;
  border: 2px solid #8b4513;
  overflow: hidden;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    border-color: #ffd700;
    box-shadow: 0 10px 30px rgba(255, 215, 0, 0.2);
  }

  .map-image-container {
    position: relative;
    width: 100%;
    height: 200px;
    overflow: hidden;
    background: #1a0f0a;

    .map-image {
      width: 100%;
      height: 100%;
      object-fit: cover;
      transition: transform 0.3s ease;
    }

    &:hover .map-image {
      transform: scale(1.05);
    }

    .completion-overlay {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(50, 205, 50, 0.3);
      display: flex;
      align-items: center;
      justify-content: center;

      svg {
        color: #32cd32;
        filter: drop-shadow(0 0 10px rgba(50, 205, 50, 0.8));
      }
    }
  }

  .map-info {
    padding: 1.5rem;
  }

  .map-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
    flex-wrap: wrap;
    gap: 0.5rem;

    h4 {
      color: #ffd700;
      font-size: 1.2rem;
      font-family: "Cinzel", serif;
    }

    .completion-badge {
      padding: 0.4rem 0.8rem;
      border-radius: 12px;
      font-size: 0.85rem;
      font-weight: 600;
      display: flex;
      align-items: center;
      gap: 0.4rem;

      &.completed {
        background: rgba(50, 205, 50, 0.2);
        color: #32cd32;
        border: 1px solid #32cd32;
      }

      &.incomplete {
        background: rgba(255, 165, 0, 0.2);
        color: #ffa500;
        border: 1px solid #ffa500;
      }
    }
  }

  .map-stats {
    display: flex;
    gap: 2rem;
    margin-bottom: 1rem;
    color: #f4e4c1;
    font-size: 0.9rem;
    flex-wrap: wrap;

    span {
      display: flex;
      align-items: center;
      gap: 0.4rem;

      svg {
        color: #ffd700;
      }
    }
  }
}

.achievements-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1rem;
}

.achievement-item {
  background: #1a0f0a;
  padding: 1.2rem;
  border-radius: 12px;
  border: 2px solid #8b4513;
  display: flex;
  align-items: center;
  gap: 1rem;
  opacity: 0.5;
  position: relative;
  transition: all 0.3s ease;

  &.unlocked {
    opacity: 1;
    border-color: #ffd700;
    background: linear-gradient(135deg, rgba(255, 215, 0, 0.05), rgba(255, 215, 0, 0.02));

    &:hover {
      transform: translateY(-3px);
      box-shadow: 0 8px 20px rgba(255, 215, 0, 0.3);
    }
  }

  .achievement-icon {
    font-size: 2.5rem;
    filter: grayscale(1);
    transition: filter 0.3s ease;
  }

  &.unlocked .achievement-icon {
    filter: grayscale(0);
  }

  .achievement-info {
    flex: 1;

    h4 {
      color: #ffd700;
      font-size: 1rem;
      margin-bottom: 0.3rem;
    }

    p {
      color: #f4e4c1;
      font-size: 0.85rem;
      line-height: 1.4;
    }
  }

  .achievement-badge {
    position: absolute;
    top: 0.5rem;
    right: 0.5rem;
    color: #32cd32;
    animation: pulse 2s infinite;
  }

  .achievement-lock {
    position: absolute;
    top: 0.5rem;
    right: 0.5rem;
    color: #8b4513;
  }
}

@keyframes pulse {
  0%,
  100% {
    opacity: 1;
    transform: scale(1);
  }
  50% {
    opacity: 0.7;
    transform: scale(1.1);
  }
}

.history-list {
  display: grid;
  gap: 1rem;
}

.history-item {
  background: #1a0f0a;
  padding: 1rem 1.5rem;
  border-radius: 12px;
  border: 2px solid #8b4513;
  display: flex;
  gap: 1.5rem;
  align-items: center;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
    transform: translateX(5px);
  }

  .game-result {
    padding: 0.6rem 1.2rem;
    border-radius: 10px;
    font-weight: 700;
    white-space: nowrap;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    flex-shrink: 0;

    &.win {
      background: rgba(50, 205, 50, 0.2);
      color: #32cd32;
      border: 2px solid #32cd32;
    }

    &.loss {
      background: rgba(220, 20, 60, 0.2);
      color: #ff6b6b;
      border: 2px solid #dc143c;
    }
  }

  .game-details {
    flex: 1;
    min-width: 0;

    p {
      color: #f4e4c1;
      font-size: 0.9rem;
      margin-bottom: 0.3rem;
      display: flex;
      align-items: center;
      gap: 0.5rem;
      flex-wrap: wrap;

      svg {
        color: #ffd700;
        flex-shrink: 0;
      }
    }

    .game-date {
      color: rgba(244, 228, 193, 0.6);
      font-size: 0.8rem;
    }
  }
}

/* =============== RESPONSIVE STYLES =============== */

/* Large tablets és kis desktop (1024px - 1280px) */
@media (max-width: 1280px) {
  .dashboard-container {
    padding: 1.5rem;
  }

  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .maps-list {
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  }

  .achievements-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  }
}

/* Tablets (768px - 1024px) */
@media (max-width: 1024px) {
  .dashboard-container {
    padding: 1.5rem 1rem;
  }

  .welcome-section {
    margin-bottom: 2rem;
    padding-top: 1rem;
  }

  .welcome-title {
    font-size: 2.5rem;
  }

  .welcome-subtitle {
    font-size: 1.1rem;
  }

  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 1.5rem;
  }

  .stat-card {
    flex-direction: row;
    padding: 1.2rem;

    .stat-icon-wrapper {
      width: 60px;
      height: 60px;

      svg {
        width: 36px;
        height: 36px;
      }
    }

    .stat-content {
      h3 {
        font-size: 0.85rem;
      }

      .stat-value {
        font-size: 1.6rem;
      }

      .stat-detail {
        font-size: 0.8rem;
      }
    }
  }

  .maps-progress-card,
  .achievements-card,
  .history-card {
    padding: 1.5rem;
  }

  .card-title {
    font-size: 1.5rem;

    .title-icon {
      width: 28px;
      height: 28px;
    }
  }

  .maps-list {
    grid-template-columns: repeat(2, 1fr);
    gap: 1.2rem;
  }

  .map-progress-item {
    .map-image-container {
      height: 180px;
    }

    .map-info {
      padding: 1.2rem;
    }
  }

  .achievements-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

/* Mobile landscape & large phones (600px - 768px) */
@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }

  .welcome-title {
    font-size: 2rem;
    margin-bottom: 0.3rem;
  }

  .welcome-subtitle {
    font-size: 1rem;
  }

  .stats-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .stat-card {
    flex-direction: row;
    padding: 1rem;
    gap: 1rem;

    &:hover {
      transform: translateY(-3px);
    }

    .stat-icon-wrapper {
      width: 55px;
      height: 55px;

      svg {
        width: 32px;
        height: 32px;
      }
    }

    .stat-content {
      h3 {
        font-size: 0.8rem;
        margin-bottom: 0.3rem;
      }

      .stat-value {
        font-size: 1.5rem;
      }

      .stat-detail {
        font-size: 0.75rem;
      }
    }
  }

  .maps-progress-card,
  .achievements-card,
  .history-card {
    padding: 1rem;
  }

  .card-title {
    font-size: 1.3rem;
    gap: 0.5rem;

    .title-icon {
      width: 24px;
      height: 24px;
    }
  }

  .maps-list {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .map-progress-item {
    .map-image-container {
      height: 160px;
    }

    .map-info {
      padding: 1rem;
    }

    .map-header {
      h4 {
        font-size: 1.1rem;
      }

      .completion-badge {
        font-size: 0.8rem;
        padding: 0.3rem 0.6rem;
      }
    }

    .map-stats {
      gap: 1rem;
      font-size: 0.85rem;
    }
  }

  .achievements-grid {
    grid-template-columns: 1fr;
  }

  .achievement-item {
    padding: 1rem;

    .achievement-icon {
      font-size: 2rem;
    }

    .achievement-info {
      h4 {
        font-size: 0.95rem;
      }

      p {
        font-size: 0.8rem;
      }
    }
  }

  .history-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.8rem;
    padding: 1rem;

    .game-result {
      width: 100%;
      justify-content: center;
      padding: 0.5rem 1rem;
    }

    .game-details {
      width: 100%;

      p {
        font-size: 0.85rem;
      }
    }
  }
}

/* Small mobile (480px - 600px) */
@media (max-width: 600px) {
  .welcome-title {
    font-size: 1.7rem;
  }

  .welcome-subtitle {
    font-size: 0.95rem;
  }

  .stat-card {
    padding: 0.9rem;

    .stat-icon-wrapper {
      width: 50px;
      height: 50px;

      svg {
        width: 28px;
        height: 28px;
      }
    }

    .stat-content {
      .stat-value {
        font-size: 1.3rem;
      }
    }
  }

  .card-title {
    font-size: 1.2rem;
    flex-direction: column;
    align-items: flex-start;
  }

  .map-progress-item {
    .map-image-container {
      height: 140px;
    }
  }
}

/* Extra small mobile (< 480px) */
@media (max-width: 480px) {
  .dashboard-container {
    padding: 0.8rem;
  }

  .welcome-section {
    margin-bottom: 1.5rem;
  }

  .welcome-title {
    font-size: 1.5rem;
  }

  .welcome-subtitle {
    font-size: 0.9rem;
  }

  .stats-grid {
    gap: 0.8rem;
  }

  .stat-card {
    flex-direction: column;
    text-align: center;
    padding: 1rem;

    .stat-icon-wrapper {
      width: 60px;
      height: 60px;
      margin: 0 auto;

      svg {
        width: 32px;
        height: 32px;
      }
    }

    .stat-content {
      width: 100%;

      .stat-value {
        font-size: 1.6rem;
      }
    }
  }

  .maps-progress-card,
  .achievements-card,
  .history-card {
    padding: 0.8rem;
  }

  .card-title {
    font-size: 1.1rem;
  }

  .map-progress-item {
    .map-image-container {
      height: 120px;
    }

    .map-info {
      padding: 0.8rem;
    }

    .map-header {
      flex-direction: column;
      align-items: flex-start;

      h4 {
        font-size: 1rem;
      }
    }

    .map-stats {
      flex-direction: column;
      gap: 0.5rem;
    }
  }

  .achievement-item {
    flex-direction: column;
    text-align: center;
    padding: 1rem;

    .achievement-icon {
      font-size: 2.5rem;
    }

    .achievement-badge,
    .achievement-lock {
      top: 0.3rem;
      right: 0.3rem;
    }
  }

  .history-item {
    padding: 0.8rem;

    .game-result {
      font-size: 0.9rem;
      padding: 0.4rem 0.8rem;
    }

    .game-details {
      p {
        font-size: 0.8rem;
      }

      .game-date {
        font-size: 0.75rem;
      }
    }
  }
}
</style>

<style scoped lang="scss">
.player-dashboard {
  min-height: 100vh;
  background: linear-gradient(135deg, #1a0f0a 0%, #2c1810 100%);
}

.dashboard-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 2rem;
}

.welcome-section {
  text-align: center;
  margin-bottom: 3rem;
  padding-top: 2rem;
}

.welcome-title {
  font-size: 3rem;
  color: #ffd700;
  font-family: "Cinzel", serif;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
  margin-bottom: 0.5rem;
}

.welcome-subtitle {
  color: #f4e4c1;
  font-size: 1.2rem;
}

.loading {
  text-align: center;
  padding: 4rem;

  .spinner {
    width: 60px;
    height: 60px;
    border: 4px solid rgba(255, 215, 0, 0.3);
    border-top-color: #ffd700;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin: 0 auto 1rem;
  }

  p {
    color: #f4e4c1;
    font-size: 1.2rem;
  }
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.error-box {
  background: rgba(220, 20, 60, 0.2);
  border: 2px solid #dc143c;
  color: #ff6b6b;
  padding: 2rem;
  border-radius: 12px;
  text-align: center;

  .retry-button {
    margin-top: 1rem;
    padding: 0.8rem 1.5rem;
    background: #8b4513;
    color: #fff;
    border: 2px solid #ffd700;
    border-radius: 8px;
    cursor: pointer;
    font-weight: 600;
    transition: all 0.3s ease;

    &:hover {
      background: #a0522d;
      transform: translateY(-2px);
    }
  }
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.stat-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1.5rem;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    border-color: #ffd700;
    box-shadow: 0 10px 30px rgba(255, 215, 0, 0.3);
  }

  &.highlight {
    border-color: #ffd700;
    background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
  }

  .stat-icon-wrapper {
    width: 70px;
    height: 70px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 12px;
    flex-shrink: 0;

    svg {
      filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
    }

    &.level {
      background: linear-gradient(135deg, #ffd700, #ffed4e);
      color: #2c1810;
    }

    &.wins {
      background: linear-gradient(135deg, #32cd32, #90ee90);
      color: #1a0f0a;
    }

    &.gold {
      background: linear-gradient(135deg, #ff8c00, #ffa500);
      color: #1a0f0a;
    }

    &.enemies {
      background: linear-gradient(135deg, #dc143c, #ff6b6b);
      color: #fff;
    }
  }

  .stat-content {
    flex: 1;

    h3 {
      color: #f4e4c1;
      font-size: 0.9rem;
      margin-bottom: 0.5rem;
      text-transform: uppercase;
      letter-spacing: 1px;
    }

    .stat-value {
      color: #ffd700;
      font-size: 2rem;
      font-weight: 700;
      margin-bottom: 0.5rem;
      font-family: "Courier New", monospace;
    }

    .stat-detail {
      color: rgba(244, 228, 193, 0.7);
      font-size: 0.85rem;
    }
  }
}

.progress-bar {
  width: 100%;
  height: 8px;
  background: #1a0f0a;
  border-radius: 4px;
  overflow: hidden;
  margin: 0.5rem 0;

  .progress-fill {
    height: 100%;
    background: linear-gradient(90deg, #ffd700, #ffed4e);
    transition: width 0.5s ease;
    box-shadow: 0 0 10px rgba(255, 215, 0, 0.5);
  }
}

.maps-progress-card,
.achievements-card,
.history-card {
  grid-column: 1 / -1;
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 2rem;
}

.card-title {
  color: #ffd700;
  font-size: 1.8rem;
  margin-bottom: 1.5rem;
  font-family: "Cinzel", serif;
  display: flex;
  align-items: center;
  gap: 0.8rem;

  .title-icon {
    filter: drop-shadow(0 2px 4px rgba(255, 215, 0, 0.5));
  }
}

.maps-list {
  display: grid;
  gap: 1.5rem;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
}

.map-progress-item {
  background: #1a0f0a;
  border-radius: 12px;
  border: 2px solid #8b4513;
  overflow: hidden;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    border-color: #ffd700;
    box-shadow: 0 10px 30px rgba(255, 215, 0, 0.2);
  }

  .map-image-container {
    position: relative;
    width: 100%;
    height: 200px;
    overflow: hidden;
    background: #1a0f0a;

    .map-image {
      width: 100%;
      height: 100%;
      object-fit: cover;
      transition: transform 0.3s ease;
    }

    &:hover .map-image {
      transform: scale(1.05);
    }

    .completion-overlay {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(50, 205, 50, 0.3);
      display: flex;
      align-items: center;
      justify-content: center;

      svg {
        color: #32cd32;
        filter: drop-shadow(0 0 10px rgba(50, 205, 50, 0.8));
      }
    }
  }

  .map-info {
    padding: 1.5rem;
  }

  .map-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
    flex-wrap: wrap;
    gap: 0.5rem;

    h4 {
      color: #ffd700;
      font-size: 1.2rem;
      font-family: "Cinzel", serif;
    }

    .completion-badge {
      padding: 0.4rem 0.8rem;
      border-radius: 12px;
      font-size: 0.85rem;
      font-weight: 600;
      display: flex;
      align-items: center;
      gap: 0.4rem;

      &.completed {
        background: rgba(50, 205, 50, 0.2);
        color: #32cd32;
        border: 1px solid #32cd32;
      }

      &.incomplete {
        background: rgba(255, 165, 0, 0.2);
        color: #ffa500;
        border: 1px solid #ffa500;
      }
    }
  }

  .map-stats {
    display: flex;
    gap: 2rem;
    margin-bottom: 1rem;
    color: #f4e4c1;
    font-size: 0.9rem;
    flex-wrap: wrap;

    span {
      display: flex;
      align-items: center;
      gap: 0.4rem;

      svg {
        color: #ffd700;
      }
    }
  }
}

.achievements-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1rem;
}

.achievement-item {
  background: #1a0f0a;
  padding: 1.2rem;
  border-radius: 12px;
  border: 2px solid #8b4513;
  display: flex;
  align-items: center;
  gap: 1rem;
  opacity: 0.5;
  position: relative;
  transition: all 0.3s ease;

  &.unlocked {
    opacity: 1;
    border-color: #ffd700;
    background: linear-gradient(135deg, rgba(255, 215, 0, 0.05), rgba(255, 215, 0, 0.02));

    &:hover {
      transform: translateY(-3px);
      box-shadow: 0 8px 20px rgba(255, 215, 0, 0.3);
    }
  }

  .achievement-icon {
    font-size: 2.5rem;
    filter: grayscale(1);
    transition: filter 0.3s ease;
  }

  &.unlocked .achievement-icon {
    filter: grayscale(0);
  }

  .achievement-info {
    flex: 1;

    h4 {
      color: #ffd700;
      font-size: 1rem;
      margin-bottom: 0.3rem;
    }

    p {
      color: #f4e4c1;
      font-size: 0.85rem;
      line-height: 1.4;
    }
  }

  .achievement-badge {
    position: absolute;
    top: 0.5rem;
    right: 0.5rem;
    color: #32cd32;
    animation: pulse 2s infinite;
  }

  .achievement-lock {
    position: absolute;
    top: 0.5rem;
    right: 0.5rem;
    color: #8b4513;
  }
}

@keyframes pulse {
  0%,
  100% {
    opacity: 1;
    transform: scale(1);
  }
  50% {
    opacity: 0.7;
    transform: scale(1.1);
  }
}

.history-list {
  display: grid;
  gap: 1rem;
}

.history-item {
  background: #1a0f0a;
  padding: 1rem 1.5rem;
  border-radius: 12px;
  border: 2px solid #8b4513;
  display: flex;
  gap: 1.5rem;
  align-items: center;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
    transform: translateX(5px);
  }

  .game-result {
    padding: 0.6rem 1.2rem;
    border-radius: 10px;
    font-weight: 700;
    white-space: nowrap;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    flex-shrink: 0;

    &.win {
      background: rgba(50, 205, 50, 0.2);
      color: #32cd32;
      border: 2px solid #32cd32;
    }

    &.loss {
      background: rgba(220, 20, 60, 0.2);
      color: #ff6b6b;
      border: 2px solid #dc143c;
    }
  }

  .game-details {
    flex: 1;
    min-width: 0;

    p {
      color: #f4e4c1;
      font-size: 0.9rem;
      margin-bottom: 0.3rem;
      display: flex;
      align-items: center;
      gap: 0.5rem;
      flex-wrap: wrap;

      svg {
        color: #ffd700;
        flex-shrink: 0;
      }
    }

    .game-date {
      color: rgba(244, 228, 193, 0.6);
      font-size: 0.8rem;
    }
  }
}

/* =============== RESPONSIVE STYLES =============== */

/* Large tablets és kis desktop (1024px - 1280px) */
@media (max-width: 1280px) {
  .dashboard-container {
    padding: 1.5rem;
  }

  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .maps-list {
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  }

  .achievements-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  }
}

/* Tablets (768px - 1024px) */
@media (max-width: 1024px) {
  .dashboard-container {
    padding: 1.5rem 1rem;
  }

  .welcome-section {
    margin-bottom: 2rem;
    padding-top: 1rem;
  }

  .welcome-title {
    font-size: 2.5rem;
  }

  .welcome-subtitle {
    font-size: 1.1rem;
  }

  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 1.5rem;
  }

  .stat-card {
    flex-direction: row;
    padding: 1.2rem;

    .stat-icon-wrapper {
      width: 60px;
      height: 60px;

      svg {
        width: 36px;
        height: 36px;
      }
    }

    .stat-content {
      h3 {
        font-size: 0.85rem;
      }

      .stat-value {
        font-size: 1.6rem;
      }

      .stat-detail {
        font-size: 0.8rem;
      }
    }
  }

  .maps-progress-card,
  .achievements-card,
  .history-card {
    padding: 1.5rem;
  }

  .card-title {
    font-size: 1.5rem;

    .title-icon {
      width: 28px;
      height: 28px;
    }
  }

  .maps-list {
    grid-template-columns: repeat(2, 1fr);
    gap: 1.2rem;
  }

  .map-progress-item {
    .map-image-container {
      height: 180px;
    }

    .map-info {
      padding: 1.2rem;
    }
  }

  .achievements-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

/* Mobile landscape & large phones (600px - 768px) */
@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }

  .welcome-title {
    font-size: 2rem;
    margin-bottom: 0.3rem;
  }

  .welcome-subtitle {
    font-size: 1rem;
  }

  .stats-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .stat-card {
    flex-direction: row;
    padding: 1rem;
    gap: 1rem;

    &:hover {
      transform: translateY(-3px);
    }

    .stat-icon-wrapper {
      width: 55px;
      height: 55px;

      svg {
        width: 32px;
        height: 32px;
      }
    }

    .stat-content {
      h3 {
        font-size: 0.8rem;
        margin-bottom: 0.3rem;
      }

      .stat-value {
        font-size: 1.5rem;
      }

      .stat-detail {
        font-size: 0.75rem;
      }
    }
  }

  .maps-progress-card,
  .achievements-card,
  .history-card {
    padding: 1rem;
  }

  .card-title {
    font-size: 1.3rem;
    gap: 0.5rem;

    .title-icon {
      width: 24px;
      height: 24px;
    }
  }

  .maps-list {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .map-progress-item {
    .map-image-container {
      height: 160px;
    }

    .map-info {
      padding: 1rem;
    }

    .map-header {
      h4 {
        font-size: 1.1rem;
      }

      .completion-badge {
        font-size: 0.8rem;
        padding: 0.3rem 0.6rem;
      }
    }

    .map-stats {
      gap: 1rem;
      font-size: 0.85rem;
    }
  }

  .achievements-grid {
    grid-template-columns: 1fr;
  }

  .achievement-item {
    padding: 1rem;

    .achievement-icon {
      font-size: 2rem;
    }

    .achievement-info {
      h4 {
        font-size: 0.95rem;
      }

      p {
        font-size: 0.8rem;
      }
    }
  }

  .history-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.8rem;
    padding: 1rem;

    .game-result {
      width: 100%;
      justify-content: center;
      padding: 0.5rem 1rem;
    }

    .game-details {
      width: 100%;

      p {
        font-size: 0.85rem;
      }
    }
  }
}

/* Small mobile (480px - 600px) */
@media (max-width: 600px) {
  .welcome-title {
    font-size: 1.7rem;
  }

  .welcome-subtitle {
    font-size: 0.95rem;
  }

  .stat-card {
    padding: 0.9rem;

    .stat-icon-wrapper {
      width: 50px;
      height: 50px;

      svg {
        width: 28px;
        height: 28px;
      }
    }

    .stat-content {
      .stat-value {
        font-size: 1.3rem;
      }
    }
  }

  .card-title {
    font-size: 1.2rem;
    flex-direction: column;
    align-items: flex-start;
  }

  .map-progress-item {
    .map-image-container {
      height: 140px;
    }
  }
}

/* Extra small mobile (< 480px) */
@media (max-width: 480px) {
  .dashboard-container {
    padding: 0.8rem;
  }

  .welcome-section {
    margin-bottom: 1.5rem;
  }

  .welcome-title {
    font-size: 1.5rem;
  }

  .welcome-subtitle {
    font-size: 0.9rem;
  }

  .stats-grid {
    gap: 0.8rem;
  }

  .stat-card {
    flex-direction: column;
    text-align: center;
    padding: 1rem;

    .stat-icon-wrapper {
      width: 60px;
      height: 60px;
      margin: 0 auto;

      svg {
        width: 32px;
        height: 32px;
      }
    }

    .stat-content {
      width: 100%;

      .stat-value {
        font-size: 1.6rem;
      }
    }
  }

  .maps-progress-card,
  .achievements-card,
  .history-card {
    padding: 0.8rem;
  }

  .card-title {
    font-size: 1.1rem;
  }

  .map-progress-item {
    .map-image-container {
      height: 120px;
    }

    .map-info {
      padding: 0.8rem;
    }

    .map-header {
      flex-direction: column;
      align-items: flex-start;

      h4 {
        font-size: 1rem;
      }
    }

    .map-stats {
      flex-direction: column;
      gap: 0.5rem;
    }
  }

  .achievement-item {
    flex-direction: column;
    text-align: center;
    padding: 1rem;

    .achievement-icon {
      font-size: 2.5rem;
    }

    .achievement-badge,
    .achievement-lock {
      top: 0.3rem;
      right: 0.3rem;
    }
  }

  .history-item {
    padding: 0.8rem;

    .game-result {
      font-size: 0.9rem;
      padding: 0.4rem 0.8rem;
    }

    .game-details {
      p {
        font-size: 0.8rem;
      }

      .game-date {
        font-size: 0.75rem;
      }
    }
  }
}
</style>
