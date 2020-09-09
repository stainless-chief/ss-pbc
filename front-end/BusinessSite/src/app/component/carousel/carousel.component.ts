import { Component, Input, OnInit } from '@angular/core';
import { StaticNames } from 'src/app/common/StaticNames';
import { MessageDescription, MessageType } from '../message/message.component';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.scss'],
})

export class CarouselComponent implements OnInit
{
  @Input()
  public images: string[];

  public selectedImage: string;
  public currentImageIndex: number = 0;

  public loading: boolean = true;
  public message: MessageDescription = {text: StaticNames.LoadInProgress, type: MessageType.Spinner };


  public ngOnInit(): void
  {
    this.currentImageIndex = 0;
    this.selectedImage = this.images[this.currentImageIndex];
  }

  public onLoad(): void
  {
    this.loading = false;
  }

   public changeImage(value: number): void
   {
     this.currentImageIndex = this.currentImageIndex + value;

     if (this.currentImageIndex === -1)
     {
       this.currentImageIndex = this.images.length - 1;
     }
     if (this.currentImageIndex === this.images.length)
     {
       this.currentImageIndex = 0;
     }

     this.loading = true;
     this.selectedImage = this.images[this.currentImageIndex];
   }
}
