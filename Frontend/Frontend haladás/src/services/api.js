import axios from "axios";
import { useAuthStore } from "@/stores/auth";

const API_BASE_URL = import.meta.env.VITE_API_URL || "https://fortivex.runasp.net/api";

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// --- Request interceptor ---
api.interceptors.request.use(
  (config) => {
    // 💡 Frissítés után a Store néha lassabban ébred fel, mint az első API hívás.
    // Ezért a tokent közvetlenül a tárolókból olvassuk ki a biztonság kedvéért.
    const token = localStorage.getItem("token") || sessionStorage.getItem("token");

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

// --- Response interceptor - hibakezelés ---
api.interceptors.response.use(
  (response) => response,
  (error) => {
    const authStore = useAuthStore();

    if (error.response?.status === 401) {
      // Ha lejárt a token vagy érvénytelen, kiléptetünk
      authStore.logout();

      // Csak akkor dobjuk vissza a kezdőre, ha nem eleve ott vagyunk
      if (window.location.pathname !== "/") {
        window.location.href = "/";
      }
    }
    return Promise.reject(error);
  },
);

// --- Auth endpoints ---
export const authAPI = {
  login: (credentials) => api.post("/accounts/login", credentials),
  register: (userData) => api.post("/accounts/register", userData),
  logout: () => {
    const authStore = useAuthStore();
    authStore.logout();
    return Promise.resolve();
  },
  // ✅ Ez hívja meg az új backend végpontot, amit a kontrollerbe tettünk
  getCurrentUser: () => api.get("/accounts/me"),
  forgotPassword: (email) => api.post("/accounts/forgot-password", { email }),
  resetPassword: (token, newPassword) =>
    api.post("/accounts/reset-password", { token, newPassword }),
};

// --- Player Stats ---
export const playerStatsAPI = {
  getStats: (playerId) => api.get(`/playerstats/${playerId}`),
  updateStats: (playerId, stats) => api.put(`/playerstats/${playerId}`, stats),
  getAllStats: () => api.get("/playerstats"),
};

// --- Player Progress ---
export const playerProgressAPI = {
  getProgress: (playerId) => api.get(`/playerprogress/${playerId}`),
  updateProgress: (playerId, progress) => api.put(`/playerprogress/${playerId}`, progress),
};

// --- Admin ---
export const adminAPI = {
  getAllPlayers: () => api.get("/accounts"),
  getPlayerStatsByAccount: (accountId) => api.get(`/playerstats/account/${accountId}`),
  updatePlayer: (playerId, data) => api.put(`/accounts/${playerId}`, data),
  deletePlayer: (playerId) => api.delete(`/accounts/${playerId}`),
  getAllStats: () => api.get("/playerstats"),
};

// --- Accounts ---
export const accountsAPI = {
  createAccount: (userData) => api.post("/accounts", userData),
  getAccount: (accountId) => api.get(`/accounts/${accountId}`),
  updateAccount: (accountId, data) => api.put(`/accounts/${accountId}`, data),
  deleteAccount: (accountId) => api.delete(`/accounts/${accountId}`),
};

export default api;
