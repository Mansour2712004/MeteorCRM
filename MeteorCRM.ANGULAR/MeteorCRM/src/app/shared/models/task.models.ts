export interface Task {
  id: string;
  title: string;
  description?: string;
  dueDate?: string;
  priority: string;
  isCompleted: boolean;
  customerId?: string;
  customerName?: string;
  createdAt: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  dueDate?: string;
  priority: number;
  customerId?: string;
}

export interface UpdateTaskRequest extends CreateTaskRequest {
  isCompleted: boolean;
}