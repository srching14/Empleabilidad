import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import Login from '../views/Login.vue'
import CourseList from '../views/CourseList.vue'
import LessonManagement from '../views/LessonManagement.vue'

const routes = [
    { path: '/login', component: Login, name: 'Login' },
    {
        path: '/',
        component: CourseList,
        name: 'Home',
        meta: { requiresAuth: true }
    },
    {
        path: '/courses/:id/lessons',
        component: LessonManagement,
        name: 'Lessons',
        meta: { requiresAuth: true }
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

router.beforeEach((to, from, next) => {
    const authStore = useAuthStore()
    if (to.meta.requiresAuth && !authStore.isAuthenticated) {
        next('/login')
    } else {
        next()
    }
})

export default router
