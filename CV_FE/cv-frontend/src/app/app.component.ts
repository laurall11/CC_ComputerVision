import { Component } from '@angular/core';
import { UploadService} from './upload.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private uploadService: UploadService) { }

  title = 'cv-frontend';
  fileToUpload: any = null;
  resultFiles : any[] = [];

onFileUpload(evFiles: Event){
  this.fileToUpload = evFiles.target;
  this.fileToUpload = this.fileToUpload.item(0);
  this.uploadService
    .upload(this.fileToUpload)
    .subscribe(result => this.resultFiles.push(result));
}

}
