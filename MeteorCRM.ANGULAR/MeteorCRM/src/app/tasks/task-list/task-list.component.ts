import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../shared/services/task.service';
import { Task } from '../../shared/models/task.models';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  isLoading = false;

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.isLoading = true;
    this.taskService.getAll().subscribe({
      next: (data) => {
        this.tasks = data;
        this.isLoading = false;
      },
      error: () => this.isLoading = false
    });
  }

  deleteTask(id: string): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.delete(id).subscribe({
        next: () => this.loadTasks()
      });
    }
  }

  toggleComplete(task: Task): void {
    this.taskService.update(task.id, {
      title: task.title,
      description: task.description,
      dueDate: task.dueDate,
      priority: ['Low', 'Medium', 'High', 'Critical'].indexOf(task.priority) + 1,
      isCompleted: !task.isCompleted,
      customerId: task.customerId
    }).subscribe({
      next: () => this.loadTasks()
    });
  }
}