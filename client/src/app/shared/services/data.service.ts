import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { IMeasure } from '../models/IMeasure';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';


const ADD_MEASURE_URL='measure';
@Injectable({
  providedIn: 'root'
})
export class DataService {constructor(private http:HttpClient) { }
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
public addMeasure = (body:IMeasure) => {
debugger;
  return this.http.post<string>(this.createCompleteRoute(ADD_MEASURE_URL, environment.baseUrl),body)
    .pipe(catchError(this.handleError));

}

private createCompleteRoute = (route: string, envAddress: string) => {
  return `${envAddress}/${route}`;
}



}
