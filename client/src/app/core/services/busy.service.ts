import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  loading = false;
  busyRequestcount = 0;

  busy(){
    this.busyRequestcount++;
    this.loading = true;
  }

  idle(){
    this.busyRequestcount--;
    if(this.busyRequestcount <= 0){
      this.busyRequestcount = 0;
      this.loading = false
    }
  }

}
