import * as axios from "axios";
import { Credentials, Information, Introduction } from ".";
import { store } from "../storage";
import { ExecutionResult } from "./models/ExecutionResult";
import { Identity } from "./models/Identity";

class PrivateApi {
  private static init() {
    const identity = store.getState().identity.value;

    return axios.default.create({
      baseURL: process.env.REACT_APP_API_PRIVATE_ENDPOINT,
      timeout: 31000,
      headers: {
        Accept: "application/json",
        ContentType: "application/json",
        Authorization: `Bearer ${identity?.token}`
      }
    });
  };

  public static async login(credentials: Credentials) {
    return await this.init().post<ExecutionResult<Identity>>("/login", credentials);
  };

  public static async getInformation() {
    return await this.init().get<ExecutionResult<Information>>("/information");
  }

  public static async saveIntroduction(intro: Introduction) {
    return await this.init().post<ExecutionResult<Introduction>>("/introduction", intro);
  }
}

export { PrivateApi };