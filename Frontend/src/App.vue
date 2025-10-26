<template>
  <div class="min-h-screen bg-gradient-to-b from-slate-950 via-slate-900 to-slate-950 text-slate-100">
    <div class="mx-auto flex min-h-screen w-full max-w-6xl flex-col gap-12 px-6 py-10 lg:px-12">
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
        </nav>
      </header>

      <main class="flex flex-1 flex-col gap-12">
        <section
          id="rolunk"
          class="grid gap-10 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-10 shadow-[0_40px_120px_-60px_rgba(0,0,0,0.65)] lg:grid-cols-[1.15fr_minmax(0,1fr)]"
        >
          <div class="flex flex-col gap-5 text-slate-100">
            <h2 class="text-xl font-semibold text-yellow-200">Szöveg a játékról</h2>
            <p class="text-base leading-relaxed text-slate-100/90">
              A Fortivex egy kooperatív akciójáték, amelyben a csapatmunka, a kitartás és a gyors döntések hoznak győzelmet. A backend API
              az összes kulcs statisztikát szolgáltatja, így a fejlesztők és a játékosok egyaránt követhetik a fejlődést.
            </p>
            <p class="text-base leading-relaxed text-slate-100/90">
              Az alábbi kezelőfelület valós időben mutatja a fontos adatokat, és kiindulási pontot ad a teljes értékű adminisztrációs
              felülethez.
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

        <section id="adatok" class="flex flex-col gap-10" aria-labelledby="adatok-cimke">
          <header class="flex flex-col gap-6 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-8 shadow-[0_30px_90px_-70px_rgba(0,0,0,0.65)] sm:flex-row sm:items-center sm:justify-between">
            <div>
              <h2 id="adatok-cimke" class="text-2xl font-semibold text-yellow-100">Adatközpont</h2>
              <p class="mt-2 max-w-xl text-sm text-slate-100/80">Közvetlen integráció az ASP.NET Core API-val.</p>
            </div>
            <dl class="grid gap-4 text-sm sm:grid-cols-3">
              <div class="flex flex-col rounded-2xl border border-yellow-600/40 bg-slate-900/60 px-5 py-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Felhasználók</dt>
                <dd class="mt-2 text-2xl font-semibold text-yellow-200">{{ accounts.length }}</dd>
              </div>
              <div class="flex flex-col rounded-2xl border border-yellow-600/40 bg-slate-900/60 px-5 py-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Admin jogosultság</dt>
                <dd class="mt-2 text-2xl font-semibold text-yellow-200">{{ admins.length }}</dd>
              </div>
              <div class="flex flex-col rounded-2xl border border-yellow-600/40 bg-slate-900/60 px-5 py-4">
                <dt class="text-xs font-semibold uppercase tracking-wide text-yellow-300/80">Összes játékidő</dt>
                <dd class="mt-2 text-2xl font-semibold text-yellow-200">{{ formattedTotalTimePlayed }}</dd>
              </div>
            </dl>
          </header>

          <div class="grid gap-6 lg:grid-cols-2">
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
                    <tr v-for="account in accounts" :key="account.id" class="bg-slate-950/80">
                      <td class="px-4 py-3 font-medium text-yellow-100" data-title="Felhasználónév">{{ account.username }}</td>
                      <td class="px-4 py-3" data-title="E-mail">{{ account.email ?? '—' }}</td>
                    </tr>
                    <tr v-if="!accounts.length">
                      <td colspan="2" class="px-4 py-6 text-center text-sm text-slate-100/60">Nincs megjeleníthető adat.</td>
                    </tr>
                  </tbody>
                  </table>
                </div>
              </div>
            </article>

            <article class="flex flex-col gap-4 rounded-3xl border border-slate-800/50 bg-slate-950/60 p-6 shadow-[0_25px_70px_-60px_rgba(0,0,0,0.7)]" role="listitem">
              <header class="flex flex-col gap-2">
                <h3 class="text-lg font-semibold text-yellow-100">Admin jogosultságok</h3>
                <p class="text-sm text-slate-100/70">Az AdminsController-ből származó adatok.</p>
              </header>
              <ul class="flex flex-col divide-y divide-slate-800/50 text-sm text-slate-100">
                <li v-for="admin in admins" :key="admin.id" class="flex flex-col gap-1 px-2 py-3">
                  <span class="font-semibold text-yellow-100">{{ resolveAccount(admin.accountId) }}</span>
                  <span class="text-xs uppercase tracking-wide text-slate-100/70">Szerepkör: {{ admin.role }}</span>
                </li>
                <li v-if="!admins.length" class="px-2 py-6 text-center text-sm text-slate-100/60">Jelenleg nincs admin jogosultság kiosztva.</li>
              </ul>
            </article>

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
                    <tr v-for="stat in stats" :key="stat.id" class="bg-slate-950/80">
                      <td class="px-4 py-3 font-medium text-yellow-100" data-title="Játékos">{{ resolveAccount(stat.accountId) }}</td>
                      <td class="px-4 py-3" data-title="Legyőzött ellenfelek">{{ stat.enemiesKilled }}</td>
                      <td class="px-4 py-3" data-title="Játékidő">{{ formatSeconds(stat.timePlayed) }}</td>
                    </tr>
                    <tr v-if="!stats.length">
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

  <Teleport to="body">
    <div
      v-if="showLogin"
      class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 px-4 py-10 backdrop-blur"
      role="dialog"
      aria-modal="true"
      @click.self="closeDialogs"
    >
      <div class="relative w-full max-w-md rounded-3xl border border-slate-700/60 bg-slate-950/95 p-8 shadow-2xl shadow-slate-950/80">
        <button
          type="button"
          class="absolute right-4 top-4 rounded-full border border-slate-500/50 bg-slate-900/70 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-yellow-100 transition hover:bg-slate-800 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
          @click="closeDialogs"
        >
          Bezárás
        </button>
        <header class="mb-6 flex flex-col gap-1 text-slate-100">
          <h2 class="text-xl font-semibold text-yellow-200">Belépés</h2>
          <p class="text-sm text-slate-100/70">
            Jelentkezz be a regisztrált felhasználóneveddel és e-mail címeddel.
          </p>
        </header>
        <form class="flex flex-col gap-5" @submit.prevent="handleLogin">
          <div class="flex flex-col gap-2">
            <label for="login-username" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">Felhasználónév</label>
            <input
              id="login-username"
              v-model.trim="loginForm.username"
              type="text"
              required
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="username"
            />
          </div>
          <div class="flex flex-col gap-2">
            <label for="login-email" class="text-sm font-semibold uppercase tracking-wide text-yellow-200">E-mail cím</label>
            <input
              id="login-email"
              v-model.trim="loginForm.email"
              type="email"
              required
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="email"
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

    <div
      v-if="showRegister"
      class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 px-4 py-10 backdrop-blur"
      role="dialog"
      aria-modal="true"
      @click.self="closeDialogs"
    >
      <div class="relative w-full max-w-md rounded-3xl border border-yellow-600/50 bg-slate-950/95 p-8 shadow-2xl shadow-slate-950/80">
        <button
          type="button"
          class="absolute right-4 top-4 rounded-full border border-slate-500/50 bg-slate-900/70 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-yellow-100 transition hover:bg-slate-800 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-yellow-300"
          @click="closeDialogs"
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
              class="w-full rounded-2xl border border-slate-700/60 bg-slate-900/60 px-4 py-3 text-sm text-yellow-100 shadow-inner shadow-slate-950/80 focus:border-yellow-400 focus:outline-none focus:ring-2 focus:ring-yellow-400/60"
              autocomplete="email"
              placeholder="opcionális"
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
  </Teleport>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'

type AccountDto = {
  id: number
  username: string
  email?: string | null
}

type AdminDto = {
  id: number
  accountId: number
  role: string
}

type PlayerProgressDto = {
  id: number
  accountId: number
  bestTime: number | null
}

type PlayerStatsDto = {
  id: number
  accountId: number
  enemiesKilled: number
  timePlayed: number
}

const API_BASE_URL = (import.meta.env.VITE_API_BASE_URL as string | undefined) ?? 'https://localhost:5001'

const loading = ref(true)
const loadError = ref('')
const lastUpdatedAt = ref<Date | null>(null)

const showLogin = ref(false)
const showRegister = ref(false)

const loginForm = reactive({
  username: '',
  email: ''
})

const registerForm = reactive({
  username: '',
  email: ''
})

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

const accounts = ref<AccountDto[]>([])
const admins = ref<AdminDto[]>([])
const progress = ref<PlayerProgressDto[]>([])
const stats = ref<PlayerStatsDto[]>([])

function resetLoginForm(clearState = true): void {
  loginForm.username = ''
  loginForm.email = ''
  if (clearState) {
    loginState.error = ''
    loginState.success = ''
    loginState.loading = false
  }
}

function resetRegisterForm(clearState = true): void {
  registerForm.username = ''
  registerForm.email = ''
  if (clearState) {
    registerState.error = ''
    registerState.success = ''
    registerState.loading = false
  }
}

function closeDialogs(): void {
  showLogin.value = false
  showRegister.value = false
}

function openLogin(): void {
  resetLoginForm()
  showRegister.value = false
  showLogin.value = true
}

function openRegister(): void {
  resetRegisterForm()
  showLogin.value = false
  showRegister.value = true
}

const trimmedBaseUrl = computed(() => API_BASE_URL.replace(/\/$/, ''))

function buildEndpoint(path: string): string {
  const cleanPath = path.replace(/^\//, '')
  return `${trimmedBaseUrl.value}/${cleanPath}`
}

async function fetchJson<T>(path: string): Promise<T> {
  const response = await fetch(buildEndpoint(path))
  if (!response.ok) {
    const message = await response.text().catch(() => '')
    throw new Error(message || `A kérés sikertelen (${response.status})`)
  }
  return response.json() as Promise<T>
}

async function postJson<T>(path: string, payload: unknown): Promise<T> {
  const response = await fetch(buildEndpoint(path), {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(payload)
  })

  if (!response.ok) {
    const message = await response.text().catch(() => '')
    throw new Error(message || `A kérés sikertelen (${response.status})`)
  }

  return response.json() as Promise<T>
}

async function reloadData(): Promise<void> {
  loading.value = true
  loadError.value = ''

  try {
    const [accountsData, adminsData, progressData, statsData] = await Promise.all([
      fetchJson<AccountDto[]>('api/accounts'),
      fetchJson<AdminDto[]>('api/admins'),
      fetchJson<PlayerProgressDto[]>('api/playerprogress'),
      fetchJson<PlayerStatsDto[]>('api/playerstats')
    ])

    accounts.value = accountsData
    admins.value = adminsData
    progress.value = progressData
    stats.value = statsData
    lastUpdatedAt.value = new Date()
  } catch (error) {
    const err = error as Error
    loadError.value = err.message
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  reloadData().catch((error: unknown) => {
    const err = error as Error
    loadError.value = err.message
    loading.value = false
  })
})

async function handleLogin(): Promise<void> {
  if (!loginForm.username.trim() || !loginForm.email.trim()) {
    loginState.error = 'Add meg a felhasználónevet és az e-mail címet is.'
    loginState.success = ''
    return
  }

  loginState.loading = true
  loginState.error = ''
  loginState.success = ''

  try {
    if (!accounts.value.length) {
      accounts.value = await fetchJson<AccountDto[]>('api/accounts')
    }

    const normalizedUsername = loginForm.username.trim().toLowerCase()
    const normalizedEmail = loginForm.email.trim().toLowerCase()

    const matchedAccount = accounts.value.find(
      (account) =>
        account.username.trim().toLowerCase() === normalizedUsername &&
        (account.email ?? '').trim().toLowerCase() === normalizedEmail
    )

    if (!matchedAccount) {
      loginState.error = 'Nem található ilyen felhasználó. Ellenőrizd az adatokat vagy regisztrálj új fiókot.'
      return
    }

    loginState.success = `Sikeres belépés, üdv újra ${matchedAccount.username}!`
  } catch (error) {
    const err = error as Error
    loginState.error = err.message
  } finally {
    loginState.loading = false
  }
}

async function handleRegister(): Promise<void> {
  if (!registerForm.username.trim()) {
    registerState.error = 'A felhasználónév megadása kötelező.'
    registerState.success = ''
    return
  }

  registerState.loading = true
  registerState.error = ''
  registerState.success = ''

  try {
    const payload = {
      username: registerForm.username.trim(),
      email: registerForm.email.trim() || null
    }

    const createdAccount = await postJson<AccountDto>('api/accounts', payload)

    registerState.success = `Sikeres regisztráció! Azonosító: #${createdAccount.id}`
    resetRegisterForm(false)
    await reloadData()
  } catch (error) {
    const err = error as Error
    registerState.error = err.message
  } finally {
    registerState.loading = false
  }
}

const accountLookup = computed(() => {
  const map = new Map<number, string>()
  for (const account of accounts.value) {
    map.set(account.id, account.username)
  }
  return map
})

function resolveAccount(id: number): string {
  return accountLookup.value.get(id) ?? `Felhasználó #${id}`
}

const topProgress = computed(() => {
  return [...progress.value]
    .filter((entry) => entry.bestTime != null)
    .sort((a, b) => (a.bestTime ?? Infinity) - (b.bestTime ?? Infinity))
    .slice(0, 5)
})

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

const totalTimePlayed = computed(() => stats.value.reduce((sum, item) => sum + item.timePlayed, 0))

const formattedTotalTimePlayed = computed(() => formatSeconds(totalTimePlayed.value))

function formatSeconds(value: number | null | undefined): string {
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

function scrollToData(): void {
  const element = document.getElementById('adatok')
  element?.scrollIntoView({ behavior: 'smooth' })
}
</script>
