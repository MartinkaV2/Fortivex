<template>
  <div class="loading-spinner" :class="sizeClass">
    <div class="spinner"></div>
    <p v-if="text" class="loading-text">{{ text }}</p>
  </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  size: {
    type: String,
    default: 'medium',
    validator: (value) => ['small', 'medium', 'large'].includes(value),
  },
  text: {
    type: String,
    default: '',
  },
});

const sizeClass = computed(() => `size-${props.size}`);
</script>

<style scoped lang="scss">
.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  padding: 2rem;

  &.size-small {
    .spinner {
      width: 30px;
      height: 30px;
      border-width: 3px;
    }

    .loading-text {
      font-size: 0.85rem;
    }
  }

  &.size-medium {
    .spinner {
      width: 60px;
      height: 60px;
      border-width: 4px;
    }

    .loading-text {
      font-size: 1rem;
    }
  }

  &.size-large {
    .spinner {
      width: 100px;
      height: 100px;
      border-width: 6px;
    }

    .loading-text {
      font-size: 1.2rem;
    }
  }
}

.spinner {
  border: 4px solid rgba(255, 215, 0, 0.2);
  border-top-color: #ffd700;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.loading-text {
  color: #f4e4c1;
  font-weight: 500;
  text-align: center;
}
</style>