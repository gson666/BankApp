import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccountCreationComponent } from './admin-account-creation.component';

describe('AdminAccountCreationComponent', () => {
  let component: AdminAccountCreationComponent;
  let fixture: ComponentFixture<AdminAccountCreationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminAccountCreationComponent]
    });
    fixture = TestBed.createComponent(AdminAccountCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
