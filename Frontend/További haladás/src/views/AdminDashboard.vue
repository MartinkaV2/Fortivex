<template>
  <div class="admin-dashboard">
    <Navbar />

    <div class="dashboard-container">
      <div class="header-section">
        <h1 class="page-title">Admin Panel</h1>
        <p class="page-subtitle">Játékosok és statisztikák kezelése</p>
      </div>

      <!-- Összesített statisztikák -->
      <div class="overview-grid">
        <div class="overview-card">
          <div class="card-icon">👥</div>
          <div class="card-content">
            <h3>Összes Játékos</h3>
            <p class="card-value">{{ allStats?.totalPlayers || 0 }}</p>
            <p class="card-detail">Aktív: {{ allStats?.activePlayers || 0 }}</p>
          </div>
        </div>

        <div class="overview-card">
          <div class="card-icon">🎮</div>
          <div class="card-content">
            <h3>Összes Menet</h3>
            <p class="card-value">{{ formatNumber(allStats?.totalGames) }}</p>
            <p class="card-detail">Ma: {{ allStats?.gamesToday || 0 }}</p>
          </div>
        </div>

        <div class="overview-card">
          <div class="card-icon">⏱️</div>
          <div class="card-content">
            <h3>Játékidő</h3>
            <p class="card-value">{{ formatHours(allStats?.totalPlayTime) }}</p>
            <p class="card-detail">Átlag: {{ formatMinutes(allStats?.avgPlayTime) }}</p>
          </div>
        </div>

        <div class="overview-card">
          <div class="card-icon">💀</div>
          <div class="card-content">
            <h3>Legyőzött Ellenségek</h3>
            <p class="card-value">{{ formatNumber(allStats?.totalEnemiesKilled) }}</p>
            <p class="card-detail">Rekord: {{ allStats?.maxWaveRecordName || "-" }}</p>
          </div>
        </div>
      </div>

      <!-- Játékosok táblázat -->
      <div class="players-section">
        <div class="section-header">
          <h2 class="section-title">Játékosok</h2>
          <div class="section-controls">
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Keresés..."
              class="search-input"
            />
            <button @click="loadPlayers" class="refresh-button">🔄 Frissítés</button>
          </div>
        </div>

        <div v-if="loading" class="loading">
          <div class="spinner"></div>
          <p>Betöltés...</p>
        </div>

        <div v-else-if="error" class="error-box">
          <p>{{ error }}</p>
          <button @click="loadPlayers" class="retry-button">Újrapróbálás</button>
        </div>

        <div v-else class="table-container">
          <table class="players-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Felhasználónév</th>
                <th>Email</th>
                <th>Szint</th>
                <th>Győzelmek</th>
                <th>Utolsó Játék</th>
                <th>Regisztráció</th>
                <th>Műveletek</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="player in filteredPlayers" :key="player.id">
                <td>{{ player.id }}</td>
                <td class="player-name">
                  <span class="status-dot" :class="player.isOnline ? 'online' : 'offline'"></span>
                  {{ player.username }}
                </td>
                <td>{{ player.email }}</td>
                <td>
                  <span class="level-badge">{{ player.level }}</span>
                </td>
                <td>{{ player.wins || 0 }}</td>
                <td>{{ formatDate(player.lastLogin) }}</td>
                <td>{{ formatDate(player.createdAt) }}</td>
                <td class="actions-cell">
                  <div class="actions-wrapper">
                    <button
                      @click="viewPlayer(player)"
                      class="action-button view"
                      title="Megtekintés"
                    >
                      👁️
                    </button>
                    <button
                      @click="editPlayer(player)"
                      class="action-button edit"
                      title="Szerkesztés"
                    >
                      ✏️
                    </button>
                    <button
                      @click="confirmDelete(player)"
                      class="action-button delete"
                      title="Törlés"
                    >
                      🗑️
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>

          <div v-if="filteredPlayers.length === 0" class="empty-state">
            <p>Nincs megjeleníthető játékos</p>
          </div>
        </div>
      </div>

      <!-- Játékos részletek modal -->
      <div v-if="selectedPlayer" class="modal-overlay" @click.self="closeModal">
        <div class="modal-content large">
          <button class="close-button" @click="closeModal">✕</button>
          <h2 class="modal-title">{{ selectedPlayer.username }} - Részletek</h2>
          <div class="player-details-grid">
            <div class="detail-section">
              <h3>Alapinformációk</h3>
              <div class="detail-row">
                <span class="label">ID:</span>
                <span class="value">{{ selectedPlayer.id }}</span>
              </div>
              <div class="detail-row">
                <span class="label">Email:</span>
                <span class="value">{{ selectedPlayer.email }}</span>
              </div>
              <div class="detail-row">
                <span class="label">Szint:</span>
                <span class="value">{{ selectedPlayer.level }}</span>
              </div>
              <div class="detail-row">
                <span class="label">Regisztráció:</span>
                <span class="value">{{ formatDate(selectedPlayer.createdAt) }}</span>
              </div>
            </div>
            <div class="detail-section">
              <h3>Játék Statisztikák</h3>
              <div class="detail-row">
                <span class="label">Győzelmek:</span>
                <span class="value">{{ selectedPlayer.wins || 0 }}</span>
              </div>
              <div class="detail-row">
                <span class="label">Összes játék:</span>
                <span class="value">{{ selectedPlayer.totalGames || 0 }}</span>
              </div>
              <div class="detail-row">
                <span class="label">Legyőzött ellenségek:</span>
                <span class="value">{{ formatNumber(selectedPlayer.enemiesKilled) }}</span>
              </div>
              <div class="detail-row">
                <span class="label">Összegyűjtött arany:</span>
                <span class="value">{{ formatNumber(selectedPlayer.totalGold) }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- ✅ ÚJ: Szerkesztő modal -->
      <div v-if="editingPlayer" class="modal-overlay" @click.self="cancelEdit">
        <div class="modal-content">
          <button class="close-button" @click="cancelEdit">✕</button>
          <h2 class="modal-title">✏️ Játékos Szerkesztése</h2>

          <div class="edit-form">
            <div class="form-group">
              <label>Felhasználónév</label>
              <input v-model="editingPlayer.username" type="text" class="form-input" />
            </div>
            <div class="form-group">
              <label>Email</label>
              <input v-model="editingPlayer.email" type="email" class="form-input" />
            </div>
          </div>

          <div class="modal-actions">
            <button @click="cancelEdit" class="modal-button cancel">Mégse</button>
            <button @click="saveEdit" class="modal-button save">Mentés</button>
          </div>
        </div>
      </div>

      <!-- Törlés megerősítés modal -->
      <div v-if="deleteConfirmPlayer" class="modal-overlay" @click.self="cancelDelete">
        <div class="modal-content">
          <h2 class="modal-title warning">Játékos Törlése</h2>
          <p class="confirmation-text">
            Biztosan törölni szeretnéd
            <strong>{{ deleteConfirmPlayer.username }}</strong> játékost? Ez a művelet nem vonható
            vissza!
          </p>
          <div class="modal-actions">
            <button @click="cancelDelete" class="modal-button cancel">Mégse</button>
            <button @click="deletePlayer" class="modal-button delete">Törlés</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useAdminStore } from "@/stores/admin";
import Navbar from "@/components/common/Navbar.vue";

const adminStore = useAdminStore();

const searchQuery = ref("");
const selectedPlayer = ref(null);
const deleteConfirmPlayer = ref(null);
const editingPlayer = ref(null);

const players = computed(() => adminStore.players);
const allStats = computed(() => adminStore.allStats);
const loading = computed(() => adminStore.loading);
const error = computed(() => adminStore.error);

// ✅ JAVÍTVA: opcionális láncolás null email esetén
const filteredPlayers = computed(() => {
  if (!searchQuery.value) return players.value;

  const query = searchQuery.value.toLowerCase();
  return players.value.filter(
    (player) =>
      player.username?.toLowerCase().includes(query) ||
      player.email?.toLowerCase().includes(query) ||
      player.id?.toString().includes(query),
  );
});

const formatNumber = (num) => {
  if (!num) return "0";
  return num.toLocaleString("hu-HU");
};

const formatDate = (dateString) => {
  if (!dateString) return "-";
  const date = new Date(dateString);
  return date.toLocaleDateString("hu-HU", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
};

const formatHours = (minutes) => {
  if (!minutes) return "0h";
  const hours = Math.floor(minutes / 60);
  return `${hours}h`;
};

const formatMinutes = (minutes) => {
  if (!minutes) return "0m";
  return `${Math.round(minutes)}m`;
};

// ✅ JAVÍTVA: egymás után fut, nem párhuzamosan
const loadPlayers = async () => {
  await adminStore.fetchAllPlayers();
  try {
    await adminStore.fetchAllStats();
  } catch {}
};

const viewPlayer = (player) => {
  selectedPlayer.value = player;
};

const closeModal = () => {
  selectedPlayer.value = null;
};

// ✅ JAVÍTVA: szerkesztő modal
const editPlayer = (player) => {
  editingPlayer.value = { ...player };
};

const saveEdit = async () => {
  if (editingPlayer.value) {
    const success = await adminStore.updatePlayer(editingPlayer.value.id, editingPlayer.value);
    if (success) {
      editingPlayer.value = null;
      await adminStore.fetchAllPlayers();
    }
  }
};

const cancelEdit = () => {
  editingPlayer.value = null;
};

const confirmDelete = (player) => {
  deleteConfirmPlayer.value = player;
};

const deletePlayer = async () => {
  if (deleteConfirmPlayer.value) {
    const success = await adminStore.deletePlayer(deleteConfirmPlayer.value.id);
    if (success) {
      deleteConfirmPlayer.value = null;
    }
  }
};

const cancelDelete = () => {
  deleteConfirmPlayer.value = null;
};

onMounted(() => {
  loadPlayers();
});
</script>

<style scoped lang="scss">
.admin-dashboard {
  min-height: 100vh;
  background: linear-gradient(135deg, #1a0f0a 0%, #2c1810 100%);
}

.dashboard-container {
  max-width: 1600px;
  margin: 0 auto;
  padding: 2rem;
}

.header-section {
  text-align: center;
  margin-bottom: 3rem;
}

.page-title {
  font-size: 3rem;
  color: #ffd700;
  font-family: "Cinzel", serif;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
  margin-bottom: 0.5rem;
}

.page-subtitle {
  color: #f4e4c1;
  font-size: 1.2rem;
}

.overview-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.overview-card {
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

  .card-icon {
    font-size: 3rem;
    flex-shrink: 0;
  }

  .card-content {
    h3 {
      color: #f4e4c1;
      font-size: 0.9rem;
      margin-bottom: 0.5rem;
      text-transform: uppercase;
    }

    .card-value {
      color: #ffd700;
      font-size: 1.8rem;
      font-weight: 700;
      margin-bottom: 0.3rem;
    }

    .card-detail {
      color: rgba(244, 228, 193, 0.7);
      font-size: 0.85rem;
    }
  }
}

.players-section {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  padding: 2rem;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.section-title {
  color: #ffd700;
  font-size: 1.8rem;
  font-family: "Cinzel", serif;
}

.section-controls {
  display: flex;
  gap: 1rem;
}

.search-input {
  padding: 0.6rem 1rem;
  border: 2px solid #8b4513;
  border-radius: 8px;
  background: #1a0f0a;
  color: #f4e4c1;
  font-size: 1rem;
  min-width: 250px;

  &:focus {
    outline: none;
    border-color: #ffd700;
  }

  &::placeholder {
    color: rgba(244, 228, 193, 0.5);
  }
}

.refresh-button {
  padding: 0.6rem 1.2rem;
  background: #8b4513;
  color: #fff;
  border: 2px solid #ffd700;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  white-space: nowrap;

  &:hover {
    background: #a0522d;
    transform: translateY(-2px);
  }
}

.loading {
  text-align: center;
  padding: 3rem;

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
    }
  }
}

.table-container {
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
}

.players-table {
  width: 100%;
  border-collapse: collapse;
  min-width: 700px; // Táblázat minimum szélesség, alatta scroll

  thead {
    background: #1a0f0a;

    th {
      padding: 1rem;
      text-align: left;
      color: #ffd700;
      font-weight: 700;
      text-transform: uppercase;
      font-size: 0.85rem;
      letter-spacing: 1px;
      border-bottom: 2px solid #8b4513;
      white-space: nowrap;
    }
  }

  tbody {
    tr {
      border-bottom: 1px solid rgba(139, 69, 19, 0.3);
      transition: all 0.2s ease;

      &:hover {
        background: rgba(255, 215, 0, 0.05);
      }

      td {
        padding: 1rem;
        color: #f4e4c1;
      }
    }
  }

  .player-name {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    font-weight: 600;

    .status-dot {
      width: 8px;
      height: 8px;
      border-radius: 50%;
      flex-shrink: 0;

      &.online {
        background: #32cd32;
        box-shadow: 0 0 8px #32cd32;
      }

      &.offline {
        background: #888;
      }
    }
  }

  .level-badge {
    background: linear-gradient(135deg, #8b4513, #a0522d);
    color: #ffd700;
    padding: 0.3rem 0.8rem;
    border-radius: 12px;
    font-weight: 700;
    font-size: 0.9rem;
    white-space: nowrap;
  }

  .actions-cell {
    .actions-wrapper {
      display: flex;
      gap: 0.5rem;
      align-items: center;
    }
  }

  .action-button {
    padding: 0.4rem 0.6rem;
    background: transparent;
    border: 1px solid #8b4513;
    border-radius: 6px;
    cursor: pointer;
    font-size: 1rem;
    transition: all 0.2s ease;

    &.view:hover {
      background: #4169e1;
      border-color: #4169e1;
    }

    &.edit:hover {
      background: #ffa500;
      border-color: #ffa500;
    }

    &.delete:hover {
      background: #dc143c;
      border-color: #dc143c;
    }
  }
}

.empty-state {
  text-align: center;
  padding: 3rem;
  color: #f4e4c1;
  font-size: 1.1rem;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-content {
  background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
  border: 3px solid #ffd700;
  border-radius: 16px;
  padding: 2.5rem;
  max-width: 500px;
  width: 100%;
  position: relative;
  max-height: 90vh;
  overflow-y: auto;

  &.large {
    max-width: 800px;
  }
}

.close-button {
  position: absolute;
  top: 1rem;
  right: 1rem;
  background: transparent;
  border: none;
  color: #ffd700;
  font-size: 1.5rem;
  cursor: pointer;
  transition: all 0.2s ease;

  &:hover {
    transform: rotate(90deg);
    color: #fff;
  }
}

.modal-title {
  color: #ffd700;
  font-size: 1.8rem;
  margin-bottom: 1.5rem;
  font-family: "Cinzel", serif;

  &.warning {
    color: #ff6b6b;
  }
}

.player-details-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.detail-section {
  h3 {
    color: #ffd700;
    font-size: 1.2rem;
    margin-bottom: 1rem;
    padding-bottom: 0.5rem;
    border-bottom: 2px solid #8b4513;
  }
}

.detail-row {
  display: flex;
  justify-content: space-between;
  padding: 0.8rem 0;
  border-bottom: 1px solid rgba(139, 69, 19, 0.3);
  gap: 1rem;

  .label {
    color: #f4e4c1;
    font-weight: 600;
  }

  .value {
    color: #ffd700;
    text-align: right;
  }
}

.edit-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
  margin-bottom: 2rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;

  label {
    color: #f4e4c1;
    font-weight: 600;
    font-size: 0.95rem;
  }
}

.form-input {
  padding: 0.7rem 1rem;
  background: #1a0f0a;
  border: 2px solid #8b4513;
  border-radius: 8px;
  color: #f4e4c1;
  font-size: 1rem;

  &:focus {
    outline: none;
    border-color: #ffd700;
  }
}

.confirmation-text {
  color: #f4e4c1;
  font-size: 1.1rem;
  line-height: 1.6;
  margin-bottom: 2rem;

  strong {
    color: #ffd700;
  }
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  flex-wrap: wrap;
}

.modal-button {
  padding: 0.8rem 1.5rem;
  border: 2px solid;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 700;
  font-size: 1rem;
  transition: all 0.3s ease;

  &.cancel {
    background: transparent;
    border-color: #8b4513;
    color: #f4e4c1;

    &:hover {
      background: rgba(139, 69, 19, 0.2);
    }
  }

  &.save {
    background: #ffd700;
    border-color: #ffd700;
    color: #1a0f0a;

    &:hover {
      background: #ffed4e;
      transform: translateY(-2px);
    }
  }

  &.delete {
    background: #dc143c;
    border-color: #dc143c;
    color: #fff;

    &:hover {
      background: #b01030;
      transform: translateY(-2px);
    }
  }
}

// ============================================
// RESPONSIVE BREAKPOINTS
// ============================================

// Nagy tablet (1024px - 1280px)
@media (max-width: 1280px) {
  .overview-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .dashboard-container {
    padding: 1.5rem;
  }
}

// Tablet (768px - 1024px)
@media (max-width: 1024px) {
  .page-title {
    font-size: 2.5rem;
  }

  .players-section {
    padding: 1.5rem;
  }

  .modal-content {
    padding: 2rem;

    &.large {
      max-width: 95vw;
    }
  }

  .player-details-grid {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }
}

// Kis tablet / nagy mobil (600px - 768px)
@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }

  .page-title {
    font-size: 2rem;
  }

  .page-subtitle {
    font-size: 1rem;
  }

  .header-section {
    margin-bottom: 2rem;
  }

  .overview-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 1rem;
  }

  .overview-card {
    padding: 1rem;
    gap: 1rem;
    flex-direction: column;
    text-align: center;

    .card-icon {
      font-size: 2.5rem;
    }

    .card-content {
      .card-value {
        font-size: 1.5rem;
      }
    }
  }

  .section-header {
    flex-direction: column;
    align-items: stretch;
  }

  .section-title {
    font-size: 1.5rem;
  }

  .section-controls {
    flex-direction: column;
  }

  .search-input {
    min-width: auto;
    width: 100%;
  }

  .refresh-button {
    width: 100%;
    text-align: center;
  }

  .players-section {
    padding: 1rem;
  }

  // Modal mobilon
  .modal-content {
    padding: 1.5rem;
    border-radius: 12px;
    max-height: 95vh;
  }

  .modal-title {
    font-size: 1.4rem;
    padding-right: 2rem; // Helyet adni a bezáró gombnak
  }

  .modal-actions {
    justify-content: stretch;

    .modal-button {
      flex: 1;
      text-align: center;
    }
  }
}

// Mobil (480px - 600px)
@media (max-width: 600px) {
  .overview-grid {
    grid-template-columns: 1fr 1fr;
    gap: 0.8rem;
  }

  .overview-card {
    padding: 0.8rem;

    .card-icon {
      font-size: 2rem;
    }

    .card-content {
      h3 {
        font-size: 0.75rem;
      }

      .card-value {
        font-size: 1.3rem;
      }

      .card-detail {
        font-size: 0.75rem;
      }
    }
  }

  .page-title {
    font-size: 1.7rem;
  }
}

// Kis mobil (< 480px)
@media (max-width: 480px) {
  .overview-grid {
    grid-template-columns: 1fr; // Egy oszlop a legkisebb képernyőn
  }

  .overview-card {
    flex-direction: row; // Visszaváltunk vízszintesre kis képernyőn
    text-align: left;
    padding: 1rem;

    .card-icon {
      font-size: 2rem;
    }

    .card-content {
      .card-value {
        font-size: 1.4rem;
      }
    }
  }

  .page-title {
    font-size: 1.5rem;
  }

  .dashboard-container {
    padding: 0.8rem;
  }

  .players-section {
    padding: 0.8rem;
    border-radius: 8px;
  }

  // Táblázat mobilon - kisebb padding
  .players-table {
    thead th {
      padding: 0.7rem 0.5rem;
      font-size: 0.75rem;
    }

    tbody td {
      padding: 0.7rem 0.5rem;
      font-size: 0.9rem;
    }

    .level-badge {
      padding: 0.2rem 0.5rem;
      font-size: 0.8rem;
    }

    .action-button {
      padding: 0.3rem 0.5rem;
      font-size: 0.9rem;
    }
  }

  .modal-content {
    padding: 1.2rem;
  }

  .modal-title {
    font-size: 1.2rem;
  }

  .detail-row {
    flex-direction: column;
    gap: 0.3rem;

    .value {
      text-align: left;
    }
  }
}
</style>
