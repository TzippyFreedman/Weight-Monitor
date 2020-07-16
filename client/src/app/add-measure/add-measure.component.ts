import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidationErrors,ValidatorFn, AbstractControl } from '@angular/forms';
import { IMeasure } from '../shared/models/IMeasure';
import { Router } from '@angular/router';
import { UserService } from '../shared/services/user.service';
import { environment } from 'src/environments/environment';
import { DataService } from '../shared/services/data.service';


@Component({
  selector: 'app-add-measure',
  templateUrl: './add-measure.component.html',
  styleUrls: ['./add-measure.component.css']
})
export class AddMeasureComponent implements OnInit {
  MIN_WEIGHT: number = 6;
  MAX_WEIGHT: number= 200;
  measureForm: FormGroup;
  loading = false;
  submitted = false;
  newMeasure = <IMeasure>{};

  constructor(private router:Router,private http:DataService)  { }

  
ngOnInit(): void {
  this.measureForm = new FormGroup({
    'weight': new FormControl('',[
    Validators.required, 
    Validators.min(1),
    this.weightValidator()])
});
}

weightValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    debugger;
const weight = control.value;
 return (weight< this.MIN_WEIGHT || weight> this.MAX_WEIGHT) ? { 'forbiddenWeight': true }: null;
  }
}

// weightValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
//   debugger;
//   //const weight = control.get('weight').value;
//  // return (weight< this.MIN_WEIGHT || weight> this.MAX_WEIGHT) ? { 'forbiddenWeight': true } : null;
//  return null;
// };



get formControls() { return this.measureForm.controls; }
get weight() { return this.measureForm.get('weight'); }

onSubmit(){
  debugger;
  this.submitted = true;
      // stop here if form is invalid
      if (this.measureForm.invalid) {
          return;
      }
      this.loading = true;
      this.newMeasure.weight=this.formControls.weight.value;
      this.http.addMeasure(this.newMeasure)
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

