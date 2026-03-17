<template>
  <div class="info-page">
    <div class="info-bg">
      <div class="info-card">
        <div class="logo-row">
          <router-link to="/">
            <img class="logo" src="@/assets/images/logo.png" alt="Fortivex Logo" />
          </router-link>
        </div>

        <h1 class="page-title">Új jelszó megadása</h1>
        <div class="divider"></div>

        <div class="content">
          <div class="section">
            <div v-if="!resetDone">
              <form @submit.prevent="handleReset" class="auth-form">
                <div class="form-group">
                  <label>Új jelszó</label>
                  <input v-model="password" type="password" placeholder="Új jelszó" required />
                </div>
                <div class="form-group">
                  <label>Jelszó megerősítése</label>
                  <input
                    v-model="passwordConfirm"
                    type="password"
                    placeholder="Jelszó újra"
                    required
                  />
                </div>
                <div v-if="error" class="error-message">{{ error }}</div>
                <button type="submit" class="submit-button" :disabled="loading">
                  {{ loading ? "Mentés..." : "Jelszó megváltoztatása" }}
                </button>
              </form>
            </div>

            <div v-else class="success-box">
              <div class="success-icon">✅</div>
              <h3>Jelszó sikeresen megváltoztatva!</h3>
              <p>Most már bejelentkezhetsz az új jelszavaddal.</p>
              <router-link to="/" class="back-link">Vissza a főoldalra</router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRoute } from "vue-router";
import { useAuthStore } from "@/stores/auth";

const authStore = useAuthStore();
const route = useRoute();

const password = ref("");
const passwordConfirm = ref("");
const loading = ref(false);
const error = ref(null);
const resetDone = ref(false);

const token = route.query.token;

const handleReset = async () => {
  if (password.value !== passwordConfirm.value) {
    error.value = "A két jelszó nem egyezik!";
    return;
  }

  loading.value = true;
  error.value = null;

  const success = await authStore.resetPassword(token, password.value);

  if (success) {
    resetDone.value = true;
  } else {
    error.value = authStore.error || "Érvénytelen vagy lejárt link.";
  }

  loading.value = false;
};
</script>

<style scoped>
/* Ugyanaz a stílus mint a ForgotPasswordView-ban */
.info-page {
  min-height: 100vh;
  background: radial-gradient(circle at top, #3b1d0f, #0f0703);
  padding: 80px 16px;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  font-family: "Georgia", serif;
}

.info-bg {
  width: 100%;
  max-width: 600px;
}

.info-card {
  background: linear-gradient(180deg, rgba(0, 0, 0, 0.45), rgba(0, 0, 0, 0.75));
  border: 1px solid rgba(245, 185, 66, 0.35);
  border-radius: 24px;
  padding: 34px 36px;
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.65);
  position: relative;
  overflow: hidden;
}

.info-card::before {
  content: "";
  position: absolute;
  inset: -2px;
  background:
    radial-gradient(circle at top left, rgba(245, 185, 66, 0.22), transparent 45%),
    radial-gradient(circle at bottom right, rgba(245, 185, 66, 0.12), transparent 55%);
  pointer-events: none;
}

.logo-row {
  display: flex;
  justify-content: center;
  margin-bottom: 18px;
}

.logo {
  width: 140px;
  filter: drop-shadow(0 0 10px rgba(245, 185, 66, 0.9));
  transition: transform 0.2s ease;
}

.logo:hover {
  transform: scale(1.05);
}

.page-title {
  color: #f5b942;
  font-size: 36px;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin: 12px 0 32px;
  text-align: center;
}

.divider {
  height: 1px;
  background: linear-gradient(to right, transparent, rgba(245, 185, 66, 0.45), transparent);
  margin-bottom: 28px;
}

.content .section {
  background: rgba(0, 0, 0, 0.35);
  border: 1px solid rgba(245, 185, 66, 0.25);
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 10px 24px rgba(0, 0, 0, 0.5);
}

.auth-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group label {
  color: #f4e4c1;
  font-weight: 600;
  font-size: 1rem;
}

.form-group input {
  padding: 0.8rem;
  border: 2px solid #8b4513;
  border-radius: 8px;
  background: #2c1810;
  color: #f4e4c1;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.form-group input:focus {
  outline: none;
  border-color: #ffd700;
  box-shadow: 0 0 10px rgba(255, 215, 0, 0.3);
}

.form-group input::placeholder {
  color: rgba(244, 228, 193, 0.5);
}

.error-message {
  background: rgba(220, 20, 60, 0.2);
  border: 2px solid #dc143c;
  color: #ff6b6b;
  padding: 0.8rem;
  border-radius: 8px;
  text-align: center;
  font-weight: 600;
}

.submit-button {
  background: #8b4513;
  color: #fff;
  border: 2px solid #ffd700;
  padding: 0.9rem;
  border-radius: 8px;
  font-size: 1.1rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
}

.submit-button:hover:not(:disabled) {
  background: #a0522d;
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(255, 215, 0, 0.4);
}

.submit-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.success-box {
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.success-icon {
  font-size: 3rem;
}

.success-box h3 {
  color: #f5b942;
  font-size: 1.5rem;
}

.success-box p {
  color: #f4e7c6;
}

.back-link {
  display: inline-block;
  color: #ffd700;
  text-decoration: underline;
  font-weight: 600;
  transition: opacity 0.2s;
}

.back-link:hover {
  opacity: 0.8;
}

@media (max-width: 768px) {
  .info-page {
    padding: 40px 12px;
  }
  .info-card {
    padding: 28px 20px;
  }
  .page-title {
    font-size: 28px;
  }
}
</style>
