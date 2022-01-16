import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { IBasket, IBasketItem, Basket, IBasketTotals } from '../shared/models/basket';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/products';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

    getBasket(id: string) {
      return this.http.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        })
      )
    }

    setBasket(basket: IBasket) {
      return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      }, error => {
        console.log(error);
      })
    }

    getCurrentBasketValue() {
      return this.basketSource.value;
    }

    addItemToBasket(item: IProduct, quantity = 1) {
      const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
      const basket = this.getCurrentBasketValue() ?? this.createBasket();
      basket.itemS = this.addOrUpdate(basket.itemS, itemToAdd, quantity);
      this.setBasket(basket);
    } 

    addOrUpdate(itemS: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
      const index = itemS.findIndex(i => i.id === itemToAdd.id);
      if(index === -1) {
        itemToAdd.quantity = quantity;
        itemS.push(itemToAdd);
      } else {
        itemS[index].quantity += quantity;
      }

      return itemS;
    
    }

    incrementItemQuantity(item: IBasketItem) {
      const basket = this.getCurrentBasketValue();
      const foundItemIndex = basket.itemS.findIndex(x => x.id === item.id);
      basket.itemS[foundItemIndex].quantity++;
      this.setBasket(basket);
    }

    decrementItemQuantity(item: IBasketItem) {
      const basket = this.getCurrentBasketValue();
      const foundItemIndex = basket.itemS.findIndex(x => x.id === item.id);
      if(basket.itemS[foundItemIndex].quantity > 1) {
        basket.itemS[foundItemIndex].quantity--;
        this.setBasket(basket);
      } else {
        this.removeItemFromBasket(item);
      }
    }

    removeItemFromBasket(item: IBasketItem) {
      const basket = this.getCurrentBasketValue();
      if(basket.itemS.some(x => x.id === item.id)) {
        basket.itemS = basket.itemS.filter(i => i.id !== item.id);
        if (basket.itemS.length > 0) {
          this.setBasket(basket);
        } else {
          this.deleteBasket(basket);
        }
      }
    }

    deleteBasket(basket: IBasket) {
      return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem('basket_id');
      }, error => {
        console.log(error);
      })
    }

    private calculateTotals() {
      const basket = this.getCurrentBasketValue();
      const shipping = 0;
      const subtotal = basket.itemS.reduce((a,b) => (b.price * b.quantity) + a, 0);
      const total = subtotal + shipping;
      this.basketTotalSource.next({shipping, total, subtotal});
    }

    private createBasket(): IBasket {
      const basket = new Basket();
      localStorage.setItem('basket_id', basket.id);
      return basket;
    }

    private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
      return {
        id: item.id,
        productName: item.name,
        price: item.price,
        pictureUrl: item.pictureUrl,
        quantity,
        brand: item.productBrand,
        type: item.productType
      }
    }

}
