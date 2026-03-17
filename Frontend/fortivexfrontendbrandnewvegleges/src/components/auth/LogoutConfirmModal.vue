<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-container">
      <button class="close-btn" @click="$emit('close')">✕</button>

      <h2>Biztosan ki szeretnél jelentkezni?</h2>
      <img src="@/assets/images/logo.png" alt="Fortivex Logo" class="modal-logo"/>
      <div class="actions">
        <button class="confirm" @click="$emit('confirm')">
          Igen
        </button>
        <button class="cancel" @click="$emit('close')">
          Nem
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
defineEmits(['confirm', 'close']);
</script>

<style scoped lang="scss">
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.65);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-container {
  position: relative;
  width: 420px;
  padding: 2.5rem 2rem;
  background: linear-gradient(135deg, #2c1810, #4a2c1a);
  border: 2px solid #ffd700;
  border-radius: 16px;
  text-align: center;
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.6);

  h2 {
    font-family: 'Cinzel', serif;
    color: #ffd700;
    font-size: 1.4rem;
    margin-bottom: 2rem;
    text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.7);
  }
}

.close-btn {
  position: absolute;
  top: 12px;
  right: 14px;
  background: none;
  border: none;
  color: #ffd700;
  font-size: 1.5rem;
  cursor: pointer;
}

.actions {
  display: flex;
  justify-content: center;
  gap: 1.5rem;
}

.confirm,
.cancel {
  padding: 0.6rem 1.6rem;
  border-radius: 10px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  border: 2px solid #ffd700;
  transition: all 0.25s ease;
}

/* IGEN */
.confirm {
  background: #8b4513;
  color: #ffd700;

  &:hover {
    background: #a0522d;
    transform: translateY(-2px);
  }
}

/* NEM */
.cancel {
  background: transparent;
  color: #f4e4c1;

  &:hover {
    background: rgba(255, 215, 0, 0.15);
    color: #ffd700;
  }
}
</style>
