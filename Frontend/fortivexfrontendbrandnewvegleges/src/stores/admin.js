import { defineStore } from "pinia";
import { ref } from "vue";
import { adminAPI } from "@/services/api";

export const useAdminStore = defineStore("admin", () => {
  const players = ref([]);
  const allStats = ref(null);
  const selectedPlayer = ref(null);
  const loading = ref(false);
  const error = ref(null);

  async function fetchAllPlayers() {
    loading.value = true;
    error.value = null;
    try {
      const response = await adminAPI.getAllPlayers();
      // Mezőnevek kezelése (PascalCase és camelCase is)
      players.value = response.data.map((p) => ({
        id: p.id || p.Id,
        username: p.username || p.Username,
        email: p.email || p.Email,
        createdAt: p.createdAt || p.CreatedAt,
        lastLogin: p.lastLogin || p.LastLogin,
        isOnline: false,
        // Stats mezők (ha benne vannak)
        level: p.level || p.Level || 1,
        wins: p.wins || p.Wins || 0,
        totalGames: p.totalGames || p.TotalGames || 0,
        enemiesKilled: p.enemiesKilled || p.EnemiesKilled || 0,
        totalGold: p.totalGold || p.TotalGold || 0,
      }));
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba a játékosok betöltésekor";
    } finally {
      loading.value = false;
    }
  }

  async function fetchPlayerById(playerId) {
    loading.value = true;
    error.value = null;
    try {
      const response = await adminAPI.getPlayerStatsByAccount(playerId);
      selectedPlayer.value = response.data;
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba a játékos betöltésekor";
    } finally {
      loading.value = false;
    }
  }

  async function updatePlayer(playerId, data) {
    loading.value = true;
    error.value = null;
    try {
      await adminAPI.updatePlayer(playerId, data);
      // ✅ Nem használjuk response.data-t, helyette frissítjük lokálisan
      const index = players.value.findIndex((p) => p.id === playerId);
      if (index !== -1) {
        players.value[index] = { ...players.value[index], ...data };
      }
      return true;
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba a játékos frissítésekor";
      return false;
    } finally {
      loading.value = false;
    }
  }

  async function deletePlayer(playerId) {
    loading.value = true;
    error.value = null;
    try {
      await adminAPI.deletePlayer(playerId);
      players.value = players.value.filter((p) => p.id !== playerId);
      return true;
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba a játékos törlésekor";
      return false;
    } finally {
      loading.value = false;
    }
  }

  async function fetchAllStats() {
    loading.value = true;
    error.value = null;
    try {
      const response = await adminAPI.getAllStats();
      const stats = response.data;
      console.log("📊 Stats adat:", stats[0]);

      // ✅ A nyers tömböt feldolgozzuk amit a dashboard vár
      allStats.value = {
        totalPlayers: players.value.length,
        activePlayers: players.value.length,

        totalGames: stats.reduce((sum, s) => sum + (s.totalGames || s.TotalGames || 0), 0),
        gamesToday: 0,

        totalPlayTime: stats.reduce((sum, s) => sum + (s.timePlayed || s.TimePlayed || 0), 0),
        avgPlayTime: stats.length
          ? stats.reduce((sum, s) => sum + (s.timePlayed || s.TimePlayed || 0), 0) / stats.length
          : 0,

        totalEnemiesKilled: stats.reduce(
          (sum, s) => sum + (s.enemiesKilled || s.EnemiesKilled || 0),
          0,
        ),
        maxWaveRecordName: "-",
      };

      // ✅ Frissítjük a játékosok stat adatait is
      //players.value = players.value.map((player) => {
      // const playerStat = stats.find(
      // (s) => Number(s.AccountId ?? s.accountId) === Number(player.id),
      //);

      // ✅ DEBUG - töröld ki ha megvan a hiba
      //console.log("Player ID:", player.id, "typeof:", typeof player.id);
      //console.log(
      //  "Stats AccountIds:",
      //  stats.map((s) => ({
      //    id: s.AccountId ?? s.accountId,
      //    typeof: typeof (s.AccountId ?? s.accountId),
      //  level: s.Level ?? s.level,
      // })),
      //);
      //console.log("Found stat:", playerStat);

      //if (playerStat) {
      // return {
      // ...player,
      // level: playerStat.Level ?? playerStat.level ?? 1,
      // wins: playerStat.Wins ?? playerStat.wins ?? 0,
      // totalGames: playerStat.TotalGames ?? playerStat.totalGames ?? 0,
      // enemiesKilled: playerStat.EnemiesKilled ?? playerStat.enemiesKilled ?? 0,
      // totalGold: playerStat.TotalGold ?? playerStat.totalGold ?? 0,
      // };
      //}
      //return player;
      //});
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba a statisztikák betöltésekor";
    } finally {
      loading.value = false;
    }
  }

  return {
    players,
    allStats,
    selectedPlayer,
    loading,
    error,
    fetchAllPlayers,
    fetchPlayerById,
    updatePlayer,
    deletePlayer,
    fetchAllStats,
  };
});
export const useUserStore = defineStore("user", {
  state: () => ({ user: null, stats: null, token: null }),
  persist: true,
});
