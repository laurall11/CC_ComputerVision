import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

const httpOptions = {
  responseType: "arraybuffer" as const
};

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  constructor(private http: HttpClient) { }

  upload(fileToUpload: File): Observable<any> {
    const endpoint = 'http://localhost:59089/api/analyzeImage';
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post(endpoint, formData, httpOptions);
  }

  upload2(){
    return this.http.get('http://localhost:59089/api/AzureVision2', {responseType: "blob"});
  }
}
