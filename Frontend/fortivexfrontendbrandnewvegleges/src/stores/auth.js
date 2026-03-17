import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { authAPI } from "@/services/api";

export const useAuthStore = defineStore("auth", () => {
  // --- ÁLLAPOT INICIALIZÁLÁS ---
  const getStoredData = (key) => localStorage.getItem(key) || sessionStorage.getItem(key);

  const token = ref(getStoredData("token") || null);
  const rawUser = getStoredData("user");
  const user = ref(rawUser ? JSON.parse(rawUser) : null);

  const loading = ref(false);
  const error = ref(null);

  // --- COMPUTED ÉRTÉKEK ---
  const isAuthenticated = computed(() => !!token.value);
  const isAdmin = computed(() => user.value?.role === "admin");
  const isPlayer = computed(() => user.value?.role === "user");

  // --- SEGÉDFÜGGVÉNYEK ---
  function mapRoleIdToRole(roleId) {
    if (typeof roleId === "string") return roleId;
    switch (roleId) {
      case 1:
        return "admin";
      case 2:
        return "user";
      default:
        return "user";
    }
  }

  function saveAuthData(userData, tokenValue, rememberMe) {
    const storage = rememberMe || localStorage.getItem("token") ? localStorage : sessionStorage;

    token.value = tokenValue;
    user.value = userData;

    storage.setItem("token", tokenValue);
    storage.setItem("user", JSON.stringify(userData));
  }

  // --- ACTIONS ---

  async function login(credentials, rememberMe = false) {
    loading.value = true;
    error.value = null;
    try {
      const response = await authAPI.login(credentials);

      const userData = {
        id: response.data.accountID || response.data.AccountID || response.data.id,
        username: response.data.userName || response.data.UserName || response.data.username,
        role: mapRoleIdToRole(response.data.role || response.data.RoleID),
      };

      saveAuthData(userData, response.data.token, rememberMe);
      return true;
    } catch (err) {
      error.value = err.response?.data?.message || "Bejelentkezési hiba";
      return false;
    } finally {
      loading.value = false;
    }
  }

  async function register(userData) {
    loading.value = true;
    error.value = null;
    try {
      const response = await authAPI.register(userData);

      const newUser = {
        id: response.data.accountID || response.data.id,
        username: response.data.userName || response.data.username,
        role: mapRoleIdToRole(response.data.RoleID || response.data.role),
      };

      saveAuthData(newUser, response.data.token, true); // Regisztrációnál alapértelmezett a mentés
      return true;
    } catch (err) {
      error.value = err.response?.data?.message || "Regisztrációs hiba";
      return false;
    } finally {
      loading.value = false;
    }
  }

  async function fetchCurrentUser() {
    if (!token.value) return;
    loading.value = true;
    try {
      const response = await authAPI.getCurrentUser();
      if (response.data) {
        const userData = {
          id: response.data.accountID || response.data.id || user.value?.id,
          username: response.data.userName || response.data.username || user.value?.username,
          role: mapRoleIdToRole(response.data.role || response.data.RoleID) || user.value?.role,
        };
        saveAuthData(userData, token.value, !!localStorage.getItem("token"));
      }
    } catch (err) {
      console.error("Fetch user error:", err);
      // ← CSAK töröljük a storage-t, NEM hívjuk a logout()-ot
      if (err.response?.status === 401) {
        user.value = null;
        token.value = null;
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("user");
      }
    } finally {
      loading.value = false;
    }
  }

  async function logout() {
    try {
      await authAPI.logout();
    } catch (err) {
      console.error("Logout error:", err);
    } finally {
      user.value = null;
      token.value = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      sessionStorage.removeItem("token");
      sessionStorage.removeItem("user");
    }
  }

  async function forgotPassword(email) {
    loading.value = true;
    error.value = null;
    try {
      await authAPI.forgotPassword(email);
      return true;
    } catch (err) {
      error.value = err.response?.data?.message || "Hiba történt";
      return false;
    } finally {
      loading.value = false;
    }
  }

  async function resetPassword(resetToken, newPassword) {
    loading.value = true;
    error.value = null;
    try {
      await authAPI.resetPassword(resetToken, newPassword);
      return true;
    } catch (err) {
      error.value = err.response?.data?.message || "Érvénytelen vagy lejárt link";
      return false;
    } finally {
      loading.value = false;
    }
  }

  return {
    user,
    token,
    loading,
    error,
    isAuthenticated,
    isAdmin,
    isPlayer,
    login,
    register,
    logout,
    fetchCurrentUser,
    forgotPassword,
    resetPassword,
  };
});
