<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-content">
      <button class="close-button" @click="$emit('close')">✕</button>

      <h2 class="modal-title">Regisztráció</h2>

      <form @submit.prevent="handleRegister" class="register-form">
        <div class="form-group">
          <label for="reg-username">Felhasználónév</label>
          <input
            id="reg-username"
            v-model="form.username"
            type="text"
            placeholder="Válassz egy felhasználónevet"
            required
            minlength="3"
          />
        </div>

        <div class="form-group">
          <label for="reg-email">Email</label>
          <input
            id="reg-email"
            v-model="form.email"
            type="email"
            placeholder="email@example.com"
            required
          />
        </div>

        <div class="form-group">
          <label for="reg-password">Jelszó</label>
          <input
            id="reg-password"
            v-model="form.password"
            type="password"
            placeholder="Minimum 6 karakter"
            required
            minlength="6"
          />
        </div>

        <div class="form-group">
          <label for="reg-password-confirm">Jelszó megerősítése</label>
          <input
            id="reg-password-confirm"
            v-model="form.passwordConfirm"
            type="password"
            placeholder="Írd be újra a jelszót"
            required
          />
        </div>

        <div v-if="error" class="error-message">
          {{ error }}
        </div>

        <button type="submit" class="submit-button" :disabled="loading">
          <span v-if="!loading">Regisztráció</span>
          <span v-else>Betöltés...</span>
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useAuthStore } from "@/stores/auth";
import { useRouter } from "vue-router";

const emit = defineEmits(["close"]);

const authStore = useAuthStore();
const router = useRouter();

const form = ref({
  username: "",
  email: "",
  password: "",
  passwordConfirm: "",
});

const loading = ref(false);
const error = ref(null);

const handleRegister = async () => {
  error.value = null;

  // ✅ Jelszó egyezés ellenőrzés
  if (form.value.password !== form.value.passwordConfirm) {
    error.value = "A jelszavak nem egyeznek!";
    return;
  }

  loading.value = true;

  // ✅ JAVÍTVA: RoleID számmal (2 = player)
  const userData = {
    UserName: form.value.username,
    PasswordHash: form.value.password,
    Email: form.value.email,
    RoleID: 2, // ✅ 1 = admin, 2 = player (az adatbázis szerint)
  };

  const success = await authStore.register(userData);

  if (success) {
    emit("close");
    router.push("/player");
  } else {
    error.value = authStore.error;
  }

  loading.value = false;
};
</script>

<style scoped lang="scss">
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.modal-content {
  background: linear-gradient(135deg, #3d2517 0%, #5c3a25 100%);
  border: 3px solid #ffd700;
  border-radius: 16px;
  padding: 2.5rem;
  max-width: 450px;
  width: 90%;
  position: relative;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.5);
  animation: slideUp 0.3s ease;
  max-height: 90vh;
  overflow-y: auto;
}

@keyframes slideUp {
  from {
    transform: translateY(30px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
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
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;

  &:hover {
    transform: rotate(90deg);
    color: #fff;
  }
}

.modal-title {
  color: #ffd700;
  font-size: 2rem;
  margin-bottom: 1.5rem;
  text-align: center;
  font-family: "Cinzel", serif;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
}

.register-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;

  label {
    color: #f4e4c1;
    font-weight: 600;
    font-size: 1rem;
  }

  input {
    padding: 0.8rem;
    border: 2px solid #8b4513;
    border-radius: 8px;
    background: #2c1810;
    color: #f4e4c1;
    font-size: 1rem;
    transition: all 0.3s ease;

    &:focus {
      outline: none;
      border-color: #ffd700;
      box-shadow: 0 0 10px rgba(255, 215, 0, 0.3);
    }

    &::placeholder {
      color: rgba(244, 228, 193, 0.5);
    }
  }
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
  margin-top: 0.5rem;

  &:hover:not(:disabled) {
    background: #a0522d;
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(255, 215, 0, 0.4);
  }

  &:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }
}
</style>
