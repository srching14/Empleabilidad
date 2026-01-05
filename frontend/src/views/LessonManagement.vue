<template>
  <div class="min-h-screen bg-gray-950 p-6 text-gray-100">
    <div class="max-w-5xl mx-auto">
      <!-- Nav -->
      <nav class="flex items-center gap-4 mb-8">
        <router-link to="/" class="p-2 hover:bg-gray-900 rounded-lg transition-all">
          <ArrowLeft class="w-6 h-6" />
        </router-link>
        <div>
          <h1 class="text-2xl font-bold">Gestión de Lecciones</h1>
          <p class="text-gray-400 text-sm">Curso ID: {{ $route.params.id }}</p>
        </div>
      </nav>

      <!-- Action Bar -->
      <div class="bg-gray-900/50 border border-gray-800 rounded-2xl p-6 mb-8 flex justify-between items-center backdrop-blur-sm">
        <h2 class="text-lg font-semibold flex items-center gap-2">
          <Library class="w-5 h-5 text-blue-500" />
          Lista de Contenido
        </h2>
        <button 
          @click="showAddModal = true"
          class="bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded-lg font-bold flex items-center gap-2 transition-all"
        >
          <PlusCircle class="w-5 h-5" />
          Añadir Lección
        </button>
      </div>

      <!-- Lessons List -->
      <div class="space-y-3">
        <div 
          v-for="(lesson, index) in lessons" 
          :key="lesson.id"
          class="group bg-gray-900 border border-gray-800 hover:border-blue-500/50 p-4 rounded-xl flex items-center gap-4 transition-all"
        >
          <div class="flex flex-col items-center gap-1">
            <button 
              @click="reorder(lesson.id, lesson.order - 1)"
              :disabled="index === 0"
              class="p-1 hover:text-blue-500 disabled:opacity-0"
            >
              <ChevronUp class="w-6 h-6" />
            </button>
            <span class="text-xs font-mono text-blue-500 font-bold">{{ lesson.order }}</span>
            <button 
              @click="reorder(lesson.id, lesson.order + 1)"
              :disabled="index === lessons.length - 1"
              class="p-1 hover:text-blue-500 disabled:opacity-0"
            >
              <ChevronDown class="w-6 h-6" />
            </button>
          </div>

          <div class="flex-1">
            <h3 class="font-semibold">{{ lesson.title }}</h3>
            <p class="text-xs text-gray-500 italic">Posición: {{ lesson.order }}</p>
          </div>

          <div class="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <button @click="deleteLesson(lesson.id)" class="p-2 text-gray-500 hover:text-red-500">
              <Trash2 class="w-5 h-5" />
            </button>
          </div>
        </div>

        <div v-if="lessons.length === 0" class="text-center py-20 bg-gray-900/30 rounded-2xl border border-dashed border-gray-800">
          <BookOpenCheck class="w-12 h-12 text-gray-700 mx-auto mb-4" />
          <p class="text-gray-500">No hay lecciones en este curso todavía.</p>
        </div>
      </div>
    </div>

    <!-- Add Lesson Modal -->
    <div v-if="showAddModal" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-sm">
      <div class="bg-gray-900 w-full max-w-lg rounded-2xl border border-gray-800 shadow-2xl">
        <div class="p-6 border-b border-gray-800 flex justify-between items-center">
          <h2 class="text-xl font-bold">Nueva Lección</h2>
          <button @click="showAddModal = false" class="text-gray-500 hover:text-white">
            <X class="w-6 h-6" />
          </button>
        </div>
        <div class="p-6 space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Título</label>
            <input 
              v-model="newLesson.title"
              class="w-full bg-gray-800 border border-gray-700 rounded-lg p-3 text-white focus:ring-2 focus:ring-blue-500 outline-none"
              placeholder="Ej: Introducción a la API"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Orden (Opcional)</label>
            <input 
              v-model.number="newLesson.order"
              type="number"
              class="w-full bg-gray-800 border border-gray-700 rounded-lg p-3 text-white focus:ring-2 focus:ring-blue-500 outline-none"
            />
          </div>
        </div>
        <div class="p-6 bg-gray-900/50 flex justify-end gap-3">
          <button @click="showAddModal = false" class="px-4 py-2 text-gray-400 hover:text-white">Cancelar</button>
          <button @click="addLesson" class="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2 rounded-lg font-bold shadow-lg shadow-blue-600/20">Añadir</button>
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
  ChevronUp, ChevronDown, BookOpenCheck, X 
} from 'lucide-vue-next'

const route = useRoute()
const lessons = ref([])
const showAddModal = ref(false)
const newLesson = ref({ title: '', order: null })

const fetchLessons = async () => {
  const res = await api.get(`/lessons/course/${route.params.id}`)
  lessons.value = res.data.sort((a,b) => a.order - b.order)
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
