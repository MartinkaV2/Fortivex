<template>
  <div class="player-progress">
    <h3 class="section-title">Haladás & Teljesítmények</h3>

    <!-- Maps Progress -->
    <div class="progress-section">
      <h4 class="subsection-title">Pálya Haladás</h4>
      <div class="maps-progress">
        <div
          v-for="map in progress?.maps || defaultMaps"
          :key="map.id"
          class="map-progress-card"
        >
          <div class="map-header">
            <div class="map-info">
              <h5 class="map-name">{{ map.name }}</h5>
              <span class="completion-status" :class="map.completed ? 'completed' : 'in-progress'">
                {{ map.completed ? '✓ Teljesítve' : '⏳ Folyamatban' }}
              </span>
            </div>
            <div class="map-percentage">
              <span class="percentage-value">{{ map.completionPercent || 0 }}%</span>
            </div>
          </div>

          <div class="progress-bar">
            <div 
              class="progress-fill" 
              :style="{ width: `${map.completionPercent || 0}%` }"
            ></div>
          </div>

          <div class="map-details">
            <div class="detail-item">
              <span class="detail-icon">⏱️</span>
              <span class="detail-text">{{ formatTime(map.bestTime) }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-icon">⭐</span>
              <span class="detail-text">{{ map.stars || 0 }}/3 Csillag</span>
            </div>
            <div class="detail-item">
              <span class="detail-icon">🏆</span>
              <span class="detail-text">{{ map.attempts || 0 }} Próbálkozás</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Achievements -->
    <div class="progress-section">
      <h4 class="subsection-title">
        Teljesítmények
        <span class="achievement-counter">
          {{ unlockedCount }}/{{ totalAchievements }}
        </span>
      </h4>
      <div class="achievements-grid">
        <div
          v-for="achievement in progress?.achievements || defaultAchievements"
          :key="achievement.id"
          class="achievement-card"
          :class="{ unlocked: achievement.unlocked }"
        >
          <div class="achievement-icon-wrapper">
            <span class="achievement-icon">{{ achievement.icon }}</span>
            <div v-if="achievement.unlocked" class="unlock-badge">✓</div>
          </div>
          <div class="achievement-content">
            <h5 class="achievement-name">{{ achievement.name }}</h5>
            <p class="achievement-description">{{ achievement.description }}</p>
            <div v-if="achievement.progress !== undefined" class="achievement-progress">
              <div class="progress-bar small">
                <div 
                  class="progress-fill" 
                  :style="{ width: `${achievement.progress}%` }"
                ></div>
              </div>
              <span class="progress-label">{{ achievement.progress }}%</span>
            </div>
            <div v-if="achievement.unlocked" class="unlock-date">
              Feloldva: {{ formatDate(achievement.unlockedAt) }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Statistics Overview -->
    <div class="progress-section">
      <h4 class="subsection-title">Általános Áttekintés</h4>
      <div class="overview-grid">
        <div class="overview-card">
          <div class="overview-icon">📊</div>
          <div class="overview-content">
            <span class="overview-label">Összes Teljesítés</span>
            <span class="overview-value">{{ totalProgress }}%</span>
          </div>
          <div class="overview-bar">
            <div class="bar-fill" :style="{ width: `${totalProgress}%` }"></div>
          </div>
        </div>

        <div class="overview-card">
          <div class="overview-icon">🎯</div>
          <div class="overview-content">
            <span class="overview-label">Pályák Befejezve</span>
            <span class="overview-value">{{ completedMaps }}/{{ totalMaps }}</span>
          </div>
          <div class="overview-bar">
            <div class="bar-fill" :style="{ width: `${(completedMaps / totalMaps) * 100}%` }"></div>
          </div>
        </div>

        <div class="overview-card">
          <div class="overview-icon">⭐</div>
          <div class="overview-content">
            <span class="overview-label">Összegyűjtött Csillagok</span>
            <span class="overview-value">{{ totalStars }}/{{ maxStars }}</span>
          </div>
          <div class="overview-bar">
            <div class="bar-fill" :style="{ width: `${(totalStars / maxStars) * 100}%` }"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  progress: {
    type: Object,
    default: () => ({}),
  },
});

const defaultMaps = [
  {
    id: 1,
    name: 'Vad Erdő',
    completed: true,
    completionPercent: 100,
    bestTime: 720,
    stars: 3,
    attempts: 5,
  },
  {
    id: 2,
    name: 'Sivatagi Végvár',
    completed: true,
    completionPercent: 100,
    bestTime: 1080,
    stars: 2,
    attempts: 8,
  },
  {
    id: 3,
    name: 'Jégmező',
    completed: false,
    completionPercent: 65,
    bestTime: null,
    stars: 0,
    attempts: 3,
  },
];

const defaultAchievements = [
  {
    id: 1,
    name: 'Első Győzelem',
    description: 'Nyerd meg az első pályádat',
    icon: '🏆',
    unlocked: true,
    unlockedAt: '2024-11-15',
  },
  {
    id: 2,
    name: 'Torony Mester',
    description: 'Építs 100 tornyot',
    icon: '🏗️',
    unlocked: true,
    progress: 100,
    unlockedAt: '2024-11-20',
  },
  {
    id: 3,
    name: 'Ellenség Vadász',
    description: 'Győzz le 1000 ellenséget',
    icon: '💀',
    unlocked: false,
    progress: 67,
  },
  {
    id: 4,
    name: 'Arany Gyűjtő',
    description: 'Gyűjts össze 50,000 aranyat',
    icon: '💰',
    unlocked: false,
    progress: 45,
  },
];

const totalAchievements = computed(() => {
  return (props.progress?.achievements || defaultAchievements).length;
});

const unlockedCount = computed(() => {
  return (props.progress?.achievements || defaultAchievements).filter(a => a.unlocked).length;
});

const totalMaps = computed(() => {
  return (props.progress?.maps || defaultMaps).length;
});

const completedMaps = computed(() => {
  return (props.progress?.maps || defaultMaps).filter(m => m.completed).length;
});

const totalStars = computed(() => {
  return (props.progress?.maps || defaultMaps).reduce((sum, map) => sum + (map.stars || 0), 0);
});

const maxStars = computed(() => {
  return totalMaps.value * 3;
});

const totalProgress = computed(() => {
  const maps = props.progress?.maps || defaultMaps;
  if (maps.length === 0) return 0;
  const avg = maps.reduce((sum, map) => sum + (map.completionPercent || 0), 0) / maps.length;
  return Math.round(avg);
});

const formatTime = (seconds) => {
  if (!seconds) return 'Nincs';
  const mins = Math.floor(seconds / 60);
  const secs = seconds % 60;
  return `${mins}:${secs.toString().padStart(2, '0')}`;
};

const formatDate = (dateString) => {
  if (!dateString) return '';
  const date = new Date(dateString);
  return date.toLocaleDateString('hu-HU', { year: 'numeric', month: 'long', day: 'numeric' });
};
</script>

<style scoped lang="scss">
.player-progress {
  width: 100%;
}

.section-title {
  color: #ffd700;
  font-size: 1.8rem;
  font-family: 'Cinzel', serif;
  margin-bottom: 2rem;
}

.progress-section {
  margin-bottom: 3rem;

  &:last-child {
    margin-bottom: 0;
  }
}

.subsection-title {
  color: #ffd700;
  font-size: 1.3rem;
  margin-bottom: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;

  .achievement-counter {
    font-size: 1rem;
    color: #f4e4c1;
    background: #2c1810;
    padding: 0.3rem 0.8rem;
    border-radius: 12px;
    border: 1px solid #8b4513;
  }
}

.maps-progress {
  display: grid;
  gap: 1.5rem;
}

.map-progress-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
    transform: translateX(5px);
  }
}

.map-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1rem;
}

.map-info {
  flex: 1;

  .map-name {
    color: #ffd700;
    font-size: 1.2rem;
    margin-bottom: 0.5rem;
  }

  .completion-status {
    display: inline-block;
    padding: 0.3rem 0.8rem;
    border-radius: 12px;
    font-size: 0.8rem;
    font-weight: 600;

    &.completed {
      background: rgba(50, 205, 50, 0.2);
      color: #32cd32;
      border: 1px solid #32cd32;
    }

    &.in-progress {
      background: rgba(255, 165, 0, 0.2);
      color: #ffa500;
      border: 1px solid #ffa500;
    }
  }
}

.map-percentage {
  .percentage-value {
    font-size: 1.5rem;
    font-weight: 700;
    color: #ffd700;
  }
}

.progress-bar {
  width: 100%;
  height: 12px;
  background: #1a0f0a;
  border-radius: 6px;
  overflow: hidden;
  margin-bottom: 1rem;

  &.small {
    height: 6px;
  }

  .progress-fill {
    height: 100%;
    background: linear-gradient(90deg, #ffd700, #ffed4e);
    transition: width 0.5s ease;
    box-shadow: 0 0 10px rgba(255, 215, 0, 0.5);
  }
}

.map-details {
  display: flex;
  gap: 2rem;
  flex-wrap: wrap;

  .detail-item {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    color: #f4e4c1;
    font-size: 0.9rem;

    .detail-icon {
      font-size: 1.2rem;
    }
  }
}

.achievements-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

.achievement-card {
  background: #1a0f0a;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  gap: 1rem;
  opacity: 0.6;
  transition: all 0.3s ease;
  position: relative;

  &.unlocked {
    opacity: 1;
    border-color: #ffd700;

    &::before {
      content: '';
      position: absolute;
      top: -2px;
      left: -2px;
      right: -2px;
      bottom: -2px;
      background: linear-gradient(135deg, transparent, rgba(255, 215, 0, 0.1));
      border-radius: 12px;
      z-index: 0;
    }
  }

  &:hover {
    transform: translateY(-3px);
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.3);
  }

  > * {
    position: relative;
    z-index: 1;
  }
}

.achievement-icon-wrapper {
  position: relative;
  flex-shrink: 0;

  .achievement-icon {
    font-size: 3rem;
    display: block;
  }

  .unlock-badge {
    position: absolute;
    bottom: -5px;
    right: -5px;
    width: 24px;
    height: 24px;
    background: #32cd32;
    border-radius: 50%;
    border: 2px solid #1a0f0a;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    font-size: 0.8rem;
    font-weight: 700;
  }
}

.achievement-content {
  flex: 1;

  .achievement-name {
    color: #ffd700;
    font-size: 1.1rem;
    margin-bottom: 0.5rem;
  }

  .achievement-description {
    color: #f4e4c1;
    font-size: 0.9rem;
    line-height: 1.5;
    margin-bottom: 0.8rem;
  }

  .achievement-progress {
    display: flex;
    align-items: center;
    gap: 0.8rem;
    margin-bottom: 0.5rem;

    .progress-bar {
      flex: 1;
      margin: 0;
    }

    .progress-label {
      color: #ffd700;
      font-size: 0.85rem;
      font-weight: 700;
      min-width: 40px;
      text-align: right;
    }
  }

  .unlock-date {
    color: rgba(244, 228, 193, 0.7);
    font-size: 0.8rem;
    font-style: italic;
  }
}

.overview-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
}

.overview-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
    transform: translateY(-3px);
  }

  .overview-icon {
    font-size: 2.5rem;
  }

  .overview-content {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .overview-label {
      color: #f4e4c1;
      font-size: 0.9rem;
    }

    .overview-value {
      color: #ffd700;
      font-size: 1.5rem;
      font-weight: 700;
    }
  }

  .overview-bar {
    width: 100%;
    height: 8px;
    background: #1a0f0a;
    border-radius: 4px;
    overflow: hidden;

    .bar-fill {
      height: 100%;
      background: linear-gradient(90deg, #32cd32, #90ee90);
      transition: width 0.5s ease;
    }
  }
}

@media (max-width: 768px) {
  .achievements-grid {
    grid-template-columns: 1fr;
  }

  .map-details {
    flex-direction: column;
    gap: 0.8rem;
  }
}
</style>