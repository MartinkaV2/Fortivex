// src/stores/game.js
import { defineStore } from "pinia";
import { ref } from "vue";
import { gameAPI } from "@/services/api";

export const useGameStore = defineStore("game", () => {
  const maps = ref([]);
  const enemies = ref([]);
  const loading = ref(false);
  const error = ref(null);

  async function fetchMaps() {
    loading.value = true;
    error.value = null;
    try {
      const response = await gameAPI.getMaps();
      maps.value = response.data;
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba a pályák betöltésekor";
      console.error("Fetch maps error:", err);
    } finally {
      loading.value = false;
    }
  }

  async function fetchEnemies() {
    loading.value = true;
    error.value = null;
    try {
      const response = await gameAPI.getEnemies();
      enemies.value = response.data;
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba az ellenségek betöltésekor";
      console.error("Fetch enemies error:", err);
    } finally {
      loading.value = false;
    }
  }

  function resetStore() {
    maps.value = [];
    enemies.value = [];
    error.value = null;
  }

  return {
    maps,
    enemies,
    loading,
    error,
    fetchMaps,
    fetchEnemies,
    resetStore,
  };
});

export const useUserStore = defineStore("user", {
  state: () => ({ user: null, stats: null, token: null }),
  persist: true,
});
