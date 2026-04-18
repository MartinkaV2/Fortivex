<template>
  <div class="player-stats">
    <div class="stats-header">
      <h3 class="stats-title">Statisztikák</h3>
      <button @click="$emit('refresh')" class="refresh-btn">
        🔄 Frissítés
      </button>
    </div>

    <div class="stats-grid">
      <!-- Level Card -->
      <div class="stat-card level-card">
        <div class="card-icon">🏆</div>
        <div class="card-content">
          <h4 class="card-title">Szint</h4>
          <p class="card-value">{{ stats?.level || 1 }}</p>
          <div class="progress-wrapper">
            <div class="progress-bar">
              <div class="progress-fill" :style="{ width: `${xpProgress}%` }"></div>
            </div>
            <span class="progress-text">
              {{ stats?.currentXp || 0 }} / {{ stats?.nextLevelXp || 100 }} XP
            </span>
          </div>
        </div>
      </div>

      <!-- Games Stats -->
      <div class="stat-card">
        <div class="card-icon">🎮</div>
        <div class="card-content">
          <h4 class="card-title">Játékok</h4>
          <div class="multi-stats">
            <div class="mini-stat">
              <span class="mini-label">Összes</span>
              <span class="mini-value">{{ stats?.totalGames || 0 }}</span>
            </div>
            <div class="mini-stat">
              <span class="mini-label">Győzelem</span>
              <span class="mini-value win">{{ stats?.wins || 0 }}</span>
            </div>
            <div class="mini-stat">
              <span class="mini-label">Vereség</span>
              <span class="mini-value loss">{{ stats?.losses || 0 }}</span>
            </div>
          </div>
          <div class="win-rate">
            <span>Győzelmi arány:</span>
            <span class="rate-value">{{ winRate }}%</span>
          </div>
        </div>
      </div>

      <!-- Gold Stats -->
      <div class="stat-card">
        <div class="card-icon">💰</div>
        <div class="card-content">
          <h4 class="card-title">Arany</h4>
          <p class="card-value">{{ formatNumber(stats?.currentGold) }}</p>
          <p class="card-detail">
            Összesen: {{ formatNumber(stats?.totalGold) }}
          </p>
        </div>
      </div>

      <!-- Enemies Killed -->
      <div class="stat-card">
        <div class="card-icon">💀</div>
        <div class="card-content">
          <h4 class="card-title">Legyőzött Ellenségek</h4>
          <p class="card-value">{{ formatNumber(stats?.enemiesKilled) }}</p>
          <p class="card-detail">
            Átlag/játék: {{ averageKillsPerGame }}
          </p>
        </div>
      </div>

      <!-- Waves -->
      <div class="stat-card">
        <div class="card-icon">🌊</div>
        <div class="card-content">
          <h4 class="card-title">Hullámok</h4>
          <p class="card-value">{{ stats?.maxWaveReached || 0 }}</p>
          <p class="card-detail">Legjobb eredmény</p>
        </div>
      </div>

      <!-- Playtime -->
      <div class="stat-card">
        <div class="card-icon">⏱️</div>
        <div class="card-content">
          <h4 class="card-title">Játékidő</h4>
          <p class="card-value">{{ formatPlayTime(stats?.totalPlayTime) }}</p>
          <p class="card-detail">
            Átlag: {{ formatPlayTime(stats?.avgPlayTime) }}/játék
          </p>
        </div>
      </div>

      <!-- Towers Built -->
      <div class="stat-card">
        <div class="card-icon">🏗️</div>
        <div class="card-content">
          <h4 class="card-title">Tornyok</h4>
          <p class="card-value">{{ formatNumber(stats?.towersBuilt) }}</p>
          <p class="card-detail">Összesen építve</p>
        </div>
      </div>

      <!-- Damage Dealt -->
      <div class="stat-card">
        <div class="card-icon">⚔️</div>
        <div class="card-content">
          <h4 class="card-title">Sebzés</h4>
          <p class="card-value">{{ formatNumber(stats?.totalDamage) }}</p>
          <p class="card-detail">Összes okozott sebzés</p>
        </div>
      </div>
    </div>

    <!-- Quick Stats Bar -->
    <div class="quick-stats">
      <div class="quick-stat-item">
        <span class="quick-icon">📅</span>
        <div class="quick-info">
          <span class="quick-label">Utoljára játszott</span>
          <span class="quick-value">{{ formatDate(stats?.lastPlayed) }}</span>
        </div>
      </div>
      <div class="quick-stat-item">
        <span class="quick-icon">🎯</span>
        <div class="quick-info">
          <span class="quick-label">Kedvenc pálya</span>
          <span class="quick-value">{{ stats?.favoriteMap || 'Nincs' }}</span>
        </div>
      </div>
      <div class="quick-stat-item">
        <span class="quick-icon">🔥</span>
        <div class="quick-info">
          <span class="quick-label">Győzelmi sorozat</span>
          <span class="quick-value">{{ stats?.winStreak || 0 }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  stats: {
    type: Object,
    default: () => ({}),
  },
});

defineEmits(['refresh']);

const xpProgress = computed(() => {
  if (!props.stats) return 0;
  const current = props.stats.currentXp || 0;
  const needed = props.stats.nextLevelXp || 100;
  return Math.min((current / needed) * 100, 100);
});

const winRate = computed(() => {
  const total = props.stats?.totalGames || 0;
  const wins = props.stats?.wins || 0;
  if (total === 0) return 0;
  return Math.round((wins / total) * 100);
});

const averageKillsPerGame = computed(() => {
  const total = props.stats?.totalGames || 0;
  const kills = props.stats?.enemiesKilled || 0;
  if (total === 0) return 0;
  return Math.round(kills / total);
});

const formatNumber = (num) => {
  if (!num) return '0';
  return num.toLocaleString('hu-HU');
};

const formatPlayTime = (minutes) => {
  if (!minutes) return '0m';
  const hours = Math.floor(minutes / 60);
  const mins = minutes % 60;
  if (hours > 0) {
    return `${hours}h ${mins}m`;
  }
  return `${mins}m`;
};

const formatDate = (dateString) => {
  if (!dateString) return 'Soha';
  const date = new Date(dateString);
  const now = new Date();
  const diffMs = now - date;
  const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));
  
  if (diffDays === 0) return 'Ma';
  if (diffDays === 1) return 'Tegnap';
  if (diffDays < 7) return `${diffDays} napja`;
  
  return date.toLocaleDateString('hu-HU', { month: 'short', day: 'numeric' });
};
</script>

<style scoped lang="scss">
.player-stats {
  width: 100%;
}

.stats-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;

  .stats-title {
    color: #ffd700;
    font-size: 1.8rem;
    font-family: 'Cinzel', serif;
  }

  .refresh-btn {
    padding: 0.6rem 1.2rem;
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
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  gap: 1rem;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    border-color: #ffd700;
    box-shadow: 0 8px 20px rgba(255, 215, 0, 0.3);
  }

  &.level-card {
    grid-column: span 2;
    background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
    border-color: #ffd700;

    @media (max-width: 768px) {
      grid-column: span 1;
    }
  }

  .card-icon {
    font-size: 2.5rem;
    flex-shrink: 0;
  }

  .card-content {
    flex: 1;

    .card-title {
      color: #f4e4c1;
      font-size: 0.85rem;
      text-transform: uppercase;
      letter-spacing: 1px;
      margin-bottom: 0.5rem;
    }

    .card-value {
      color: #ffd700;
      font-size: 2rem;
      font-weight: 700;
      margin-bottom: 0.5rem;
      line-height: 1;
    }

    .card-detail {
      color: rgba(244, 228, 193, 0.7);
      font-size: 0.85rem;
      margin: 0;
    }
  }
}

.progress-wrapper {
  margin-top: 0.8rem;

  .progress-bar {
    width: 100%;
    height: 10px;
    background: #1a0f0a;
    border-radius: 5px;
    overflow: hidden;
    margin-bottom: 0.5rem;

    .progress-fill {
      height: 100%;
      background: linear-gradient(90deg, #ffd700, #ffed4e);
      transition: width 0.5s ease;
      box-shadow: 0 0 10px rgba(255, 215, 0, 0.5);
    }
  }

  .progress-text {
    color: rgba(244, 228, 193, 0.9);
    font-size: 0.85rem;
  }
}

.multi-stats {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 0.8rem;
  margin-bottom: 0.8rem;

  .mini-stat {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 0.5rem;
    background: #1a0f0a;
    border-radius: 8px;

    .mini-label {
      color: rgba(244, 228, 193, 0.7);
      font-size: 0.7rem;
      text-transform: uppercase;
      margin-bottom: 0.3rem;
    }

    .mini-value {
      color: #ffd700;
      font-size: 1.3rem;
      font-weight: 700;

      &.win {
        color: #32cd32;
      }

      &.loss {
        color: #ff6b6b;
      }
    }
  }
}

.win-rate {
  display: flex;
  justify-content: space-between;
  padding: 0.5rem;
  background: #1a0f0a;
  border-radius: 8px;
  font-size: 0.85rem;

  span {
    color: #f4e4c1;
  }

  .rate-value {
    color: #ffd700;
    font-weight: 700;
  }
}

.quick-stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1rem;
  padding: 1.5rem;
  background: #1a0f0a;
  border: 2px solid #8b4513;
  border-radius: 12px;
}

.quick-stat-item {
  display: flex;
  align-items: center;
  gap: 1rem;

  .quick-icon {
    font-size: 2rem;
  }

  .quick-info {
    display: flex;
    flex-direction: column;

    .quick-label {
      color: rgba(244, 228, 193, 0.7);
      font-size: 0.8rem;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .quick-value {
      color: #ffd700;
      font-weight: 700;
      font-size: 1.1rem;
    }
  }
}

@media (max-width: 640px) {
  .stats-grid {
    grid-template-columns: 1fr;
  }

  .quick-stats {
    grid-template-columns: 1fr;
  }
}
</style>