import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
  id: 'user',
  state: () => ({
    user: null,
    stats: null,
    token: null
  }),
  actions: {
    // Betöltés localStorage-ből (F5 után)
    loadFromStorage() {
      const saved = localStorage.getItem('userStore')
      if (saved) {
        Object.assign(this.$state, JSON.parse(saved))
      }
    },

    // Mentés localStorage-be
    saveToStorage() {
      localStorage.setItem('userStore', JSON.stringify(this.$state))
    },

    setUser(user) {
      this.user = user
      this.saveToStorage()
    },

    setStats(stats) {
      this.stats = stats
      this.saveToStorage()
    },

    setToken(token) {
      this.token = token
      this.saveToStorage()
    },

    logout() {
      this.user = null
      this.stats = null
      this.token = null
      localStorage.removeItem('userStore')
    }
  }
})
