<template>
  <div class="support-page">
    <div class="container">
      <!-- LOGO -->
      <router-link to="/">
        <img class="logo" src="@/assets/images/logo.png" alt="Fortivex Logo" />
      </router-link>
      <h2>Fortivex Támogatás</h2>
      <p class="subtitle">Írj nekünk, és igyekszünk minél hamarabb válaszolni.</p>

      <!-- FORM -->
      <form class="support-form" @submit.prevent="submitSupport">
        <div class="form-row">
          <label
            >Név
            <input type="text" v-model="form.name" required />
          </label>
        </div>

        <div class="form-row">
          <label
            >Email cím
            <input type="email" v-model="form.email" required />
          </label>
        </div>

        <div class="form-row">
          <label
            >Kategória
            <select v-model="form.category" required>
              <option disabled value="">Válassz kategóriát</option>
              <option>Hiba / Bug</option>
              <option>Fiók probléma</option>
              <option>Javaslat</option>
              <option>Egyéb</option>
            </select>
          </label>
        </div>

        <div class="form-row">
          <label
            >Tárgy
            <input type="text" v-model="form.subject" required />
          </label>
        </div>

        <div class="form-row">
          <label
            >Üzenet
            <textarea v-model="form.message" rows="4" required></textarea>
          </label>
        </div>

        <div class="form-row" style="display: none">
          <label
            >Időpont
            <input type="text" v-model="form.time" readonly />
          </label>
        </div>

        <!-- ACTIONS -->
        <div class="actions">
          <button type="submit" class="submit-button" :disabled="loading">
            {{ loading ? "Küldés..." : "Küldés" }}
          </button>
          <button type="button" class="cancel-button" @click="resetForm">Mégse</button>
        </div>

        <!-- STATUS -->
      </form>

      <transition name="toast">
        <div class="logo-toast-wrapper">
          <div v-if="success" class="toast-success">✅ Üzenet elküldve! Hamarosan válaszolunk.</div>
        </div>
      </transition>

      <div v-if="error" class="status error">{{ error }}</div>
    </div>
  </div>

  <!-- FOOTER -->
  <footer class="support-footer">
    <div class="container">
      <p>&copy; 2026 Fortivex Projekt | <router-link to="/">Vissza a főoldalra</router-link></p>
    </div>
  </footer>
</template>

<script setup>
import { ref } from "vue";
import emailjs from "@emailjs/browser";
import logo from "@/assets/images/logo.png";

const form = ref({
  name: "",
  email: "",
  category: "",
  subject: "",
  message: "",
  time: "",
});

const loading = ref(false);
const success = ref(false);
const error = ref(null);

const resetForm = () => {
  form.value = { name: "", email: "", category: "", subject: "", message: "", time: "" };
  error.value = null;
  success.value = false;
};

const submitSupport = async () => {
  if (loading.value) return;

  loading.value = true;
  error.value = null;

  try {
    const serviceID = "service_03cvdh8"; // EmailJS service ID
    const templateID = "template_nqjngxa"; // EmailJS template ID
    const publicKey = "kY2Yn76EvWkTnvzda"; // EmailJS public key

    await emailjs.send(
      serviceID,
      templateID,
      {
        name: form.value.name,
        from_email: form.value.email,
        category: form.value.category,
        subject: form.value.subject,
        message: form.value.message,
        time: new Date().toLocaleString(),
      },
      publicKey,
    );

    success.value = true;

    setTimeout(() => {
      success.value = false;
    }, 2000);
    //resetForm();

    // automatikusan eltűnteti a státuszt 3 mp múlva
    setTimeout(() => (success.value = false), 3000);
  } catch (e) {
    console.error("EmailJS hiba:", e);
    error.value = "Nem sikerült elküldeni az üzenetet. Próbáld újra később.";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.support-page {
  min-height: calc(100vh - 120px);
  padding: 80px 16px 100px;
  background:
    radial-gradient(circle at top, rgba(245, 185, 66, 0.08), transparent 60%),
    linear-gradient(180deg, #0e0a06 0%, #060402 100%);
  display: flex;
  flex-direction: column;
  align-items: center;
}

.container {
  max-width: 680px;
  width: 100%;
  text-align: center;
}

.logo {
  width: 140px;
  filter: drop-shadow(0 0 10px rgba(245, 185, 66, 0.9));
  transition: transform 0.2s ease;
}

.logo:hover {
  transform: scale(1.05);
}

@keyframes float {
  0% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-6px);
  }
  100% {
    transform: translateY(0);
  }
}

h2 {
  font-size: 50px;
  color: #f5b942;
  margin-bottom: 8px;
}

.subtitle {
  color: rgba(245, 230, 200, 0.7);
  margin-bottom: 32px;
}

.support-form {
  padding: 28px;
  border-radius: 18px;
  background: rgba(0, 0, 0, 0.45);
  border: 1px solid rgba(245, 185, 66, 0.25);
  backdrop-filter: blur(6px);
  text-align: left;
}

.form-row {
  margin-bottom: 16px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  color: #f5b942;
}

.form-row label {
  display: block;
  font-weight: 600;
  color: #f5b942;
  margin-bottom: 10px;
  font-family: "Georgia", serif;
  font-size: 18px;
}

.form-row input,
.form-row textarea,
.form-row select {
  width: 100%;
  padding: 12px 14px;
  border-radius: 12px;
  border: 1px solid rgba(245, 185, 66, 0.25);
  background: rgba(0, 0, 0, 0.35);
  color: #f4e7c6;
  outline: none;
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease;
}

.form-row input:focus,
.form-row textarea:focus,
.form-row select:focus {
  border-color: rgba(245, 185, 66, 0.7);
  box-shadow: 0 0 12px rgba(245, 185, 66, 0.35);
}

.actions {
  display: flex;
  gap: 12px;
  margin-top: 12px;
  flex-direction: row;
}

.submit-button {
  padding: 12px 20px;
  border-radius: 12px;
  border: 1px solid rgba(245, 185, 66, 0.5);
  background: rgba(245, 185, 66, 0.15);
  color: #f5e6c8;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.2s;
  text-align: center;
}

.submit-button:hover {
  background: rgba(245, 185, 66, 0.25);
  transform: translateY(-1px);
  text-align: center;
}

.cancel-button {
  padding: 12px 20px;
  border-radius: 12px;
  border: 1px solid rgba(255, 80, 80, 0.5);
  background: rgba(255, 80, 80, 0.15);
  color: #f5e6c8;
  font-weight: 700;
  cursor: pointer;
  text-align: center;
}

.cancel-button:hover {
  background: rgba(255, 80, 80, 0.25);
  transform: translateY(-1px);
  text-align: center;
}

.status {
  margin-top: 16px;
  padding: 10px 12px;
  border-radius: 12px;
}

.status.success {
  border: 1px solid rgba(66, 245, 96, 0.6);
  background: rgba(66, 245, 96, 0.15);
  color: #f4e7c6;
}

.status.error {
  border: 1px solid rgba(255, 80, 80, 0.45);
  background: rgba(255, 80, 80, 0.12);
  color: #f4e7c6;
}

.support-footer {
  margin-top: auto;
  padding: 20px 0;
  background: #0e0a06;
  text-align: center;
  color: #f4e7c6;
  border-top: 1px solid rgba(245, 185, 66, 0.2);
}
.support-footer a {
  color: #f5b942;
  text-decoration: none;
}
.support-footer a:hover {
  text-decoration: underline;
}
/* === TOAST === */

.toast-success {
  position: fixed;
  top: 24px;
  left: 50%;
  transform: translateX(-50%);
  background: rgba(20, 20, 20, 0.95);
  border: 1px solid rgba(66, 245, 96, 0.6);
  color: #eaffea;
  padding: 14px 22px;
  border-radius: 14px;
  font-weight: 600;
  z-index: 9999;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.6);
}

/* === ANIMATION === */

.toast-enter-active {
  transition: all 0.45s cubic-bezier(0.22, 1, 0.36, 1);
}
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from {
  opacity: 0;
  transform: translate(-50%, -20px);
}
.toast-enter-to {
  opacity: 1;
  transform: translate(-50%, 0);
}

.toast-leave-from {
  opacity: 1;
  transform: translate(-50%, 0);
}
.toast-leave-to {
  opacity: 0;
  transform: translate(-50%, -20px);
}
.logo-toast-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
}
.label {
  pointer-events: none;
  color: rgba(245, 185, 66, 0.7);
  font-size: 14px;
  margin-bottom: 6px;
}

.form-row select option {
  background: #1a0a00;
  color: #f4e7c6;
}
</style>
