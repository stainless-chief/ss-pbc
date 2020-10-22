import { Component, Input } from '@angular/core';

import { Project } from 'src/app/shared/project.model';
import { ResourcesService } from 'src/app/core/resources.service';

@Component({
  selector: 'app-project-full',
  templateUrl: './project-full.component.html',
  styleUrls: ['./project-full.component.scss'],
})
export class ProjectFullComponent {
  @Input()
  public project: Project;

  constructor(public textMessages: ResourcesService) {}
}