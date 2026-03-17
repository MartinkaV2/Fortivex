<template>
  <section id="enemies" class="enemies-section">
    <div class="container">
      <h2 class="section-title">Ellenségek</h2>
      <p class="section-subtitle">
        Ismerd meg ellenfeleidet, és használd ki gyengeségeiket a győzelemhez!
      </p>

      <div v-if="loading" class="loading">
        <LoadingSpinner text="Ellenségek betöltése..." />
      </div>

      <div v-else class="enemies-grid">
        <div v-for="enemy in enemies" :key="enemy.id" class="enemy-card">
          <div class="enemy-header">
            <div class="enemy-icon-wrapper" :class="{ 'is-shaman': enemy.name === 'Sámán','is-clickable': enemy.images  }">
              <img
                class="enemy-icon"
                :src="enemy.images ? enemy.images[enemy.currentImageIndex] : enemy.icon"
                alt="enemy icon"
                @click="enemy.images && nextEnemyImage(enemy)"
              />
              <div class="rarity-indicator" :class="`rarity-${enemy.rarity}`"></div>
            </div>
            <div class="enemy-title-section">
              <h3 class="enemy-name">{{ enemy.name }}</h3>
              <span class="enemy-type" :class="`type-${enemy.type}`">
                {{ enemy.type }}
              </span>
            </div>
          </div>

          <p class="enemy-description">{{ enemy.description }}</p>

          <div class="enemy-stats-section">
            <h4 class="stats-title">Tulajdonságok</h4>

            <div class="stat-bar-item">
              <div class="stat-bar-header">
                <span class="stat-label">❤️ Életerő</span>
                <span class="stat-value">{{ enemy.health }}%</span>
              </div>
              <div class="stat-bar">
                <div class="stat-fill health" :style="{ width: `${enemy.health}%` }"></div>
              </div>
            </div>

            <div class="stat-bar-item">
              <div class="stat-bar-header">
                <span class="stat-label">⚡ Sebesség</span>
                <span class="stat-value">{{ enemy.speed }}%</span>
              </div>
              <div class="stat-bar">
                <div class="stat-fill speed" :style="{ width: `${enemy.speed}%` }"></div>
              </div>
            </div>
          </div>

          <div v-if="enemy.abilities" class="enemy-abilities">
            <h4 class="abilities-title">Képességek</h4>
            <div class="abilities-list">
              <div v-for="ability in enemy.abilities" :key="ability" class="ability-wrapper">
                <span class="ability-badge">
                  {{ ability }}
                </span>
                <div class="ability-tooltip">
                  {{ getAbilityDescription(ability) }}
                </div>
              </div>
            </div>
          </div>
          <div class="enemy-rewards">
            <div class="reward-item">
              <span class="reward-icon">💰</span>
              <span class="reward-value">{{ enemy.goldReward || 10 }} arany</span>
            </div>
            <div class="reward-item">
              <span class="reward-icon">✨</span>
              <span class="reward-value">{{ enemy.xpReward || 5 }} XP</span>
            </div>
          </div>
        </div>
      </div>

      <div v-if="!loading && enemies.length === 0" class="empty-state">
        <p>Még nincsenek elérhető ellenség adatok</p>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";
import LoadingSpinner from "@/components/common/LoadingSpinner.vue";
import MonsterImg from "@/assets/images/enemies/Monsters_.png";
import WolfImg from "@/assets/images/enemies/Wolf.png";
import OrkImg from "@/assets/images/enemies/Ogre_.png";
import ShamanImg from "@/assets/images/enemies/Monster_shaman.png";
import ShamanImg2 from "@/assets/images/enemies/W_Saman_95px2.png";
import TankImg from "@/assets/images/enemies/S_Tank_0.png";
import BatImg from "@/assets/images/enemies/Bat_60px_small.png";
import MonsterImg2 from "@/assets/images/enemies/Orc_95px (Egyéni)2.png"

const enemies = ref([
  {
    id: 1,
    name: "Goblin Harcos",
    description:
      "Gyors ellenség, nagy számban támad. Két variánsa van: Goblin Harcos és Goblin Dárdás.",
    images: [MonsterImg, MonsterImg2],
    currentImageIndex: 0,
    type: "Gyalogos",
    rarity: "common",
    health: 9,
    speed: 80,
    armor: 10,
    goldReward: 1,
    xpReward: 3,
    abilities: ["Gyors mozgás", "Csordaszellem"],
  },
  {
    id: 2,
    name: "Ork Harcos",
    description: "Lomha sebességű, nagy életerővel rendelkező ellenség. Közelharci egység.",
    icon: OrkImg,
    type: "Nehézgyalogos",
    rarity: "uncommon",
    health: 17,
    speed: 60,
    armor: 40,
    goldReward: 1,
    xpReward: 3,
    abilities: ["Szívósság"],
  },
  {
    id: 3,
    name: "Sámán",
    description:
      "Lomha, de nagyon veszélyes ellenfél. Képes a szövetségesei gyógyítására. Elsőszámú célpont.",
    images: [ShamanImg, ShamanImg2],
    currentImageIndex: 0,
    type: "Gyógyító",
    rarity: "rare",
    health: 40,
    speed: 40,
    armor: 60,
    goldReward: 1,
    xpReward: 3,
    abilities: ["Óriási életerő", "Gyógyítás"],
  },
  {
    id: 4,
    name: "Farkas",
    description:
      "Ellusive, közelharci ellenség. Egy fürgén mozgó, nehezen hozzáférhető ellenfél, aki képes kikerülni a tornyaid hatótávolságából.",
    icon: WolfImg,
    type: "Gyalogos",
    rarity: "rare",
    health: 6,
    speed: 100,
    armor: 20,
    goldReward: 1,
    xpReward: 3,
    abilities: ["Eluzivitás", "Csordaszellem"],
  },
  {
    id: 5,
    name: "Denevér",
    description: "Egy ellenfél, amely szárnyait használva képes átrepülni a tornyaid felett.",
    icon: BatImg,
    type: "Speciális",
    rarity: "epic",
    health: 5,
    speed: 70,
    armor: 40,
    goldReward: 1,
    xpReward: 3,
    abilities: ["Repülés", "Eluzivitás"],
  },
  {
    id: 6,
    name: "Hatalmas Tank",
    description:
      "A végső ellenség. Lassú menetet hajt végre a pályán, rendkívül magas életerővel rendelkezik és képes füstködbe vonni önmagát és a szövetségeseit.",
    icon: TankImg,
    type: "Final Boss",
    rarity: "epic",
    health: 100,
    speed: 30,
    armor: 99,
    goldReward: 1,
    xpReward: 20,
    abilities: ["Óriási életerő", "Füstalak"],
  },
]);

const loading = ref(false);

const loadEnemies = async () => {
  loading.value = true;
  try {
  } catch (error) {
    console.error("Error loading enemies:", error);
    // Keep default enemies if API fails
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  // Uncomment to load from API
  // loadEnemies();
});

const abilityDescriptions = {
  "Gyors mozgás": "Az egységek átlagon felüli mozgási sebességgel rendelkeznek.",
  Csordaszellem: "Egy hullám összesen 3-4 egységet idéz meg.",
  Szívósság: "Az egység magas védelmi statisztikával rendelkezik.Óriásölő eszközökre érzékeny",
  "Óriási életerő":
    "Az egység rendkívül magas életerővel rendelkezik, így sokáig életben marad a harcban.",
  Gyógyítás: "Képes gyógyítani a szövetségeseit, így hosszabb ideig marad életben a csatában.",
  Eluzivitás: "Az egység magas gyorsasága miatt ez a lény külön prioritásnak számít.",
  Gyógyítás: "Képes gyógyítani a szövetségeseit, így hosszabb ideig marad életben a csatában.",
  "Óriási életerő":
    "Az egység rendkívül magas életerővel rendelkezik, így sokáig életben marad a harcban.",
  Füstalak:
    "Képes egy füstgránátot kihajítani a pályára, amely megcélozhatatlanná változtatja önmagát és a benne lévő szövetségeseket 2 másodpercre.",
  Repülés: "Az egység levegőben van, így közelharci tornyok nem képesek azt megtámadni.",
  Eluzivitás: "Az egység magas gyorsasága miatt ez a lény külön prioritásnak számít.",
};
const getAbilityDescription = (ability) => {
  return abilityDescriptions[ability] || "Nincs leírás ehhez a képességhez.";
};

const nextEnemyImage = (enemy) => {
  if (!enemy.images) return;

  enemy.currentImageIndex = (enemy.currentImageIndex + 1) % enemy.images.length;
};
</script>

<style scoped lang="scss">
.enemies-section {
  padding: 5rem 0;
  background: #2c1810;
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

.enemies-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 2rem;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
}

.enemy-card {
  background: #1a0f0a;
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

.enemy-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.enemy-icon-wrapper {
  position: relative;

  .enemy-icon {
    font-size: 4rem;
    display: block;
    width: 100%; /* vagy pl. 80px, 100px, ami kell */
    height: 150px; /* fix magasság */
    object-fit: cover; /* kitölti a keretet arányosan */
    border-radius: 8px; /* opcionális: szép lekerekített sarkok */
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

.enemy-title-section {
  flex: 1;

  .enemy-name {
    color: #ffd700;
    font-size: 1.4rem;
    font-weight: 700;
    margin-bottom: 0.3rem;
  }

  .enemy-type {
    display: inline-block;
    padding: 0.3rem 0.8rem;
    border-radius: 12px;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.5px;

    &.type-gyalogos {
      background: rgba(139, 69, 19, 0.3);
      color: #d2691e;
    }

    &.type-nehézgyalogos {
      background: rgba(128, 128, 128, 0.3);
      color: #c0c0c0;
    }

    &.type-boss {
      background: rgba(220, 20, 60, 0.3);
      color: #ff6b6b;
    }

    &.type-mágikus {
      background: rgba(138, 43, 226, 0.3);
      color: #9370db;
    }

    &.type-elit {
      background: rgba(255, 215, 0, 0.3);
      color: #ffd700;
    }

    &.type-speciális {
      background: rgba(255, 140, 0, 0.3);
      color: #ffa500;
    }

    &.type-repülő {
      background: rgba(135, 206, 235, 0.3);
      color: #87ceeb;
    }
  }
}

.enemy-description {
  color: #f4e4c1;
  font-size: 0.9rem;
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.enemy-stats-section {
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

    &.health {
      background: linear-gradient(90deg, #dc143c, #ff6b6b);
    }

    &.speed {
      background: linear-gradient(90deg, #32cd32, #90ee90);
    }

    &.armor {
      background: linear-gradient(90deg, #4169e1, #87ceeb);
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

.enemy-abilities {
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
}

.enemy-rewards {
  display: flex;
  justify-content: space-around;
  padding: 1rem;
  background: #2c1810;
  border-radius: 8px;
  border: 1px solid #8b4513;

  .reward-item {
    display: flex;
    align-items: center;
    gap: 0.5rem;

    .reward-icon {
      font-size: 1.5rem;
    }

    .reward-value {
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

  max-width: 220px; /* 🔥 EZ KELL */
  white-space: normal; /* 🔥 EZ A KULCS */
  word-wrap: break-word; /* hosszú szavakhoz */
  text-align: left;

  opacity: 0;
  pointer-events: none;
  transition: opacity 0.2s ease;
  z-index: 10;
}

/* kis nyíl */
.ability-tooltip::after {
  content: "";
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translateX(-50%);
  border-width: 5px;
  border-style: solid;
  border-color: #1e1e2e transparent transparent transparent;
}

/* HOVER */
.ability-wrapper:hover .ability-tooltip {
  opacity: 1;
}

.enemy-icon-wrapper.is-shaman,.enemy-icon-wrapper.is-clickable {
  cursor: pointer;

  &:hover {
    transform: scale(1.03);
    box-shadow: 0 0 15px rgba(255, 215, 0, 0.4);
  }

  .enemy-icon {
    height: 150px;
    object-fit: contain;
  }
}
</style>
