import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { RequestResult } from '../model/RequestResult';
import { Project } from '../model/Project';
import { Category } from '../model/Category';
import { Account } from '../model/Account';

import { environment } from 'src/environments/environment';
import { ProjectPreview } from '../model/ProjectPreview';
import { Introduction } from '../model/Introduction';

@Injectable()
export class DataService
{
  private httpClient: HttpClient;
  private endpoint = environment.apiEndpoint;

  public constructor(http: HttpClient)
  {
    this.httpClient = http;
  }

  public getIntroduction(): Promise<RequestResult<Introduction>>
  {
    return this.httpClient
               .get<RequestResult<Introduction>>(this.endpoint + 'introduction')
               .toPromise();
  }

  public getCategories(): Promise<RequestResult<Array<Category>>>
  {
    return this.httpClient
               .get<RequestResult<Array<Category>>>(this.endpoint + 'categories')
               .toPromise();
  }

  public getEverythingCategory(): Promise<RequestResult<Category>>
  {
    return this.httpClient
               .get<RequestResult<Category>>(this.endpoint + 'categories/everything')
               .toPromise();
  }

  public getCategory(id: number): Promise<RequestResult<Category>>
  {
    return this.httpClient
               .get<RequestResult<Category>>(this.endpoint + 'categories/' + id)
               .toPromise();
  }

  public getProject(code: string): Promise<RequestResult<Project>>
  {
    // well, it's way easy and faster to parse code from url, so it will be this way
    return this.httpClient
               .get<RequestResult<Project>>(this.endpoint + 'projects/' + code)
               .toPromise();
  }

  public getProjectsPreview(start: number, length: number, categoryCode: string): Promise<RequestResult<Array<ProjectPreview>>>
  {
    const categoryParam = typeof categoryCode !== 'undefined' && categoryCode ? '&categorycode=' + categoryCode : '';

    return this.httpClient
               .get<RequestResult<Array<ProjectPreview>>>
               (
                 this.endpoint + 'projects/search?'
                 + 'start=' + start
                 + '&length=' + length
                 + categoryParam
               )
               .toPromise();
  }

// --------------------------------------------------------------------

  public saveAccount(account: Account): Promise<RequestResult<Account>>
  {
    if (account.id)
    {
      return this.updateAccount(account);
    }
    else
    {
      return this.createAccount(account);
    }
  }

  private createAccount(account: Account): Promise<RequestResult<Account>>
  {
    return this.httpClient
               .post<RequestResult<Account>>(this.endpoint + 'accounts', account)
               .toPromise();
  }

  private updateAccount(account: Account): Promise<RequestResult<Account>>
  {
    return this.httpClient
               .patch<RequestResult<Account>>(this.endpoint + 'accounts', account)
               .toPromise();
  }

  public deleteAccount(account: Account): Promise<RequestResult<any>>
  {
    return this.httpClient
               .request<RequestResult<boolean>>('delete', this.endpoint + 'accounts', { body: account,})
               .toPromise();
  }

// --------------------------------------------------------------------






  public updateIntroduction(introdcution: Introduction): Promise<RequestResult<Introduction>> {
    return this.httpClient
      .request<RequestResult<Introduction>>('patch', this.endpoint + 'introduction', {
        body: introdcution,
      })
      .toPromise();
  }











  public save(category: Category): Promise<RequestResult<any>> {
    return this.httpClient
      .post<RequestResult<any>>(this.endpoint + 'category', category)
      .toPromise();
  }

  public delete(category: Category): Promise<RequestResult<any>> {
    return this.httpClient
      .request<RequestResult<any>>('delete', this.endpoint + 'category', {
        body: category,
      })

      .toPromise();
  }

  public saveProject(project: Project): Promise<RequestResult<any>> {
    return this.httpClient
      .post<RequestResult<any>>(this.endpoint + 'project', project)
      .toPromise();
  }

  public deleteProject(project: Project): Promise<RequestResult<any>> {
    return this.httpClient
      .request<RequestResult<any>>('delete', this.endpoint + 'project', {
        body: project,
      })

      .toPromise();
  }


  public uploadFile(fileToUpload: File): Promise<RequestResult<string>>{
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.httpClient.post<RequestResult<string>>(this.endpoint + 'upload', formData).toPromise();
  }


  public countAccount(): Promise<RequestResult<number>> {
    return this.httpClient
      .get<RequestResult<number>>(this.endpoint + 'accounts')
      .toPromise();
  }

  public getAccount(id: number): Promise<RequestResult<Account>>
  {
    return this.httpClient
    .get<RequestResult<Account>>(
      this.endpoint +
        'accounts/' + id
    )
    .toPromise();
  }

  public getAccounts(start: number, length: number): Promise<RequestResult<Account[]>>
  {
    return this.httpClient
    .get<RequestResult<Account[]>>(
      this.endpoint +
        'accounts/search?' +
        'start=' +
        start +
        '&length=' +
        length
    )
    .toPromise();
  }



}
