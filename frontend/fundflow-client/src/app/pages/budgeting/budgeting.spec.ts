import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Budgeting } from './budgeting';

describe('Budgeting', () => {
  let component: Budgeting;
  let fixture: ComponentFixture<Budgeting>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Budgeting],
    }).compileComponents();

    fixture = TestBed.createComponent(Budgeting);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
