import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { first } from 'rxjs/operators';
import { IUser } from '../shared/models/IUser';
import { UserService } from '../shared/services/user.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  loading = false;
  submitted = false;
  userToRegister=<IUser>{};

  constructor(
      private router: Router,
      private userService:UserService 

    
  ) {

      // redirect to home if already logged in
      // if (this.authenticationService.currentUserValue) {
      //     this.router.navigate(['/']);
      // }
  }



  ngOnInit(): void {

    this.registerForm = new FormGroup({
      
      
        'email': new FormControl('',[
          Validators.required,
          Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")
        ]),
          'password': new FormControl('',[
            Validators.required,
             Validators.minLength(6)]),
            'firstName': new FormControl('',[
              Validators.required]),
              'lastName': new FormControl('',[
               Validators.required]),
               'age': new FormControl('',[
                Validators.required])

  });
  }

  get formControls() { return this.registerForm.controls; }

  
  onSubmit() {
      this.submitted = true;

    

      // stop here if form is invalid
      if (this.registerForm.invalid) {
          return;
      }

      this.loading = true;
   
     this.userToRegister=this.registerForm.value;


      this.userService.register(this.userToRegister)
          .pipe(first())
          .subscribe(
              data => {
                  //this.alertService.success('Registration successful', true);
                  alert("Registration completed. please login with your password and user name.")
                  this.router.navigate(['/login']);
              },
              error => {
                 // this.alertService.error(error);
                 alert(error)
                  this.loading = false;
              });
  }
}



