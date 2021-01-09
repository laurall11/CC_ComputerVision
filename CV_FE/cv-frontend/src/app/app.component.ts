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

onFileUpload(files: Event){
  this.fileToUpload = files.target;
  this.fileToUpload = this.fileToUpload.files.item(0);
  console.log(this.fileToUpload);
  this.uploadService
    .upload(this.fileToUpload)
    .subscribe(result => this.resultFiles.push(result));
}

}
