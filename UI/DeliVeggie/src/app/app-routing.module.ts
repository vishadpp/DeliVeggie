import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component'
import { ProductComponent } from './components/product/product.component'

const routes: Routes = [
  {
    path: '',
    redirectTo: '/Product',
    pathMatch: 'full'
  },
  {
    path: 'Product',
    component: LayoutComponent,
    children:[
      { path:'', component: ProductComponent, pathMatch: 'full' }
    ]
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
