import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLibraryNotes } from './game-library-notes';

describe('GameLibraryNotes', () => {
  let component: GameLibraryNotes;
  let fixture: ComponentFixture<GameLibraryNotes>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameLibraryNotes]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameLibraryNotes);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
