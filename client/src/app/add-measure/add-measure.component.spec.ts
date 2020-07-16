import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMeasureComponent } from './add-measure.component';

describe('AddMeasureComponent', () => {
  let component: AddMeasureComponent;
  let fixture: ComponentFixture<AddMeasureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMeasureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMeasureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
