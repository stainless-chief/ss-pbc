import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ResourcesService } from 'src/app/core/resources.service';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss'],
})
export class PaginatorComponent {
  @Input()
  public maxPage: number;

  @Input()
  public minPage: number;

  @Input()
  public currentPage: number;

  @Output()
  public changePage: EventEmitter<number> = new EventEmitter<number>();

  @Output()
  public nextPageClick: EventEmitter<void> = new EventEmitter();

  @Output()
  public previousPageClick: EventEmitter<void> = new EventEmitter();

  constructor(public textMessages: ResourcesService) {}

  public changePageEnterPress(pageNumber: number) {
    this.changePage.emit(pageNumber);
  }

  public btnNextClick() {
    this.nextPageClick.emit();
  }

  public btnPreviousClick() {
    this.previousPageClick.emit();
  }
}