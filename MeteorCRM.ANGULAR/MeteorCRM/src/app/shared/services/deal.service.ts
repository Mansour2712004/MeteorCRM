import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Deal, CreateDealRequest, UpdateDealRequest } from '../models/deal.models';

@Injectable({
  providedIn: 'root'
})
export class DealService {
  private apiUrl = 'https://localhost:7058/api/Deals';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Deal[]> {
    return this.http.get<Deal[]>(this.apiUrl);
  }

  getById(id: string): Observable<Deal> {
    return this.http.get<Deal>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateDealRequest): Observable<Deal> {
    return this.http.post<Deal>(this.apiUrl, request);
  }

  update(id: string, request: UpdateDealRequest): Observable<Deal> {
    return this.http.put<Deal>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}