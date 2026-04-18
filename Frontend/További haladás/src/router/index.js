import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "@/stores/auth";
import SupportPage from "@/views/SupportPage.vue";
import Help from "@/components/support/Help.vue";
import Gyik from "@/components/support/Gyik.vue";
import Terms from "@/components/support/Terms.vue";
import Privacy from "@/components/support/Privacy.vue";

const routes = [
  {
    path: "/",
    component: () => import("@/views/HomePage.vue"),
  },
  {
    path: "/support",
    component: SupportPage,
  },
  // Önálló útvonalak, nem a SupportPage gyerekei!
  {
    path: "/support/help",
    component: Help,
  },
  {
    path: "/support/gyik",
    component: Gyik,
  },
  {
    path: "/support/terms",
    component: Terms,
  },
  {
    path: "/support/privacy",
    component: Privacy,
  },
  {
    path: "/admin",
    component: () => import("@/views/AdminDashboard.vue"),
    meta: { requiresAuth: true, role: "admin" },
  },
  {
    path: "/player",
    component: () => import("@/views/PlayerDashboard.vue"),
    meta: { requiresAuth: true, role: "player" },
  },
  { path: "/:pathMatch(.*)*", redirect: "/support" },
  {
    path: "/forgot-password",
    component: () => import("@/views/ForgotPasswordView.vue"),
  },
  {
    path: "/reset-password",
    component: () => import("@/views/ResetPasswordView.vue"),
  },
];
// Dynamically add routes from the 'modules' directory
const moduleFiles = import.meta.glob("../modules/**/router.js", { eager: true });

for (const modulePath in moduleFiles) {
  const moduleRoutes = moduleFiles[modulePath].default;
  moduleRoutes.forEach((route) => {
    routes.push(route);
  });
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL), // ← BASE_URL hozzáadva!
  routes,
  scrollBehavior(to) {
    if (to.hash) {
      return {
        el: to.hash,
        behavior: "smooth",
      };
    }
    return { top: 0 };
  },
});

// Navigation guards
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();

  if (to.meta.requiresAuth) {
    if (!authStore.isAuthenticated) {
      next("/");
      return;
    }

    if (to.meta.role === "admin" && !authStore.isAdmin) {
      next("/player"); // ← Ez is "/player" lesz most
      return;
    }

    if (to.meta.role === "player" && authStore.isAdmin) {
      next("/admin"); // ← Ez is "/admin" lesz most
      return;
    }
  }

  next();
});

export default router;
