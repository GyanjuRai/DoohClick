import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class MainLayoutService {

    private _isSidebarVisible$ = new BehaviorSubject<boolean>(true);

    /**
     * 
     * @returns isSidebarVisible$ Observable 
     */
    get sideBar$(): Observable<boolean> {
        return this._isSidebarVisible$.asObservable();
    }

    toggleSidebar() {
        this._isSidebarVisible$.next(!this._isSidebarVisible$.value);
    }
}