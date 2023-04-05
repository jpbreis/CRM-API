import { Component, OnInit } from '@angular/core';
import { OperadorService } from 'src/app/services/operador.service';
import { Operador } from 'src/app/interface/operador.interface';

@Component({
  selector: 'app-operador-tb',
  templateUrl: './operador-tb.component.html',
  styleUrls: ['./operador-tb.component.css']
})
export class OperadorTbComponent implements OnInit {
  operadores: Operador[] = [];
  headers: string[] = [];

  constructor(private operadorService: OperadorService) { }

  ngOnInit(): void {
    this.operadorService.getOperadores().subscribe(operadores => {
      this.operadores = operadores;
      this.headers = Object.keys(operadores[0]);
    });
  }
}
