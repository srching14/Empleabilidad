<template>
  <div class="min-h-screen w-full flex items-center justify-center bg-gray-950 p-4">
    <div class="max-w-md w-full bg-gray-900/50 backdrop-blur-xl border border-gray-800 p-8 rounded-2xl shadow-2xl">
      <div class="text-center mb-8">
        <div class="inline-flex items-center justify-center w-16 h-16 bg-blue-600/20 rounded-xl mb-4">
          <GraduationCap class="w-8 h-8 text-blue-500" />
        </div>
        <h1 class="text-3xl font-bold text-white">Bienvenido</h1>
        <p class="text-gray-400 mt-2">Plataforma de Cursos Técnicos</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-gray-400 mb-2">Email</label>
          <div class="relative">
            <Mail class="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500" />
            <input 
              v-model="email" 
              type="email" 
              required
              class="w-full bg-gray-800 border border-gray-700 rounded-lg py-2.5 pl-10 pr-4 text-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all outline-none"
              placeholder="admin@courseplatform.com"
            />
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-400 mb-2">Contraseña</label>
          <div class="relative">
            <Lock class="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500" />
            <input 
              v-model="password" 
              type="password" 
              required
              class="w-full bg-gray-800 border border-gray-700 rounded-lg py-2.5 pl-10 pr-4 text-white focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all outline-none"
              placeholder="••••••••"
            />
          </div>
        </div>

        <div v-if="error" class="bg-red-500/10 border border-red-500/50 text-red-500 p-3 rounded-lg text-sm text-center">
          {{ error }}
        </div>

        <button 
          type="submit" 
          :disabled="loading"
          class="w-full bg-blue-600 hover:bg-blue-500 disabled:opacity-50 text-white font-semibold py-3 rounded-lg shadow-lg shadow-blue-500/20 transition-all flex items-center justify-center gap-2"
        >
          {{ loading ? 'Iniciando sesión...' : 'Entrar' }}
          <ArrowRight v-if="!loading" class="w-5 h-5" />
        </button>
      </form>

      <div class="mt-8 pt-8 border-t border-gray-800 text-center text-sm text-gray-500">
        Demo: admin@courseplatform.com / Admin123!
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
