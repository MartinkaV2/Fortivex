<template>
  <div class="home-page">
    <Navbar />

    <main class="main-content">
      <section class="hero-section">
        <video class="hero-video" autoplay muted loop playsinline>
          <source src="/hero-video.mp4" type="video/mp4" />
        </video>

        <div class="hero-overlay"></div>

        <div class="hero-content">
          <h1 class="hero-title">FORTIVEX PROJEKT</h1>
          <p class="hero-subtitle">Védd meg a váradat a sötétség erőitől!</p>

          <div class="hero-buttons">
            <!-- LETÖLTÉS DROPDOWN -->
            <div class="download-dropdown" ref="dropdownRef">
              <button
                class="cta-button primary"
                @click="toggleDropdown"
                :class="{ active: isOpen }"
              >
                Játék Letöltése
                <span class="arrow" :class="{ rotated: isOpen }">▾</span>
              </button>

              <transition name="dropdown">
                <div v-if="isOpen" class="dropdown-menu">
                  <a
                    href="/downloads/Fortivex_The_Game.zip"
                    download
                    class="dropdown-item"
                    @click="isOpen = false"
                  >
                    <span class="platform-icon">🖥️</span>
                    <span class="platform-info">
                      <strong>Windows</strong>
                      <small>.zip, ~73.5 MB</small>
                    </span>
                  </a>

                  <a
                    href="/downloads/Fortivex_Linux.zip"
                    download
                    class="dropdown-item"
                    @click="isOpen = false"
                  >
                    <span class="platform-icon">🐧</span>
                    <span class="platform-info">
                      <strong>Linux</strong>
                      <small>.zip, ~67.5 MB</small>
                    </span>
                  </a>

                  <a
                    href="/downloads/Fortivex_Android.apk"
                    download
                    class="dropdown-item"
                    @click="isOpen = false"
                  >
                    <span class="platform-icon">📱</span>
                    <span class="platform-info">
                      <strong>Android</strong>
                      <small>.apk, ~86.4 MB</small>
                    </span>
                  </a>
                </div>
              </transition>
            </div>
            <!-- LETÖLTÉS DROPDOWN VÉGE -->

            <button class="cta-button secondary" @click="scrollToSection('description')">
              Tudj meg többet
            </button>
          </div>
        </div>
      </section>

      <GameDescription />
      <MapsList />
      <EnemiesList />
      <TowersList />

      <section id="contact" class="contact-section">
        <div class="container">
          <h2 class="section-title">Elérhetőségek</h2>
          <div class="contact-content">
            <div class="contact-info">
              <div class="contact-item">
                <span class="contact-icon">📧</span>
                <div>
                  <h4>Email</h4>
                  <p class="email">fortivexgames@gmail.com</p>
                </div>
              </div>
              <div class="contact-item">
                <span class="contact-icon">💬</span>
                <div>
                  <h4>Discord</h4>
                  <p>discord.gg/fortivex</p>
                </div>
              </div>
              <div class="contact-item support-card">
                <span class="contact-icon">🎫</span>
                <div>
                  <h4>Támogatás / Hibajelentés</h4>
                  <p>Ha a Súgóban nem találtál választ, írj közvetlenül a fejlesztőknek.</p>
                  <RouterLink to="/support" class="support-button">Submit ticket</RouterLink>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <Footer />
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";
import Navbar from "@/components/common/Navbar.vue";
import Footer from "@/components/common/Footer.vue";
import GameDescription from "@/components/home/GameDescription.vue";
import MapsList from "@/components/home/MapsList.vue";
import EnemiesList from "@/components/home/EnemiesList.vue";
import TowersList from "@/components/home/TowersList.vue";

const showContactForm = ref(false);

// Dropdown logika
const isOpen = ref(false);
const dropdownRef = ref(null);

const toggleDropdown = () => {
  isOpen.value = !isOpen.value;
};

const handleClickOutside = (event) => {
  if (dropdownRef.value && !dropdownRef.value.contains(event.target)) {
    isOpen.value = false;
  }
};

onMounted(() => document.addEventListener("click", handleClickOutside));
onUnmounted(() => document.removeEventListener("click", handleClickOutside));

const scrollToSection = (sectionId) => {
  const element = document.getElementById(sectionId);
  if (element) {
    element.scrollIntoView({ behavior: "smooth" });
  }
};
</script>

<style scoped lang="scss">
/* Reset a teljes oldalra */
html,
body {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
  overflow-x: hidden;
  font-family: "Arial", sans-serif; /* opcionális */
}

.home-page {
  width: 100%;
  min-height: 100vh;
  background: linear-gradient(135deg, #1a0f0a 0%, #2c1810 100%);
}

/* Main tartalom */
.main-content {
  width: 100%;
  margin: 0;
  padding: 0;
  flex: 1;
}

/* HERO */
.hero-section {
  position: relative;
  width: 100%;
  height: 100vh; /* teljes képernyő */
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  overflow: hidden;
  background:
    linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)),
    url("/hero-bg.jpg") center/cover no-repeat;
  padding: 0;
}

.hero-video {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  z-index: 1;
  filter: blur(6px) brightness(0.7);
}

.hero-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.7));
  z-index: 2;
}

.hero-content {
  max-width: 800px;
  width: 90%;
  z-index: 3;
}

/* Hero címek és gombok */
.hero-title {
  font-size: 4rem;
  color: #ffd700;
  font-family: "Cinzel", serif;
  text-shadow: 4px 4px 8px rgba(0, 0, 0, 0.9);
  margin-bottom: 1rem;
  animation: fadeInUp 1s ease;
}

.hero-subtitle {
  font-size: 1.5rem;
  color: #f4e4c1;
  margin-bottom: 2rem;
  animation: fadeInUp 1s ease 0.2s both;
}

.hero-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
  animation: fadeInUp 1s ease 0.4s both;
}

.cta-button {
  padding: 1rem 2rem;
  font-size: 1.1rem;
  font-weight: 700;
  border: 2px solid #ffd700;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.cta-button.primary {
  background: #8b4513;
  color: #fff;
}

.cta-button.primary:hover {
  background: #a0522d;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(255, 215, 0, 0.4);
}

.cta-button.secondary {
  background: transparent;
  color: #ffd700;
}

.cta-button.secondary:hover {
  background: rgba(255, 215, 0, 0.1);
  transform: translateY(-3px);
}

/* Fade animáció */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Kontakt szekció */
.contact-section {
  padding: 5rem 1rem;
  background: #2c1810;
}

.contact-content {
  max-width: 600px;
  margin: 0 auto;
}

.contact-info {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.contact-item {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  background: #2c1810;
  padding: 1.5rem;
  border: 2px solid #8b4513;
  border-radius: 12px;
}

.contact-icon {
  font-size: 2.5rem;
}

.contact-item h4 {
  color: #ffd700;
  font-size: 1.2rem;
  margin-bottom: 0.3rem;
}

.contact-item p {
  color: #f4e4c1;
}
.email-link {
  pointer-events: auto;
  position: relative;
  z-index: 10;
  cursor: pointer;
  color: #ffd700;
  text-decoration: underline;

  &:hover {
    color: #fff;
  }
}
.support-button {
  background: #3d2517;
  border: 2px solid #ffd700;
  color: #fff;
  padding: 0.6rem 1.2rem;
  border-radius: 6px;
  cursor: pointer;
  margin-top: 0.8rem;
  transition: all 0.3s ease;
  font-weight: 600;
  &:hover {
    background: #5c3a25;
    transform: translateY(-2px);
    box-shadow: 0 6px 15px rgba(255, 215, 0, 0.3);
  }
}

/* Reszponzív */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
  }

  .hero-buttons {
    flex-direction: column;
    align-items: stretch; // <<< fontos
  }

  .hero-content {
    width: 95%;
  }

  /* ÚJ */

  .download-dropdown {
    width: 100%;
  }

  .cta-button {
    width: 100%;
  }
}

@media (max-width: 1024px) and (min-width: 769px) {
  .download-dropdown {
    width: 100%;
  }

  .dropdown-menu {
    position: static; /* <<< EZ A LÉNYEG */
    margin-top: 10px;
    width: 100%;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.4);
  }

  .hero-buttons {
    align-items: stretch;
  }

  .cta-button {
    width: 100%;
  }

  .hero-section {
    min-height: 100vh;
    height: auto;
  }
}

.download-dropdown {
  position: relative;
  display: inline-block;
}

.arrow {
  display: inline-block;
  transition: transform 0.2s ease;
  font-size: 1.1em;
}

.arrow.rotated {
  transform: rotate(180deg);
}

.dropdown-menu {
  position: absolute;
  top: calc(100% + 8px);
  left: 0;
  min-width: 220px;
  background: #2c1a0e;
  border: 1px solid #c8a84b;
  border-radius: 10px;
  overflow: hidden;
  box-shadow:
    0 10px 40px rgba(0, 0, 0, 0.6),
    0 0 0 1px rgba(200, 168, 75, 0.2);
  z-index: 100;
}

.dropdown-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 16px;
  color: #f0d080;
  text-decoration: none !important;
  transition: background 0.15s;
  border-bottom: 1px solid rgba(200, 168, 75, 0.15);
}

.dropdown-item:last-child {
  border-bottom: none;
}

.dropdown-item:hover {
  background: rgba(200, 168, 75, 0.15);
  color: #ffd700;
}

.platform-icon {
  font-size: 1.4em;
}

.platform-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.platform-info strong {
  font-size: 0.95rem;
  font-weight: 600;
}

.platform-info small {
  font-size: 0.75rem;
  opacity: 0.65;
  color: #c8a84b;
}

.dropdown-enter-active,
.dropdown-leave-active {
  transition:
    opacity 0.2s ease,
    transform 0.2s ease;
}

.dropdown-enter-from,
.dropdown-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}
</style>
