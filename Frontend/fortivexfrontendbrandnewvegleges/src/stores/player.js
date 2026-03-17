import { defineStore } from "pinia";
import api from "@/services/api";

export const usePlayerStore = defineStore("player", {
  state: () => ({
    stats: null,
    progress: null,
    loading: false,
    error: null,
  }),

  getters: {
    isLoggedIn: (state) => !!state.stats,
  },

  actions: {
    async fetchStats(accountId) {
      this.loading = true;
      this.error = null;
      try {
        // ✅ JAVÍTVA: api használata apiClient helyett
        const response = await api.get(`/playerstats/account/${accountId}`);
        this.stats = response.data;
      } catch (error) {
        this.error = error.message;
        console.error("Error fetching stats:", error);
      } finally {
        this.loading = false;
      }
    },

    async fetchProgress(accountId) {
      this.loading = true;
      try {
        // ✅ JAVÍTVA: api használata apiClient helyett
        const [mapsResponse, achievementsResponse] = await Promise.all([
          api.get(`/playermapprogress/${accountId}`),
          api.get(`/playerachievements/${accountId}`),
        ]);

        this.progress = {
          maps: mapsResponse.data,
          achievements: achievementsResponse.data,
        };
      } catch (error) {
        this.error = error.message;
        console.error("Error fetching progress:", error);
      } finally {
        this.loading = false;
      }
    },

    setPlayer(player) {
      this.stats = player;
    },

    logout() {
      this.stats = null;
      this.progress = null;
    },
  },
});

export const useUserStore = defineStore("user", {
  state: () => ({ user: null, stats: null, token: null }),
  persist: true,
});
