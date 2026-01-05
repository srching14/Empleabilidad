<template>
  <div class="min-h-screen flex items-center justify-center p-6 relative overflow-hidden bg-[#030712]">
    <!-- Background Decor -->
    <div class="fixed top-0 left-0 w-full h-full pointer-events-none z-0">
      <div class="absolute top-[20%] left-[10%] w-[40%] h-[40%] bg-blue-600/10 blur-[120px] rounded-full animate-pulse"></div>
      <div class="absolute bottom-[20%] right-[10%] w-[40%] h-[40%] bg-indigo-600/10 blur-[120px] rounded-full animate-pulse" style="animation-delay: 2s"></div>
    </div>

    <div class="w-full max-w-md relative z-10">
      <div class="glass-card rounded-[2.5rem] p-10 shadow-[0_0_100px_-20px_rgba(59,130,246,0.3)] border border-white/10">
        <!-- Header -->
        <div class="text-center mb-10">
          <div class="w-16 h-16 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-lg shadow-blue-500/20">
            <UserPlus class="w-8 h-8 text-white" />
          </div>
          <h1 class="text-3xl font-black text-white tracking-tight mb-2">Crear Cuenta</h1>
          <p class="text-gray-400 font-medium">Únete a nuestra plataforma técnica</p>
        </div>

        <form @submit.prevent="handleRegister" class="space-y-6">
          <div class="space-y-2">
            <label class="block text-xs font-bold text-gray-400 ml-1 uppercase tracking-widest">Email</label>
            <div class="relative group">
              <Mail class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500 group-focus-within:text-blue-500 transition-colors" />
              <input 
                v-model="email" 
                type="email" 
                required
                class="w-full glass-input rounded-2xl py-4 pl-12 pr-4 text-white placeholder:text-gray-600"
                placeholder="tu@ejemplo.com"
              />
            </div>
          </div>

          <div class="space-y-2">
            <label class="block text-xs font-bold text-gray-400 ml-1 uppercase tracking-widest">Contraseña</label>
            <div class="relative group">
              <Lock class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500 group-focus-within:text-blue-500 transition-colors" />
              <input 
                v-model="password" 
                type="password" 
                required
                class="w-full glass-input rounded-2xl py-4 pl-12 pr-4 text-white placeholder:text-gray-600"
                placeholder="••••••••"
              />
            </div>
          </div>

          <div v-if="error" class="bg-red-500/10 border border-red-500/20 text-red-500 px-4 py-3 rounded-xl text-sm font-bold flex items-center gap-2 animate-shake">
            <AlertCircle class="w-4 h-4 flex-shrink-0" />
            {{ error }}
          </div>
          
          <div v-if="success" class="bg-emerald-500/10 border border-emerald-500/20 text-emerald-500 px-4 py-3 rounded-xl text-sm font-bold flex items-center gap-2">
            <CheckCircle2 class="w-4 h-4 flex-shrink-0" />
            Registro exitoso. Redirigiendo...
          </div>

          <button 
            type="submit" 
            :disabled="loading"
            class="w-full bg-blue-600 hover:bg-blue-500 text-white py-4 rounded-2xl font-black flex items-center justify-center gap-2 transition-all shadow-xl shadow-blue-900/20 active:scale-95 disabled:opacity-50"
          >
            {{ loading ? 'Creando cuenta...' : 'Registrarse' }}
            <ArrowRight class="w-5 h-5" />
          </button>
        </form>

        <div class="mt-8 pt-8 border-t border-white/5 text-center">
          <p class="text-gray-500 text-sm font-medium">
            ¿Ya tienes cuenta? 
            <router-link to="/login" class="text-blue-500 hover:text-white font-bold transition-colors">Entrar aquí</router-link>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { UserPlus, Mail, Lock, ArrowRight, AlertCircle, CheckCircle2 } from 'lucide-vue-next'
import api from '../services/api'

const email = ref('')
const password = ref('')
const error = ref('')
const success = ref(false)
const loading = ref(false)
const router = useRouter()

const handleRegister = async () => {
  error.value = ''
  loading.value = true
  try {
    await api.post('/auth/register', {
      email: email.value,
      password: password.value
    })
    success.value = true
    setTimeout(() => {
      router.push('/login')
    }, 2000)
  } catch (err) {
    error.value = err.response?.data?.[0]?.description || err.response?.data || 'Error al registrar'
    if (typeof error.value === 'object') error.value = 'Error en el registro'
  } finally {
    loading.value = false
  }
}
</script>
