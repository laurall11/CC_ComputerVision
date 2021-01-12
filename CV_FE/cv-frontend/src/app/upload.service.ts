import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  constructor(private http: HttpClient) { }

  upload(fileToUpload: File): Observable<any> {
    const endpoint = 'http://localhost:59089/api/analyzeImage';
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post(endpoint, formData);
  }

  downloadAudio(){
    return this.http.get('http://localhost:59089/api/getAudio', {responseType: "blob"});
  }

  downloadDescrition(){
    return this.http.get('http://localhost:59089/api/getDescription');
  }
}
