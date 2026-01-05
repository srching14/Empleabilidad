import { defineStore } from 'pinia'
import api from '../services/api'

export const useAuthStore = defineStore('auth', {
    state: () => ({
        token: localStorage.getItem('token') || null,
        email: localStorage.getItem('email') || null,
    }),
    getters: {
        isAuthenticated: (state) => !!state.token,
    },
    actions: {
        async login(email, password) {
            try {
                const response = await api.post('/auth/login', { email, password })
                this.token = response.data.token
                this.email = email
                localStorage.setItem('token', this.token)
                localStorage.setItem('email', this.email)
                return true
            } catch (error) {
                console.error('Login failed', error)
                throw error
            }
        },
        logout() {
            this.token = null
            this.email = null
            localStorage.removeItem('token')
            localStorage.removeItem('email')
        },
    },
})
