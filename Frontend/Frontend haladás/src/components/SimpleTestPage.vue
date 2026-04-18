<template>
  <div class="test-page">
    <h1>Fortivex Projekt</h1>
    <p>Ha ezt látod, akkor működik a Vue alkalmazás!</p>
    <button @click="count++" class="test-button">
      Kattintások: {{ count }}
    </button>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const count = ref(0);
</script>

<style scoped>
.test-page {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #1a0f0a 0%, #2c1810 100%);
  color: #ffd700;
  text-align: center;
  padding: 2rem;
}

h1 {
  font-size: 3rem;
  font-family: 'Cinzel', serif;
  margin-bottom: 1rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
}

p {
  font-size: 1.5rem;
  color: #f4e4c1;
  margin-bottom: 2rem;
}

.test-button {
  padding: 1rem 2rem;
  background: #8b4513;
  color: #fff;
  border: 2px solid #ffd700;
  border-radius: 8px;
  font-size: 1.2rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
}

.test-button:hover {
  background: #a0522d;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(255, 215, 0, 0.4);
}
</style>