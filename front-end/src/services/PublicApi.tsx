import * as axios from "axios";
import { Introduction, ExecutionResult, Category, ProjectPreview } from "./";
import { Project } from "./models/Project";

class PublicApi {
  private static init() {
    return axios.default.create({
      baseURL: process.env.REACT_APP_API_PUBLIC_ENDPOINT,
      timeout: 31000,
      headers: {
        Accept: "application/json"
      }
    });
  };

  public static async getIntroduction() {
    return await this.init().get<ExecutionResult<Introduction>>("/introduction");
  };

  public static async getCategories() {
    return await this.init().get<ExecutionResult<Category[]>>("/categories");
  };

  public static async getCategory(id: number) {
    return await this.init().get<ExecutionResult<Category>>(`/category/${id}`);
  };

  public static async getProjects(page: number, categoryCode: string | null) {
    const length : number = parseInt(process.env.REACT_APP_PAGING_PROJECTS_MAX ?? "10");
    const start : number = length * (page - 1);

    const categoryParam = categoryCode !== null && categoryCode
      ? "&categorycode=" + categoryCode
      : "";

    return await this.init().get<ExecutionResult<ProjectPreview[]>>(
      "projects/search?"
      + "start=" + start
      + "&length=" + length
      + categoryParam);
  };

  public static async getProject(code: string | undefined) {
    return await this.init().get<ExecutionResult<Project>>("/projects/" + code);
  };
}

export { PublicApi };