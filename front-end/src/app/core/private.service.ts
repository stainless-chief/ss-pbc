import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Account } from '../admin/account.model';
import { Category } from '../shared/category.model';
import { Information } from '../admin/information.model';
import { Introduction } from '../introduction/introduction.model';
import { Project } from '../shared/project.model';
import { RequestResult } from '../shared/request-result.model';
import { environment } from 'src/environments/environment';
import { StorageService } from './storage.service';
import { DatePipe } from '@angular/common';

@Injectable()
export class PrivateService {
  private endpoint = environment.apiEndpoint;

  public constructor(
    private httpClient: HttpClient,
    private storage: StorageService,
    private datepipe: DatePipe
  ) {}

  public getInformation(): Promise<RequestResult<Information>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Token: this.storage.getToken(),
    });

    return this.httpClient
      .get<RequestResult<Information>>(this.endpoint + 'information', {
        headers,
      })
      .toPromise();
  }

  public saveIntroduction(
    introduction: Introduction
  ): Promise<RequestResult<Introduction>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    const formData = new FormData();
    this.fillFormData(formData, 'introduction', introduction);

    return this.httpClient
      .post<RequestResult<Introduction>>(
        this.endpoint + 'introduction',
        formData,
        { headers }
      )
      .toPromise();
  }

  public saveCategory(category: Category): Promise<RequestResult<Category>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .post<RequestResult<Category>>(this.endpoint + 'category', category, {
        headers,
      })
      .toPromise();
  }

  public deleteCategory(category: Category): Promise<RequestResult<boolean>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .request<RequestResult<boolean>>('delete', this.endpoint + 'category', {
        body: category,
        headers,
      })
      .toPromise();
  }

  public countAccount(): Promise<RequestResult<number>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .get<RequestResult<number>>(this.endpoint + 'accounts', { headers })
      .toPromise();
  }

  public getAccount(id: number): Promise<RequestResult<Account>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .get<RequestResult<Account>>(this.endpoint + 'accounts/' + id, {
        headers,
      })
      .toPromise();
  }

  public getRoles(): Promise<RequestResult<string[]>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .get<RequestResult<string[]>>(this.endpoint + 'roles', { headers })
      .toPromise();
  }

  public getAccounts(
    start: number,
    length: number
  ): Promise<RequestResult<Account[]>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .get<RequestResult<Account[]>>(
        this.endpoint + 'accounts/search?start=' + start + '&length=' + length,
        { headers }
      )
      .toPromise();
  }

  public saveAccount(account: Account): Promise<RequestResult<Account>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .post<RequestResult<Account>>(this.endpoint + 'accounts', account, {
        headers,
      })
      .toPromise();
  }

  public deleteAccount(account: Account): Promise<RequestResult<boolean>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .request<RequestResult<boolean>>('delete', this.endpoint + 'accounts', {
        body: account,
        headers,
      })
      .toPromise();
  }

  public deleteProject(project: Project): Promise<RequestResult<boolean>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    return this.httpClient
      .request<RequestResult<boolean>>('delete', this.endpoint + 'project', {
        body: project,
        headers,
      })
      .toPromise();
  }

  public saveProject(project: Project): Promise<RequestResult<Project>> {
    const headers = new HttpHeaders({ Token: this.storage.getToken() });

    const formData = new FormData();
    this.fillFormData(formData, 'project', project);

    return this.httpClient
      .post<RequestResult<Project>>(this.endpoint + 'project', formData, {
        headers,
      })
      .toPromise();
  }

  // Usage: this.fillFormData(formData, 'project', project);
  private fillFormData(form: FormData, namespace: string, item: any): void {
    for (const property in item) {
      if (typeof item[property] === 'object') {
        if (item[property] instanceof Date) {
          form.append(
            namespace + `[${property}]`,
            this.datepipe.transform(item[property], 'yyyy/MM/dd')
          );
        } else if (Array.isArray(item[property])) {
          let index = 0;
          item[property].forEach((element: any) => {
            this.fillFormData(
              form,
              `${namespace}[${property}][${index}]`,
              element
            );
            index++;
          });
        }
        // W3.org: 'A File object is a Blob object with a name attribute, which is a string;'
        else if (item[property] && typeof item[property].name === 'string') {
          form.append(
            `${namespace}[${property}]`,
            item[property],
            item[property].name
          );
        } else {
          this.fillFormData(form, namespace + `[${property}]`, item[property]);
        }
      } else {
        form.append(namespace + `[${property}]`, item[property]);
      }
    }
  }
}