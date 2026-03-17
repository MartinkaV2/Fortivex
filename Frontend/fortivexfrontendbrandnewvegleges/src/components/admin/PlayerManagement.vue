<template>
  <div class="player-management">
    <DataTable
      :data="players"
      :columns="columns"
      :actions="actions"
      :filters="filters"
      :loading="loading"
      :exportable="true"
      search-placeholder="Keresés játékos név, email vagy ID alapján..."
      empty-message="Nincs megjeleníthető játékos"
      @action="handleAction"
      @row-click="viewPlayerDetails"
      @refresh="$emit('refresh')"
    >
      <!-- Custom cell for player name with status -->
      <template #cell-username="{ row }">
        <div class="player-cell">
          <span class="status-indicator" :class="row.isOnline ? 'online' : 'offline'"></span>
          <span class="player-name">{{ row.username }}</span>
          <span v-if="row.isAdmin" class="admin-badge">Admin</span>
        </div>
      </template>

      <!-- Custom cell for level -->
      <template #cell-level="{ row }">
        <div class="level-cell">
          <span class="level-badge" :class="getLevelClass(row.level)">
            {{ row.level }}
          </span>
        </div>
      </template>

      <!-- Custom cell for win rate -->
      <template #cell-winRate="{ row }">
        <div class="winrate-cell">
          <span class="winrate-value" :class="getWinRateClass(row.winRate)">
            {{ row.winRate }}%
          </span>
          <div class="winrate-bar">
            <div class="bar-fill" :style="{ width: `${row.winRate}%` }"></div>
          </div>
        </div>
      </template>
    </DataTable>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import DataTable from './DataTable.vue';

const props = defineProps({
  players: {
    type: Array,
    required: true,
  },
  loading: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits(['action', 'view-player', 'refresh']);

const columns = [
  {
    key: 'id',
    label: 'ID',
    sortable: true,
  },
  {
    key: 'username',
    label: 'Felhasználónév',
    sortable: true,
  },
  {
    key: 'email',
    label: 'Email',
    sortable: true,
  },
  {
    key: 'level',
    label: 'Szint',
    sortable: true,
  },
  {
    key: 'wins',
    label: 'Győzelmek',
    sortable: true,
    format: (value) => value || 0,
  },
  {
    key: 'totalGames',
    label: 'Összes Játék',
    sortable: true,
    format: (value) => value || 0,
  },
  {
    key: 'winRate',
    label: 'Győzelmi Arány',
    sortable: true,
  },
  {
    key: 'lastPlayed',
    label: 'Utolsó Játék',
    sortable: true,
    format: (value) => formatDate(value),
  },
  {
    key: 'createdAt',
    label: 'Regisztráció',
    sortable: true,
    format: (value) => formatDate(value),
  },
];

const actions = [
  {
    name: 'view',
    label: 'Megtekintés',
    icon: '👁️',
    class: 'view',
  },
  {
    name: 'edit',
    label: 'Szerkesztés',
    icon: '✏️',
    class: 'edit',
  },
  {
    name: 'delete',
    label: 'Törlés',
    icon: '🗑️',
    class: 'delete',
  },
];

const filters = [
  {
    label: 'Online játékosok',
    value: 'online',
    fn: (player) => player.isOnline,
  },
  {
    label: 'Adminok',
    value: 'admin',
    fn: (player) => player.isAdmin,
  },
  {
    label: 'Aktív (7 napon belül)',
    value: 'active',
    fn: (player) => {
      if (!player.lastPlayed) return false;
      const lastPlayed = new Date(player.lastPlayed);
      const daysDiff = (new Date() - lastPlayed) / (1000 * 60 * 60 * 24);
      return daysDiff <= 7;
    },
  },
  {
    label: 'Magas szintű (20+)',
    value: 'highLevel',
    fn: (player) => player.level >= 20,
  },
];

const handleAction = ({ action, row }) => {
  emit('action', { action, player: row });
};

const viewPlayerDetails = (player) => {
  emit('view-player', player);
};

const getLevelClass = (level) => {
  if (level >= 50) return 'legendary';
  if (level >= 30) return 'epic';
  if (level >= 15) return 'rare';
  return 'common';
};

const getWinRateClass = (winRate) => {
  if (winRate >= 70) return 'excellent';
  if (winRate >= 50) return 'good';
  if (winRate >= 30) return 'average';
  return 'low';
};

const formatDate = (dateString) => {
  if (!dateString) return '-';
  const date = new Date(dateString);
  return date.toLocaleDateString('hu-HU', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};
</script>

<style scoped lang="scss">
.player-management {
  width: 100%;
}

.player-cell {
  display: flex;
  align-items: center;
  gap: 0.8rem;

  .status-indicator {
    width: 10px;
    height: 10px;
    border-radius: 50%;
    flex-shrink: 0;

    &.online {
      background: #32cd32;
      box-shadow: 0 0 8px #32cd32;
      animation: pulse 2s infinite;
    }

    &.offline {
      background: #888;
    }
  }

  .player-name {
    font-weight: 600;
    color: #ffd700;
  }

  .admin-badge {
    padding: 0.2rem 0.6rem;
    background: rgba(220, 20, 60, 0.2);
    color: #ff6b6b;
    border: 1px solid #dc143c;
    border-radius: 10px;
    font-size: 0.7rem;
    font-weight: 700;
    text-transform: uppercase;
  }
}

@keyframes pulse {
  0%,
  100% {
    opacity: 1;
  }
  50% {
    opacity: 0.5;
  }
}

.level-cell {
  .level-badge {
    display: inline-block;
    padding: 0.4rem 1rem;
    border-radius: 12px;
    font-weight: 700;
    font-size: 1rem;

    &.common {
      background: linear-gradient(135deg, #888, #aaa);
      color: #fff;
    }

    &.rare {
      background: linear-gradient(135deg, #4169e1, #6495ed);
      color: #fff;
      box-shadow: 0 0 10px rgba(65, 105, 225, 0.5);
    }

    &.epic {
      background: linear-gradient(135deg, #9932cc, #ba55d3);
      color: #fff;
      box-shadow: 0 0 10px rgba(153, 50, 204, 0.5);
    }

    &.legendary {
      background: linear-gradient(135deg, #ffd700, #ffed4e);
      color: #8b4513;
      box-shadow: 0 0 15px rgba(255, 215, 0, 0.7);
      animation: shimmer 3s infinite;
    }
  }
}

@keyframes shimmer {
  0%,
  100% {
    filter: brightness(1);
  }
  50% {
    filter: brightness(1.3);
  }
}

.winrate-cell {
  .winrate-value {
    display: block;
    font-weight: 700;
    margin-bottom: 0.3rem;

    &.excellent {
      color: #32cd32;
    }

    &.good {
      color: #90ee90;
    }

    &.average {
      color: #ffa500;
    }

    &.low {
      color: #ff6b6b;
    }
  }

  .winrate-bar {
    width: 100px;
    height: 6px;
    background: #1a0f0a;
    border-radius: 3px;
    overflow: hidden;

    .bar-fill {
      height: 100%;
      background: linear-gradient(90deg, #32cd32, #90ee90);
      transition: width 0.3s ease;
    }
  }
}
</style>