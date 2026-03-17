<template>
  <div class="data-table">
    <div class="table-header">
      <div class="table-controls">
        <input
          v-model="searchQuery"
          type="text"
          :placeholder="searchPlaceholder"
          class="search-input"
          @input="handleSearch"
        />
        <select v-if="filters.length > 0" v-model="selectedFilter" class="filter-select">
          <option value="">Összes</option>
          <option v-for="filter in filters" :key="filter.value" :value="filter.value">
            {{ filter.label }}
          </option>
        </select>
      </div>
      <div class="table-actions">
        <button @click="$emit('refresh')" class="action-btn refresh">
          🔄 Frissítés
        </button>
        <button v-if="exportable" @click="handleExport" class="action-btn export">
          📥 Export
        </button>
      </div>
    </div>

    <div class="table-wrapper">
      <table class="data-table-content">
        <thead>
          <tr>
            <th v-for="column in columns" :key="column.key" @click="handleSort(column)">
              <div class="th-content">
                <span>{{ column.label }}</span>
                <span v-if="column.sortable" class="sort-icon">
                  <span v-if="sortColumn === column.key">
                    {{ sortDirection === 'asc' ? '▲' : '▼' }}
                  </span>
                  <span v-else class="sort-placeholder">⇅</span>
                </span>
              </div>
            </th>
            <th v-if="actions.length > 0" class="actions-column">Műveletek</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="loading">
            <td :colspan="columns.length + (actions.length > 0 ? 1 : 0)" class="loading-cell">
              <LoadingSpinner size="small" text="Betöltés..." />
            </td>
          </tr>
          <template v-else>
            <tr v-for="row in paginatedData" :key="row.id" @click="$emit('row-click', row)">
              <td v-for="column in columns" :key="column.key">
                <slot :name="`cell-${column.key}`" :row="row" :value="row[column.key]">
                  {{ formatCell(row[column.key], column) }}
                </slot>
              </td>
              <td v-if="actions.length > 0" class="actions-cell">
                <button
                  v-for="action in actions"
                  :key="action.name"
                  @click.stop="handleAction(action.name, row)"
                  class="action-button"
                  :class="action.class"
                  :title="action.label"
                >
                  {{ action.icon }}
                </button>
              </td>
            </tr>
          </template>
          <tr v-if="!loading && filteredData.length === 0">
            <td :colspan="columns.length + (actions.length > 0 ? 1 : 0)" class="empty-cell">
              {{ emptyMessage }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="paginated && filteredData.length > 0" class="table-footer">
      <div class="pagination-info">
        {{ paginationInfo }}
      </div>
      <div class="pagination-controls">
        <button
          @click="currentPage--"
          :disabled="currentPage === 1"
          class="page-btn"
        >
          ◀ Előző
        </button>
        <div class="page-numbers">
          <button
            v-for="page in visiblePages"
            :key="page"
            @click="currentPage = page"
            class="page-number"
            :class="{ active: currentPage === page }"
          >
            {{ page }}
          </button>
        </div>
        <button
          @click="currentPage++"
          :disabled="currentPage === totalPages"
          class="page-btn"
        >
          Következő ▶
        </button>
      </div>
      <div class="page-size-control">
        <label>Sorok/oldal:</label>
        <select v-model.number="pageSize" class="page-size-select">
          <option :value="10">10</option>
          <option :value="25">25</option>
          <option :value="50">50</option>
          <option :value="100">100</option>
        </select>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import LoadingSpinner from '@/components/common/LoadingSpinner.vue';

const props = defineProps({
  data: {
    type: Array,
    required: true,
  },
  columns: {
    type: Array,
    required: true,
  },
  actions: {
    type: Array,
    default: () => [],
  },
  filters: {
    type: Array,
    default: () => [],
  },
  loading: {
    type: Boolean,
    default: false,
  },
  paginated: {
    type: Boolean,
    default: true,
  },
  exportable: {
    type: Boolean,
    default: false,
  },
  searchPlaceholder: {
    type: String,
    default: 'Keresés...',
  },
  emptyMessage: {
    type: String,
    default: 'Nincs megjeleníthető adat',
  },
});

const emit = defineEmits(['row-click', 'action', 'refresh']);

const searchQuery = ref('');
const selectedFilter = ref('');
const sortColumn = ref('');
const sortDirection = ref('asc');
const currentPage = ref(1);
const pageSize = ref(10);

const filteredData = computed(() => {
  let result = [...props.data];

  // Apply search
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase();
    result = result.filter(row => {
      return props.columns.some(col => {
        const value = row[col.key];
        return value && value.toString().toLowerCase().includes(query);
      });
    });
  }

  // Apply filter
  if (selectedFilter.value) {
    const filterConfig = props.filters.find(f => f.value === selectedFilter.value);
    if (filterConfig && filterConfig.fn) {
      result = result.filter(filterConfig.fn);
    }
  }

  // Apply sort
  if (sortColumn.value) {
    result.sort((a, b) => {
      const aVal = a[sortColumn.value];
      const bVal = b[sortColumn.value];
      
      if (aVal === bVal) return 0;
      
      const comparison = aVal > bVal ? 1 : -1;
      return sortDirection.value === 'asc' ? comparison : -comparison;
    });
  }

  return result;
});

const totalPages = computed(() => {
  return Math.ceil(filteredData.value.length / pageSize.value);
});

const paginatedData = computed(() => {
  if (!props.paginated) return filteredData.value;
  
  const start = (currentPage.value - 1) * pageSize.value;
  const end = start + pageSize.value;
  return filteredData.value.slice(start, end);
});

const visiblePages = computed(() => {
  const pages = [];
  const maxVisible = 5;
  const half = Math.floor(maxVisible / 2);
  
  let start = Math.max(1, currentPage.value - half);
  let end = Math.min(totalPages.value, start + maxVisible - 1);
  
  if (end - start < maxVisible - 1) {
    start = Math.max(1, end - maxVisible + 1);
  }
  
  for (let i = start; i <= end; i++) {
    pages.push(i);
  }
  
  return pages;
});

const paginationInfo = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value + 1;
  const end = Math.min(currentPage.value * pageSize.value, filteredData.value.length);
  return `${start}-${end} / ${filteredData.value.length}`;
});

const handleSearch = () => {
  currentPage.value = 1;
};

const handleSort = (column) => {
  if (!column.sortable) return;
  
  if (sortColumn.value === column.key) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
  } else {
    sortColumn.value = column.key;
    sortDirection.value = 'asc';
  }
};

const handleAction = (actionName, row) => {
  emit('action', { action: actionName, row });
};

const formatCell = (value, column) => {
  if (value === null || value === undefined) return '-';
  
  if (column.format) {
    return column.format(value);
  }
  
  return value;
};

const handleExport = () => {
  const csv = convertToCSV(filteredData.value);
  downloadCSV(csv, 'export.csv');
};

const convertToCSV = (data) => {
  if (data.length === 0) return '';
  
  const headers = props.columns.map(col => col.label).join(',');
  const rows = data.map(row => {
    return props.columns.map(col => {
      const value = row[col.key];
      return `"${value}"`;
    }).join(',');
  });
  
  return [headers, ...rows].join('\n');
};

const downloadCSV = (csv, filename) => {
  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement('a');
  const url = URL.createObjectURL(blob);
  
  link.setAttribute('href', url);
  link.setAttribute('download', filename);
  link.style.visibility = 'hidden';
  
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

watch([selectedFilter, pageSize], () => {
  currentPage.value = 1;
});
</script>

<style scoped lang="scss">
.data-table {
  width: 100%;
  background: #2c1810;
  border: 2px solid #8b4513;
  border-radius: 12px;
  overflow: hidden;
}

.table-header {
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
  background: #1a0f0a;
  border-bottom: 2px solid #8b4513;
}

.table-controls {
  display: flex;
  gap: 1rem;
  flex: 1;
  min-width: 300px;

  .search-input {
    flex: 1;
    padding: 0.7rem 1rem;
    background: #2c1810;
    border: 2px solid #8b4513;
    border-radius: 8px;
    color: #f4e4c1;
    font-size: 1rem;

    &:focus {
      outline: none;
      border-color: #ffd700;
    }

    &::placeholder {
      color: rgba(244, 228, 193, 0.5);
    }
  }

  .filter-select {
    padding: 0.7rem 1rem;
    background: #2c1810;
    border: 2px solid #8b4513;
    border-radius: 8px;
    color: #f4e4c1;
    cursor: pointer;

    &:focus {
      outline: none;
      border-color: #ffd700;
    }
  }
}

.table-actions {
  display: flex;
  gap: 0.8rem;

  .action-btn {
    padding: 0.7rem 1.2rem;
    background: #8b4513;
    color: #fff;
    border: 2px solid #ffd700;
    border-radius: 8px;
    cursor: pointer;
    font-weight: 600;
    transition: all 0.3s ease;

    &:hover {
      background: #a0522d;
      transform: translateY(-2px);
    }

    &.export {
      background: #4169e1;
      border-color: #87ceeb;
    }
  }
}

.table-wrapper {
  overflow-x: auto;
}

.data-table-content {
  width: 100%;
  border-collapse: collapse;

  thead {
    background: #1a0f0a;

    th {
      padding: 1rem;
      text-align: left;
      color: #ffd700;
      font-weight: 700;
      text-transform: uppercase;
      font-size: 0.85rem;
      letter-spacing: 1px;
      border-bottom: 2px solid #8b4513;
      cursor: pointer;
      user-select: none;

      &:hover {
        background: rgba(255, 215, 0, 0.05);
      }

      .th-content {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 0.5rem;

        .sort-icon {
          font-size: 0.7rem;

          .sort-placeholder {
            opacity: 0.3;
          }
        }
      }

      &.actions-column {
        cursor: default;
        
        &:hover {
          background: transparent;
        }
      }
    }
  }

  tbody {
    tr {
      border-bottom: 1px solid rgba(139, 69, 19, 0.3);
      transition: all 0.2s ease;
      cursor: pointer;

      &:hover {
        background: rgba(255, 215, 0, 0.05);
      }

      td {
        padding: 1rem;
        color: #f4e4c1;

        &.loading-cell,
        &.empty-cell {
          text-align: center;
          padding: 3rem;
          color: rgba(244, 228, 193, 0.7);
          cursor: default;
        }

        &.actions-cell {
          display: flex;
          gap: 0.5rem;
          justify-content: center;
        }
      }
    }
  }
}

.action-button {
  padding: 0.5rem 0.7rem;
  background: transparent;
  border: 1px solid #8b4513;
  border-radius: 6px;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.2s ease;

  &:hover {
    transform: scale(1.1);
  }

  &.view {
    &:hover {
      background: #4169e1;
      border-color: #4169e1;
    }
  }

  &.edit {
    &:hover {
      background: #ffa500;
      border-color: #ffa500;
    }
  }

  &.delete {
    &:hover {
      background: #dc143c;
      border-color: #dc143c;
    }
  }
}

.table-footer {
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
  background: #1a0f0a;
  border-top: 2px solid #8b4513;
}

.pagination-info {
  color: #f4e4c1;
  font-size: 0.9rem;
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 0.5rem;

  .page-btn {
    padding: 0.5rem 1rem;
    background: #8b4513;
    color: #fff;
    border: 2px solid #ffd700;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 600;
    transition: all 0.3s ease;

    &:hover:not(:disabled) {
      background: #a0522d;
    }

    &:disabled {
      opacity: 0.4;
      cursor: not-allowed;
    }
  }

  .page-numbers {
    display: flex;
    gap: 0.3rem;

    .page-number {
      width: 36px;
      height: 36px;
      background: #2c1810;
      color: #f4e4c1;
      border: 1px solid #8b4513;
      border-radius: 6px;
      cursor: pointer;
      transition: all 0.2s ease;

      &:hover {
        border-color: #ffd700;
        color: #ffd700;
      }

      &.active {
        background: #8b4513;
        color: #ffd700;
        border-color: #ffd700;
      }
    }
  }
}

.page-size-control {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #f4e4c1;
  font-size: 0.9rem;

  .page-size-select {
    padding: 0.5rem;
    background: #2c1810;
    border: 2px solid #8b4513;
    border-radius: 6px;
    color: #f4e4c1;
    cursor: pointer;

    &:focus {
      outline: none;
      border-color: #ffd700;
    }
  }
}

@media (max-width: 768px) {
  .table-header {
    flex-direction: column;
    align-items: stretch;
  }

  .table-footer {
    flex-direction: column;
    align-items: stretch;
  }

  .pagination-controls {
    justify-content: center;
  }
}
</style>