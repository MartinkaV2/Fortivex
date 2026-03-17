<template>
  <div class="stats-overview">
    <h3 class="section-title">Statisztikai Áttekintés</h3>

    <!-- Main Stats Grid -->
    <div class="main-stats-grid">
      <div class="stat-card primary">
        <div class="card-header">
          <div class="card-icon">👥</div>
          <h4 class="card-title">Játékosok</h4>
        </div>
        <div class="card-body">
          <p class="stat-value">{{ formatNumber(stats?.totalPlayers) }}</p>
          <div class="stat-details">
            <div class="detail-item">
              <span class="detail-label">Online:</span>
              <span class="detail-value online">{{ stats?.onlinePlayers || 0 }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Aktív (ma):</span>
              <span class="detail-value">{{ stats?.activeToday || 0 }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Új (7 nap):</span>
              <span class="detail-value">{{ stats?.newPlayers || 0 }}</span>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="card-header">
          <div class="card-icon">🎮</div>
          <h4 class="card-title">Játékok</h4>
        </div>
        <div class="card-body">
          <p class="stat-value">{{ formatNumber(stats?.totalGames) }}</p>
          <div class="stat-details">
            <div class="detail-item">
              <span class="detail-label">Ma:</span>
              <span class="detail-value">{{ stats?.gamesToday || 0 }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Átlag/nap:</span>
              <span class="detail-value">{{ stats?.avgGamesPerDay || 0 }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Győzelmi arány:</span>
              <span class="detail-value">{{ stats?.globalWinRate || 0 }}%</span>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="card-header">
          <div class="card-icon">⏱️</div>
          <h4 class="card-title">Játékidő</h4>
        </div>
        <div class="card-body">
          <p class="stat-value">{{ formatHours(stats?.totalPlayTime) }}</p>
          <div class="stat-details">
            <div class="detail-item">
              <span class="detail-label">Átlag/játékos:</span>
              <span class="detail-value">{{ formatHours(stats?.avgPlayTimePerPlayer) }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Átlag/játék:</span>
              <span class="detail-value">{{ formatMinutes(stats?.avgPlayTimePerGame) }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Leghosszabb:</span>
              <span class="detail-value">{{ formatMinutes(stats?.longestGame) }}</span>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="card-header">
          <div class="card-icon">💀</div>
          <h4 class="card-title">Ellenségek</h4>
        </div>
        <div class="card-body">
          <p class="stat-value">{{ formatNumber(stats?.totalEnemiesKilled) }}</p>
          <div class="stat-details">
            <div class="detail-item">
              <span class="detail-label">Ma:</span>
              <span class="detail-value">{{ formatNumber(stats?.enemiesKilledToday) }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Átlag/játék:</span>
              <span class="detail-value">{{ stats?.avgEnemiesPerGame || 0 }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">Rekord:</span>
              <span class="detail-value">{{ formatNumber(stats?.mostEnemiesKilled) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Top Players -->
    <div class="top-section">
      <h4 class="subsection-title">🏆 Top Játékosok</h4>
      <div class="top-players-grid">
        <div v-for="(player, index) in stats?.topPlayers || []" :key="player.id" class="top-player-card">
          <div class="rank-badge" :class="`rank-${index + 1}`">
            {{ index + 1 }}
          </div>
          <div class="player-info">
            <h5 class="player-name">{{ player.username }}</h5>
            <div class="player-stats">
              <span class="player-stat">Szint: {{ player.level }}</span>
              <span class="player-stat">Győzelem: {{ player.wins }}</span>
              <span class="player-stat">Winrate: {{ player.winRate }}%</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Most Popular Maps -->
    <div class="top-section">
      <h4 class="subsection-title">🗺️ Legnépszerűbb Pályák</h4>
      <div class="popular-maps">
        <div v-for="map in stats?.popularMaps || []" :key="map.id" class="map-stat-card">
          <div class="map-header">
            <h5 class="map-name">{{ map.name }}</h5>
            <span class="play-count">{{ map.playCount }} játék</span>
          </div>
          <div class="map-bar">
            <div class="bar-fill" :style="{ width: `${getMapPercentage(map.playCount)}%` }"></div>
          </div>
          <div class="map-details">
            <span>Győzelmi arány: {{ map.winRate }}%</span>
            <span>Átlag idő: {{ formatMinutes(map.avgTime) }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Recent Activity -->
    <div class="top-section">
      <h4 class="subsection-title">📊 Aktivitás Trend (Utolsó 7 nap)</h4>
      <div class="activity-chart">
        <div
          v-for="day in stats?.activityTrend || []"
          :key="day.date"
          class="activity-bar"
        >
          <div class="bar-wrapper">
            <div
              class="bar"
              :style="{ height: `${getActivityPercentage(day.players)}%` }"
              :title="`${day.players} játékos`"
            ></div>
          </div>
          <span class="bar-label">{{ formatDayLabel(day.date) }}</span>
          <span class="bar-value">{{ day.players }}</span>
        </div>
      </div>
    </div>

    <!-- System Stats -->
    <div class="system-stats">
      <h4 class="subsection-title">⚙️ Rendszer Információk</h4>
      <div class="system-grid">
        <div class="system-item">
          <span class="system-label">Szerver Státusz:</span>
          <span class="system-value online">🟢 Online</span>
        </div>
        <div class="system-item">
          <span class="system-label">Adatbázis Méret:</span>
          <span class="system-value">{{ stats?.dbSize || '0 MB' }}</span>
        </div>
        <div class="system-item">
          <span class="system-label">API Válaszidő:</span>
          <span class="system-value">{{ stats?.apiResponseTime || '0' }}ms</span>
        </div>
        <div class="system-item">
          <span class="system-label">Utolsó Frissítés:</span>
          <span class="system-value">{{ formatDateTime(stats?.lastUpdate) }}</span>
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

const formatNumber = (num) => {
  if (!num) return '0';
  return num.toLocaleString('hu-HU');
};

const formatHours = (minutes) => {
  if (!minutes) return '0h';
  const hours = Math.floor(minutes / 60);
  const mins = minutes % 60;
  if (hours > 0) {
    return `${hours}h ${mins}m`;
  }
  return `${mins}m`;
};

const formatMinutes = (minutes) => {
  if (!minutes) return '0m';
  return `${Math.round(minutes)}m`;
};

const formatDateTime = (dateString) => {
  if (!dateString) return '-';
  const date = new Date(dateString);
  return date.toLocaleString('hu-HU', {
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

const formatDayLabel = (dateString) => {
  if (!dateString) return '';
  const date = new Date(dateString);
  return date.toLocaleDateString('hu-HU', { weekday: 'short', month: 'numeric', day: 'numeric' });
};

const maxMapPlays = computed(() => {
  const maps = props.stats?.popularMaps || [];
  if (maps.length === 0) return 1;
  return Math.max(...maps.map(m => m.playCount));
});

const getMapPercentage = (playCount) => {
  return (playCount / maxMapPlays.value) * 100;
};

const maxActivity = computed(() => {
  const trend = props.stats?.activityTrend || [];
  if (trend.length === 0) return 1;
  return Math.max(...trend.map(d => d.players));
});

const getActivityPercentage = (players) => {
  return (players / maxActivity.value) * 100;
};
</script>

<style scoped lang="scss">
.stats-overview {
  width: 100%;
}

.section-title {
  color: #ffd700;
  font-size: 1.8rem;
  font-family: 'Cinzel', serif;
  margin-bottom: 2rem;
}

.subsection-title {
  color: #ffd700;
  font-size: 1.3rem;
  margin-bottom: 1.5rem;
}

.main-stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.stat-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-5px);
    border-color: #ffd700;
    box-shadow: 0 10px 30px rgba(255, 215, 0, 0.3);
  }

  &.primary {
    background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
    border-color: #ffd700;
  }

  .card-header {
    display: flex;
    align-items: center;
    gap: 1rem;
    margin-bottom: 1rem;

    .card-icon {
      font-size: 2.5rem;
    }

    .card-title {
      color: #ffd700;
      font-size: 1.1rem;
      font-weight: 600;
    }
  }

  .card-body {
    .stat-value {
      color: #ffd700;
      font-size: 2.5rem;
      font-weight: 700;
      margin-bottom: 1rem;
      line-height: 1;
    }

    .stat-details {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;

      .detail-item {
        display: flex;
        justify-content: space-between;
        padding: 0.5rem;
        background: #1a0f0a;
        border-radius: 6px;

        .detail-label {
          color: rgba(244, 228, 193, 0.7);
          font-size: 0.85rem;
        }

        .detail-value {
          color: #ffd700;
          font-weight: 700;

          &.online {
            color: #32cd32;
          }
        }
      }
    }
  }
}

.top-section {
  margin-bottom: 3rem;
}

.top-players-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
}

.top-player-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
    transform: translateX(5px);
  }

  .rank-badge {
    width: 50px;
    height: 50px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    font-weight: 700;
    border-radius: 50%;
    flex-shrink: 0;

    &.rank-1 {
      background: linear-gradient(135deg, #ffd700, #ffed4e);
      color: #8b4513;
      box-shadow: 0 0 15px rgba(255, 215, 0, 0.7);
    }

    &.rank-2 {
      background: linear-gradient(135deg, #c0c0c0, #e8e8e8);
      color: #666;
    }

    &.rank-3 {
      background: linear-gradient(135deg, #cd7f32, #e89b59);
      color: #fff;
    }

    &:not(.rank-1):not(.rank-2):not(.rank-3) {
      background: #1a0f0a;
      color: #ffd700;
      border: 2px solid #8b4513;
    }
  }

  .player-info {
    flex: 1;

    .player-name {
      color: #ffd700;
      font-size: 1.1rem;
      margin-bottom: 0.5rem;
    }

    .player-stats {
      display: flex;
      gap: 1rem;
      font-size: 0.85rem;

      .player-stat {
        color: rgba(244, 228, 193, 0.8);
      }
    }
  }
}

.popular-maps {
  display: grid;
  gap: 1rem;
}

.map-stat-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
  }

  .map-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;

    .map-name {
      color: #ffd700;
      font-size: 1.1rem;
    }

    .play-count {
      color: rgba(244, 228, 193, 0.8);
      font-size: 0.9rem;
    }
  }

  .map-bar {
    width: 100%;
    height: 8px;
    background: #1a0f0a;
    border-radius: 4px;
    overflow: hidden;
    margin-bottom: 0.8rem;

    .bar-fill {
      height: 100%;
      background: linear-gradient(90deg, #ffd700, #ffed4e);
      transition: width 0.5s ease;
    }
  }

  .map-details {
    display: flex;
    justify-content: space-between;
    color: rgba(244, 228, 193, 0.7);
    font-size: 0.85rem;
  }
}

.activity-chart {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 2rem;
  display: flex;
  justify-content: space-around;
  align-items: flex-end;
  gap: 1rem;
  min-height: 250px;
}

.activity-bar {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;

  .bar-wrapper {
    width: 100%;
    height: 150px;
    display: flex;
    align-items: flex-end;
    justify-content: center;

    .bar {
      width: 80%;
      background: linear-gradient(180deg, #ffd700, #d4af37);
      border-radius: 4px 4px 0 0;
      transition: height 0.5s ease;
      min-height: 10px;
      cursor: pointer;

      &:hover {
        filter: brightness(1.2);
      }
    }
  }

  .bar-label {
    color: #f4e4c1;
    font-size: 0.75rem;
    text-align: center;
  }

  .bar-value {
    color: #ffd700;
    font-weight: 700;
    font-size: 0.9rem;
  }
}

.system-stats {
  background: #1a0f0a;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 2rem;
}

.system-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.system-item {
  display: flex;
  justify-content: space-between;
  padding: 1rem;
  background: #2c1810;
  border-radius: 8px;
  border: 1px solid #8b4513;

  .system-label {
    color: rgba(244, 228, 193, 0.7);
    font-size: 0.9rem;
  }

  .system-value {
    color: #ffd700;
    font-weight: 700;

    &.online {
      color: #32cd32;
    }
  }
}

@media (max-width: 768px) {
  .activity-chart {
    overflow-x: auto;
    justify-content: flex-start;
  }

  .activity-bar {
    min-width: 60px;
  }
}
</style>
