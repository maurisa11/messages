import { Component, OnInit } from '@angular/core';
import { MessagesService } from '../../services/messages.service';
import { Message } from '../../models/message.model';

@Component({
  selector: 'app-message-panel',
  templateUrl: './message-panel.component.html',
  styleUrls: ['./message-panel.component.scss']
})
export class MessagePanelComponent implements OnInit {
  
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

  public messages: Message[] | undefined;
  public text: string = '';




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
