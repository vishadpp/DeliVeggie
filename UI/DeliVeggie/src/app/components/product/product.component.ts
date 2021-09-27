import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/product.model'
import { ProductService } from '../../services/product.service'

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  constructor(private productService: ProductService) { }

  display:string = "";
  productList: Product[] = [];
  product = new Product('', '', '', '');
  
  ngOnInit(): void {
    this.display = "list";
    this.getAllProducts();
  }

  getAllProducts(){
    this.productService.getAllProducts().subscribe(res => {
      this.productList = <Product[]>res;
    });

  }

  viewDetails(id: string, index: number){
    this.getProductDetails(id,(index+1).toString());
  }

  getProductDetails(id: string, index: string){
    this.productService.getProductDetails(id).subscribe(res => {
      let result = <Product>res;
      if(result !== null){
        this.product.id = index;
        this.product.name = result.name;
        this.product.entryDate = result.entryDate;
        this.product.price = result.price;
        this.display = "details";
      }
    });
  }

  returnToList(){
    this.product = new Product('', '', '', '');
    this.getAllProducts();
    this.display = "list";
  }

}
