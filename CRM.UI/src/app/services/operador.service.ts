import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OperadorInt } from '../interface/operador.interface';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environments';

@Injectable({
    providedIn: 'root'
})
export class OperadorService {
    private url = "Operador"
    constructor(private http: HttpClient) { }

    public getOperadores() : Observable<OperadorInt[]> {
        return this.http.get<OperadorInt[]>(`${environment.apiUrl}/${this.url}`);
    }

    public updateOperador(operador: OperadorInt) : Observable<OperadorInt[]> {
        return this.http.put<OperadorInt[]>(`${environment.apiUrl}/${this.url}`, operador);
    }

    public createOperador(operador: OperadorInt) : Observable<OperadorInt[]> {
        return this.http.post<OperadorInt[]>(`${environment.apiUrl}/${this.url}`, operador);
    }
}

