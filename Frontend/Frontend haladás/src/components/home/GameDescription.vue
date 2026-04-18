<template>
  <section id="description" class="game-description">
    <div class="description-content">
      <!-- SZÖVEG RÉSZ -->
      <div class="text-side">
        <h2 class="section-title">A Játékról</h2>
        <div class="description-text">
          <p>
            A <strong>Fortivex Projekt</strong> egy epikus tower defense játék, ahol stratégiai
            gondolkodással és gyors döntésekkel kell megvédened a váradat a megszámlálhatatlan
            ellenség hullámaitól.
          </p>
          <p>
            Építs tornyokat, fejleszd őket, és használd ki az ellenségek gyengeségeit, hogy
            győzelmet arass! Minden pálya új kihívást és különleges ellenségeket tartogat.
          </p>

          <!-- FEATURE GRID -->
          <div class="features-grid">
            <div class="feature-item">
              <div class="feature-icon">🏰</div>
              <div class="feature-info">
                <h4>Stratégiai Mélység</h4>
                <p>Építs és fejlessz különböző tornyokat</p>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">🗺️</div>
              <div class="feature-info">
                <h4>Változatos Pályák</h4>
                <p>Fedezz fel egyedi környezeteket</p>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">👹</div>
              <div class="feature-info">
                <h4>Sokféle Ellenség</h4>
                <p>Különböző képességekkel rendelkező ellenfelek</p>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">⚡</div>
              <div class="feature-info">
                <h4>Progresszió Rendszer</h4>
                <p>Fejlődj és szerezz új képességeket</p>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">🏆</div>
              <div class="feature-info">
                <h4>Teljesítmények</h4>
                <p>Oldj meg kihívásokat és szerezz jutalmakat</p>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">📊</div>
              <div class="feature-info">
                <h4>Statisztikák</h4>
                <p>Kövesd nyomon fejlődésed</p>
              </div>
            </div>
          </div>

          <!-- GAMEPLAY STATS -->
          <div class="gameplay-stats">
            <div class="stat">
              <span class="stat-number">3</span>
              <span class="stat-label">Egyedi Pálya</span>
            </div>
            <div class="stat">
              <span class="stat-number">6</span>
              <span class="stat-label">Különböző Ellenség</span>
            </div>
            <div class="stat">
              <span class="stat-number">5</span>
              <span class="stat-label">Torony Típus</span>
            </div>
            <div class="stat">
              <span class="stat-number">5</span>
              <span class="stat-label">Teljesítmény</span>
            </div>
          </div>
        </div>
      </div>

      <!-- VIDEÓ ÉS LETÖLTÉS GOMB -->
      <!-- VIDEÓ ÉS LETÖLTÉS GOMB -->
      <div class="visual-side">
        <div class="video-wrapper" :class="{ fullscreen: isFullscreen }">
          <video class="preview-video" controls playsinline>
            <source src="/preview-video.mp4" type="video/mp4" />
          </video>

          <button class="expand-btn" @click.stop="toggleFullscreen">⛶</button>
        </div>

        <!-- VIDEÓ ALATTI TARTALOM -->
        <div class="preview-info">
          <h3>Próbáld ki most!</h3>
          <p>Tapasztald meg a Fortivex izgalmát</p>
          <!-- LETÖLTÉS DROPDOWN -->
          <div class="download-dropdown" ref="dropdownRef">
            <button class="cta-button primary" @click="toggleDropdown" :class="{ active: isOpen }">
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
                  href="/downloads/Fortivex_Mobile.apk"
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
        </div>

        <!-- EXTRA INFO CARDS -->
        <div class="info-cards">
          <div class="info-card">
            <div class="card-icon">💻</div>
            <div class="card-content">
              <h4>Platform</h4>
              <p>Windows, Linux, Mobil</p>
            </div>
          </div>

          <div class="info-card">
            <div class="card-icon">👥</div>
            <div class="card-content">
              <h4>Játékosok</h4>
              <p>Single Player</p>
            </div>
          </div>

          <div class="info-card">
            <div class="card-icon">🎮</div>
            <div class="card-content">
              <h4>Műfaj</h4>
              <p>Stratégia</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";

const isFullscreen = ref(false);
const toggleFullscreen = () => {
  isFullscreen.value = !isFullscreen.value;
};

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
</script>
<style scoped>
.game-description {
  padding: 5rem 2rem;
  background: #2c1810;
}

.description-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 4rem;
  max-width: 1400px;
  margin: 0 auto;

  @media (max-width: 1024px) {
    grid-template-columns: 1fr;
  }
}

.text-side {
  .description-text {
    p {
      color: #f4e4c1;
      line-height: 1.8;
    }
  }

  .features-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 1.5rem;
    margin: 2.5rem 0;

    @media (max-width: 640px) {
      grid-template-columns: 1fr;
    }
  }

  .gameplay-stats {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 1.5rem;
    margin-top: 3rem;

    @media (max-width: 768px) {
      grid-template-columns: repeat(2, 1fr);
    }
  }
}

.visual-side {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  align-items: center;
  width: 100%;

  .preview-info {
    padding: 2rem;
    text-align: center;

    h3 {
      color: #ffd700;
      font-size: 1.5rem;
      margin-bottom: 0.5rem;
    }

    p {
      color: #f4e4c1;
      margin-bottom: 1.5rem;
    }

    .download-button {
      background: linear-gradient(135deg, #8b4513, #a0522d);
      color: #fff;
      border: 2px solid #ffd700;
      padding: 1rem 2.5rem;
      border-radius: 12px;
      font-size: 1.1rem;
      font-weight: 700;
      cursor: pointer;
      display: inline-flex;
      align-items: center;
      gap: 0.8rem;
      transition: all 0.3s ease;

      .button-icon {
        font-size: 1.3rem;
      }

      &:hover {
        transform: translateY(-3px);
        box-shadow: 0 8px 24px rgba(255, 215, 0, 0.4);
      }
    }
  }

  .info-cards {
    display: grid;
    gap: 1rem;
    width: 100%;

    @media (max-width: 768px) {
      grid-template-columns: 1fr;
    }

    .info-card {
      background: #1a0f0a;
      border: 2px solid #8b4513;
      border-radius: 12px;
      padding: 1.5rem;
      display: flex;
      align-items: center;
      gap: 1.5rem;
      transition: all 0.3s ease;

      &:hover {
        border-color: #ffd700;
        transform: translateX(5px);
      }

      .card-icon {
        font-size: 2.5rem;
      }

      .card-content {
        h4 {
          color: #ffd700;
          font-size: 1.1rem;
          margin-bottom: 0.3rem;
        }

        p {
          color: #f4e4c1;
          margin: 0;
          font-size: 0.95rem;
        }
      }
    }
  }
}

.video-wrapper {
  position: relative;
  width: 100%;
  max-width: 700px;
  aspect-ratio: 16 / 9;
  border: 3px solid #8b4513;
  border-radius: 16px;
  overflow: hidden;
  background: #000;
  cursor: pointer;
}

.preview-video {
  width: 100%;
  height: 100%;
  object-fit: cover;
  position: absolute;
  top: 0;
  left: 0;
}

.video-overlay {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  color: #ffd700;
  z-index: 2;
  transition: opacity 0.3s ease;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1.5rem;
  margin: 2.5rem 0;

  @media (max-width: 640px) {
    grid-template-columns: 1fr;
  }
}

.feature-item {
  display: flex;
  gap: 1rem;
  padding: 1.5rem;
  background: #1a0f0a;
  border: 2px solid #8b4513;
  border-radius: 12px;
  transition: all 0.3s ease;

  &:hover {
    border-color: #ffd700;
    transform: translateY(-3px);
    box-shadow: 0 8px 20px rgba(255, 215, 0, 0.2);
  }

  .feature-icon {
    font-size: 2.5rem;
    flex-shrink: 0;
  }

  .feature-info {
    h4 {
      color: #ffd700;
      font-size: 1.3rem;
      margin-bottom: 0.5rem;
    }

    p {
      color: #f4e4c1;
      font-size: 1.1rem;
      line-height: 1.4;
      margin: 0;
    }
  }
}
.gameplay-stats {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1.5rem;
  margin-top: 3rem;

  @media (max-width: 768px) {
    grid-template-columns: repeat(2, 1fr);
  }
}

.stat {
  text-align: center;
  padding: 1.5rem;
  background: linear-gradient(135deg, #1a0f0a 0%, #2c1810 100%);
  border: 2px solid #8b4513;
  border-radius: 12px;

  .stat-number {
    display: block;
    font-size: 2.5rem;
    font-weight: 700;
    color: #ffd700;
    margin-bottom: 0.5rem;
  }

  .stat-label {
    display: block;
    color: #f4e4c1;
    font-size: 0.9rem;
  }
}
.description-text {
  p {
    color: #f4e4c1;
    font-size: 1.3rem;
    font-family: "Source Sans Pro", serif;
    font-weight: 500;
    line-height: 1.8;
    margin-bottom: 1.5rem;
    margin-top: 1.5rem;

    strong {
      color: #ffd700;
      font-weight: 700;
    }
  }
  .info-cards {
    display: grid;
    gap: 1rem;
  }

  .info-card {
    background: #1a0f0a;
    border: 2px solid #8b4513;
    border-radius: 12px;
    padding: 1.5rem;
    display: flex;
    align-items: center;
    gap: 1.5rem;
    transition: all 0.3s ease;

    &:hover {
      border-color: #ffd700;
      transform: translateX(5px);
    }

    .card-icon {
      font-size: 2.5rem;
    }

    .card-content {
      h4 {
        color: #ffd700;
        font-size: 1.1rem;
        margin-bottom: 0.3rem;
      }

      p {
        color: #f4e4c1;
        margin: 0;
        font-size: 0.95rem;
      }
    }
  }
  .expand-btn {
    position: absolute;
    bottom: 1rem;
    right: 1rem;
    background: rgba(0, 0, 0, 0.6);
    border: none;
    color: #ffd700;
    font-size: 1.4rem;
    padding: 0.4rem 0.6rem;
    border-radius: 6px;
    cursor: pointer;
    z-index: 3;
  }

  .video-wrapper.fullscreen {
    position: fixed;
    inset: 0;
    z-index: 9999;
    max-width: none;
    border-radius: 0;
  }

  .video-wrapper.fullscreen .preview-video {
    width: 100%;
    height: 100%;
    object-fit: contain;
  }
  .section-title {
    font-size: 3rem; /* nagyobb cím */
    margin-bottom: 3rem; /* térköz a szöveg felé */
    font-family: "Cinzel", serif; /* ugyanaz a font, mint a dizájn */
    color: #ffd700;
    letter-spacing: 0.05em;
  }
}

.preview-info > a {
  background: linear-gradient(135deg, #8b4513, #a0522d);
  color: #fff;
  border: 2px solid #ffd700;
  padding: 1rem 2.5rem;
  border-radius: 12px;
  font-size: 1.1rem;
  font-weight: 700;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.8rem;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-3px);
    box-shadow: 0 8px 24px rgba(255, 215, 0, 0.4);
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
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.6);
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
  border-bottom: 1px solid rgba(200, 168, 75, 0.2);
  border-radius: 0;
}

.dropdown-item:last-child {
  border-bottom: none;
}

.dropdown-item:hover {
  background: rgba(200, 168, 75, 0.15);
}

.platform-icon {
  font-size: 1.4em;
  flex-shrink: 0;
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

.cta-button.primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 12px 28px;
  background: #8b4513;
  color: #ffd700;
  border: 2px solid #c8a84b;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 700;
  cursor: pointer;
  transition:
    background 0.2s,
    transform 0.1s;
}

.cta-button.primary:hover {
  background: #a0522d;
  transform: scale(1.03);
}
</style>
