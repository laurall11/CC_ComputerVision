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

  //immediately display image on page after selecting
  var reader = new FileReader();

  reader.onload = function (e) {
    var img = document.getElementById('img-prev');
    img?.setAttribute('src', e.target?.result as string);
    img?.setAttribute('style', 'max-height: 500px; max-width: 500px;');
  }

  reader.readAsDataURL(this.fileToUpload);

  console.log(this.fileToUpload);
  this.uploadService
    .upload(this.fileToUpload)
    .subscribe(result => {

      var audioCtx = new window.AudioContext();
        var hm = audioCtx.decodeAudioData(result).then(function (decodedData) {
          console.log(decodedData);
        });

      this.resultFiles.push(result)});
    }
    
    test(files) {
      
      let buffer = new ArrayBuffer(1210892);
      let audioCtx = new window.AudioContext();
      let context = new AudioContext();

      
      this.fileToUpload = files.target;
      this.fileToUpload = this.fileToUpload.files.item(0);

    
      let reader = new FileReader();
      
    
      reader.readAsArrayBuffer(this.fileToUpload);

      reader.onload = function() {
        console.log(reader.result);
        buffer = reader.result as ArrayBuffer;
        audioCtx.decodeAudioData(buffer).then(function (decodedData) {
          console.log(decodedData)
      });
      
      reader.onerror = function() {
        console.log(reader.error);
      };
      
    }
  }
  }