import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { IAuthenticater } from '../models/IAuthenticater';
import {  IUser } from '../models/IUser';
import { environment } from 'src/environments/environment';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';


const LOGIN_URL='user/login';
const REGISTER_URL='user/register';
@Injectable({

    providedIn: 'root',
  })
  export class UserService {
    constructor(private http:HttpClient) { }
    private handleError(error: HttpErrorResponse) {
      if (error.error.message  ) {
        // A client-side or network error occurred. Handle it accordingly.
        return throwError(new Error(`An error occurred:${error.error.message}`) );
      } 
      // if( error.error.errorMessage){
      //   return throwError(new Error(`An error occurred:${error.error.errorMessage} `));

      // }
      else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong,
        console.error(
          `Backend returned code ${error.status}, ` +
          `body was: ${error.error}`);
      }
      return throwError(
        'Something bad happened; please try again later.');
    };
    public login = (body:IAuthenticater) => {
    
      return this.http.post<string>(this.createCompleteRoute(LOGIN_URL, environment.baseUrl),body)
        .pipe(catchError(this.handleError));
    
    }
    public register = (user:IUser) => {
      return this.http.post<boolean>(this.createCompleteRoute(REGISTER_URL, environment.baseUrl),user)
        .pipe(catchError(this.handleError));
  
    }
    private createCompleteRoute = (route: string, envAddress: string) => {
      return `${envAddress}/${route}`;
    }
   
   
   
  }
