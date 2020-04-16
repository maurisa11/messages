import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class MessagesService {

    private endpoints = {
        getMessages: '/api/messages',
        createMessage: '/api/messages/create'
    }
    constructor(private http: HttpClient) {
    }

    getMessages() {
        console.log('service getMessages');
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


}
