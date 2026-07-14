import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnomalyDetection } from './anomaly-detection';

describe('AnomalyDetection', () => {
  let component: AnomalyDetection;
  let fixture: ComponentFixture<AnomalyDetection>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnomalyDetection],
    }).compileComponents();

    fixture = TestBed.createComponent(AnomalyDetection);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
