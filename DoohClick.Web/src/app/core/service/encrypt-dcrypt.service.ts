import * as CryptoJS from 'crypto-js';
import { AppConst } from '../../app-const';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class EncryptDecryptService {
  private secretKey: string;
  constructor() {
    this.secretKey = AppConst.data?.secretKey;
  }

  encrypt(value: string): any {
    if (value) {
      return CryptoJS.AES.encrypt(value, this.secretKey).toString();
    } else {
      return null;
    }
  }

  decrypt(textToDecrypt: string): any {
    if (textToDecrypt) {
      const bytes = CryptoJS.AES.decrypt(textToDecrypt, this.secretKey);
      return bytes.toString(CryptoJS.enc.Utf8);
    } else {
      return null;
    }
  }
}
