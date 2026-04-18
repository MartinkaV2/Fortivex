<template>
  <div class="info-page">
    <div class="info-bg">
      <div class="info-card">
       <div class="logo-row">
          <router-link to="/">
            <img class="logo" src="@/assets/images/logo.png" alt="Fortivex Logo" />
          </router-link>
        </div>


        <h1 class="page-title">
          <slot name="title" />
        </h1>

        <div class="divider"></div>

        <div class="content">
          <slot />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.info-page {
  min-height: 100vh;
  background: radial-gradient(circle at top, #3b1d0f, #0f0703);
  padding: 80px 16px;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  font-family: 'Georgia', serif;
}

.info-bg {
  width: 100%;
  max-width: 1000px;
}

.info-card {
  background: linear-gradient(180deg, rgba(0,0,0,0.45), rgba(0,0,0,0.75));
  border: 1px solid rgba(245, 185, 66, 0.35);
  border-radius: 24px;
  padding: 34px 36px;
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.65);
  position: relative;
  overflow: hidden;
}

.info-card::before {
  content: "";
  position: absolute;
  inset: -2px;
  background: radial-gradient(circle at top left, rgba(245,185,66,0.22), transparent 45%),
              radial-gradient(circle at bottom right, rgba(245,185,66,0.12), transparent 55%);
  pointer-events: none;
}

.logo-row {
  display: flex;
  justify-content: center;
  margin-bottom: 18px;
}

.logo {
  width: 140px;
  filter: drop-shadow(0 0 10px rgba(245, 185, 66, 0.9));
  transition: transform 0.2s ease;
}

.logo:hover {
  transform: scale(1.05);
}


.page-title {
  color: #f5b942;
  font-size: 42px;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin: 12px 0 32px; /* <-- nagyobb alsó margó */
  text-align: center;  /* <-- középre igazítás */
}

h1{
    color: #f5b942;
  font-size: 42px;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin: 12px 0 32px; /* <-- nagyobb alsó margó */
  text-align: center;  /* <-- középre igazítás */
}

.divider {
  height: 1px;
  background: linear-gradient(to right, transparent, rgba(245, 185, 66, 0.45), transparent);
  margin-bottom: 28px;
}

.content {
  position: relative;
  z-index: 1;
}

.content .section {
  background: rgba(0, 0, 0, 0.35);
  border: 1px solid rgba(245, 185, 66, 0.25);
  border-radius: 16px;
  padding: 24px;
  margin-bottom: 22px;
  box-shadow: 0 10px 24px rgba(0, 0, 0, 0.5);
}

.content .section h2 {
  color: #f5b942;
  margin-bottom: 12px;
  font-size: 22px;
}

.content .section p {
  line-height: 1.7;
  font-size: 16px;
  color: #f4e7c6;
}

@media (max-width: 768px) {
  .info-page {
    padding: 40px 12px;
  }
  .info-card {
    padding: 28px 20px;
  }
  .page-title {
    font-size: 34px;
  }
}
</style>
