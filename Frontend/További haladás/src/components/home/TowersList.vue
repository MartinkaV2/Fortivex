<template>
  <section id="towers" class="towers-section">
    <div class="container">
      <h2 class="section-title">Tornyok</h2>
      <p class="section-subtitle">
        Válassz és fejlessz tornyokat, hogy megvédd a váradat a támadóktól!
      </p>

      <div v-if="loading" class="loading">
        <LoadingSpinner text="Tornyok betöltése..." />
      </div>

      <div v-else-if="towers.length === 0" class="empty-state">
        <p>Még nincsenek elérhető torony adatok</p>
      </div>

      <div v-else class="towers-grid">
        <div v-for="tower in towers" :key="tower.id" class="tower-card">
          <div class="tower-header">
            <div class="tower-icon-wrapper">
              <img
                class="tower-icon"
                :src="tower.images[tower.currentImageIndex]"
                alt="tower icon"
                @click="nextTowerImage(tower)"
              />
              <div class="rarity-indicator" :class="`rarity-${tower.rarity}`"></div>
            </div>
            <div class="tower-title-section">
              <h3 class="tower-name">{{ tower.name }}</h3>
              <span class="tower-type" :class="`type-${tower.type}`">
                {{ tower.type }}
              </span>
            </div>
          </div>

          <p class="tower-description">{{ tower.description }}</p>

          <div class="tower-stats-section">
            <h4 class="stats-title">Tulajdonságok</h4>

            <div class="stat-bar-item" v-for="stat in ['damage', 'range', 'fireRate']" :key="stat">
              <div class="stat-bar-header">
                <span class="stat-label">
                  {{ statLabels[stat] }}
                </span>
                <span class="stat-value">{{ tower[stat] }}%</span>
              </div>
              <div class="stat-bar">
                <div class="stat-fill" :class="stat" :style="{ width: `${tower[stat]}%` }"></div>
              </div>
            </div>
          </div>

          <div v-if="tower.abilities" class="tower-abilities">
            <h4 class="abilities-title">Képességek</h4>
            <div class="abilities-list">
              <div v-for="ability in tower.abilities" :key="ability" class="ability-wrapper">
                <span class="ability-badge">
                  {{ ability }}
                </span>
                <div class="ability-tooltip">
                  {{ getAbilityDescription(ability) }}
                </div>
              </div>
            </div>
          </div>

          <div class="tower-costs">
            <div class="cost-item">
              <span class="cost-icon">💰</span>
              <span class="cost-value">{{ tower.goldCost }} arany</span>
            </div>
            <div class="cost-item">
              <span class="cost-icon">⚙️</span>
              <span class="cost-value">{{ tower.buildTime }} mp építési idő</span>
            </div>
          </div>
        </div>
        <!-- ✅ tower-card lezárása -->
      </div>
      <!-- ✅ towers-grid lezárása -->
    </div>
    <!-- ✅ container lezárása -->
  </section>
</template>

<script setup>
import { ref } from "vue";
import LoadingSpinner from "@/components/common/LoadingSpinner.vue";

// Példaképp ide jöhetnek a torony ikonok, cseréld ki a saját képeidre:
import ArrowTowerImg from "@/assets/images/towers/Towers3.png";
import ArrowTowerLv2Img from "@/assets/images/towers/Arrow2.png";
import ArrowTowerLv3Img from "@/assets/images/towers/Arrow3.png";
import CannonTowerImg from "@/assets/images/towers/Towers5.png";
import CannonTowerLv2Img from "@/assets/images/towers/Bomb2.png";
import CannonTowerLv3Img from "@/assets/images/towers/Bomb3.png";
import MagicTowerImg from "@/assets/images/towers/Towers4.png";
import MagicTowerLv2Img from "@/assets/images/towers/Magic2.png";
import MagicTowerLv3Img from "@/assets/images/towers/Magic3.png";
import GuardTowerImg from "@/assets/images/towers/Guard_lvl_1_95px.png";
import GuardTowerLv2Img from "@/assets/images/towers/Guard2.png";
import GuardTowerLv3Img from "@/assets/images/towers/Guard_3.png";
import ArrowTower2Img from "@/assets/images/towers/Towers2.png";
import ArrowTower2Lv2Img from "@/assets/images/towers/Arrow22.png";
import ArrowTower2Lv3Img from "@/assets/images/towers/Arrow23.png";

const towers = ref([
  {
    id: 1,
    name: "Nyílvesszős Torony",
    description: "Gyors támadású torony, hatékony gyalogság ellen.",
    images: [
      ArrowTowerImg, // level 1
      ArrowTowerLv2Img, // level 2
      ArrowTowerLv3Img, // level 3
    ],
    currentImageIndex: 0,
    type: "Fizikai",
    rarity: "common",
    damage: 70,
    range: 60,
    fireRate: 85,
    goldCost: 15,
    buildTime: 1,
    abilities: ["Gyors lövések", "Közepes hatótáv", "Fizikai sebzés"],
  },
  {
    id: 2,
    name: "Ágyútorony",
    description: "Erős robbanótámadás, lassabb tüzeléssel, hatékony az ellenséggel szemben.",
    images: [
      CannonTowerImg, // level 1
      CannonTowerLv2Img, // level 2
      CannonTowerLv3Img, // level 3
    ],
    currentImageIndex: 0,
    type: "Robbanó",
    rarity: "uncommon",
    damage: 90,
    range: 50,
    fireRate: 40,
    goldCost: 10,
    buildTime: 1,
    abilities: ["Robbanó lövedék", "Lassú tüzelés", "Területsebzés"],
  },
  {
    id: 3,
    name: "Mágikus Torony",
    description: "Sebzi az ellenséget varázslattal.",
    images: [
      MagicTowerImg, // level 1
      MagicTowerLv2Img, // level 2
      MagicTowerLv3Img, // level 3
    ],
    currentImageIndex: 0,
    type: "Mágikus",
    rarity: "rare",
    damage: 80,
    range: 70,
    fireRate: 60,
    goldCost: 6,
    buildTime: 1,
    abilities: ["Varázslatos sebzés", "Terület hatás"],
  },
  {
    id: 4,
    name: "Íjász Torony",
    description: "Hosszú hatótávú torony, lassú tüzeléssel, hatékony légies ellenfelek ellen.",
    images: [
      ArrowTower2Img, // level 1
      ArrowTower2Lv2Img, // level 2
      ArrowTower2Lv3Img, // level 3
    ],
    currentImageIndex: 0,
    type: "Fizikai",
    rarity: "epic",
    damage: 75,
    range: 90,
    fireRate: 50,
    goldCost: 5,
    buildTime: 1,
    abilities: ["Hosszú hatótáv", "Légies célpontok ellen hatékony", "Fizikai sebzés"],
  },
  {
    id: 5,
    name: "Katonai Torony",
    description: "Három katona hősiesen harcol az ellenség ellen karddal, nyílakkal, puskákkal.",
    images: [
      GuardTowerImg, // level 1
      GuardTowerLv2Img, // level 2
      GuardTowerLv3Img, // level 3
    ],
    currentImageIndex: 0,
    type: "Fizikai",
    rarity: "common",
    damage: 65,
    range: 55,
    fireRate: 75,
    goldCost: 4,
    buildTime: 1,
    abilities: ["Karddal támad", "Íjakkal lő", "Puskával céloz", "Fizikai sebzés"],
  },
]);

const loading = ref(false);

const statLabels = {
  damage: "Sebzés",
  range: "Hatótáv",
  fireRate: "Tűzgyorsaság",
};
const nextTowerImage = (tower) => {
  tower.currentImageIndex = (tower.currentImageIndex + 1) % tower.images.length;
};

const getAbilityDescription = (ability) => {
  const descriptions = {
    "Gyors lövések": "Nagyon gyors tüzelési sebességgel támad",
    "Közepes hatótáv": "Közepes távolságról ellenségeket céloz",
    "Robbanó lövedék": "Robbanó sebzést okoz a célpontnak",
    "Lassú tüzelés": "Lassabb tüzelési sebesség",
    Területsebzés: "Több ellenséget sebez egyszerre",
    "Varázslatos sebzés": "Mágikus sebzést okoz",
    "Terület hatás": "Hatással van a környező ellenségekre",
    "Hosszú hatótáv": "Nagy távolságból támad",
    "Légies célpontok ellen hatékony": "Extra sebzés légies egységek ellen",
    "Karddal támad": "Közelharcban kardot használ",
    "Íjakkal lő": "Közepes távolságból íjjal támad",
    "Puskával céloz": "Hosszú távon puskával lő",
    "Fizikai sebzés": "Fizikai sebzést okoz",
  };

  return descriptions[ability] || "Speciális képesség";
};
</script>

<style scoped lang="scss">
.towers-section {
  padding: 5rem 0;
  background: #1a0f0a;
}

.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 2rem;
}

.section-title {
  font-size: 3rem;
  color: #ffd700;
  text-align: center;
  font-family: "Cinzel", serif;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
  margin-bottom: 1rem;

  @media (max-width: 768px) {
    font-size: 2rem;
  }
}

.section-subtitle {
  text-align: center;
  color: #f4e4c1;
  font-size: 1.1rem;
  margin-bottom: 3rem;
  max-width: 600px;
  margin-left: auto;
  margin-right: auto;
}

.loading {
  display: flex;
  justify-content: center;
  padding: 4rem 0;
}

.towers-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 2rem;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
}

.tower-card {
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 16px;
  padding: 1.5rem;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;

  &::before {
    content: "";
    position: absolute;
    top: -2px;
    left: -2px;
    right: -2px;
    bottom: -2px;
    background: linear-gradient(135deg, transparent, rgba(255, 215, 0, 0.1));
    border-radius: 16px;
    opacity: 0;
    transition: opacity 0.3s ease;
    z-index: 0;
  }

  &:hover {
    transform: translateY(-8px);
    border-color: #ffd700;
    box-shadow: 0 12px 40px rgba(255, 215, 0, 0.3);

    &::before {
      opacity: 1;
    }
  }

  > * {
    position: relative;
    z-index: 1;
  }
}

.tower-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.tower-icon-wrapper {
  position: relative;
  cursor: pointer;
  display: inline-block;
  border-radius: 12px;
  transition:
    transform 0.2s ease,
    box-shadow 0.2s ease;

  &:hover {
    transform: scale(1.03);
    box-shadow: 0 0 15px rgba(255, 215, 0, 0.4);
  }

  &:active {
    transform: scale(0.98);
  }

  .tower-icon {
    width: 100%;
    height: 150px;
    object-fit: cover;
    border-radius: 8px;
    display: block;
  }

  .rarity-indicator {
    position: absolute;
    bottom: -5px;
    right: -5px;
    width: 20px;
    height: 20px;
    border-radius: 50%;
    border: 2px solid #1a0f0a;

    &.rarity-common {
      background: #808080;
    }

    &.rarity-uncommon {
      background: #32cd32;
      box-shadow: 0 0 10px #32cd32;
    }

    &.rarity-rare {
      background: #4169e1;
      box-shadow: 0 0 10px #4169e1;
    }

    &.rarity-epic {
      background: #9932cc;
      box-shadow: 0 0 10px #9932cc;
    }

    &.rarity-legendary {
      background: #ffd700;
      box-shadow: 0 0 10px #ffd700;
    }
  }
}

.tower-title-section {
  flex: 1;

  .tower-name {
    color: #ffd700;
    font-size: 1.4rem;
    font-weight: 700;
    margin-bottom: 0.3rem;
  }

  .tower-type {
    display: inline-block;
    padding: 0.3rem 0.8rem;
    border-radius: 12px;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.5px;

    &.type-fizikai {
      background: rgba(139, 69, 19, 0.3);
      color: #d2691e;
    }

    &.type-robbantó {
      background: rgba(128, 128, 128, 0.3);
      color: #c0c0c0;
    }

    &.type-mágikus {
      background: rgba(138, 43, 226, 0.3);
      color: #9370db;
    }

    &.type-speciális {
      background: rgba(255, 140, 0, 0.3);
      color: #ffa500;
    }
  }
}

.tower-description {
  color: #f4e4c1;
  font-size: 0.9rem;
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.tower-stats-section {
  margin-bottom: 1.5rem;

  .stats-title {
    color: #ffd700;
    font-size: 0.9rem;
    text-transform: uppercase;
    letter-spacing: 1px;
    margin-bottom: 1rem;
  }
}

.stat-bar-item {
  margin-bottom: 1rem;

  &:last-child {
    margin-bottom: 0;
  }
}

.stat-bar-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.4rem;

  .stat-label {
    color: #f4e4c1;
    font-size: 0.85rem;
    font-weight: 600;
  }

  .stat-value {
    color: #ffd700;
    font-size: 0.85rem;
    font-weight: 700;
  }
}

.stat-bar {
  width: 100%;
  height: 10px;
  background: #2c1810;
  border-radius: 5px;
  overflow: hidden;
  border: 1px solid rgba(139, 69, 19, 0.5);

  .stat-fill {
    height: 100%;
    transition: width 0.5s ease;
    position: relative;

    &::after {
      content: "";
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
      animation: shimmer 2s infinite;
    }

    &.damage {
      background: linear-gradient(90deg, #dc143c, #ff6b6b);
    }

    &.range {
      background: linear-gradient(90deg, #4169e1, #87ceeb);
    }

    &.fireRate {
      background: linear-gradient(90deg, #32cd32, #90ee90);
    }
  }
}

@keyframes shimmer {
  0%,
  100% {
    transform: translateX(-100%);
  }
  50% {
    transform: translateX(100%);
  }
}

.tower-abilities {
  margin-bottom: 1.5rem;

  .abilities-title {
    color: #ffd700;
    font-size: 0.9rem;
    text-transform: uppercase;
    letter-spacing: 1px;
    margin-bottom: 0.8rem;
  }

  .abilities-list {
    display: flex;
    flex-wrap: wrap;
    gap: 0.5rem;

    .ability-badge {
      padding: 0.4rem 0.8rem;
      background: rgba(138, 43, 226, 0.2);
      color: #9370db;
      border: 1px solid rgba(138, 43, 226, 0.5);
      border-radius: 12px;
      font-size: 0.75rem;
      font-weight: 600;
    }

    .ability-wrapper:hover .ability-badge {
      background: rgba(138, 43, 226, 0.4);
      color: #9370db;
      border-color: rgba(138, 43, 226, 0.7);
      transform: scale(1.05);
      cursor: pointer;
    }
  }
  .ability-wrapper {
    position: relative; /* FONTOS a tooltip pozícionáláshoz */

    &:hover .ability-badge {
      background: rgba(138, 43, 226, 0.4);
      color: #9370db;
      border-color: rgba(138, 43, 226, 0.7);
      transform: scale(1.05);
      cursor: pointer;
    }

    &:hover .ability-tooltip {
      opacity: 1;
    }
  }
}

.tower-costs {
  display: flex;
  justify-content: space-around;
  padding: 1rem;
  background: #2c1810;
  border-radius: 8px;
  border: 1px solid #8b4513;

  .cost-item {
    display: flex;
    align-items: center;
    gap: 0.5rem;

    .cost-icon {
      font-size: 1.5rem;
    }

    .cost-value {
      color: #ffd700;
      font-weight: 700;
      font-size: 0.9rem;
    }
  }
}

.empty-state {
  text-align: center;
  padding: 4rem;
  color: #f4e4c1;
  font-size: 1.1rem;
}

section {
  scroll-margin-top: 90px; /* vagy navbarod magassága */
}

/* TOOLTIP */
.ability-tooltip {
  position: absolute;
  bottom: 120%;
  left: 50%;
  transform: translateX(-90%);
  white-space: nowrap;

  background: #fff;
  color: #333;
  padding: 0.4rem 0.6rem;
  border-radius: 6px;
  font-size: 0.7rem;

  max-width: 220px;
  white-space: normal;
  word-wrap: break-word;
  text-align: left;
  height: inherit;

  opacity: 0;
  pointer-events: none;
  transition: opacity 0.2s ease;
  z-index: 10;

  /* kis nyíl */
  &::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    transform: translateX(-50%);
    border-width: 5px;
    border-style: solid;
    border-color: #1e1e2e transparent transparent transparent;
  }
}

/* HOVER */
.ability-wrapper:hover .ability-tooltip {
  opacity: 1;
}
</style>
