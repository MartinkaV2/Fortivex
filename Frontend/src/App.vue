<template>
  <div class="min-h-screen bg-gradient-to-b from-slate-950 via-slate-900 to-slate-950 text-slate-100">
    <div class="mx-auto flex min-h-screen w-full max-w-6xl flex-col gap-12 px-6 py-10 lg:px-12">
      <!-- Header with logo and navigation -->
      <header class="flex flex-col gap-6 sm:flex-row sm:items-center sm:justify-between">
        <div class="flex items-center gap-3">
          <span class="flex h-12 w-12 items-center justify-center rounded-full bg-yellow-700/40 text-2xl font-bold uppercase text-yellow-200">F</span>
          <span class="text-2xl font-semibold tracking-wide">Fortivex</span>
        </div>
        <nav class="flex flex-wrap items-center gap-4 text-sm font-medium sm:gap-6" aria-label="Fő navigáció">
          <a
            href="#rolunk"
            class="transition hover:text-yellow-200 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300/70"
            >Rólunk</a
          >
          <a
            href="#adatok"
            class="transition hover:text-yellow-200 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300/70"
            >Adatközpont</a
          >
          <a
            href="https://github.com"
            target="_blank"
            rel="noopener"
            class="transition hover:text-yellow-200 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300/70"
            >GitHub</a
          >
          <!-- If user is logged in, show profile and logout -->
          <template v-if="currentUser">
            <button
              type="button"
              class="inline-flex items-center gap-2 rounded-full border border-slate-500/40 bg-slate-100/90 px-4 py-2 font-semibold text-slate-950 shadow-lg shadow-slate-900/40 transition hover:bg-yellow-300 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
              @click="openProfile"
            >
              <span class="hidden sm:inline">Profil:</span>
              <span>{{ currentUser.username }}</span>
            </button>
            <button
              type="button"
              class="rounded-full border border-yellow-600/40 bg-yellow-700/80 px-4 py-2 font-semibold text-slate-950 shadow-lg shadow-yellow-900/40 transition hover:bg-yellow-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
              @click="handleLogout"
            >
              Kijelentkezés
            </button>
          </template>
          <!-- Otherwise show login and registration buttons -->
          <template v-else>
            <button
              type="button"
              class="rounded-full border border-yellow-600/40 bg-yellow-700/80 px-4 py-2 font-semibold text-slate-950 shadow-lg shadow-yellow-900/40 transition hover:bg-yellow-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
              @click="openLogin"
            >
              Belépés
            </button>
            <button
              type="button"
              class="rounded-full border border-slate-400/60 bg-slate-200/90 px-4 py-2 font-semibold text-slate-950 shadow-lg shadow-slate-900/40 transition hover:bg-yellow-300 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
              @click="openRegister"
            >
              Regisztráció
            </button>
          </template>
        </nav>
      </header>

      <main class="flex flex-1 flex-col gap-12">
        <!-- About section -->
        <section
          id="rolunk"
          class="grid gap-10 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-10 shadow-[0_40px_120px_-60px_rgba(0,0,0,0.65)] lg:grid-cols-[1.15fr_minmax(0,1fr)]"
        >
          <div class="flex flex-col gap-5 text-slate-100">
            <h2 class="text-xl font-semibold text-yellow-200">Szöveg a játékról</h2>
            <p class="text-base leading-relaxed text-slate-100/90">
              A Fortivex egy kooperatív akciójáték, amelyben a csapatmunka, a kitartás és a gyors döntések hoznak győzelmet. A
              backend API az összes kulcs statisztikát szolgáltatja, így a fejlesztők és a játékosok egyaránt követhetik a
              fejlődést.
            </p>
            <p class="text-base leading-relaxed text-slate-100/90">
              Az alábbi kezelőfelület valós időben mutatja a fontos adatokat, és kiindulási pontot ad a teljes értékű
              adminisztrációs felülethez.
            </p>
            <button
              type="button"
              class="mt-2 inline-flex items-center gap-2 rounded-full border border-yellow-500/50 bg-yellow-600/90 px-5 py-2 text-sm font-semibold text-slate-950 shadow-lg shadow-yellow-900/50 transition hover:bg-yellow-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
              @click="scrollToData"
            >
              Mutasd az adatokat
            </button>
          </div>

          <div class="flex flex-col items-center justify-center text-center">
            <h1 class="text-4xl font-semibold tracking-tight text-yellow-100 sm:text-5xl">Fortivex Projekt</h1>
            <p class="mt-3 text-lg font-medium text-slate-100/80">„Az erőd, ami veled épül.”</p>
            <dl class="mt-8 grid w-full gap-4 text-left sm:grid-cols-3">
              <div class="rounded-2xl border border-slate-700/50 bg-slate-900/40 p-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Backend</dt>
                <dd class="mt-2 text-xl font-semibold text-yellow-200">ASP.NET Core API</dd>
              </div>
              <div class="rounded-2xl border border-slate-700/50 bg-slate-900/40 p-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Adattárolás</dt>
                <dd class="mt-2 text-xl font-semibold text-yellow-200">MySQL</dd>
              </div>
              <div class="rounded-2xl border border-slate-700/50 bg-slate-900/40 p-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Frontend</dt>
                <dd class="mt-2 text-xl font-semibold text-yellow-200">Vue 3 + Tailwind</dd>
              </div>
            </dl>
          </div>

          <ul class="grid gap-4 rounded-2xl border border-slate-800/50 bg-slate-950/70 p-6 sm:grid-cols-3" aria-label="Fő funkciók">
            <li class="flex flex-col gap-2 rounded-2xl border border-yellow-600/30 bg-yellow-700/10 p-4 text-sm text-slate-100">
              <span class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Egyedi statisztikák</span>
              <span>Minden játékos teljesítménye részletesen követhető.</span>
            </li>
            <li class="flex flex-col gap-2 rounded-2xl border border-yellow-600/30 bg-yellow-700/10 p-4 text-sm text-slate-100">
              <span class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Azonnali szinkron</span>
              <span>Az API adatai közvetlenül jelennek meg a felületen.</span>
            </li>
            <li class="flex flex-col gap-2 rounded-2xl border border-yellow-600/30 bg-yellow-700/10 p-4 text-sm text-slate-100">
              <span class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Bővíthető modulok</span>
              <span>Egységes kódszerkezet, könnyű továbbfejlesztés.</span>
            </li>
          </ul>
        </section>

        <!-- Status section: loading/error/time stamp -->
        <section class="flex justify-center" aria-live="polite">
          <p
            v-if="loading"
            class="flex items-center gap-3 rounded-full border border-yellow-500/50 bg-yellow-600/90 px-6 py-3 text-sm font-semibold text-slate-950 shadow-lg shadow-yellow-900/50"
          >
            Adatok betöltése…
          </p>
          <p
            v-else-if="loadError"
            class="flex flex-wrap items-center justify-center gap-3 rounded-2xl border border-yellow-400/70 bg-yellow-900/70 px-6 py-4 text-center text-sm text-yellow-100 shadow-lg shadow-yellow-900/40"
          >
            Hiba történt az adatok lekérése közben: {{ loadError }}
          </p>
          <p
            v-else
            class="flex flex-wrap items-center justify-center gap-3 rounded-full border border-slate-700/50 bg-slate-900/80 px-6 py-3 text-sm font-medium text-yellow-100 shadow-lg shadow-slate-950/60"
          >
            Utolsó frissítés: {{ formattedUpdatedAt }}
            <button
              type="button"
              class="rounded-full border border-yellow-500/50 bg-yellow-600/90 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-slate-950 transition hover:bg-yellow-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
              @click="reloadData"
            >
              Frissítés
            </button>
          </p>
        </section>

        <!-- Data section with tables and lists -->
        <section id="adatok" class="flex flex-col gap-10" aria-labelledby="adatok-cimke">
          <header class="flex flex-col gap-6 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-8 shadow-[0_30px_90px_-70px_rgba(0,0,0,0.65)] sm:flex-row sm:items-center sm:justify-between">
            <div>
              <h2 id="adatok-cimke" class="text-2xl font-semibold text-yellow-100">Adatközpont</h2>
              <p class="mt-2 max-w-xl text-sm text-slate-100/80">Közvetlen integráció az ASP.NET Core API-val.</p>
            </div>
            <dl class="grid gap-4 text-sm sm:grid-cols-3">
              <div class="flex flex-col rounded-2xl border border-yellow-600/40 bg-slate-900/60 px-5 py-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Felhasználók</dt>
                <dd class="mt-2 text-2xl font-semibold text-yellow-200">{{ visibleAccounts.length }}</dd>
              </div>
              <div class="flex flex-col rounded-2xl border border-yellow-600/40 bg-slate-900/60 px-5 py-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Admin jogosultság</dt>
                <dd class="mt-2 text-2xl font-semibold text-yellow-200">{{ isAdmin ? visibleAdmins.length : '—' }}</dd>
              </div>
                <div class="flex flex-col rounded-2xl border border-yellow-600/40 bg-slate-900/60 px-5 py-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Összes játékidő</dt>
                <dd class="mt-2 text-2xl font-semibold text-yellow-200">{{ formattedTotalTimePlayed }}</dd>
              </div>
            </dl>
          </header>

          <div class="grid gap-6 lg:grid-cols-2">
            <!-- Accounts table -->
            <article class="flex flex-col gap-4 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-6 shadow-[0_25px_70px_-60px_rgba(0,0,0,0.7)]" role="listitem">
              <header class="flex flex-col gap-2">
                <h3 class="text-lg font-semibold text-yellow-100">Felhasználói fiókok</h3>
                <p class="text-sm text-slate-100/70">Az AccountsController által szolgáltatott adatok.</p>
              </header>
              <div class="overflow-hidden rounded-2xl border border-slate-800/50">
                <div class="overflow-x-auto">
                  <table class="min-w-full divide-y divide-slate-800/60 text-left text-sm text-slate-100/90">
                    <thead class="bg-slate-900/60 text-xs uppercase tracking-wide text-yellow-200">
                      <tr>
                        <th scope="col" class="px-4 py-3">Felhasználónév</th>
                        <th scope="col" class="px-4 py-3">E-mail</th>
                      </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-800/40">
                      <tr v-for="account in visibleAccounts" :key="account.id" class="bg-slate-950/80">
                        <td class="px-4 py-3 font-medium text-yellow-100" data-title="Felhasználónév">{{ account.username }}</td>
                        <td class="px-4 py-3" data-title="E-mail">{{ account.email ?? '—' }}</td>
                      </tr>
                      <tr v-if="!visibleAccounts.length">
                        <td colspan="2" class="px-4 py-6 text-center text-sm text-slate-100/60">Nincs megjeleníthető adat.</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </article>

            <!-- Admin roles list -->
            <article class="flex flex-col gap-4 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-6 shadow-[0_25px_70px_-60px_rgba(0,0,0,0.7)]" role="listitem">
              <header class="flex flex-col gap-2">
                <h3 class="text-lg font-semibold text-yellow-100">Admin jogosultságok</h3>
                <p class="text-sm text-slate-100/70">Az AdminsController-ből származó adatok.</p>
              </header>
              <ul class="flex flex-col divide-y divide-slate-800/50 text-sm text-slate-100">
                <li v-for="admin in visibleAdmins" :key="admin.id" class="flex flex-col gap-1 px-2 py-3">
                  <span class="font-semibold text-yellow-100">{{ resolveAccount(admin.accountId) }}</span>
                  <span class="text-xs uppercase tracking-wide text-slate-100/70">Szerepkör: {{ admin.role }}</span>
                </li>
                <li v-if="!visibleAdmins.length" class="px-2 py-6 text-center text-sm text-slate-100/60">
                  {{ isAdmin ? 'Jelenleg nincs admin jogosultság kiosztva.' : 'Csak admin felhasználók érhetik el ezt a listát.' }}
                </li>
              </ul>
            </article>

            <!-- Top progress table -->
            <article class="flex flex-col gap-4 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-6 shadow-[0_25px_70px_-60px_rgba(0,0,0,0.7)]" role="listitem">
              <header class="flex flex-col gap-2">
                <h3 class="text-lg font-semibold text-yellow-100">Legjobb idők</h3>
                <p class="text-sm text-slate-100/70">PlayerProgressController eredményei.</p>
              </header>
              <div class="overflow-hidden rounded-2xl border border-slate-800/50">
                <div class="overflow-x-auto">
                  <table class="min-w-full divide-y divide-slate-800/60 text-left text-sm text-slate-100/90">
                    <thead class="bg-slate-900/60 text-xs uppercase tracking-wide text-yellow-200">
                      <tr>
                        <th scope="col" class="px-4 py-3">Játékos</th>
                        <th scope="col" class="px-4 py-3">Legjobb idő</th>
                      </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-800/40">
                      <tr v-for="entry in topProgress" :key="entry.id" class="bg-slate-950/80">
                        <td class="px-4 py-3 font-medium text-yellow-100" data-title="Játékos">{{ resolveAccount(entry.accountId) }}</td>
                        <td class="px-4 py-3" data-title="Legjobb idő">{{ formatSeconds(entry.bestTime) }}</td>
                      </tr>
                      <tr v-if="!topProgress.length">
                        <td colspan="2" class="px-4 py-6 text-center text-sm text-slate-100/60">Még nincsenek felvitt eredmények.</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </article>

            <!-- Player statistics table -->
            <article class="flex flex-col gap-4 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-6 shadow-[0_25px_70px_-60px_rgba(0,0,0,0.7)]" role="listitem">
              <header class="flex flex-col gap-2">
                <h3 class="text-lg font-semibold text-yellow-100">Játékos statisztikák</h3>
                <p class="text-sm text-slate-100/70">PlayerStatsController összesítései.</p>
              </header>
              <div class="overflow-hidden rounded-2xl border border-slate-800/50">
                <div class="overflow-x-auto">
                  <table class="min-w-full divide-y divide-slate-800/60 text-left text-sm text-slate-100/90">
                    <thead class="bg-slate-900/60 text-xs uppercase tracking-wide text-yellow-200">
                      <tr>
                        <th scope="col" class="px-4 py-3">Játékos</th>
                        <th scope="col" class="px-4 py-3">Legyőzött ellenfelek</th>
                        <th scope="col" class="px-4 py-3">Játékidő</th>
                      </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-800/40">
                      <tr v-for="stat in visibleStats" :key="stat.id" class="bg-slate-950/80">
                        <td class="px-4 py-3 font-medium text-yellow-100" data-title="Játékos">{{ resolveAccount(stat.accountId) }}</td>
                        <td class="px-4 py-3" data-title="Legyőzött ellenfelek">{{ stat.enemiesKilled }}</td>
                        <td class="px-4 py-3" data-title="Játékidő">{{ formatSeconds(stat.timePlayed) }}</td>
                      </tr>
                      <tr v-if="!visibleStats.length">
                        <td colspan="3" class="px-4 py-6 text-center text-sm text-slate-100/60">Még nem érkeztek statisztikák.</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </article>
          </div>
        </section>
      </main>

      <footer id="kilepes" class="border-t border-slate-800/60 pt-6 text-center text-sm text-slate-100/70">
        Fenntartva szöveg 2026
      </footer>
    </div>
  </div>

  <!-- Modals rendered in body using Teleport -->
  <Teleport to="body">
    <!-- Login modal -->
    <div
      v-if="showLogin"
      class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 px-4 py-10 backdrop-blur"
      role="dialog"
      aria-modal="true"
      @click.self="closeAuthDialogs"
    >
      <div class="relative w-full max-w-md rounded-3xl border border-slate-700/60 bg-slate-950/95 p-8 shadow-2xl shadow-slate-950/80">
        <button
          type="button"
          class="absolute right-4 top-4 rounded-full border border-slate-500/50 bg-slate-900/70 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-yellow-100 transition hover:bg-slate-800 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
          @click="closeAuthDialogs"
        >
          Bezárás
        </button>
        <header class="mb-6 flex flex-col gap-1 text-slate-100">
          <h2 class="text-xl font-semibold text-yellow-200">Belépés</h2>
          <p class="text-sm text-slate-100/70">
            Jelentkezz be a felhasználóneveddel vagy e-mail címeddel és a jelszóval.
          </p>
        </header>
        <form class="flex flex-col gap-5" @submit.prevent="handleLogin">
          <div class="flex flex-col gap-2">
            <label for="login-identifier" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Felhasználónév vagy e-mail cím</label>
            <input
              id="login-identifier"
              v-model.trim="loginForm.identifier"
              type="text"
              required
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="username"
              placeholder="Felhasználónév vagy e-mail"
            />
          </div>
          <div class="flex flex-col gap-2">
            <label for="login-password" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Jelszó</label>
            <input
              id="login-password"
              v-model="loginForm.password"
              type="password"
              required
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="current-password"
            />
          </div>
          <p v-if="loginState.error" class="rounded-2xl border border-yellow-500/50 bg-yellow-900/50 px-4 py-3 text-sm text-yellow-100">
            {{ loginState.error }}
          </p>
          <p v-else-if="loginState.success" class="rounded-2xl border border-slate-600/50 bg-slate-900/60 px-4 py-3 text-sm text-slate-100">
            {{ loginState.success }}
          </p>
          <button
            type="submit"
            class="inline-flex items-center justify-center rounded-2xl border border-yellow-500/60 bg-yellow-600/90 px-5 py-3 text-sm font-semibold uppercase tracking-wide text-slate-950 shadow-lg shadow-yellow-900/60 transition hover:bg-yellow-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300 disabled:cursor-wait disabled:opacity-70"
            :disabled="loginState.loading"
          >
            <span v-if="loginState.loading">Belépés folyamatban…</span>
            <span v-else>Belépés</span>
          </button>
          <button
            type="button"
            class="text-left text-sm font-semibold text-yellow-200 underline-offset-4 transition hover:text-yellow-100 hover:underline"
            @click="openRegister"
          >
            Még nincs fiókod? Regisztráció
          </button>
        </form>
      </div>
    </div>

    <!-- Registration modal -->
    <div
      v-if="showRegister"
      class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 px-4 py-10 backdrop-blur"
      role="dialog"
      aria-modal="true"
      @click.self="closeAuthDialogs"
    >
      <div class="relative w-full max-w-md rounded-3xl border border-yellow-600/50 bg-slate-950/95 p-8 shadow-2xl shadow-slate-950/80">
        <button
          type="button"
          class="absolute right-4 top-4 rounded-full border border-slate-500/50 bg-slate-900/70 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-yellow-100 transition hover:bg-slate-800 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
          @click="closeAuthDialogs"
        >
          Bezárás
        </button>
        <header class="mb-6 flex flex-col gap-1 text-slate-100">
          <h2 class="text-xl font-semibold text-yellow-200">Regisztráció</h2>
          <p class="text-sm text-slate-100/70">
            Hozz létre új fiókot, a megadott adatok azonnal bekerülnek az adatbázisba.
          </p>
        </header>
        <form class="flex flex-col gap-5" @submit.prevent="handleRegister">
          <div class="flex flex-col gap-2">
            <label for="register-username" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Felhasználónév</label>
            <input
              id="register-username"
              v-model.trim="registerForm.username"
              type="text"
              required
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="username"
            />
          </div>
          <div class="flex flex-col gap-2">
            <label for="register-email" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">E-mail cím</label>
            <input
              id="register-email"
              v-model.trim="registerForm.email"
              type="email"
              required
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="email"
            />
          </div>
          <div class="flex flex-col gap-2">
            <label for="register-password" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Jelszó</label>
            <input
              id="register-password"
              v-model="registerForm.password"
              type="password"
              required
              minlength="6"
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="new-password"
            />
          </div>
          <p v-if="registerState.error" class="rounded-2xl border border-yellow-500/50 bg-yellow-900/50 px-4 py-3 text-sm text-yellow-100">
            {{ registerState.error }}
          </p>
          <p v-else-if="registerState.success" class="rounded-2xl border border-slate-600/50 bg-slate-900/60 px-4 py-3 text-sm text-slate-100">
            {{ registerState.success }}
          </p>
          <button
            type="submit"
            class="inline-flex items-center justify-center rounded-2xl border border-yellow-500/60 bg-yellow-600/90 px-5 py-3 text-sm font-semibold uppercase tracking-wide text-slate-950 shadow-lg shadow-yellow-900/60 transition hover:bg-yellow-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300 disabled:cursor-wait disabled:opacity-70"
            :disabled="registerState.loading"
          >
            <span v-if="registerState.loading">Regisztráció folyamatban…</span>
            <span v-else>Regisztráció</span>
          </button>
          <button
            type="button"
            class="text-left text-sm font-semibold text-yellow-200 underline-offset-4 transition hover:text-yellow-100 hover:underline"
            @click="openLogin"
          >
            Már van fiókod? Belépés
          </button>
        </form>
      </div>
    </div>

    <!-- Profile modal -->
    <div
      v-if="showProfile && currentUser"
      class="fixed inset-0 z-40 flex items-center justify-center bg-slate-950/80 px-4 py-10 backdrop-blur"
      role="dialog"
      aria-modal="true"
      @click.self="closeProfile"
    >
      <div class="relative w-full max-w-lg rounded-3xl border border-slate-700/60 bg-slate-950/95 p-8 shadow-2xl shadow-slate-950/80">
        <button
          type="button"
          class="absolute right-4 top-4 rounded-full border border-slate-500/50 bg-slate-900/70 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-yellow-100 transition hover:bg-slate-800 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
          @click="closeProfile"
        >
          Bezárás
        </button>
        <header class="mb-6 flex flex-col gap-2 text-slate-100">
          <h2 class="text-xl font-semibold text-yellow-200">Profil</h2>
          <p class="text-sm text-slate-100/70">Áttekintés a saját Fortivex statisztikáidról.</p>
        </header>
        <dl class="grid gap-4 rounded-3xl border border-slate-800/50 bg-slate-950/70 p-6 text-sm sm:grid-cols-2">
          <div class="flex flex-col gap-1">
            <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Felhasználónév</dt>
            <dd class="text-base font-semibold text-yellow-100">{{ currentUser.username }}</dd>
          </div>
          <div class="flex flex-col gap-1">
            <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">E-mail</dt>
            <dd class="text-base font-semibold text-yellow-100">{{ currentUser.email ?? '—' }}</dd>
          </div>
          <div class="flex flex-col gap-1">
            <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Szerepkör</dt>
            <dd class="text-base font-semibold text-yellow-100">{{ currentUser.isAdmin ? 'Admin' : 'Felhasználó' }}</dd>
          </div>
          <div class="flex flex-col gap-1">
            <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Összes játékidő</dt>
            <dd class="text-base font-semibold text-yellow-100">{{ formatSeconds(currentUserStats?.timePlayed ?? 0) }}</dd>
          </div>
        </dl>
        <section class="mt-6 grid gap-4 sm:grid-cols-2">
          <article class="rounded-3xl border border-slate-800/60 bg-slate-900/60 p-5">
            <h3 class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Legjobb idő</h3>
            <p class="mt-2 text-lg font-semibold text-yellow-100">{{ formatSeconds(currentUserProgress?.bestTime ?? null) }}</p>
          </article>
          <article class="rounded-3xl border border-slate-800/60 bg-slate-900/60 p-5">
            <h3 class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Legyőzött ellenfelek</h3>
            <p class="mt-2 text-lg font-semibold text-yellow-100">{{ currentUserStats?.enemiesKilled ?? 0 }}</p>
          </article>
        </section>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { computed, onMounted, reactive, ref } from 'vue'
import api from './api'

// Determine the backend API base URL from environment or default
// NOTE: backend base includes '/api' so endpoints used below should NOT repeat 'api/' prefix
const API_BASE_URL = import.meta?.env?.VITE_API_BASE_URL || 'http://localhost:5105/api'

// Basic loading and error states
const loading = ref(true)
const loadError = ref('')
const lastUpdatedAt = ref(null)

// State flags for dialogs
const showLogin = ref(false)
const showRegister = ref(false)
const showProfile = ref(false)

// Currently authenticated user
const currentUser = ref(null)

// --- JWT / token helpers -------------------------------------------------
function saveToken(token) {
  try {
    localStorage.setItem('token', token)
  } catch (e) {
    /* ignore */
  }
}
function getToken() {
  try {
    return localStorage.getItem('token')
  } catch (e) {
    return null
  }
}
function removeToken() {
  try {
    localStorage.removeItem('token')
  } catch (e) {
    /* ignore */
  }
}

function decodeJwt(token) {
  try {
    const parts = token.split('.')
    if (parts.length < 2) return null
    // base64url -> base64
    const payload = parts[1].replace(/-/g, '+').replace(/_/g, '/')
    const json = decodeURIComponent(
      atob(payload)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    )
    return JSON.parse(json)
  } catch (e) {
    return null
  }
}

function setCurrentUserFromToken() {
  const token = getToken()
  if (!token) {
    currentUser.value = null
    return
  }
  const payload = decodeJwt(token)
  if (!payload) {
    currentUser.value = null
    return
  }
  
  // Get stored role and username from localStorage
  const storedRole = localStorage.getItem('role')
  const storedUsername = localStorage.getItem('username')
  
  currentUser.value = {
    username: storedUsername || payload.sub || payload.name || payload.unique_name || 'user',
    email: payload.email || null,
    role: storedRole || payload.role || 'user',
    isAdmin: storedRole === 'admin' || payload.role === 'admin' || (Array.isArray(payload.role) && payload.role.includes('admin')),
    id: payload.sub || payload.userId || payload.id || null,
    token
  }
}
// -------------------------------------------------------------------------

// Login form: identifier can be username or email, plus password
const loginForm = reactive({
  identifier: '',
  password: ''
})

// Registration form requires username, email and password
const registerForm = reactive({
  username: '',
  email: '',
  password: ''
})

// Status objects for login and registration operations
const loginState = reactive({
  loading: false,
  error: '',
  success: ''
})
const registerState = reactive({
  loading: false,
  error: '',
  success: ''
})

// Collections fetched from the backend
const accounts = ref([])
const admins = ref([])
const progress = ref([])
const stats = ref([])

// Helper: reset login form and optionally the state
function resetLoginForm(clearState = true) {
  loginForm.identifier = ''
  loginForm.password = ''
  if (clearState) {
    loginState.error = ''
    loginState.success = ''
    loginState.loading = false
  }
}

// Helper: reset registration form and optionally the state
function resetRegisterForm(clearState = true) {
  registerForm.username = ''
  registerForm.email = ''
  registerForm.password = ''
  if (clearState) {
    registerState.error = ''
    registerState.success = ''
    registerState.loading = false
  }
}

// Close any authentication dialogs
function closeAuthDialogs() {
  showLogin.value = false
  showRegister.value = false
}

// Close profile dialog
function closeProfile() {
  showProfile.value = false
}

// Open login dialog
function openLogin() {
  resetLoginForm()
  showRegister.value = false
  showProfile.value = false
  showLogin.value = true
}

// Open registration dialog
function openRegister() {
  resetRegisterForm()
  showLogin.value = false
  showProfile.value = false
  showRegister.value = true
}

// Open profile dialog (if user exists)
function openProfile() {
  if (currentUser.value) {
    showProfile.value = true
  }
}

// Logout: clear current user and reload data
function handleLogout() {
  currentUser.value = null
  removeToken()
  localStorage.removeItem('role')
  localStorage.removeItem('username')
  showProfile.value = false
  resetLoginForm()
  resetRegisterForm()
  loginState.success = ''
  registerState.success = ''
  reloadData().catch(() => {
    /* swallow errors */
  })
}

// Use axios instance for API calls (interceptor adds Authorization header)
// Compute trimmed base URL for API endpoints (still available if needed)
const trimmedBaseUrl = computed(() => API_BASE_URL.replace(/\/$/, ''))

// Helper to perform GET requests using axios
async function fetchJson(path) {
  try {
    const cleanPath = path.replace(/^\//, '')
    // use axios instance with baseURL set to include '/api'
    const res = await api.get(cleanPath)
    return res.data
  } catch (err) {
    const message = err?.response?.data || err?.message || String(err)
    throw new Error(message)
  }
}

// Helper to perform POST requests with JSON body using axios
async function postJson(path, payload) {
  try {
    const cleanPath = path.replace(/^\//, '')
    // use axios instance with baseURL set to include '/api'
    const res = await api.post(cleanPath, payload)
    return res.data
  } catch (err) {
    const message = err?.response?.data || err?.message || String(err)
    throw new Error(message)
  }
}

// Reload all data from backend controllers
async function reloadData() {
  loading.value = true
  loadError.value = ''
  try {
    const [accountsData, adminsData, progressData, statsData] = await Promise.all([
      fetchJson('accounts'),
      fetchJson('admins'),
      fetchJson('playerprogress'),
      fetchJson('playerstats')
    ])
    accounts.value = accountsData
    admins.value = adminsData
    progress.value = progressData
    stats.value = statsData
    lastUpdatedAt.value = new Date()
  } catch (error) {
    loadError.value = error.message
  } finally {
    loading.value = false
  }
}

// Fetch data on component mount
onMounted(() => {
  // Initialize auth state from token, then load data
  try {
    setCurrentUserFromToken()
  } catch (e) {
    /* ignore */
  }
  reloadData().catch(error => {
    loadError.value = error.message
    loading.value = false
  })
})

// Handle user login: validate fields and authenticate via API
async function handleLogin() {
  if (!loginForm.identifier.trim() || !loginForm.password.trim()) {
    loginState.error = 'Add meg az azonosítót és a jelszót is.'
    loginState.success = ''
    return
  }
  loginState.loading = true
  loginState.error = ''
  loginState.success = ''
  try {
    const payload = {
      UserName: loginForm.identifier.trim(),
      PasswordHash: loginForm.password
    }
    const res = await postJson('accounts/login', payload)
    // response may contain token or user object
    let token = null
    if (typeof res === 'string') token = res
    else if (res?.token) token = res.token
    else if (res?.accessToken) token = res.accessToken
    else if (res?.jwt) token = res.jwt

    if (token) {
      saveToken(token)
      // Also store role and username from response
      if (res?.role) localStorage.setItem('role', res.role)
      if (res?.username) localStorage.setItem('username', res.username)
      setCurrentUserFromToken()
    } else if (res?.username) {
      // fallback: server returned user object
      currentUser.value = {
        username: res.username,
        role: res.role || 'user',
        isAdmin: res.role === 'admin'
      }
    }

    loginState.success = `Sikeres belépés, üdv újra ${currentUser.value?.username || 'felhasználó'}!`
    await reloadData()
    closeAuthDialogs()
    resetLoginForm()
    showProfile.value = true
  } catch (error) {
    loginState.error = error.message
  } finally {
    loginState.loading = false
  }
}

// Handle user registration: validate fields and create via API
async function handleRegister() {
  if (!registerForm.username.trim() || !registerForm.email.trim() || !registerForm.password.trim()) {
    registerState.error = 'Minden mező kitöltése kötelező.'
    registerState.success = ''
    return
  }
  registerState.loading = true
  registerState.error = ''
  registerState.success = ''
  try {
    const payload = {
      UserName: registerForm.username.trim(),
      Email: registerForm.email.trim(),
      PasswordHash: registerForm.password,
      Role: "user"  // Add role field
    }
    console.log('Sending registration request:', payload);
    const res = await postJson('accounts/register', payload)
    console.log('Registration response:', res);
    let token = null
    if (typeof res === 'string') token = res
    else if (res?.token) token = res.token
    else if (res?.accessToken) token = res.accessToken

    if (token) {
      saveToken(token)
      setCurrentUserFromToken()
    } else if (res?.username) {
      currentUser.value = res
    }

    registerState.success = `Sikeres regisztráció! Üdv, ${currentUser.value?.username || registerForm.username}!`
    await reloadData()
    closeAuthDialogs()
    resetRegisterForm()
    showProfile.value = true
  } catch (error) {
    registerState.error = error.message
  } finally {
    registerState.loading = false
  }
}

// Determine if the current user is admin
const isAdmin = computed(() => currentUser.value?.isAdmin ?? false)

// Filter accounts visible to current user: all for admin, own for user or all when no user
const visibleAccounts = computed(() => {
  if (isAdmin.value || !currentUser.value) {
    return accounts.value
  }
  return accounts.value.filter(account => account.id === currentUser.value?.id)
})

// Filter admins visible to current user: all for admin, own for user or all when no user
const visibleAdmins = computed(() => {
  if (isAdmin.value || !currentUser.value) {
    return admins.value
  }
  return admins.value.filter(admin => admin.accountId === currentUser.value?.id)
})

// Filter progress entries visible to current user
const visibleProgress = computed(() => {
  if (isAdmin.value || !currentUser.value) {
    return progress.value
  }
  return progress.value.filter(entry => entry.accountId === currentUser.value?.id)
})

// Filter stats entries visible to current user
const visibleStats = computed(() => {
  if (isAdmin.value || !currentUser.value) {
    return stats.value
  }
  return stats.value.filter(entry => entry.accountId === currentUser.value?.id)
})

// Build lookup map from account id to username
const accountLookup = computed(() => {
  const map = new Map()
  for (const account of accounts.value) {
    map.set(account.id, account.username)
  }
  return map
})

// Resolve account id to username
function resolveAccount(id) {
  return accountLookup.value.get(id) ?? `Felhasználó #${id}`
}

// Compute top progress list sorted by best time
const topProgress = computed(() => {
  return [...visibleProgress.value]
    .filter(entry => entry.bestTime != null)
    .sort((a, b) => (a.bestTime ?? Infinity) - (b.bestTime ?? Infinity))
    .slice(0, 5)
})

// Format last updated timestamp
const formattedUpdatedAt = computed(() => {
  if (!lastUpdatedAt.value) {
    return 'nincs frissítés'
  }
  return new Intl.DateTimeFormat('hu-HU', {
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  }).format(lastUpdatedAt.value)
})

// Sum total time played for visible stats
const totalTimePlayed = computed(() => visibleStats.value.reduce((sum, item) => sum + item.timePlayed, 0))

// Format total time played
const formattedTotalTimePlayed = computed(() => formatSeconds(totalTimePlayed.value))

// Current user's progress entry
const currentUserProgress = computed(() => {
  if (!currentUser.value) {
    return null
  }
  return progress.value.find(entry => entry.accountId === currentUser.value?.id) ?? null
})

// Current user's stats entry
const currentUserStats = computed(() => {
  if (!currentUser.value) {
    return null
  }
  return stats.value.find(entry => entry.accountId === currentUser.value?.id) ?? null
})

// Format seconds into mm:ss or h m
function formatSeconds(value) {
  if (value == null) {
    return '—'
  }
  const minutes = Math.floor(value / 60)
  const seconds = value % 60
  if (minutes >= 60) {
    const hours = Math.floor(minutes / 60)
    const remainingMinutes = minutes % 60
    return `${hours} h ${remainingMinutes} m`
  }
  return `${minutes}:${seconds.toString().padStart(2, '0')}`
}

// Scroll to data section when user clicks button in hero
function scrollToData() {
  const element = document.getElementById('adatok')
  element?.scrollIntoView({ behavior: 'smooth' })
}
</script>