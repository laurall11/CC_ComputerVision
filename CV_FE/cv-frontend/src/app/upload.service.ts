import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  constructor(private http: HttpClient) { }

  upload(fileToUpload: File): Observable<any> {
    const endpoint = 'https://localhost:44336/api/analyzeImage';
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post(endpoint, formData, {responseType: "arraybuffer"});
  }
}
