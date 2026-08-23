import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PortfolioDetail } from './portfolio-detail';

describe('PortfolioDetail', () => {
  let component: PortfolioDetail;
  let fixture: ComponentFixture<PortfolioDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PortfolioDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(PortfolioDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
