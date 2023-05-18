import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { NameDialogComponent } from 'src/app/shared/name-dialog/name-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  connection = new HubConnectionBuilder().withUrl("http://localhost:5153/chat").build();

  messages: Message[] = [];

  userName!: string;
  messageControl = new FormControl('');

  constructor(private dialogRef: MatDialog,
    private _snackBar: MatSnackBar) {
    this.openDialog();
  }

  ngOnInit(): void {
  }

  private openDialog() {
    const dialog = this.dialogRef.open(NameDialogComponent, {
      width: '350px',
      data: this.userName,
      disableClose: true
    });

    dialog.afterClosed().subscribe((result) => {
      this.userName = result;

      this.startConnection();
      this.openSnackBar(result);
    });
  }

  startConnection(): void {
    this.connection.on("newMessage", (userName: string, text: string) => {
      this.messages.push({
        text: text,
        userName: userName
      });
    });

    this.connection.on('newUser', (userNane: string) => {
        this.openSnackBar(userNane);
    });

    this.connection.on("previousMessages", (messages: Message[]) => {
      this.messages = messages;
    });

    this.connection.start().then(() => {
      this.connection.send('newUser', this.userName, this.connection.connectionId);
    });
  }

  openSnackBar(userName: string): void {
    const message = this.userName === userName ? 'Você acabou de entrar' : `${userName} acabou de entrar`
    this._snackBar.open(message, 'Fechar', { duration: 5000, horizontalPosition: 'right', verticalPosition: 'top' });
  }

  sendMessage(): void {
    this.connection.send('newMessage', this.userName, this.messageControl.value).then(() => {
      this.messageControl.setValue('');
    })
  }
}

interface Message {
  userName: string,
  text: string
}
