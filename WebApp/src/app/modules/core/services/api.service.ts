import { Injectable } from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable()
export class ApiService {

  private baseApiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {
  }

  get(path: string, params: HttpParams = new HttpParams()): Observable<any> {
    const url = this.baseApiUrl + path;
    return this.httpClient.get<any>(url, {params});
  }

  post(path: string, body: any = new HttpParams()): Observable<any> {
    const url = this.baseApiUrl + path;
    return this.httpClient.post<any>(url, body);
  }

  put(path: string, body: any = new HttpParams()): Observable<any> {
    const url = this.baseApiUrl + path;
    return this.httpClient.put<any>(url, body);
  }

  delete(path: string): Observable<void> {
    const url = this.baseApiUrl + path;
    return this.httpClient.delete<void>(url);
  }
}
