<template>
  <div class="min-h-screen w-full flex items-center justify-center p-4 relative overflow-hidden">
    <!-- Decorative Glows -->
    <div class="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] bg-blue-600/20 blur-[120px] rounded-full animate-pulse"></div>
    <div class="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] bg-purple-600/20 blur-[120px] rounded-full animate-pulse" style="animation-delay: 2s"></div>

    <div class="max-w-md w-full glass-card p-10 rounded-[2rem] shadow-[0_0_50px_-12px_rgba(0,0,0,0.5)] relative z-10 transition-all hover:border-white/10">
      <div class="text-center mb-10">
        <div class="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl mb-6 shadow-xl shadow-blue-500/20 animate-float">
          <GraduationCap class="w-10 h-10 text-white" />
        </div>
        <h1 class="text-4xl font-bold text-white tracking-tight mb-2">Bienvenido</h1>
        <p class="text-gray-400 font-medium">Plataforma de Cursos Técnicos</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        <div class="space-y-2">
          <label class="block text-sm font-semibold text-gray-300 ml-1">Email</label>
          <div class="relative group">
            <Mail class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500 group-focus-within:text-blue-500 transition-colors" />
            <input 
              v-model="email" 
              type="email" 
              required
              class="w-full glass-input rounded-xl py-3.5 pl-12 pr-4 text-white placeholder:text-gray-600 focus:bg-gray-800/80"
              placeholder="admin@courseplatform.com"
            />
          </div>
        </div>

        <div class="space-y-2">
          <label class="block text-sm font-semibold text-gray-300 ml-1">Contraseña</label>
          <div class="relative group">
            <Lock class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500 group-focus-within:text-blue-500 transition-colors" />
            <input 
              v-model="password" 
              type="password" 
              required
              class="w-full glass-input rounded-xl py-3.5 pl-12 pr-4 text-white placeholder:text-gray-600 focus:bg-gray-800/80"
              placeholder="••••••••"
            />
          </div>
        </div>

        <div v-if="error" class="bg-red-500/10 border border-red-500/20 text-red-400 p-4 rounded-xl text-sm text-center font-medium animate-shake">
          {{ error }}
        </div>

        <button 
          type="submit" 
          :disabled="loading"
          class="w-full bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-500 hover:to-indigo-500 disabled:opacity-50 text-white font-bold py-4 rounded-xl shadow-xl shadow-blue-900/40 transition-all active:scale-[0.98] flex items-center justify-center gap-3 group"
        >
          <span v-if="loading" class="animate-spin rounded-full h-5 w-5 border-2 border-white/30 border-t-white"></span>
          <span v-else>{{ 'Iniciar Sesión' }}</span>
          <ArrowRight v-if="!loading" class="w-5 h-5 group-hover:translate-x-1 transition-transform" />
        </button>
      </form>

      <div class="mt-10 pt-8 border-t border-white/5 text-center">
        <p class="text-xs text-gray-500 uppercase tracking-widest font-bold mb-4">Credenciales Demo</p>
        <div class="inline-flex flex-col gap-1 bg-white/5 px-4 py-3 rounded-xl border border-white/5">
          <span class="text-xs text-blue-400 font-mono">admin@courseplatform.com</span>
          <span class="text-xs text-gray-400 font-mono">Admin123!</span>
        </div>
      </div>
      <div class="mt-8 pt-8 border-t border-white/5 text-center">
          <p class="text-gray-500 text-sm font-medium">
            ¿No tienes cuenta? 
            <router-link to="/register" class="text-blue-500 hover:text-white font-bold transition-colors">Regístrate gratis</router-link>
          </p>
          <div class="mt-4 opacity-30 group cursor-default">
            <p class="text-[10px] font-mono text-gray-600">Demo: admin@courseplatform.com / Admin123!</p>
          </div>
        </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { GraduationCap, Mail, Lock, ArrowRight } from 'lucide-vue-next'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const loading = ref(false)
const error = ref('')

const handleLogin = async () => {
  loading.value = true
  error.value = ''
  try {
    await authStore.login(email.value, password.value)
    router.push('/')
  } catch (err) {
    error.value = 'Credenciales inválidas o error de conexión'
  } finally {
    loading.value = false
  }
}
</script>
