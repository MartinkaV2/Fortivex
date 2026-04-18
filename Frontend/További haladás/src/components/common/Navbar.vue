<template>
  <nav class="navbar" :class="{ 'navbar-hidden': isHidden }">
    <div class="navbar-container">
      <!-- LEFT: Logo + Title -->
      <div class="navbar-left">
        <router-link to="/" class="navbar-logo">
          <img src="@/assets/images/logo.png" alt="Fortivex Logo" class="logo" />
          <span class="game-title">FORTIVEX PROJEKT</span>
        </router-link>
      </div>

      <!-- CENTER: Desktop Menu -->
      <div class="navbar-center desktop-menu">
        <router-link to="/" class="nav-link">Főoldal</router-link>
        <!--<router-link to="/#description" class="nav-link">A Játékról</router-link>-->
        <router-link to="/#maps" class="nav-link">Pályák</router-link>
        <router-link to="/#enemies" class="nav-link">Ellenségek</router-link>
        <router-link to="/#towers" class="nav-link">Tornyok</router-link>
        <router-link to="/#contact" class="nav-link">Elérhetőség</router-link>
      </div>

      <!-- RIGHT: Auth (desktop) -->
      <div class="navbar-right desktop-auth">
        <template v-if="!isAuthenticated">
          <div class="auth-dropdown">
            <button @click="toggleDropdown" class="auth-button">
              <span>Bejelentkezés / Regisztráció</span>
              <span class="arrow" :class="{ open: dropdownOpen }">▼</span>
            </button>

            <div v-if="dropdownOpen" class="dropdown-menu">
              <button @click="openLogin" class="dropdown-item">Bejelentkezés</button>
              <button @click="openRegister" class="dropdown-item">Regisztráció</button>
            </div>
          </div>
        </template>

        <template v-else>
          <div class="user-info">
            <router-link v-if="isPlayer" to="/player" class="username-link">
              {{ user?.username }}
            </router-link>
            <router-link v-else-if="isAdmin" to="/admin" class="username-link">
              {{ user?.username }}
            </router-link>
            <button @click="requestLogout" class="logout-button">Kijelentkezés</button>
          </div>
        </template>
      </div>

      <!-- HAMBURGER ICON -->
      <button class="hamburger" @click="toggleMobileMenu">
        <span :class="{ open: mobileMenuOpen }"></span>
        <span :class="{ open: mobileMenuOpen }"></span>
        <span :class="{ open: mobileMenuOpen }"></span>
      </button>
    </div>

    <!-- MOBILE MENU -->
    <div v-if="mobileMenuOpen" class="mobile-menu">
      <router-link @click="closeMobileMenu" to="/" class="mobile-link">Főoldal</router-link>
      <router-link @click="closeMobileMenu" to="/#description" class="mobile-link"
        >A Játékról</router-link
      >
      <router-link @click="closeMobileMenu" to="/#maps" class="mobile-link">Pályák</router-link>
      <router-link @click="closeMobileMenu" to="/#enemies" class="mobile-link"
        >Ellenségek</router-link
      >
      <router-link @click="closeMobileMenu" to="/#towers" class="mobile-link">Tornyok</router-link>
      <router-link @click="closeMobileMenu" to="/#contact" class="mobile-link"
        >Elérhetőség</router-link
      >

      <template v-if="isAuthenticated">
        <router-link v-if="isPlayer" @click="closeMobileMenu" to="/player" class="mobile-link"
          >Profil</router-link
        >
        <router-link v-if="isAdmin" @click="closeMobileMenu" to="/admin" class="mobile-link"
          >Admin Panel</router-link
        >
      </template>

      <div class="mobile-auth">
        <template v-if="!isAuthenticated">
          <button @click="openLogin" class="mobile-auth-btn">Bejelentkezés</button>
          <button @click="openRegister" class="mobile-auth-btn">Regisztráció</button>
        </template>

        <template v-else>
          <button @click="requestLogout" class="mobile-auth-btn">Kijelentkezés</button>
        </template>
      </div>
    </div>

    <LoginModal v-if="loginModalOpen" @close="loginModalOpen = false" />
    <RegisterModal v-if="registerModalOpen" @close="registerModalOpen = false" />
    <LogoutConfirmModal
      v-if="logoutConfirmOpen"
      @confirm="confirmLogout"
      @close="logoutConfirmOpen = false"
    />
  </nav>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from "vue";
import { useAuthStore } from "@/stores/auth";
import { useRouter } from "vue-router";
import LoginModal from "@/components/auth/LoginModal.vue";
import RegisterModal from "@/components/auth/RegisterModal.vue";
import LogoutConfirmModal from "@/components/auth/LogoutConfirmModal.vue";

const authStore = useAuthStore();
const router = useRouter();

const dropdownOpen = ref(false);
const loginModalOpen = ref(false);
const registerModalOpen = ref(false);
const mobileMenuOpen = ref(false);

const isAuthenticated = computed(() => authStore.isAuthenticated);
const isPlayer = computed(() => authStore.isPlayer);
const isAdmin = computed(() => authStore.isAdmin);
const user = computed(() => authStore.user);

const toggleDropdown = () => {
  dropdownOpen.value = !dropdownOpen.value;
};

const openLogin = () => {
  dropdownOpen.value = false;
  // mobileMenuOpen.value = false;
  loginModalOpen.value = true;
};

const openRegister = () => {
  dropdownOpen.value = false;
  // mobileMenuOpen.value = false;
  registerModalOpen.value = true;
};

const logoutConfirmOpen = ref(false);

const requestLogout = () => {
  mobileMenuOpen.value = false;
  logoutConfirmOpen.value = true;
};

const confirmLogout = async () => {
  logoutConfirmOpen.value = false;
  await authStore.logout();
  router.push("/");
};

const toggleMobileMenu = () => {
  mobileMenuOpen.value = !mobileMenuOpen.value;
};

const closeMobileMenu = () => {
  mobileMenuOpen.value = false;
};

// Auto-hide navbar on scroll
const isHidden = ref(false);
let lastScrollY = 0;

const handleScroll = () => {
  const currentScrollY = window.scrollY;

  // Ha az oldal tetején vagyunk, mindig mutassuk a navbart
  if (currentScrollY < 10) {
    isHidden.value = false;
  }
  // Ha lefelé görgetünk, rejtsük el
  else if (currentScrollY > lastScrollY && currentScrollY > 100) {
    isHidden.value = true;
    dropdownOpen.value = false; // Zárjuk be a dropdownt görgetéskor
  }
  // Ha felfelé görgetünk, mutassuk meg
  else if (currentScrollY < lastScrollY) {
    isHidden.value = false;
  }

  lastScrollY = currentScrollY;
};

onMounted(() => {
  window.addEventListener("scroll", handleScroll, { passive: true });
});

onUnmounted(() => {
  window.removeEventListener("scroll", handleScroll);
});
</script>

<style scoped lang="scss">
.navbar {
  background: linear-gradient(135deg, #2c1810 0%, #4a2c1a 100%);
  padding: 1rem 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
  position: sticky;
  top: 0;
  z-index: 100;
  border-bottom: 3px solid #8b4513;
  transition: transform 0.3s ease-in-out;
}

.navbar-hidden {
  transform: translateY(-100%);
}

/* --- 3 COLUMN GRID --- */
.navbar-container {
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 1rem;
}

/* --- LOGO + TITLE --- */
.navbar-logo {
  display: flex;
  align-items: center;
  gap: 1rem;
  text-decoration: none;
}

.logo {
  height: 55px;
  width: auto;
}

.game-title {
  font-size: 1.7rem;
  font-weight: bold;
  color: #ffd700;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
  font-family: "Cinzel", serif;
}

/* --- DESKTOP MENU --- */
.desktop-menu {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.nav-link {
  position: relative;
  display: inline-block;
  color: #f4e4c1;
  text-decoration: none;
  font-size: 1.1rem;
  font-weight: 600;
  padding: 0.6rem 1.4rem;
  border-radius: 10px;
  background: linear-gradient(135deg, rgba(255, 215, 0, 0.08), rgba(255, 215, 0, 0.02));
  border: 1px solid rgba(255, 215, 0, 0.25);
  box-shadow:
    inset 0 0 0 rgba(255, 215, 0),
    0 4px 12px rgba(0, 0, 0, 0.4);
  transition: all 0.25s ease;
}

.nav-link:hover {
  color: #ffd700;
  transform: translateY(-2px);
  background: linear-gradient(135deg, rgba(255, 215, 0, 0.18), rgba(255, 215, 0, 0.06));
  box-shadow:
    0 0 12px rgba(255, 215, 0, 0.35),
    0 6px 16px rgba(0, 0, 0, 0.5);
}

.nav-link:active {
  transform: translateY(0);
  box-shadow:
    0 0 6px rgba(255, 215, 0, 0.25),
    inset 0 2px 6px rgba(0, 0, 0, 0.5);
}

.nav-link.router-link-active {
  color: #ffd700;
  background: linear-gradient(135deg, rgba(255, 215, 0, 0.12), rgba(255, 215, 0, 0.04));
}

/* --- AUTH (DESKTOP) --- */
.desktop-auth {
  display: flex;
  justify-content: flex-end;
  align-items: center;
}

.auth-dropdown {
  position: relative;
}

.auth-button {
  background: #8b4513;
  color: #fff;
  border: 2px solid #ffd700;
  padding: 0.6rem 1.2rem;
  border-radius: 8px;
  cursor: pointer;
  font-size: 1rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.3s ease;
}

.auth-button:hover {
  background: #a0522d;
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
}

.arrow {
  transition: transform 0.3s ease;
}

.arrow.open {
  transform: rotate(180deg);
}

.dropdown-menu {
  position: absolute;
  top: calc(100% + 0.5rem);
  right: 0;
  background: #3d2517;
  border: 2px solid #ffd700;
  border-radius: 8px;
  min-width: 200px;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.4);
  overflow: hidden;
}

.dropdown-item {
  display: block;
  width: 100%;
  padding: 0.8rem 1.2rem;
  background: transparent;
  color: #f4e4c1;
  border: none;
  text-align: left;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.2s ease;
}

.dropdown-item:hover {
  background: #8b4513;
  color: #ffd700;
}

.dropdown-item:not(:last-child) {
  border-bottom: 1px solid rgba(255, 215, 0, 0.2);
}

.user-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.username {
  color: #ffd700;
  font-weight: 600;
  font-size: 1.1rem;
  max-width: 140px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.logout-button {
  background: #8b4513;
  color: #fff;
  border: 2px solid #ffd700;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
}

.logout-button:hover {
  background: #a0522d;
  transform: translateY(-2px);
}

/* --- HAMBURGER --- */
.hamburger {
  display: none;
  width: 40px;
  height: 40px;
  border: 2px solid #ffd700;
  background: transparent;
  border-radius: 10px;
  cursor: pointer;
  position: relative;
  flex-shrink: 0;
}

.hamburger span {
  display: block;
  height: 3px;
  width: 70%;
  background: #ffd700;
  margin: 5px auto;
  transition: all 0.3s ease;
}

.hamburger span.open:nth-child(1) {
  transform: translateY(8px) rotate(45deg);
}
.hamburger span.open:nth-child(2) {
  opacity: 0;
}
.hamburger span.open:nth-child(3) {
  transform: translateY(-8px) rotate(-45deg);
}

/* ------------------- MOBILE MENU ------------------- */
.mobile-menu {
  display: none;
  flex-direction: column;
  gap: 0.5rem;
  padding: 1rem 2rem 1.5rem;
  background: linear-gradient(135deg, #2c1810 0%, #4a2c1a 100%);
  border-bottom: 3px solid #8b4513;
}

.mobile-link {
  display: block;
  color: #f4e4c1;
  text-decoration: none;
  padding: 0.8rem 1rem;
  border-radius: 10px;
  border: 1px solid rgba(255, 215, 0, 0.25);
  background: rgba(255, 215, 0, 0.06);
  font-weight: 600;
}

.mobile-link:hover {
  color: #ffd700;
  background: rgba(255, 215, 0, 0.14);
}

.mobile-auth {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
  margin-top: 0.5rem;
}

.mobile-auth-btn {
  flex: 1;
  padding: 0.8rem 1rem;
  border-radius: 10px;
  border: 2px solid #ffd700;
  background: #8b4513;
  color: #fff;
  font-weight: 600;
}

.mobile-auth-btn:hover {
  background: #a0522d;
}

/* ------------------- RESPONSIVE ------------------- */

/* Tablet és kisebb (1024px alatt) - Hamburger menü */
@media (max-width: 1024px) {
  .desktop-menu,
  .desktop-auth {
    display: none;
  }

  .hamburger {
    display: block;
  }

  .mobile-menu {
    display: flex;
  }

  .navbar-container {
    grid-template-columns: 1fr auto;
  }
}

/* Kis tablet és mobil (768px alatt) - Kisebb logo és cím */
@media (max-width: 768px) {
  .navbar {
    padding: 0.8rem 1rem;
  }

  .logo {
    height: 45px;
  }

  .game-title {
    font-size: 1.3rem;
  }

  .navbar-logo {
    gap: 0.8rem;
  }

  .mobile-menu {
    padding: 1rem 1rem 1.5rem;
  }
}

/* Nagyon kicsi mobil (480px alatt) - Még kompaktabb */
@media (max-width: 480px) {
  .logo {
    height: 40px;
  }

  .game-title {
    font-size: 1.1rem;
  }

  .navbar-logo {
    gap: 0.6rem;
  }
}
.username-link {
  color: #ffd700;
  font-weight: 600;
  font-size: 1.1rem;
  max-width: 140px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  text-decoration: none;
  transition: all 0.3s ease;
  cursor: pointer;
  display: inline-block;

  &:hover {
    color: #ffed4e;
    text-shadow: 0 0 10px rgba(255, 215, 0, 0.5);
  }
}
</style>
