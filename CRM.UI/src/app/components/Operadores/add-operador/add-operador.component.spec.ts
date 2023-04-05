import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOperadorComponent } from './add-operador.component';

describe('AddOperadorComponent', () => {
  let component: AddOperadorComponent;
  let fixture: ComponentFixture<AddOperadorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddOperadorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddOperadorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
