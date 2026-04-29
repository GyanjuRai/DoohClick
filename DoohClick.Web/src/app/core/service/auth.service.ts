import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AppConst } from '../../app-const';
import { JwtHelperService } from '@auth0/angular-jwt';
import { EncryptDecryptService } from './encrypt-dcrypt.service';
import { MvLoginResponse } from '../model/account.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private storageKey: string;
  jwtHelper: JwtHelperService = new JwtHelperService();

  constructor(
    private http: HttpClient,
    private router: Router,
    private encrypDecrypService: EncryptDecryptService,
  ) {
    this.storageKey = AppConst.data?.storageKey;
  }

  setSession(response: MvLoginResponse) {
    const user = this.jwtHelper.decodeToken(response.accessToken);
    this.setLocalStorage('accessToken', response.accessToken);
    this.setLocalStorage('userId', user['UserId']);
    this.setLocalStorage('tenantId', user['TenantId']);
    this.setLocalStorage('tenantCode', user['TenantCode']);
    this.setLocalStorage('userUuid', user['UserUuId']);
    this.setLocalStorage('fullName', user['FullName']);
    this.setLocalStorage('userRole', user['UserRole']);
    this.setLocalStorage('email', user['Email']);
  }

  setLocalStorage(key: string, value: any): void {
    const data = localStorage.getItem(this.storageKey);
    if (data) {
      let dataDcrypted = this.encrypDecrypService.decrypt(data);
      let dataJson = JSON.parse(dataDcrypted ?? {});
      dataJson = Object.assign({}, dataJson, { [key]: value });
      localStorage.setItem(
        this.storageKey,
        this.encrypDecrypService.encrypt(JSON.stringify(dataJson)),
      );
    } else {
      let dataNew = Object.assign({}, { [key]: value });
      localStorage.setItem(
        this.storageKey,
        this.encrypDecrypService.encrypt(JSON.stringify(dataNew)),
      );
    }
  }

  getLocalStorage(key: string): any {
    const data = localStorage.getItem(this.storageKey);
    if (data) {
      let dataDcrypted = this.encrypDecrypService.decrypt(data);
      let dataJson = JSON.parse(dataDcrypted ?? {});
      return dataJson[key] || null;
    }
    return null;
  }

  isAuthenticated(): boolean {
    const token = this.getLocalStorage('accessToken');
    if (token) {
      return true;
    }
    return false;
  }

  isTokenExpired(): boolean {
    const token = (this.getLocalStorage('accessToken') ?? '') as string;
    return token ? this.jwtHelper.isTokenExpired(token) : true;
  }

  navigate(route: string[]): void {
    this.router.navigate(route);
  }

  getAccessToken() {
    return this.getLocalStorage('accessToken');
  }

  getRefreshToken() {
    return this.getLocalStorage('accessToken');
  }

  getUserId() {
    return Number(this.getLocalStorage('userId'));
  }

  getTenantCode() {
    return this.getLocalStorage('tenantCode');
  }

  getUserFullName() {
    return this.getLocalStorage('fullName');
  }

  getUserRole() {
    return this.getLocalStorage('userRole');
  }

  getUserEmail() {
    return this.getLocalStorage('email');
  }

  clearAuth() {
    // this.clearSession();
    this.clearStorage();
  }

  //   private clearSession() {
  //     sessionStorage.clear();
  //   }

  private clearStorage() {
    localStorage.clear();
  }
}
