import { Injectable, OnInit, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { Message } from '../models/message.model';

@Injectable()
export class MessagesService {

  private hubConnection: HubConnection;
  messageReceived = new EventEmitter<Message>();  

  private connectionIsEstablished = false;

  private endpoints = {
    getMessages: '/api/messages',
    createMessage: '/api/messages/create'
  }
  constructor(private http: HttpClient) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  getMessages() {
    return this.http.get(this.endpoints.getMessages);
  }

  createMessage(text: string) {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Cache-Control': 'no-cache'
    });
    let options = {
      headers: httpHeaders
    };
    return this.http.post(this.endpoints.createMessage, JSON.stringify(text), options);
  }

  private createConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44330/message')
      .build();
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('MessageReceived', (data: any) => {
      this.messageReceived.emit(data);  
    });
  }

  private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        console.error(err);
        setTimeout(function () { this.startConnection(); }, 5000);  
      });
  }

}
