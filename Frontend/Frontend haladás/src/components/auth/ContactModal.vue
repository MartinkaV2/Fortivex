<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal">
      <h2>🎫 Támogatás / Hibajelentés</h2>
      <p class="subtitle">Írj nekünk, és igyekszünk minél hamarabb válaszolni.</p>

      <form @submit.prevent="submitSupport">
        <label>
          Email címed
          <input type="email" v-model="form.email" required />
        </label>

        <label>
          Kategória
          <select v-model="form.category" required>
            <option disabled value="">Válassz kategóriát</option>
            <option>Hiba / Bug</option>
            <option>Fiók probléma</option>
            <option>Javaslat</option>
            <option>Egyéb</option>
          </select>
        </label>

        <label>
          Tárgy
          <input type="text" v-model="form.subject" required />
        </label>

        <label>
          Üzenet
          <textarea v-model="form.message" rows="4" required></textarea>
        </label>

        <div class="actions">
          <button type="submit" class="submit">Küldés</button>
          <button type="button" class="cancel" @click="$emit('close')">Mégse</button>
        </div>

        <p v-if="success" class="success">✅ Üzenet elküldve! Hamarosan válaszolunk.</p>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";

const emit = defineEmits(["close"]);

const form = ref({
  name: "",
  email: "",
  category: "",
  subject: "",
  message: "",
});

const loading = ref(false);
const success = ref(false);
const error = ref(null);

const submit = async () => {
  loading.value = true;
  error.value = null;

  try {
    // ⏳ később ide jön az API / email küldés
    await new Promise((resolve) => setTimeout(resolve, 1500));

    success.value = true;

    setTimeout(() => {
      emit("close");
      success.value = false;
    }, 2000);
  } catch (e) {
    error.value = "Nem sikerült elküldeni az üzenetet.";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
textarea {
  min-height: 120px;
  resize: vertical;
}
.email-link {
  cursor: pointer;
  color: #ffd700;
  text-decoration: underline;

  &:hover {
    color: #fff;
  }
}
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.75);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 2000;
  animation: fadeIn 0.3s ease;
}

.modal-content {
  background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
  border: 3px solid #ffd700;
  border-radius: 16px;
  padding: 2.5rem;
  width: 90%;
  max-width: 460px;
  position: relative;
  box-shadow: 0 15px 50px rgba(0, 0, 0, 0.6);
  animation: slideUp 0.35s ease;
}

/* Animációk */
@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(40px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.modal-title {
  color: #ffd700;
  font-size: 2rem;
  text-align: center;
  margin-bottom: 1.8rem;
  font-family: "Cinzel", serif;
  text-shadow: 2px 2px 6px rgba(0, 0, 0, 0.8);
}

.close-button {
  position: absolute;
  top: 1rem;
  right: 1rem;
  background: none;
  border: none;
  color: #ffd700;
  font-size: 1.5rem;
  cursor: pointer;
  transition: transform 0.2s ease;

  &:hover {
    transform: rotate(90deg);
    color: #fff;
  }
}
.form-group {
  margin-bottom: 1.5rem;

  label {
    display: block;
    margin-bottom: 0.5rem;
    color: #f4e4c1;
    font-weight: 600;
  }

  input,
  textarea {
    width: 100%;
    padding: 0.75rem 1rem;
    border: 2px solid #ccc;
    border-radius: 8px;
    font-size: 1rem;
    transition: border-color 0.3s ease;

    &:focus {
      border-color: #ffd700;
      outline: none;
      box-shadow: 0 0 8px rgba(255, 215, 0, 0.5);
    }
  }
}

.modal-form {
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

.form-group label {
  color: #f4e4c1;
  font-weight: 600;
  margin-bottom: 0.3rem;
}

.form-group input,
.form-group textarea {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 8px;
  padding: 0.8rem;
  color: #f4e4c1;
  font-size: 1rem;
}

.form-group textarea {
  min-height: 120px;
  resize: vertical;
}

.form-group input:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #ffd700;
  box-shadow: 0 0 10px rgba(255, 215, 0, 0.3);
}

.submit-button {
  margin-top: 1rem;
  background: #8b4513;
  color: #fff;
  border: 2px solid #ffd700;
  padding: 0.9rem;
  border-radius: 8px;
  font-size: 1.1rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;

  &:hover {
    background: #a0522d;
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(255, 215, 0, 0.4);
  }
}
/* ⏳ Spinner */
.spinner {
  width: 18px;
  height: 18px;
  border: 3px solid rgba(255, 215, 0, 0.4);
  border-top: 3px solid #ffd700;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  margin: 0 auto;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

/* ❌ Hiba */
.error-message {
  background: rgba(220, 20, 60, 0.2);
  border: 2px solid #dc143c;
  color: #ff6b6b;
  padding: 0.8rem;
  border-radius: 8px;
  text-align: center;
  font-weight: 600;
}

/* ✅ Siker */
.success-box {
  text-align: center;
  animation: fadeInUp 0.4s ease;

  h3 {
    color: #7cfc00;
    font-size: 1.6rem;
    margin-bottom: 0.5rem;
  }

  p {
    color: #f4e4c1;
  }
}
@media (max-width: 480px) {
  .modal-content {
    padding: 1.8rem;
  }

  .modal-title {
    font-size: 1.6rem;
  }

  .submit-button {
    font-size: 1rem;
  }
}
</style>
