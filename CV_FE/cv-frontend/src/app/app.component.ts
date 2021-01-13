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

  document.getElementById('loadingNotif')?.setAttribute('style', 'display: initial');

  this.fileToUpload = files.target;
  this.fileToUpload = this.fileToUpload.files.item(0);

  //shoe image immediately
  var reader = new FileReader();

  reader.onload = function (e) {
    var img = document.getElementById('img-prev');
    img?.setAttribute('src', e.target?.result as string);
    img?.setAttribute('style', 'display:none;');
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
    .downloadDescrition()
    .subscribe(resultDescription => {
      console.log(resultDescription);

        var textField = document.getElementById("description");
        if (textField != null)
        textField.innerText = resultDescription;
        var img = document.getElementById('img-prev');
      img?.setAttribute('style', 'display: initial; max-height: 500px; max-width: 500px;');

    });

    this.uploadService
      .downloadAudio()
      .subscribe(getResult => {
        let reader = new FileReader();
        let audioCtxx = new window.AudioContext();
        var source = audioCtxx.createBufferSource();
        var audio = document.getElementById('audioPlayer');
        var audioplayer = document.getElementById('audioSrc');

        reader.readAsArrayBuffer(getResult);
        reader.onload = function () {
          console.log(reader.result);
          var buffer = reader.result as ArrayBuffer;
          audioCtxx.decodeAudioData(buffer, function (buff) {
            source.buffer = buff;
            source.connect(audioCtxx.destination);
            console.log(audioplayer);

            //create src element
            const url = window.URL.createObjectURL(getResult);

            var sourcee = document.createElement('source');
            sourcee.src = url;

            audio?.appendChild(sourcee);
            audio?.setAttribute('style', 'display: initial;');
            document.getElementById('loadingNotif')?.setAttribute('style', 'display: none');
          });
        }
      });
  }
  }
