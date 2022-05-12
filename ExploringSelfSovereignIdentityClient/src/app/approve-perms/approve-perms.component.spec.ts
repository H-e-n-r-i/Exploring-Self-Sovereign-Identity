import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovePermsComponent } from './approve-perms.component';

describe('ApprovePermsComponent', () => {
  let component: ApprovePermsComponent;
  let fixture: ComponentFixture<ApprovePermsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApprovePermsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovePermsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
