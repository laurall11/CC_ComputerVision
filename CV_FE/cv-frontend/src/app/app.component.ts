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

onFileUpload(files: Event){

  //remove all previous elements and stop previous audio
  var description : HTMLParagraphElement = document.getElementById('description') as HTMLParagraphElement;
  description.innerHTML = "";

  var image : HTMLImageElement = document.getElementById('img-prev') as HTMLImageElement;
  image.src = "";

  //get div with audio player in it and remove old audio player
  var audiodiv : HTMLDivElement = document.getElementById('audioDiv') as HTMLDivElement;
  if (audiodiv.hasChildNodes()) {
    var lastAudio: HTMLAudioElement = audiodiv.lastChild as HTMLAudioElement;
    lastAudio.pause();
    lastAudio.remove();
  }

  
  //get file out of filearray
  this.fileToUpload = files.target;
  this.fileToUpload = this.fileToUpload.files.item(0);
  
  //check if image is too big
  if (this.fileToUpload.size < 4194304 || this.fileToUpload < 1) {
    
    //show loading text
    var loadingNotif: HTMLParagraphElement = document.getElementById('loadingNotif') as HTMLParagraphElement;
    loadingNotif.setAttribute('style', 'font-size: 14pt;');
    loadingNotif.textContent = "Ihr Bild wird analysiert...";
    
    //show uploaded image immediately
    var reader = new FileReader();
    
    reader.onload = function (e) {
      var img = document.getElementById('img-prev');
      img?.setAttribute('src', e.target?.result as string);
      img?.setAttribute('style', 'display:none;');
    }
    
    reader.readAsDataURL(this.fileToUpload);
    
    //send image to backend
    this.uploadService
    .upload(this.fileToUpload)
    .subscribe(result => {
      
      //get created audio
      this.getAudio();
    });
  } else {
        //show loading text
        var loadingNotif: HTMLParagraphElement = document.getElementById('loadingNotif') as HTMLParagraphElement;
        loadingNotif.setAttribute('style', 'font-size: 14pt;');
        loadingNotif.textContent = "Ihr Bild ist zu groß...";
  }
  }
  
  
  getAudio() {
    
    this.uploadService
    .downloadDescrition()
    .subscribe(resultDescription => {
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


        reader.readAsArrayBuffer(getResult);
        reader.onload = function () {
          var buffer = reader.result as ArrayBuffer;
          audioCtxx.decodeAudioData(buffer, function (buff) {
            source.buffer = buff;
            source.connect(audioCtxx.destination);

            //create new audio element
            var audiodiv: HTMLDivElement = document.getElementById('audioDiv') as HTMLDivElement;

            var audioPlayer = document.createElement('audio');
            audioPlayer.controls = true;
            audioPlayer.autoplay = true;

            //create src element
            const url = window.URL.createObjectURL(getResult);

            var sourcee = document.createElement('source');
            sourcee.src = url;

            audioPlayer?.appendChild(sourcee);
            audiodiv?.appendChild(audioPlayer);
            var loadingNotif : HTMLParagraphElement = document.getElementById('loadingNotif') as HTMLParagraphElement;
            loadingNotif.setAttribute('style', 'font-size: 14pt;');
            loadingNotif.textContent = "Klicken Sie auf die drei Punkte, um das Audio herunter zu laden!";
          });
        }
      });
  }
  }
