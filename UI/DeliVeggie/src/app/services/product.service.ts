import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  baseurl = environment.apiurl;

  constructor(private http: HttpClient) { }

  getAllProducts() {
    let reqHeader = new HttpHeaders();
    reqHeader.append('content-type', 'application/json');
    return this.http.get(this.baseurl + 'api/Product/GetAllProducts', { headers: reqHeader });
  }

  getProductDetails(id: string) {
    let reqHeader = new HttpHeaders();
    reqHeader.append('content-type', 'application/json');
    return this.http.get(this.baseurl + 'api/Product/GetProductDetails?id=' + id, { headers: reqHeader });
  }
}
