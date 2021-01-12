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

  var reader = new FileReader();

  reader.onload = function (e) {
    var img = document.getElementById('img-prev');
    img?.setAttribute('src', e.target?.result as string);
    img?.setAttribute('style', 'max-height: 500px; max-width: 500px;');
  }

  reader.readAsDataURL(this.fileToUpload);

  this.uploadService
    .upload(this.fileToUpload)
    .subscribe(result => {
      this.getAudio();
    });
    }

  getAudio() {
    this.uploadService
      .downloadAudio()
      .subscribe(getResult => {
        let reader = new FileReader();
        let audioCtxx = new window.AudioContext();
        var source = audioCtxx.createBufferSource();

        reader.readAsArrayBuffer(getResult);
        reader.onload = function () {
          console.log(reader.result);
          var bufferr = reader.result as ArrayBuffer;
          audioCtxx.decodeAudioData(bufferr, function (buff) {
            source.buffer = buff;
            source.connect(audioCtxx.destination);
            source.start(0);
          });
        }
      });
  }
  }
