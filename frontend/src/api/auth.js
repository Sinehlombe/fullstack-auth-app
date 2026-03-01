import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

const api = axios.create({
  baseURL: API_BASE_URL,
})

// Attach token to every request automatically
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Auth calls
export const registerUser = (data) => {
  return api.post('/auth/register', data)
}

export const loginUser = (data) => {
  return api.post('/auth/login', data)
}

export const getUserDetails = () => {
  return api.get('/user/me')
}
