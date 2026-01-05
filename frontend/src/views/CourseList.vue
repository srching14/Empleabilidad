<template>
  <div class="min-h-screen p-6 lg:p-12 relative overflow-hidden">
    <!-- Background Decor -->
    <div class="fixed top-0 left-0 w-full h-full pointer-events-none z-0">
      <div class="absolute top-[20%] right-[-5%] w-[30%] h-[30%] bg-blue-600/10 blur-[100px] rounded-full"></div>
      <div class="absolute bottom-[20%] left-[-5%] w-[30%] h-[30%] bg-indigo-600/10 blur-[100px] rounded-full"></div>
    </div>

    <div class="max-w-7xl mx-auto relative z-10">
      <!-- Header Section -->
      <header class="flex flex-col md:flex-row md:items-end justify-between gap-6 mb-12">
        <div>
          <div class="inline-flex items-center gap-2 bg-blue-500/10 text-blue-400 px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider mb-4 border border-blue-500/20">
            <span class="relative flex h-2 w-2">
              <span class="animate-ping absolute inline-flex h-full w-full rounded-full bg-blue-400 opacity-75"></span>
              <span class="relative inline-flex rounded-full h-2 w-2 bg-blue-500"></span>
            </span>
            Administración
          </div>
          <h1 class="text-4xl md:text-5xl font-bold text-white tracking-tight flex items-center gap-4">
            Dashboard
          </h1>
          <p class="text-gray-400 mt-3 text-lg font-medium">Gestiona tu catálogo de cursos y lecciones</p>
        </div>

        <div class="flex items-center gap-4">
          <button 
            @click="showCreateModal = true"
            class="group bg-white text-gray-950 hover:bg-blue-50 px-6 py-3 rounded-2xl font-bold flex items-center gap-2 transition-all shadow-[0_0_20px_rgba(255,255,255,0.1)] active:scale-95"
          >
            <Plus class="w-5 h-5 group-hover:rotate-90 transition-transform" />
            Nuevo Curso
          </button>
          <button @click="handleLogout" class="glass-card hover:bg-red-500/10 hover:border-red-500/20 p-3 rounded-2xl transition-all group">
            <LogOut class="w-6 h-6 text-gray-400 group-hover:text-red-500" />
          </button>
        </div>
      </header>

      <!-- Stats Bar (Simulated for visual richness) -->
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4 mb-12">
        <div v-for="(stat, i) in stats" :key="i" class="glass-card p-6 rounded-3xl border-white/5">
          <p class="text-gray-500 text-xs font-bold uppercase tracking-widest mb-1">{{ stat.label }}</p>
          <p class="text-2xl font-bold text-white">{{ stat.value }}</p>
        </div>
      </div>

      <!-- Controls -->
      <div class="flex flex-col md:flex-row gap-4 mb-10">
        <div class="flex-1 relative group">
          <Search class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500 group-focus-within:text-blue-500 transition-colors" />
          <input 
            v-model="search"
            class="w-full glass-input rounded-2xl py-4 pl-12 pr-4 text-white"
            placeholder="Buscar por título del curso..."
          />
        </div>
        <div class="flex gap-2">
          <select 
            v-model="filterStatus"
            class="glass-input rounded-2xl px-6 py-4 text-white font-medium min-w-[180px] appearance-none cursor-pointer"
          >
            <option value="All">Todos los estados</option>
            <option value="Draft">Borradores</option>
            <option value="Published">Publicados</option>
          </select>
        </div>
      </div>

      <!-- Main Content Container -->
      <div v-if="loading" class="flex flex-col items-center justify-center py-32 space-y-4">
        <div class="relative w-16 h-16">
          <div class="absolute inset-0 border-4 border-blue-500/20 rounded-full"></div>
          <div class="absolute inset-0 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"></div>
        </div>
        <p class="text-gray-500 font-bold animate-pulse">Cargando catálogo...</p>
      </div>

      <div v-else-if="courses.length === 0" class="glass-card rounded-[2.5rem] p-20 text-center border-dashed border-white/10">
        <div class="w-20 h-20 bg-gray-800/50 rounded-3xl flex items-center justify-center mx-auto mb-6">
          <Search class="w-10 h-10 text-gray-600" />
        </div>
        <h2 class="text-2xl font-bold text-white mb-2">No se encontraron cursos</h2>
        <p class="text-gray-500 max-w-sm mx-auto">Intenta ajustar tus filtros o crea un nuevo curso para comenzar.</p>
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        <div 
          v-for="course in courses" 
          :key="course.id"
          class="group glass-card rounded-[2rem] overflow-hidden hover:border-blue-500/30 transition-all duration-500 flex flex-col hover:-translate-y-2 hover:shadow-blue-500/5"
        >
          <!-- Card Body -->
          <div class="p-8 flex-1">
            <div class="flex justify-between items-start mb-6">
              <div 
                :class="[
                  'px-4 py-1.5 rounded-xl text-[10px] font-black uppercase tracking-tighter border',
                  course.status === 'Published' 
                    ? 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20' 
                    : 'bg-amber-500/10 text-amber-400 border-amber-500/20'
                ]"
              >
                {{ course.status }}
              </div>
              <div class="flex gap-2 translate-x-4 opacity-0 group-hover:opacity-100 group-hover:translate-x-0 transition-all duration-300">
                <button @click="deleteCourse(course.id)" class="bg-red-500/10 hover:bg-red-500 text-red-500 hover:text-white p-2.5 rounded-xl transition-all border border-red-500/20">
                  <Trash2 class="w-4 h-4" />
                </button>
              </div>
            </div>

            <h3 class="text-2xl font-bold text-white mb-3 leading-tight group-hover:text-blue-400 transition-colors">{{ course.title }}</h3>
            <p class="text-gray-400 text-sm line-clamp-3 leading-relaxed mb-6">{{ course.description || 'Sin descripción detallada disponible para este curso.' }}</p>
            
            <div class="flex items-center gap-3 text-gray-500 text-xs font-bold uppercase tracking-widest bg-white/5 py-2 px-4 rounded-xl w-fit">
              <Layers class="w-4 h-4" />
              {{ course.lessons?.length || 0 }} Lecciones
            </div>
          </div>
          
          <!-- Card Footer -->
          <div class="p-6 bg-white/[0.02] border-t border-white/5 flex items-center justify-between gap-4">
            <router-link 
              :to="'/courses/' + course.id + '/lessons'"
              class="flex-1 bg-white/5 hover:bg-blue-600 text-white hover:text-white px-4 py-3 rounded-xl font-bold text-sm text-center transition-all flex items-center justify-center gap-2"
            >
              <BookOpen class="w-4 h-4" />
              Contenido
            </router-link>
            <button 
              v-if="course.status === 'Draft'"
              @click="publishCourse(course.id)"
              class="bg-emerald-600/10 hover:bg-emerald-600 text-emerald-400 hover:text-white px-4 py-3 rounded-xl font-bold text-sm transition-all border border-emerald-500/20 flex items-center gap-2"
              title="Publicar Curso"
            >
              <Send class="w-4 h-4" />
              <span>Publicar</span>
            </button>
            <button 
              v-else
              @click="unpublishCourse(course.id)"
              class="bg-amber-600/10 hover:bg-amber-600 text-amber-400 hover:text-white px-4 py-3 rounded-xl font-bold text-sm transition-all border border-amber-500/20 flex items-center gap-2"
              title="Quitar Publicación"
            >
              <X class="w-4 h-4" />
              <span>Borrador</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <div v-if="totalPages > 1" class="mt-20 flex justify-center items-center gap-4 bg-gray-900/40 p-2 rounded-2xl border border-white/5 w-fit mx-auto backdrop-blur-xl">
        <button 
          v-for="p in totalPages" 
          :key="p"
          @click="page = p"
          :class="[
            'w-12 h-12 rounded-xl flex items-center justify-center font-bold transition-all text-sm',
            page === p 
              ? 'bg-white text-gray-950 shadow-xl' 
              : 'text-gray-500 hover:text-white hover:bg-white/5'
          ]"
        >
          {{ p }}
        </button>
      </div>
    </div>

    <!-- Create Modal -->
    <div v-if="showCreateModal" class="fixed inset-0 z-[100] flex items-center justify-center p-6 bg-black/60 backdrop-blur-md overflow-y-auto">
      <div class="glass-card w-full max-w-xl rounded-[2.5rem] shadow-[0_0_100px_-20px_rgba(59,130,246,0.3)] animate-in zoom-in-95 duration-300">
        <div class="p-8 border-b border-white/5 flex justify-between items-center bg-white/[0.02]">
          <div>
            <h2 class="text-2xl font-bold text-white">Nuevo Curso</h2>
            <p class="text-gray-400 text-sm mt-1">Configura los detalles básicos</p>
          </div>
          <button @click="showCreateModal = false" class="text-gray-500 hover:text-white bg-white/5 p-2 rounded-xl transition-all">
            <X class="w-6 h-6" />
          </button>
        </div>
        
        <div class="p-8 space-y-6">
          <div class="space-y-2">
            <label class="block text-sm font-bold text-gray-400 ml-1 uppercase tracking-widest">Nombre del Programa</label>
            <input 
              v-model="newCourseTitle"
              class="w-full glass-input rounded-2xl py-4 px-5 text-white placeholder:text-gray-600"
              placeholder="Ej: Master en Arquitectura Limpia con .NET 8"
            />
          </div>
          <div class="space-y-2">
            <label class="block text-sm font-bold text-gray-400 ml-1 uppercase tracking-widest">Resumen del Curso</label>
            <textarea 
              v-model="newCourseDescription"
              rows="4"
              class="w-full glass-input rounded-2xl py-4 px-5 text-white placeholder:text-gray-600 resize-none"
              placeholder="Describe brevemente de qué trata este curso..."
            ></textarea>
          </div>
        </div>

        <div class="p-8 bg-white/[0.02] border-t border-white/5 flex justify-end gap-4 rounded-b-[2.5rem]">
          <button @click="showCreateModal = false" class="px-6 py-3 text-gray-400 hover:text-white font-bold transition-all">Descartar</button>
          <button 
            @click="createCourse" 
            class="bg-blue-600 hover:bg-blue-500 text-white px-10 py-3 rounded-2xl font-bold transition-all shadow-xl shadow-blue-900/20 active:scale-95"
          >
            Crear Programa
          </button>
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
  Trash2, BookOpen, Send, X, Layers 
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
const newCourseDescription = ref('')

const stats = ref([
  { label: 'Cursos Activos', value: '12' },
  { label: 'Estudiantes', value: '1.2k' },
  { label: 'Lecciones', value: '84' },
  { label: 'Tasa de Éxito', value: '98%' }
])

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
    totalPages.value = Math.ceil(res.data.totalCount / 5)
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const createCourse = async () => {
  if (!newCourseTitle.value) return
  await api.post('/courses', { 
    title: newCourseTitle.value,
    description: newCourseDescription.value
  })
  newCourseTitle.value = ''
  newCourseDescription.value = ''
  showCreateModal.value = false
  fetchCourses()
}

const publishCourse = async (id) => {
  try {
    await api.post(`/courses/${id}/publish`)
    fetchCourses()
  } catch (error) {
    const msg = error.response?.data || error.message
    alert(typeof msg === 'string' ? msg : 'Error al publicar: Asegúrate de tener al menos una lección.')
  }
}

const unpublishCourse = async (id) => {
  try {
    await api.post(`/courses/${id}/unpublish`)
    fetchCourses()
  } catch (error) {
    console.error(error)
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
