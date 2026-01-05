<template>
  <div class="min-h-screen p-6 lg:p-12 relative overflow-hidden">
    <!-- Background Decor -->
    <div class="fixed top-0 left-0 w-full h-full pointer-events-none z-0">
      <div class="absolute top-0 right-0 w-[50%] h-[50%] bg-blue-600/5 blur-[120px] rounded-full"></div>
      <div class="absolute bottom-0 left-0 w-[50%] h-[50%] bg-indigo-600/5 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-5xl mx-auto relative z-10">
      <!-- Navigation & Header -->
      <nav class="flex flex-col md:flex-row md:items-center gap-6 mb-12">
        <router-link to="/" class="group glass-card p-4 rounded-2xl hover:bg-white/5 transition-all w-fit">
          <ArrowLeft class="w-6 h-6 text-gray-400 group-hover:text-white group-hover:-translate-x-1 transition-all" />
        </router-link>
        <div>
          <h1 class="text-3xl font-bold text-white tracking-tight">Gestión de Contenido</h1>
          <div class="flex items-center gap-2 mt-2">
            <span class="text-blue-500 font-mono text-xs font-bold bg-blue-500/10 px-2 py-1 rounded-md border border-blue-500/20">CURSO</span>
            <p class="text-gray-500 text-sm font-medium">{{ courseTitle || 'Cargando detalles...' }}</p>
          </div>
        </div>
      </nav>

      <!-- Action Bar -->
      <div class="glass-card rounded-3xl p-8 mb-10 flex flex-col md:flex-row justify-between items-center gap-6">
        <div class="flex items-center gap-4">
          <div class="w-12 h-12 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl flex items-center justify-center shadow-lg shadow-blue-500/20">
            <Library class="w-6 h-6 text-white" />
          </div>
          <div>
            <h2 class="text-xl font-bold text-white">Estructura del Programa</h2>
            <p class="text-gray-500 text-sm">Organiza y reordena las lecciones</p>
          </div>
        </div>
        <button 
          @click="showAddModal = true"
          class="w-full md:w-auto bg-white text-gray-950 hover:bg-blue-50 px-8 py-3.5 rounded-2xl font-bold flex items-center justify-center gap-2 transition-all shadow-xl active:scale-95"
        >
          <PlusCircle class="w-5 h-5" />
          Nueva Lección
        </button>
      </div>

      <!-- Lessons List -->
      <div class="space-y-4">
        <div 
          v-for="(lesson, index) in lessons" 
          :key="lesson.id"
          class="group glass-card rounded-2xl p-5 flex items-center gap-6 transition-all duration-300 hover:border-blue-500/30 hover:bg-white/[0.03]"
        >
          <!-- Reorder Controls -->
          <div class="flex flex-col items-center gap-1 bg-gray-800/50 p-2 rounded-xl border border-white/5">
            <button 
              @click="reorder(lesson.id, lesson.order - 1)"
              :disabled="index === 0"
              class="p-1 text-gray-500 hover:text-blue-400 disabled:opacity-0 transition-colors"
            >
              <ChevronUp class="w-6 h-6" />
            </button>
            <div class="w-8 h-8 rounded-lg bg-blue-500/10 flex items-center justify-center">
              <span class="text-sm font-black text-blue-500">{{ lesson.order }}</span>
            </div>
            <button 
              @click="reorder(lesson.id, lesson.order + 1)"
              :disabled="index === lessons.length - 1"
              class="p-1 text-gray-500 hover:text-blue-400 disabled:opacity-0 transition-colors"
            >
              <ChevronDown class="w-6 h-6" />
            </button>
          </div>

          <!-- Lesson Info -->
          <div class="flex-1">
            <h3 class="text-lg font-bold text-white group-hover:text-blue-400 transition-colors">{{ lesson.title }}</h3>
            <div class="flex items-center gap-4 mt-2">
              <span class="text-[10px] font-black uppercase tracking-widest text-gray-500 flex items-center gap-1.5">
                <Clock class="w-3 h-3" />
                15 MIN
              </span>
              <span class="text-[10px] font-black uppercase tracking-widest text-gray-500 flex items-center gap-1.5">
                <FileText class="w-3 h-3" />
                RECURSOS
              </span>
            </div>
          </div>

          <!-- Actions -->
          <div class="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-all duration-300 translate-x-2 group-hover:translate-x-0">
            <button @click="deleteLesson(lesson.id)" class="bg-red-500/10 hover:bg-red-500 text-red-500 hover:text-white p-3 rounded-xl border border-red-500/10 transition-all">
              <Trash2 class="w-5 h-5" />
            </button>
          </div>
        </div>

        <!-- Empty State -->
        <div v-if="lessons.length === 0" class="glass-card rounded-[2.5rem] py-24 text-center border-dashed border-white/10">
          <div class="w-20 h-20 bg-gray-800/50 rounded-3xl flex items-center justify-center mx-auto mb-6">
            <BookOpenCheck class="w-10 h-10 text-gray-600" />
          </div>
          <h2 class="text-2xl font-bold text-white mb-2">Sin contenido aún</h2>
          <p class="text-gray-500 max-w-sm mx-auto mb-8">Empieza a construir tu programa educativo añadiendo la primera lección hoy mismo.</p>
          <button @click="showAddModal = true" class="text-blue-500 font-bold hover:text-white transition-colors">Añadir lección inicial →</button>
        </div>
      </div>
    </div>

    <!-- Add Lesson Modal -->
    <div v-if="showAddModal" class="fixed inset-0 z-[100] flex items-center justify-center p-6 bg-black/60 backdrop-blur-md">
      <div class="glass-card w-full max-w-xl rounded-[2.5rem] shadow-[0_0_100px_-20px_rgba(59,130,246,0.3)] animate-in zoom-in-95 duration-300">
        <div class="p-8 border-b border-white/5 flex justify-between items-center bg-white/[0.02]">
          <div>
            <h2 class="text-2xl font-bold text-white">Nueva Lección</h2>
            <p class="text-gray-400 text-sm mt-1">Define el título y la secuencia</p>
          </div>
          <button @click="showAddModal = false" class="text-gray-500 hover:text-white bg-white/5 p-2 rounded-xl transition-all">
            <X class="w-6 h-6" />
          </button>
        </div>
        
        <div class="p-8 space-y-6">
          <div class="space-y-2">
            <label class="block text-sm font-bold text-gray-400 ml-1 uppercase tracking-widest">Título de la Lección</label>
            <input 
              v-model="newLesson.title"
              class="w-full glass-input rounded-2xl py-4 px-5 text-white placeholder:text-gray-600"
              placeholder="Ej: Fundamentos de la Arquitectura de Software"
            />
          </div>
          <div class="space-y-2">
            <label class="block text-sm font-bold text-gray-400 ml-1 uppercase tracking-widest">Orden en la Secuencia (Opcional)</label>
            <input 
              v-model.number="newLesson.order"
              type="number"
              class="w-full glass-input rounded-2xl py-4 px-5 text-white"
              placeholder="Por defecto: final de la lista"
            />
          </div>
        </div>

        <div class="p-8 bg-white/[0.02] border-t border-white/5 flex justify-end gap-4 rounded-b-[2.5rem]">
          <button @click="showAddModal = false" class="px-6 py-3 text-gray-400 hover:text-white font-bold transition-all">Cancelar</button>
          <button 
            @click="addLesson" 
            class="bg-blue-600 hover:bg-blue-500 text-white px-10 py-3 rounded-2xl font-bold transition-all shadow-xl shadow-blue-900/20 active:scale-95"
          >
            Añadir Contenido
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../services/api'
import { 
  ArrowLeft, Library, PlusCircle, Trash2, 
  ChevronUp, ChevronDown, BookOpenCheck, X,
  Clock, FileText 
} from 'lucide-vue-next'

const route = useRoute()
const lessons = ref([])
const courseTitle = ref('')
const showAddModal = ref(false)
const newLesson = ref({ title: '', order: null })

const fetchLessons = async () => {
  try {
    const [lessonsRes, courseRes] = await Promise.all([
      api.get(`/lessons/course/${route.params.id}`),
      api.get(`/courses/${route.params.id}`)
    ])
    lessons.value = lessonsRes.data.sort((a,b) => a.order - b.order)
    courseTitle.value = courseRes.data.title
  } catch (error) {
    console.error(error)
  }
}

const addLesson = async () => {
  if (!newLesson.value.title) return
  const courseId = route.params.id
  const order = newLesson.value.order || (lessons.value.length + 1)
  await api.post('/lessons', { 
    courseId, 
    title: newLesson.value.title, 
    order 
  })
  showAddModal.value = false
  newLesson.value = { title: '', order: null }
  fetchLessons()
}

const deleteLesson = async (id) => {
  if (confirm('¿Borrar esta lección?')) {
    await api.delete(`/lessons/${id}`)
    fetchLessons()
  }
}

const reorder = async (id, newOrder) => {
  await api.post(`/lessons/${id}/reorder`, { newOrder })
  fetchLessons()
}

onMounted(fetchLessons)
</script>
