import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLibraryDetails } from './game-library-details';

describe('GameLibraryDetails', () => {
  let component: GameLibraryDetails;
  let fixture: ComponentFixture<GameLibraryDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameLibraryDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameLibraryDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
