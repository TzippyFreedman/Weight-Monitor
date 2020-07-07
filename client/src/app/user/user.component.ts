import { Component, OnInit } from '@angular/core';
import { IUser } from '../shared/models/IUser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  user:IUser;
  userFileId:string;
  constructor(private route:ActivatedRoute) { }

  
  ngOnInit(): void {
    this.route.paramMap.subscribe( paramMap => {
      this.userFileId = paramMap.get('userFileId');

  });

}
}
