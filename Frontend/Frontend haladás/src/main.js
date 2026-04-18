// src/main.js
import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import "./assets/styles/main.scss";
import { useUserStore } from "@/stores/user"; // a user store-t betöltjük

const app = createApp(App);

const pinia = createPinia();
app.use(pinia);
app.use(router);

// --- STORE PERZISZTÁLÁS BETÖLTÉSE ---
const userStore = useUserStore();
userStore.loadFromStorage(); // F5 után visszatölti a user + token + stats adatokat

app.mount("#app");
