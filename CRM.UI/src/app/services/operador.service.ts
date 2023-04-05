import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Operador } from '../interface/operador.interface';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environments';

@Injectable({
    providedIn: 'root'
})
export class OperadorService {
    private url = "Operador"
    constructor(private http: HttpClient) { }

    public getOperadores() : Observable<Operador[]> {
        return this.http.get<Operador[]>(`${environment.apiUrl}/${this.url}`);
    }

    public updateOperador(operador: Operador) : Observable<Operador[]> {
        return this.http.put<Operador[]>(`${environment.apiUrl}/${this.url}`, operador);
    }

    public createOperador(operador: Operador) : Observable<Operador[]> {
        return this.http.post<Operador[]>(`${environment.apiUrl}/${this.url}`, operador);
    }
}

