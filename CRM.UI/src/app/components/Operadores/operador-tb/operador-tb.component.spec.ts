import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OperadorTbComponent } from './operador-tb.component';

describe('OperadorTbComponent', () => {
  let component: OperadorTbComponent;
  let fixture: ComponentFixture<OperadorTbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OperadorTbComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OperadorTbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
