import { Component, OnInit } from '@angular/core';
import { MessagesService } from 'src/app/services/messages.service';
import { Message } from 'src/app/models/message.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public messages: Message[] | undefined;
  public text: string = '';

  constructor(private messagesService: MessagesService) {
    this.messagesService.messageReceived.subscribe(
      response => {
        if (this.messages) {
          this.messages.unshift(response);
        }
      }
    );
  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService.getMessages()
      .subscribe(response => {
        this.messages = response as Message[];
      }, error => {
        console.error(error);
      });
  }

  createMessage() {
    this.messagesService.createMessage(this.text)
      .subscribe(response => { 
        if (response) {
          this.text = '';
        }
      }, 
      error => { 
        console.error(error); 
      });
  }

}
