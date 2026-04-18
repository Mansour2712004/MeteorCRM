import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  user: any;
  today = new Date();

  constructor() {}

  ngOnInit(): void {
    const userData = localStorage.getItem('user');
    this.user = userData ? JSON.parse(userData) : null;
  }
}