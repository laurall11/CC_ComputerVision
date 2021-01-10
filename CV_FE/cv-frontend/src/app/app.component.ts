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
  audioCtx = new window.AudioContext();
  context = new AudioContext();

onFileUpload(files: Event){
  this.fileToUpload = files.target;
  this.fileToUpload = this.fileToUpload.files.item(0);
  this.uploadService
    .upload(this.fileToUpload)
    .subscribe(result => {

      result.arrayBuffer().then((resultat: ArrayBuffer) =>{

        this.audioCtx.decodeAudioData(resultat).then(function (decodedData) {
          console.log(decodedData);
        });

      });

      this.resultFiles.push(result)});
}

}
