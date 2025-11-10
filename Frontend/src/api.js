// src/api.js
import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5105/api", // ide jön a backend címe (API alap URL)
  timeout: 10000 // 10s timeout to help detect hung requests
})

// Request interceptor: attach Authorization header when token exists in localStorage
api.interceptors.request.use(config => {
  try {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers = config.headers || {}
      config.headers['Authorization'] = `Bearer ${token}`
    }
  } catch (e) {
    // ignore localStorage errors
  }
  return config
}, error => Promise.reject(error))

export default api
