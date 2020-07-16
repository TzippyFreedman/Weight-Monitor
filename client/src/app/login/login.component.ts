import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {  FormGroup, Validators, FormControl } from '@angular/forms';
import { IAuthenticater } from '../shared/models/IAuthenticater';
import { UserService } from '../shared/services/user.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
     
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    userToAuthenticate = <IAuthenticater>{};

    constructor(private router:Router,private http:UserService)  { }
  
    
  ngOnInit(): void {
    this.loginForm = new FormGroup({
      'emailAddress': new FormControl('',[
      Validators.required,
      Validators.minLength(2),
      Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")


    ]),
    'password': new FormControl('',[
      Validators.required,
      Validators.minLength(6)
    ])
  });
debugger;
 
  }
  get formControls() { return this.loginForm.controls; }

  onSubmit(){
    
    this.submitted = true;
        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }
        this.loading = true;
        this.userToAuthenticate.password=this.formControls.password.value;
        this.userToAuthenticate.email= this.formControls.emailAddress.value;
        this.http.login(this.userToAuthenticate)
            .subscribe(
                result => {
                    this.router.navigate([environment.userURL, result]);
                },
                error => {
                    alert(error);
                    this.loading = false;
                });
    }
}

