<template>
  <footer class="footer">
    <div class="footer-container">
      <div class="footer-content">
        <div class="footer-section">
          <h3 class="footer-title">Fortivex Projekt</h3>
          <p class="footer-description">
            Védd meg a váradat a sötétség erőitől ebben az epikus tower defense játékban!
          </p>
        </div>

        <div class="footer-section">
          <h4 class="section-title">Linkek</h4>
          <ul class="footer-links">
            <li><router-link to="/">Főoldal</router-link></li>
            <li><a href="#description">A Játékról</a></li>
            <li><a href="#maps">Pályák</a></li>
            <li><a href="#enemies">Ellenségek</a></li>
            <li><a href="#contact">Kapcsolat</a></li>
          </ul>
        </div>

        <div class="footer-section">
          <h4 class="section-title">Közösség</h4>
          <ul class="footer-links">
            <li><a href="https://discord.gg/fortivex" target="_blank">Discord</a></li>
            <li><a href="https://twitter.com/fortivex" target="_blank">Twitter</a></li>
            <li><a href="https://facebook.com/fortivex" target="_blank">Facebook</a></li>
            <li><a href="https://youtube.com/fortivex" target="_blank">YouTube</a></li>
          </ul>
        </div>

        <div class="footer-section">
          <h4 class="section-title">Támogatás</h4>
          <ul class="footer-links">
            <li><router-link to="/support/help">Súgó</router-link></li>
            <li><router-link to="/support/gyik">GYIK</router-link></li>
            <li><router-link to="/support/terms">Felhasználási Feltételek</router-link></li>
             <li><router-link to="/support/privacy">Adatvédelem</router-link></li>
          </ul>
        </div>
      </div>

      <div class="footer-bottom">
        <p class="copyright">
          &copy; {{ currentYear }} Fortivex Projekt. Minden jog fenntartva.
        </p>
        <div class="social-icons">
          <a href="https://discord.gg/fortivex" class="social-icon" title="Discord">💬</a>
          <a href="https://twitter.com/fortivex" class="social-icon" title="Twitter">🐦</a>
          <a href="https://facebook.com/fortivex" class="social-icon" title="Facebook">📘</a>
          <a href="https://youtube.com/fortivex" class="social-icon" title="YouTube">📺</a>
        </div>
      </div>
    </div>
  </footer>
</template>

<script setup>
import { computed } from 'vue';

const currentYear = computed(() => new Date().getFullYear());
</script>

<style scoped lang="scss">
.footer {
  background: linear-gradient(135deg, #0d0805 0%, #1a0f0a 100%);
  border-top: 3px solid #8b4513;
  padding: 3rem 0 1rem;
  margin-top: auto;
}

.footer-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 2rem;
}

.footer-content {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  gap: 3rem;
  margin-bottom: 2rem;

  @media (max-width: 1024px) {
    grid-template-columns: repeat(2, 1fr);
  }

  @media (max-width: 640px) {
    grid-template-columns: 1fr;
  }
}

.footer-section {
  .footer-title {
    color: #ffd700;
    font-size: 1.5rem;
    font-family: 'Cinzel', serif;
    margin-bottom: 1rem;
    text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
  }

  .footer-description {
    color: #f4e4c1;
    line-height: 1.6;
    font-size: 0.95rem;
  }

  .section-title {
    color: #ffd700;
    font-size: 1.1rem;
    margin-bottom: 1rem;
    font-weight: 600;
  }

  .footer-links {
    list-style: none;
    display: flex;
    flex-direction: column;
    gap: 0.5rem;

    li a {
      color: #f4e4c1;
      text-decoration: none;
      transition: all 0.3s ease;
      display: inline-block;

      &:hover {
        color: #ffd700;
        transform: translateX(5px);
      }
    }
  }
}

.footer-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 2rem;
  border-top: 1px solid rgba(139, 69, 19, 0.5);
  flex-wrap: wrap;
  gap: 1rem;

  @media (max-width: 640px) {
    flex-direction: column;
    text-align: center;
  }
}

.copyright {
  color: rgba(244, 228, 193, 0.7);
  font-size: 0.9rem;
}

.social-icons {
  display: flex;
  gap: 1rem;

  .social-icon {
    width: 40px;
    height: 40px;
    display: flex;
    align-items: center;
    justify-content: center;
    background: #2c1810;
    border: 2px solid #8b4513;
    border-radius: 50%;
    font-size: 1.2rem;
    transition: all 0.3s ease;
    text-decoration: none;

    &:hover {
      background: #8b4513;
      border-color: #ffd700;
      transform: translateY(-3px);
      box-shadow: 0 6px 12px rgba(255, 215, 0, 0.3);
    }
  }
}
</style>
