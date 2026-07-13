import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Grants } from './grants';

describe('Grants', () => {
  let component: Grants;
  let fixture: ComponentFixture<Grants>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Grants],
    }).compileComponents();

    fixture = TestBed.createComponent(Grants);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
