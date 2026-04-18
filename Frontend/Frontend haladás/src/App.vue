<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script setup>
console.log(import.meta.env.VITE_API_URL);
import { onMounted } from "vue";
import { useAuthStore } from "@/stores/auth";
const authStore = useAuthStore();

onMounted(async () => {
  // Ha van token, próbáljuk betölteni a felhasználót
  if (authStore.token) {
    await authStore.fetchCurrentUser();
  }
});
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css2?family=Cinzel:wght@400;600;700&family=Inter:wght@400;500;600;700&display=swap");

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: "Inter", sans-serif;
  background: #1a0f0a;
  color: #f4e4c1;
  overflow-x: hidden;
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

#app {
  min-height: 100vh;
}

// Scrollbar styling
::-webkit-scrollbar {
  width: 12px;
  height: 12px;
}

::-webkit-scrollbar-track {
  background: #1a0f0a;
}

::-webkit-scrollbar-thumb {
  background: #8b4513;
  border-radius: 6px;
  border: 2px solid #1a0f0a;

  &:hover {
    background: #a0522d;
  }
}

// Selection color
::selection {
  background: rgba(255, 215, 0, 0.3);
  color: #ffd700;
}

// Smooth scroll
html {
  scroll-behavior: smooth;
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

// Link styling
a {
  color: #ffd700;
  text-decoration: none;
  transition: color 0.3s ease;

  &:hover {
    color: #ffed4e;
  }
}

// Button reset
button {
  font-family: inherit;
}

// Input styling
input,
textarea {
  font-family: inherit;
}
</style>
