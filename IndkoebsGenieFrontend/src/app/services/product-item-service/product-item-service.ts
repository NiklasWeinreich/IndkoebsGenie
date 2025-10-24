import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ProductItemService {
  private readonly productItemApiUrl = environment.apiUrl + '/productitem';

  constructor(private http: HttpClient) {}

  getAllproductItems() {
    console.log('Fetching product items from: ' + this.productItemApiUrl);
    return this.http.get<any>(this.productItemApiUrl);
  }
}
