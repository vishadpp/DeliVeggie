export class Product {
    id: string;
    name: string;
    entryDate: string;
    price: string;
    
    constructor(id: string, name: string, entryDate: string, price: string) {
        this.id = id;
        this.name = name;
        this.entryDate = entryDate;
        this.price = price;
      }
}
