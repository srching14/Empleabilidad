<template>
  <div class="min-h-screen bg-gray-950 p-6">
    <div class="max-w-7xl mx-auto">
      <!-- Header -->
      <div class="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
        <div>
          <h1 class="text-3xl font-bold text-white flex items-center gap-3">
            <LayoutDashboard class="w-8 h-8 text-blue-500" />
            Panel de Cursos
          </h1>
          <p class="text-gray-400 mt-1">Gestiona tus contenidos y publicaciones</p>
        </div>
        <div class="flex items-center gap-3">
          <button 
            @click="showCreateModal = true"
            class="bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded-lg font-medium flex items-center gap-2 transition-all shadow-lg shadow-blue-600/20"
          >
            <Plus class="w-5 h-5" />
            Nuevo Curso
          </button>
          <button @click="handleLogout" class="text-gray-400 hover:text-white p-2 transition-colors">
            <LogOut class="w-6 h-6" />
          </button>
        </div>
      </div>

      <!-- Filters & Search -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8 bg-gray-900/50 p-4 rounded-xl border border-gray-800">
        <div class="relative">
          <Search class="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500" />
          <input 
            v-model="search"
            class="w-full bg-gray-800 border border-gray-700 rounded-lg py-2 pl-10 pr-4 text-white focus:ring-2 focus:ring-blue-500 outline-none"
            placeholder="Buscar por título..."
          />
        </div>
        <select 
          v-model="filterStatus"
          class="bg-gray-800 border border-gray-700 rounded-lg px-4 py-2 text-white focus:ring-2 focus:ring-blue-500 outline-none"
        >
          <option value="All">Todos los estados</option>
          <option value="Draft">Borrador</option>
          <option value="Published">Publicado</option>
        </select>
      </div>

      <!-- Course Cards -->
      <div v-if="loading" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div 
          v-for="course in courses" 
          :key="course.id"
          class="bg-gray-900 border border-gray-800 rounded-2xl overflow-hidden hover:border-gray-700 transition-all flex flex-col group"
        >
          <div class="p-6 flex-1">
            <div class="flex justify-between items-start mb-4">
              <span 
                :class="[
                  'px-2 py-1 rounded text-xs font-bold uppercase',
                  course.status === 'Published' ? 'bg-green-500/10 text-green-500' : 'bg-yellow-500/10 text-yellow-500'
                ]"
              >
                {{ course.status }}
              </span>
              <div class="flex gap-2">
                <button @click="deleteCourse(course.id)" class="text-gray-500 hover:text-red-500 transition-colors">
                  <Trash2 class="w-5 h-5" />
                </button>
              </div>
            </div>
            <h3 class="text-xl font-bold text-white mb-2">{{ course.title }}</h3>
            <p class="text-sm text-gray-400 mb-4 line-clamp-2">ID: {{ course.id }}</p>
          </div>
          
          <div class="p-4 bg-gray-900/50 border-t border-gray-800 flex items-center justify-between">
            <router-link 
              :to="'/courses/' + course.id + '/lessons'"
              class="text-blue-500 hover:text-blue-400 font-medium text-sm flex items-center gap-1"
            >
              <BookOpen class="w-4 h-4" />
              Ver Lecciones
            </router-link>
            <button 
              v-if="course.status === 'Draft'"
              @click="publishCourse(course.id)"
              class="text-green-500 hover:text-green-400 font-medium text-sm flex items-center gap-1"
            >
              <Send class="w-4 h-4" />
              Publicar
            </button>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <div class="mt-12 flex justify-center gap-2">
        <button 
          v-for="p in totalPages" 
          :key="p"
          @click="page = p"
          :class="[
            'w-10 h-10 rounded-lg flex items-center justify-center border transition-all',
            page === p ? 'bg-blue-600 border-blue-600 text-white shadow-lg' : 'bg-gray-900 border-gray-800 text-gray-400 hover:border-gray-700'
          ]"
        >
          {{ p }}
        </button>
      </div>
    </div>

    <!-- Create Modal -->
    <div v-if="showCreateModal" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-sm">
      <div class="bg-gray-900 w-full max-w-lg rounded-2xl border border-gray-800 shadow-2xl overflow-hidden">
        <div class="p-6 border-b border-gray-800 flex justify-between items-center">
          <h2 class="text-xl font-bold text-white">Nuevo Curso</h2>
          <button @click="showCreateModal = false" class="text-gray-500 hover:text-white">
            <X class="w-6 h-6" />
          </button>
        </div>
        <div class="p-6">
          <label class="block text-sm font-medium text-gray-400 mb-2">Título del Curso</label>
          <input 
            v-model="newCourseTitle"
            class="w-full bg-gray-800 border border-gray-700 rounded-lg p-3 text-white focus:ring-2 focus:ring-blue-500 outline-none"
            placeholder="Ej: Master en .NET 8"
          />
        </div>
        <div class="p-6 bg-gray-900/50 flex justify-end gap-3">
          <button @click="showCreateModal = false" class="px-4 py-2 text-gray-400 hover:text-white font-medium">Cancelar</button>
          <button @click="createCourse" class="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2 rounded-lg font-bold shadow-lg shadow-blue-600/20">Crear Curso</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import api from '../services/api'
import { 
  LayoutDashboard, Plus, Search, LogOut, 
  Trash2, BookOpen, Send, X 
} from 'lucide-vue-next'

const authStore = useAuthStore()
const router = useRouter()

const courses = ref([])
const loading = ref(false)
const search = ref('')
const filterStatus = ref('All')
const page = ref(1)
const totalPages = ref(1)
const showCreateModal = ref(false)
const newCourseTitle = ref('')

const fetchCourses = async () => {
  loading.value = true
  try {
    const params = {
      query: search.value,
      page: page.value,
      pageSize: 5
    }
    if (filterStatus.value !== 'All') {
      params.status = filterStatus.value
    }
    const res = await api.get('/courses', { params })
    courses.value = res.data.items
    totalPages.value = res.data.totalPages
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const createCourse = async () => {
  if (!newCourseTitle.value) return
  await api.post('/courses', { title: newCourseTitle.value })
  newCourseTitle.value = ''
  showCreateModal.value = false
  fetchCourses()
}

const publishCourse = async (id) => {
  try {
    await api.post(`/courses/${id}/publish`)
    fetchCourses()
  } catch (error) {
    alert(error.response?.data?.message || 'No se puede publicar el curso (asegúrate de que tenga lecciones activas)')
  }
}

const deleteCourse = async (id) => {
  if (confirm('¿Realmente deseas borrar este curso?')) {
    await api.delete(`/courses/${id}`)
    fetchCourses()
  }
}

const handleLogout = () => {
  authStore.logout()
  router.push('/login')
}

watch([search, filterStatus, page], () => fetchCourses())
onMounted(fetchCourses)
</script>
