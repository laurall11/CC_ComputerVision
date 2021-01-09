import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'image/*'
  })
};

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  constructor(private http: HttpClient) { }

  upload(fileToUpload: File): Observable<File> {
    const endpoint = 'https://localhost:44336/api/analyzeImage';
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post<File>(endpoint, formData, httpOptions);
  }
}
